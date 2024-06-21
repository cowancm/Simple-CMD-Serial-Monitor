using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO.Ports;

namespace SerialToUSBCommunication
{
    public class Serial
    {
        private readonly string? _COMPort;

        private SerialPort _port;

        public enum BAUD_RATE
        {
            FortyEightHundred = 4800,
            NinetySixHundred = 9600,
            NineteenThousandTwoHundred = 19200,
            ThirtyEightThousandFourHundred = 38400,
            SeventySixThousandEightHundred = 76800,
            OneHundredFifteenThousandTwoHundred = 115200
        }

        public Serial(BAUD_RATE baudEnum = BAUD_RATE.OneHundredFifteenThousandTwoHundred, Parity Parity = Parity.None, string DeviceName = "Silicon Labs CP210x USB to UART Bridge")
        {
            _COMPort = null;

            //Grab port that matches the serial driver we're using
            foreach (string portNameOnDevice in SerialPort.GetPortNames())
            {
                if (IsDesiredDevice(portNameOnDevice, DeviceName))
                {
                    _COMPort = portNameOnDevice;
                    break;
                }
            }

            //throw error if never found...
            if (_COMPort == null)
                throw new Exception("Device not found! Check your USB connection, make sure there are no programs actively using this port, and ensure \"Silicon Labs CP210x USB to UART Bridge\" driver is installed correctly. The driver can be installed at:\nhttps://www.silabs.com/developers/usb-to-uart-bridge-vcp-drivers?tab=downloads\n\n Restart the program.");

            _port = new()
            {
                PortName = _COMPort,
                BaudRate = (int) baudEnum,
                Parity = Parity.None,
                DataBits = 8
            };

            _port.Open();
        }


        private static bool IsDesiredDevice(string portName, string desiredDeviceDescription)
        {
            string query = "SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%" + portName + "%'";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    string description = obj["Description"]?.ToString();
                    if (description != null && description.Contains(desiredDeviceDescription))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void open()
        {
            _port.Open();
        }

        public void Close()
        {
            _port.Close();
        }

        public void ChangeBAUD(BAUD_RATE BaudEnum)
        {
            _port.BaudRate = (int)BaudEnum;
        }

        public void ChangeDeviceOrCOM(string DeviceName)
        {
            _port.PortName = DeviceName;
        }

        public string ReadFromPort()
        {
            return _port.ReadLine();
        }

        public void WriteToPort(string UART)
        {
            //write with carridge return and new line
            _port.Write(UART + "\r\n");
        }

        public async Task<string> ReadFromPortAsync()
        {
            return await Task.Run(() => _port.ReadLine());
        }

        public async Task WriteToPortAsync(string UART)
        {
            await Task.Run(() => _port.Write(UART + "\r\n"));
        }
    }
}

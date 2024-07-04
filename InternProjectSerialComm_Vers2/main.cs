using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialToUSBCommunication
{
    //note: I coded this for a command line, maybe add an interface?

    //TODO:
    //1. Make an interface IControl that outlines the contracts that will let us control the vehicle from
    //   * Interface should have contracts for move forward, turn right, turn left etc so for example Keyboard controller class and xbox controller class implements Icontrol
    //
    //2. Add logging for GPS and movement data w/ time stamps (per Alvin) -> make class that implements ILogger, poops out txt files with this information (add hashing for security purposes? (also per Alvin))
    //3. Maybe make some type of graphical interface instead of jank cmd?


    public class main
    {
        public static async Task Main()
        {
            Console.WriteLine("Input the BAUD rate of your device by inputing the corresponding number:\n");
            bool incorrectBAUDCheck = true;
            Serial.BAUD_RATE baudRate = Serial.BAUD_RATE.OneHundredFifteenThousandTwoHundred;

            while (incorrectBAUDCheck)
            {
                Console.WriteLine("[1]\t\t  4800 BPS");
                Console.WriteLine("[2]\t\t  9600 BPS");
                Console.WriteLine("[3]\t\t 19200 BPS");
                Console.WriteLine("[4]\t\t 38400 BPS");
                Console.WriteLine("[5]\t\t 76800 BPS");
                Console.WriteLine("[6]\t\t115200 BPS\n\n");

                var input = Console.ReadLine();


                if ((string.IsNullOrWhiteSpace(input)))
                {
                    Console.WriteLine("Error, please try again");
                    continue;
                }

                int inputShouldBeNum;

                try
                {
                    inputShouldBeNum = int.Parse(input);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error, please try again");
                    continue;
                }

                switch (inputShouldBeNum)
                {
                    case 1:
                        baudRate = Serial.BAUD_RATE.FortyEightHundred;
                        break;
                    case 2:
                        baudRate = Serial.BAUD_RATE.NinetySixHundred;
                        break;
                    case 3:
                        baudRate = Serial.BAUD_RATE.NineteenThousandTwoHundred;
                        break;
                    case 4:
                        baudRate = Serial.BAUD_RATE.ThirtyEightThousandFourHundred;
                        break;
                    case 5:
                        baudRate = Serial.BAUD_RATE.SeventySixThousandEightHundred;
                        break;
                    case 6:
                        baudRate = Serial.BAUD_RATE.OneHundredFifteenThousandTwoHundred;
                        break;
                    default:
                        Console.WriteLine("Error, please try again");
                        continue;
                }

                incorrectBAUDCheck = false;
            }

            Console.WriteLine("\nAttempting to connect to port...");

            var comPort = new Serial(baudRate);

            Task readTask = Task.Run(() => ReadFromPort(comPort));
            Task writeTask = Task.Run(() => WriteCommandsToPort(comPort));

            await Task.WhenAll(readTask, writeTask);

            Console.ReadLine();
        }

        public static async Task ReadFromPort(Serial port)
        {
            while (true)
            {
                string data = port.ReadFromPort();

                if (data != null)
                {
                    Console.WriteLine(data);
                }

                await Task.Delay(10);
            }
        }

        public static async Task WriteCommandsToPort(Serial port)
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Empty input, please try again...");
                    continue;
                }

                CheckIfUserWantsToStop(input, port);

                port.WriteToPort(input);
                await Task.Delay(10);
            }
        }

        public static void CheckIfUserWantsToStop(string input, Serial port)
        {
            if ((input == "stop")||(input == "Stop"))
            {
                Console.WriteLine("Ending Operation... It was a pleasure :)");
                
                Environment.Exit(0);
                port.Close();
            }
        }


    }
}

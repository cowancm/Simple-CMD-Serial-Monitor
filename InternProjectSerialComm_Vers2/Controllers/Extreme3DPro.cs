using MissionOperationControl.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MissionOperationControl.Controllers
{

    //Extreme3DPro is a joystick Joe M. gave me --- Chris
    public class Extreme3DPro : IController
    {
        public Extreme3DPro()
        {
        }
        public bool Forwards()
        {
            return true;
        }

        public bool Backwards()
        {
            return false;
        }

        public int Angle()
        {
            return 0;
        }

    }
}

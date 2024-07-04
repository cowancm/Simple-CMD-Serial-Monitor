using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionOperationControl.Contracts
{
    public interface IController
    {
        public bool Forwards();

        public bool Backwards();

        public int Angle();

    }
}

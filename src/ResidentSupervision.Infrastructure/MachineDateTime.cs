using System;
using ResidentSupervision.Common;

namespace ResidentSupervision.Infrastructure {
    public class MachineDateTime : IDateTime {
        public DateTime Now => DateTime.Now;

        public int CurrentYear => DateTime.Now.Year;
    }
}
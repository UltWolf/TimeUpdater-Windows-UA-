using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TimeUpdater_Windows_.Data
{
    public struct SystemTime
    {
        public ushort Year;
        public ushort Month;
        public ushort DayOfWeek;
        public ushort Day;
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public ushort Millisecond;

    };
}

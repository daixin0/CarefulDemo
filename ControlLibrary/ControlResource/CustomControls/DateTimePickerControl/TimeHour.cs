using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlResources.CustomControls.DateTimePickerControl
{
    public class TimeHour
    {
        public int Hour1 { get; set; }
        public int Hour2 { get; set; }
        public int Hour3 { get; set; }
        public int Hour4 { get; set; }
        public int Hour5 { get; set; }
        public int Hour6 { get; set; }

        public TimeHour(int hour1, int hour2, int hour3, int hour4, int hour5, int hour6)
        {
            Hour1 = hour1;
            Hour2 = hour2;
            Hour3 = hour3;
            Hour4 = hour4;
            Hour5 = hour5;
            Hour6 = hour6;

        }
    }
}

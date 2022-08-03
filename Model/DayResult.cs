using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Texode_test_step_analyzer.Model
{
    public class DayResult
    {
        public int Day { set; get; }
        public int Rank { set; get; }
        public int Steps { set; get; }
        public string Status { set; get; } = "";
        public string User { set; get; }

        public override string ToString()
        {
            return $"{Steps}";
        }

    }
}

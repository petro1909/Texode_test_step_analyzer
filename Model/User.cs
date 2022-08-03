using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Texode_test_step_analyzer.Model
{
    public class User
    {
        public string Name { set; get; }
        public double AverageStepsCount { private set; get; }
        public int WorstStepsCount { private set; get; }
        public int BestStepsCount { private set; get; }
        public List<DayResult> DayResults { set; get; }


        public User(string name)
        {
            Name = name;
            DayResults = new List<DayResult>();
        }
        public User(string name, DayResult dr) : this(name)
        {
            DayResults.Add(dr);
            AverageStepsCount = WorstStepsCount = BestStepsCount = dr.Steps;
        }

        public void AddDayResult(DayResult dr)
        {
            DayResults.Add(dr);
            DayResults.Sort((x, y) => x.Day.CompareTo(y.Day));

            if (dr.Steps < WorstStepsCount)
            {
                WorstStepsCount = dr.Steps;
            }

            if (dr.Steps > BestStepsCount)
            {
                BestStepsCount = dr.Steps;
            }

            AverageStepsCount = ((AverageStepsCount * (DayResults.Count - 1)) + dr.Steps) / DayResults.Count;
            AverageStepsCount = Math.Round(AverageStepsCount, 2);
        }

        public override bool Equals(object obj)
        {
            if (obj is not User) return false;
            return Name.Equals((obj as User).Name, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }
    }
}

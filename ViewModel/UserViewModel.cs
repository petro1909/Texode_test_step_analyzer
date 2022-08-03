using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Texode_test_step_analyzer.Model;

namespace Texode_test_step_analyzer.ViewModel
{
    class UserViewModel : BaseViewModel
    {
        public readonly User User;
        public UserViewModel(User user)
        {
            User = user;
        }

        public string Name => User.Name;

        public double AverageSteps => User.AverageStepsCount;

        public int WorstStepResult => User.WorstStepsCount;


        public int BestStepResult => User.BestStepsCount;


        public SolidColorBrush UserColor
        {
            get
            {
                double BestAndAverageDelta = BestStepResult / AverageSteps;
                double AverageAndWorstDelta = AverageSteps / WorstStepResult;
                return BestAndAverageDelta > 1.2 || AverageAndWorstDelta > 1.2 ? new SolidColorBrush(Color.FromArgb(50, 212, 16, 16)) : new SolidColorBrush(Colors.White);
            }
        }

    }
}

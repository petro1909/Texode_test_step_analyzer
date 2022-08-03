using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Texode_test_step_analyzer.Model;
using Texode_test_step_analyzer.Command;
using Texode_test_step_analyzer.Parser;
using LiveCharts.Wpf;
using LiveCharts;
using LiveCharts.Configurations;
using System.Windows.Media;
using Microsoft.Win32;

namespace Texode_test_step_analyzer.ViewModel
{
    class AppViewModel : BaseViewModel
    {
        private readonly FileService FileService;
        private readonly UserJsonParser UserParser;
        const string JsonTestFolder = "JsonTests";
        string[] FilesPaths;
        public AppViewModel()
        {
            FileService = new FileService();
            UserParser = new UserJsonParser();


            string testJsonFilesFolderPath = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\\Resources";
            FilesPaths = FileService.GetAllFilesPathsFromSpecifiedFolder($"{testJsonFilesFolderPath}\\{JsonTestFolder}");
        }

        private ObservableCollection<UserViewModel> _Users;
        public ObservableCollection<UserViewModel> Users
        {
            set
            {
                _Users = value;
                OnPropertyChanged();
            }
            get
            {
                HashSet<User> users = UserParser.GetUsersFromJsonFiles(FilesPaths);
                _Users = new ObservableCollection<UserViewModel>(users.Select(u => new UserViewModel(u)));
                return _Users;
            }
        }

        private SeriesCollection _UserDaySteps;
        public SeriesCollection UserDaySteps
        {
            set
            {
                _UserDaySteps = value;
                OnPropertyChanged();
            }
            get
            {
                if (SelectedItem != null)
                {
                    var mapper = Mappers.Xy<DayResult>().X(u => u.Day).Y(u => u.Steps).Fill(u =>
                    {
                        if (SelectedItem == null) return Brushes.White;
                        if (u.Steps == SelectedItem.WorstStepResult) return Brushes.Red;
                        if (u.Steps == SelectedItem.BestStepResult) return Brushes.Green;
                        else return Brushes.White;
                    });
                    _UserDaySteps = new SeriesCollection(mapper) { new LineSeries() { PointGeometrySize = 10 }};
                    _UserDaySteps[0].Values = new ChartValues<DayResult>(SelectedItem.User.DayResults);
                }
                return _UserDaySteps;
            }
        }

        private UserViewModel _SelectedItem;
        public UserViewModel SelectedItem
        {
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                OnPropertyChanged("UserDaySteps");
            }
            get => _SelectedItem;
        }

        private RelayCommand _SaveUserToJsonCommand;
        public RelayCommand SaveUserToJsonCommand => _SaveUserToJsonCommand ??= new RelayCommand(obj =>
        {
            if (SelectedItem == null || string.IsNullOrEmpty(SelectedItem.Name)) return;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Файлы программ (*.json)|*.json";
            dialog.FileName = $"{SelectedItem.Name}.json";
            if (dialog.ShowDialog() == false) return;

            UserParser.WriteUserToJsonString(SelectedItem.User, dialog.FileName);
        });


        private RelayCommand _ChooseJsonFilesCommand;
        public RelayCommand ChooseJsonFilesCommand => _ChooseJsonFilesCommand ??= new RelayCommand(obj =>
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Файлы программ (*.json)|*.json";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == false) return;
            if (UserDaySteps != null)
            {
                UserDaySteps[0].Values.Clear();
            }
            FilesPaths = dialog.FileNames;
            OnPropertyChanged("Users");
        });

    }
}

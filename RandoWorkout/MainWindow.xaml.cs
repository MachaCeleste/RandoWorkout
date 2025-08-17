using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace RandoWorkout
{
    public partial class MainWindow : Window
    {
        public ViewModelHelper ViewModel { get; }
        public string SavePath;

        private int UnneededItems = 0;
        private int Rupees = 0;
        private int HeartPieces = 0;
        private int SeaCharts = 0;
        private int Squats = 0;
        private int BacklogSquats = 0;
        private int Curls = 0;
        private int BacklogCurls = 0;
        private int Shrugs = 0;
        private int BacklogShrugs = 0;
        private int OverheadPress = 0;
        private int BacklogOverheadPress = 0;

        public MainWindow()
        {
            InitializeComponent();
            FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileInfo.CompanyName, fileInfo.ProductName);
            ViewModel = new ViewModelHelper { ConsoleOutput = "" };
            DataContext = ViewModel;
            var loadCheck = LoadLog();
            if (!string.IsNullOrEmpty(loadCheck))
            {
                ConsolePrint(loadCheck);
                WriteOBSLog();
            }
            ConsolePrint("System ready!");
            UpdateViewModel();
        }

        #region ConsoleControls
        private void ConsolePrint(string message)
        {
            ViewModel.ConsoleOutput += $"{GetTimeNow()}: {message}\n";
            ConsoleOutBox.ScrollToEnd();
        }

        private void ConsoleClear(string message)
        {
            ViewModel.ConsoleOutput = $"{GetTimeNow()}: {message}\n";
        }

        private string GetTimeNow()
        {
            return DateTime.Now.ToString("yyyy.MM.dd-HH:mm:ss");
        }
        #endregion

        private void WriteOBSLog()
        {
            if (!File.Exists(SavePath))
                Directory.CreateDirectory(SavePath);
            var filePath = Path.Combine(SavePath, "OBSLog.txt");
            var output = $"Unneeded Items: {UnneededItems}            Squats: {BacklogSquats}/{Squats}\n" +
                         $"        Rupees: {Rupees}             Curls: {BacklogCurls}/{Curls}\n" +
                         $"  Heart Pieces: {HeartPieces}            Shrugs: {BacklogShrugs}/{Shrugs}\n" +
                         $"    Sea Charts: {SeaCharts}    Overhead Press: {BacklogOverheadPress}/{OverheadPress}";
            try
            {
                File.WriteAllText(filePath, output);
            }
            catch (Exception ex)
            {
                ConsolePrint($"Save Error: {ex.Message}");
            }
        }

        private string LoadLog()
        {
            var filePath = Path.Combine(SavePath, "OBSLog.txt");
            if (!File.Exists(filePath))
                return "Save File not found!";
            var text = File.ReadAllText(filePath);

            List<string> failures = new List<string>();
            if (!int.TryParse(Regex.Match(text, @"Unneeded Items:\s*(\d+)").Groups[1].Value, out UnneededItems)) failures.Add("Unneeded Items");
            if (!int.TryParse(Regex.Match(text, @"Rupees:\s*(\d+)").Groups[1].Value, out Rupees)) failures.Add("Rupees");
            if (!int.TryParse(Regex.Match(text, @"Heart Pieces:\s*(\d+)").Groups[1].Value, out HeartPieces)) failures.Add("Heart Pieces");
            if (!int.TryParse(Regex.Match(text, @"Sea Charts:\s*(\d+)").Groups[1].Value, out SeaCharts)) failures.Add("Sea Charts");

            var _squats = Regex.Match(text, @"Squats:\s*(\d+)\/(\d+)");
            if (!int.TryParse(_squats.Groups[1].Value, out BacklogSquats)) failures.Add("Backlog Squats");
            if (!int.TryParse(_squats.Groups[2].Value, out Squats)) failures.Add("Squats");
            
            var _curls = Regex.Match(text, @"Curls:\s*(\d+)\/(\d+)");
            if (!int.TryParse(_curls.Groups[1].Value, out BacklogCurls)) failures.Add("Backlog Curls");
            if (!int.TryParse(_curls.Groups[2].Value, out Curls)) failures.Add("Curls");
            
            var _shrugs = Regex.Match(text, @"Shrugs:\s*(\d+)\/(\d+)");
            if (!int.TryParse(_shrugs.Groups[1].Value, out BacklogShrugs)) failures.Add("Backlog Shrugs");
            if (!int.TryParse(_shrugs.Groups[2].Value, out Shrugs)) failures.Add("Shrugs");

            var _overheadPress = Regex.Match(text, @"Overhead Press:\s*(\d+)\/(\d+)");
            if (!int.TryParse(_overheadPress.Groups[1].Value, out BacklogOverheadPress)) failures.Add("Backlog Overhead Press");
            if (!int.TryParse(_overheadPress.Groups[2].Value, out OverheadPress)) failures.Add("Overhead Press");

            string output = "";
            if (failures.Count > 0)
                output = $"The following items have failed to load: {string.Join(", ", failures)}.";
            return output;
        }

        #region Generation
        private void GenExercise(Exercise type)
        {
            Dictionary<Exercise, int> Weights;
            switch (type)
            {
                case Exercise.Curls:
                    Weights = new Dictionary<Exercise, int>
                    {
                        { Exercise.Curls, 70 },
                        { Exercise.Squats, 10 },
                        { Exercise.Shrugs, 10 },
                        { Exercise.Overhead_Press, 10 }
                    };
                    break;
                case Exercise.Squats:
                    Weights = new Dictionary<Exercise, int>
                    {
                        { Exercise.Curls, 10 },
                        { Exercise.Squats, 70 },
                        { Exercise.Shrugs, 10 },
                        { Exercise.Overhead_Press, 10 }
                    };
                    break;
                case Exercise.Shrugs:
                    Weights = new Dictionary<Exercise, int>
                    {
                        { Exercise.Curls, 10 },
                        { Exercise.Squats, 10 },
                        { Exercise.Shrugs, 70 },
                        { Exercise.Overhead_Press, 10 }
                    };
                    break;
                case Exercise.Overhead_Press:
                    Weights = new Dictionary<Exercise, int>
                    {
                        { Exercise.Curls, 10 },
                        { Exercise.Squats, 10 },
                        { Exercise.Shrugs, 10 },
                        { Exercise.Overhead_Press, 70 }
                    };
                    break;
                default:
                    Weights = new Dictionary<Exercise, int>
                    {
                        { Exercise.Curls, 50 },
                        { Exercise.Squats, 25 },
                        { Exercise.Shrugs, 15 },
                        { Exercise.Overhead_Press, 10 }
                    };
                    break;
            }
            var loot = LootPicker(Weights);
            AddExercise(loot.Key);
            WriteOBSLog();
            UpdateViewModel();
            ConsolePrint($"You owe 5 {loot.Key}!");
        }

        private KeyValuePair<Exercise, int> LootPicker(Dictionary<Exercise, int> lootTable)
        {
            Random rng = new Random(Guid.NewGuid().GetHashCode());
            int totalWeight = lootTable.Values.Sum();
            int roll = rng.Next(0, totalWeight);
            foreach (var kvp in lootTable)
            {
                roll -= kvp.Value;
                if (roll < 0)
                    return kvp;
            }
            throw new InvalidOperationException("Invalid loot weights!");
        }

        private void AddExercise(Exercise exercise)
        {
            switch (exercise)
            {
                case Exercise.Squats:
                    Squats += 5;
                    BacklogSquats += 5;
                    break;
                case Exercise.Curls:
                    Curls += 5;
                    BacklogCurls += 5;
                    break;
                case Exercise.Shrugs:
                    Shrugs += 5;
                    BacklogShrugs += 5;
                    break;
                case Exercise.Overhead_Press:
                    OverheadPress += 5;
                    BacklogOverheadPress += 5;
                    break;
            }
        }

        private void ClearBacklog()
        {
            BacklogSquats = 0;
            BacklogCurls = 0;
            BacklogShrugs = 0;
            BacklogOverheadPress = 0;
        }

        private void UpdateViewModel()
        {
            ViewModel.UnneededItemsCount = UnneededItems.ToString();
            ViewModel.RupeeCount = Rupees.ToString();
            ViewModel.HeartPiecesCount = HeartPieces.ToString();
            ViewModel.SeaChartsCount = SeaCharts.ToString();
            ViewModel.SquatsLabel = $"Squats: {BacklogSquats}/{Squats}";
            ViewModel.CurlsLabel = $"Curls: {BacklogCurls}/{Curls}";
            ViewModel.ShrugsLabel = $"Shrugs: {BacklogShrugs}/{Shrugs}";
            ViewModel.OverheadPressLabel = $"Overhead Press: {BacklogOverheadPress}/{OverheadPress}";
        }

        private enum Exercise
        {
            None,
            Squats,
            Curls,
            Shrugs,
            Overhead_Press
        }
        #endregion

        #region Buttons
        private void AddUI_Click(object sender, RoutedEventArgs e)
        {
            UnneededItems++;
            GenExercise(Exercise.None);
        }

        private void AddR_Click(object sender, RoutedEventArgs e)
        {
            Rupees++;
            GenExercise(Exercise.Curls);
        }

        private void AddHP_Click(object sender, RoutedEventArgs e)
        {
            HeartPieces++;
            GenExercise(Exercise.Squats);
        }

        private void AddSC_Click(object sender, RoutedEventArgs e)
        {
            SeaCharts++;
            GenExercise(Exercise.Overhead_Press);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            UnneededItems = 0;
            Rupees = 0;
            HeartPieces = 0;
            SeaCharts = 0;
            Squats = 0;
            Curls = 0;
            Shrugs = 0;
            OverheadPress = 0;
            ClearBacklog();
            WriteOBSLog();
            UpdateViewModel();
            ConsoleClear("System Reset!");
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearBacklog();
            WriteOBSLog();
            UpdateViewModel();
            ConsoleClear("Log Cleared!");
        }
        #endregion
    }
}
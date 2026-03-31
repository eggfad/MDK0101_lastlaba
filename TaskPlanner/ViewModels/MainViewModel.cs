using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using TaskPlanner.Models;

namespace TaskPlanner.ViewModels
{
    public class MainViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private string _currentTime;
        private string _newTaskTitle = string.Empty;
        private DateTime _newTaskDeadline = DateTime.Now.AddDays(1);
        private DispatcherTimer _timer;

        public ObservableCollection<TaskItem> Tasks { get; } = new ObservableCollection<TaskItem>();

        public string CurrentTime
        {
            get => _currentTime;
            set { _currentTime = value; OnPropertyChanged(nameof(CurrentTime)); }
        }

        public string NewTaskTitle
        {
            get => _newTaskTitle;
            set { _newTaskTitle = value; OnPropertyChanged(nameof(NewTaskTitle)); }
        }

        public DateTime NewTaskDeadline
        {
            get => _newTaskDeadline;
            set { _newTaskDeadline = value; OnPropertyChanged(nameof(NewTaskDeadline)); }
        }

        public RelayCommand AddTaskCommand { get; }
        public RelayCommand<TaskItem> RemoveTaskCommand { get; }
        public RelayCommand<TaskItem> CompleteTaskCommand { get; }

        public MainViewModel()
        {
            AddTaskCommand = new RelayCommand(AddTask, () => !string.IsNullOrWhiteSpace(NewTaskTitle));
            RemoveTaskCommand = new RelayCommand<TaskItem>(task => Tasks.Remove(task));
            CompleteTaskCommand = new RelayCommand<TaskItem>(task =>
            {
                task.IsCompleted = true;
                task.Refresh();
            });

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += OnTimerTick;
            _timer.Start();

            UpdateCurrentTime();

            // Демо-данные
            Tasks.Add(new TaskItem { Title = "Сдать лабораторную работу", Deadline = DateTime.Now.AddHours(3) });
            Tasks.Add(new TaskItem { Title = "Купить продукты", Deadline = DateTime.Now.AddDays(2) });
            Tasks.Add(new TaskItem { Title = "Просроченная задача", Deadline = DateTime.Now.AddHours(-5) });
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            UpdateCurrentTime();
            foreach (var task in Tasks)
                task.Refresh();
        }

        private void UpdateCurrentTime()
        {
            CurrentTime = DateTime.Now.ToString("dd MMMM yyyy, HH:mm:ss",
                new System.Globalization.CultureInfo("ru-RU"));
        }

        private void AddTask()
        {
            if (string.IsNullOrWhiteSpace(NewTaskTitle))
                return;

            Tasks.Add(new TaskItem
            {
                Title = NewTaskTitle,
                Deadline = NewTaskDeadline
            });

            NewTaskTitle = string.Empty;
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
    }
}
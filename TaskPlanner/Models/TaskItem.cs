using System;
using System.ComponentModel;
using System.Globalization;

namespace TaskPlanner.Models
{
    public class TaskItem : INotifyPropertyChanged
    {
        private string _title = string.Empty;
        private DateTime _deadline;
        private bool _isCompleted;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public DateTime Deadline
        {
            get => _deadline;
            set
            {
                _deadline = value;
                OnPropertyChanged(nameof(Deadline));
                OnPropertyChanged(nameof(TimeLeft));
                OnPropertyChanged(nameof(IsOverdue));
                OnPropertyChanged(nameof(DeadlineFormatted));
                OnPropertyChanged(nameof(TimeLeftFormatted));
            }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
                OnPropertyChanged(nameof(TimeLeftFormatted));
            }
        }

        public TimeSpan TimeLeft => Deadline - DateTime.Now;

        public bool IsOverdue => DateTime.Now > Deadline && !IsCompleted;

        public string DeadlineFormatted => Deadline.ToString("dd MMMM yyyy, HH:mm", new CultureInfo("ru-RU"));

        public string TimeLeftFormatted
        {
            get
            {
                if (IsCompleted) return "✅ Выполнено";
                var span = TimeLeft;
                if (span.TotalSeconds <= 0) return "❌ ПРОСРОЧЕНО";
                if (span.TotalDays >= 1)
                    return $"{(int)span.TotalDays} д. {span.Hours} ч. {span.Minutes} мин.";
                if (span.TotalHours >= 1)
                    return $"{span.Hours} ч. {span.Minutes} мин.";
                return $"{span.Minutes} мин. {span.Seconds} сек.";
            }
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(TimeLeft));
            OnPropertyChanged(nameof(IsOverdue));
            OnPropertyChanged(nameof(TimeLeftFormatted));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using TaskPlanner.Models;
using TaskPlanner.ViewModels;

namespace TaskPlanner.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() != true) return;

            var doc = new FlowDocument
            {
                PagePadding = new Thickness(40),
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 12
            };

            // Заголовок
            doc.Blocks.Add(new Paragraph(new Run("📋 Список задач"))
            {
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 6)
            });

            // Дата печати
            var culture = new System.Globalization.CultureInfo("ru-RU");
            doc.Blocks.Add(new Paragraph(
                new Run($"🖨️ Распечатано: {DateTime.Now.ToString("dd MMMM yyyy, HH:mm", culture)}"))
            {
                FontSize = 10,
                Foreground = Brushes.Gray,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            });

            // Таблица задач
            var table = new Table();
            table.Columns.Add(new TableColumn { Width = new GridLength(2, GridUnitType.Star) });
            table.Columns.Add(new TableColumn { Width = new GridLength(2, GridUnitType.Star) });
            table.Columns.Add(new TableColumn { Width = new GridLength(1.5, GridUnitType.Star) });
            table.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });

            // Заголовок таблицы
            var headerGroup = new TableRowGroup();
            var headerRow = new TableRow { Background = Brushes.LightSteelBlue };
            foreach (var h in new[] { "Задача", "Дедлайн", "Осталось", "Статус" })
            {
                headerRow.Cells.Add(new TableCell(new Paragraph(new Run(h))
                {
                    FontWeight = FontWeights.Bold,
                    Padding = new Thickness(6, 4, 6, 4)
                }));
            }
            headerGroup.Rows.Add(headerRow);
            table.RowGroups.Add(headerGroup);

            // Строки задач
            var bodyGroup = new TableRowGroup();
            int i = 0;
            foreach (TaskItem task in vm.Tasks)
            {
                var row = new TableRow
                {
                    Background = i++ % 2 == 0 ? Brushes.White : Brushes.WhiteSmoke
                };
                var status = task.IsCompleted ? "✅ Выполнено" :
                             task.IsOverdue ? "❌ Просрочено" : "⏳ Активна";

                row.Cells.Add(Cell(task.Title));
                row.Cells.Add(Cell(task.DeadlineFormatted));
                row.Cells.Add(Cell(task.TimeLeftFormatted));
                row.Cells.Add(Cell(status, task.IsOverdue && !task.IsCompleted ? Brushes.Red :
                                   task.IsCompleted ? Brushes.Green : Brushes.Black));
                bodyGroup.Rows.Add(row);
            }
            table.RowGroups.Add(bodyGroup);
            doc.Blocks.Add(table);

            var docPaginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;
            docPaginator.PageSize = new Size(dialog.PrintableAreaWidth, dialog.PrintableAreaHeight);
            dialog.PrintDocument(docPaginator, "Список задач");
        }

        private static TableCell Cell(string text, Brush foreground = null)
        {
            return new TableCell(new Paragraph(new Run(text))
            {
                Padding = new Thickness(6, 3, 6, 3),
                Foreground = foreground ?? Brushes.Black
            });
        }
    }
}
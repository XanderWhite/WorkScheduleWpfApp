using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Xml;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WorkScheduleWpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SettingConfig settings;

        public MainWindow()
        {
            InitializeComponent();

            settings = new SettingConfig();

            FillSettings();
        }

        private void FillSettings()
        {
            datePicker_FirstWorkDay.SelectedDate = settings.FirstWorkDay;

            datePicker_FindWorkDay.SelectedDate = settings.FindWorkDay;

            comboBox_Work.SelectedIndex = settings.Work_SelectedIndex;

            comboBox_Free.SelectedIndex = settings.Free_SelectedIndex;

            textBox_Result.Text = settings.TextboxText;
        }

        private void Button_Calculate_Click(object sender, RoutedEventArgs e)
        {
            textBox_Result.Text = Calculate(datePicker_FirstWorkDay.SelectedDate.Value, datePicker_FindWorkDay.SelectedDate.Value,
                 (int)comboBox_Work.SelectedValue, (int)comboBox_Free.SelectedValue);

            settings.FirstWorkDay = datePicker_FirstWorkDay.SelectedDate.Value;
            settings.FindWorkDay = datePicker_FindWorkDay.SelectedDate.Value;
            settings.Free_SelectedIndex = comboBox_Free.SelectedIndex;
            settings.Work_SelectedIndex = comboBox_Work.SelectedIndex;
            settings.TextboxText = textBox_Result.Text;

            settings.SaveSettings();
        }

        private void DatePickerFirstWorkDay_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePicker_FindWorkDay.SelectedDate == null || datePicker_FirstWorkDay.SelectedDate == null) return;

            if (datePicker_FindWorkDay.SelectedDate.Value < datePicker_FirstWorkDay.SelectedDate.Value)
            {
                datePicker_FindWorkDay.SelectedDate = datePicker_FirstWorkDay.SelectedDate;
            }

            datePicker_FindWorkDay.DisplayDateStart = datePicker_FirstWorkDay.SelectedDate;
            datePicker_FindWorkDay.DisplayDateEnd = datePicker_FirstWorkDay.SelectedDate.Value.AddYears(2);

            textBox_Result.Text = string.Empty;
        }

        private void DatePicker_FindWorkDay_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            textBox_Result.Text = string.Empty;
        }

        private void ComboBox_Free_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBox_Result.Text = string.Empty;
        }

        private void ComboBox_Work_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBox_Result.Text = string.Empty;
        }

        private string Calculate(DateTime firstWorkDay, DateTime findWorkDay, int workDays, int freeDays)
        {
            string result;

            int summ = workDays + freeDays;

            int diffDays = (findWorkDay - firstWorkDay).Days;

            int mod = diffDays % summ;

            result = mod >= workDays
                ? string.Format("Это {1}-й ({0}) выходной день", (mod - workDays + 1).ConvertToRomanNumeral(), mod - workDays + 1)
                : string.Format("Это {1}-й ({0}) рабочий день", (mod + 1).ConvertToRomanNumeral(), mod + 1);

            return result;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox_Work.ItemsSource = Enumerable.Range(1, 31);
            comboBox_Free.ItemsSource = Enumerable.Range(1, 31);

            datePicker_FirstWorkDay.SelectedDateFormat = DatePickerFormat.Long;
            datePicker_FirstWorkDay.FirstDayOfWeek = DayOfWeek.Monday;
            datePicker_FirstWorkDay.SelectedDateChanged += DatePickerFirstWorkDay_SelectedDateChanged;

            datePicker_FindWorkDay.DisplayDateStart = datePicker_FirstWorkDay.SelectedDate;
            datePicker_FindWorkDay.DisplayDateEnd = datePicker_FirstWorkDay.SelectedDate.Value.AddYears(2);
            datePicker_FindWorkDay.SelectedDateFormat = DatePickerFormat.Long;
            datePicker_FindWorkDay.FirstDayOfWeek = DayOfWeek.Monday;
            datePicker_FindWorkDay.SelectedDateChanged += DatePicker_FindWorkDay_SelectedDateChanged;

            comboBox_Work.SelectionChanged += ComboBox_Work_SelectionChanged;

            comboBox_Free.SelectionChanged += ComboBox_Free_SelectionChanged;

            button_Calculate.Click += Button_Calculate_Click;
        }
        
    }
}

using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace View.Controls
{
    public partial class ContactControl : UserControl
    {
        public ContactControl()
        {
            InitializeComponent();
        }

        // Запрет ввода букв с клавиатуры
        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9\+\-\(\)\s]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Запрет вставки неправильного текста из буфера обмена
        private void PhoneTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                Regex regex = new Regex(@"[^0-9\+\-\(\)\s]+");
                if (regex.IsMatch(text)) e.CancelCommand();
            }
            else e.CancelCommand();
        }
    }
}
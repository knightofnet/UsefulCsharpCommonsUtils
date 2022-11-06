using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace UsefulCsharpCommonsUtils.ui.usercontrol
{
    /// <summary>
    /// Logique d'interaction pour KeyValueUc.xaml
    /// </summary>
    public partial class KeyValueUc : UserControl
    {

        public String Key { get => (string)lblKey.Content; set => lblKey.Content = value; }
        public String Value { get => txtValue.Text; set => txtValue.Text = value; }

        public String ValueWithDispatcher
        {
            get => Value;
            set => SetValueWithDispathcher(value);
        }

        public String KeyWithDispatcher
        {
            get => Key;
            set => SetKeyTextWithDispathcher(value);
        }

        public KeyValueUc()
        {
            InitializeComponent();

            lblKey.Content = string.Empty;
            txtValue.Text = string.Empty;
        }

        public void SetValueWithDispathcher(string value)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                txtValue.Text = value;
            }));
        }

        public void SetKeyTextWithDispathcher(string value)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                lblKey.Content = value;
            }));
        }
    }
}

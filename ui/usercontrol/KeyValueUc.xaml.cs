using System;
using System.Windows.Controls;

namespace UsefulCsharpCommonsUtils.ui.usercontrol
{
    /// <summary>
    /// Logique d'interaction pour KeyValueUc.xaml
    /// </summary>
    public partial class KeyValueUc : UserControl
    {

        public String Key { get => (string)lblKey.Content; set => lblKey.Content = value; }
        public String Value { get => txtValue.Text; set => txtValue.Text = value; }

        public KeyValueUc()
        {
            InitializeComponent();

            lblKey.Content = string.Empty;
            txtValue.Text = string.Empty;
        }
    }
}

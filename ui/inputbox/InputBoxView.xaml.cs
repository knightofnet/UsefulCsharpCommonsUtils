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
using System.Windows.Shapes;

namespace UsefulCsharpCommonsUtils.ui.inputbox
{
    /// <summary>
    /// Logique d'interaction pour InputBoxView.xaml
    /// </summary>
    public partial class InputBoxView : Window
    {
        public string Result { get; private set; }

        public static bool InputBox(string dialogText, string title, out string result, string initialText = null)
        {
            InputBoxView ibv = new InputBoxView();
            ibv.texte.Text = dialogText;
            ibv.Title = title;
            ibv.tbox.Text = initialText ?? string.Empty;

            ibv.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ibv.Topmost = true;
            ibv.Loaded += (sender, args) =>
            {
                ibv.Topmost = false;
            };

            if (ibv.ShowDialog() ?? false)
            {
                result = ibv.Result;
                return true;
            }

            result = ibv.Result;
            return false;
        }

        public InputBoxView()
        {
            InitializeComponent();
            Loaded += (sender, args) => tbox.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Result = tbox.Text;

            Close();
        }
    }
}

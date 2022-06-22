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

namespace UsefulCsharpCommonsUtils.ui.usercontrol
{
    /// <summary>
    /// Logique d'interaction pour btnOkCancelUc.xaml
    /// </summary>
    public partial class btnOkCancelUc : UserControl
    {
        public event RoutedEventHandler OnOkClick;
        public event RoutedEventHandler OnCancelClick;

        public btnOkCancelUc()
        {
            InitializeComponent();
            Background = null;

            btnOk.Click += (s, e) =>
            {
                OnOkClick?.Invoke(s, e);
            };

            btnCancel.Click += (s, e) =>
            {
                OnCancelClick?.Invoke(s, e);
            };

        }


    }
}

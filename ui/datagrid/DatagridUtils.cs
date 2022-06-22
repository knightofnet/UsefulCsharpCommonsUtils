using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace UsefulCsharpCommonsUtils.ui.datagrid
{
    public static class DatagridUtils
    {

        public static DataGridComboBoxColumn TextToComboxboxColumn<T>(DataGridColumn column, List<T> datasourceCombobox, DataGridAutoGeneratingColumnEventArgs e, string displayMemberPathCombobox = null)
        {
            DataGridComboBoxColumn dc = new DataGridComboBoxColumn();
            dc.Header = column.Header;
            //dc.DisplayMemberPath = "Text";
           
            dc.ItemsSource = datasourceCombobox;

            if (displayMemberPathCombobox != null)
            {
                dc.SelectedValuePath = displayMemberPathCombobox;
            }
            dc.SelectedValueBinding = new Binding(e.PropertyName);
            e.Column = dc;

            return dc;
        }

        public static DataGridTemplateColumn OneClickDgCheckbox(DataGridAutoGeneratingColumnEventArgs e)
        {
            FrameworkElementFactory checkboxFactory = new FrameworkElementFactory(typeof(CheckBox));
            checkboxFactory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            checkboxFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            checkboxFactory.SetBinding(ToggleButton.IsCheckedProperty, new Binding(e.PropertyName) { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

            DataGridTemplateColumn newCol = new DataGridTemplateColumn
            {
                Header = e.Column.Header,
                CellTemplate = new DataTemplate { VisualTree = checkboxFactory },
                SortMemberPath = e.Column.SortMemberPath
            };

            e.Column = newCol;

            return newCol;
        }

    }
}

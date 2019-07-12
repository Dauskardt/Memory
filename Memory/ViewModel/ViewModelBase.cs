using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Memory.ViewModel
{
    public class ViewModelBase : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }
    }
    public class CloseThisWindowCommand : ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            //we can only close Windows
            return (parameter is Window);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (this.CanExecute(parameter))
            {
                ((Window)parameter).Close();
            }
        }

        #endregion

        private CloseThisWindowCommand()
        {

        }

        public static readonly ICommand Instance = new CloseThisWindowCommand();
    }

    [ValueConversion(typeof(SolidColorBrush), typeof(System.Windows.Media.Color))]
    public class SolidBrushToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return ColorConverter.ConvertFromString(value.ToString());

            //return System.Drawing.ColorTranslator.FromHtml(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            //if (value == null) return null;

            //return System.Drawing.ColorTranslator.ToHtml((System.Drawing.Color)value);
        }
    }
}

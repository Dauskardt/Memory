using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfSampleBasicChart;

namespace WpfSampleBasicChart.ChartControl
{
    public class CheckBoxClass: NotifierBase
    {
        private string m_Name = "";
        public string Name
        {
            get { return m_Name; }
            set
            {
                SetProperty(ref m_Name, value);
            }
        }

        private bool m_IsChecked = true;
        public bool IsChecked
        {
            get { return m_IsChecked; }
            set
            {
                SetProperty(ref m_IsChecked, value);
            }
        }

        private SolidColorBrush m_BackColor = Brushes.Red;
        public SolidColorBrush BackColor
        {
            get { return m_BackColor; }
            set
            {
                SetProperty(ref m_BackColor, value);
            }
        }
    }
}

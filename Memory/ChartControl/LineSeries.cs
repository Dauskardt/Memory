using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSampleBasicChart
{
    public class LineSeries:NotifierBase
    {
        private ObservableCollection<DataPoint> m_MyData = new ObservableCollection<DataPoint>();
        public ObservableCollection<DataPoint> MyData
        {
            get { return m_MyData; }
            set
            {
                SetProperty(ref m_MyData, value);
            }
        }

        private string m_Name = "";
        public string Name
        {
            get { return m_Name; }
            set
            {
                SetProperty(ref m_Name, value);
            }
        }
    }
}

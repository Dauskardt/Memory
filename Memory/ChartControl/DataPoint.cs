using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSampleBasicChart
{
    public class DataPoint:NotifierBase
    {
        private int m_Zuege = new int();
        public int Zuege
        {
            get { return m_Zuege; }
            set
            {
                SetProperty(ref m_Zuege, value);
            }
        }

        private double m_Value = new double();

        public double Value
        {
            get { return m_Value; }
            set
            {
                SetProperty(ref m_Value, value);
            }
        }
    }
}

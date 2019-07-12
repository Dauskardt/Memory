using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfSampleBasicChart;

namespace Memory.View.Dialoge
{
    /// <summary>
    /// Interaktionslogik für dlgHighScores.xaml
    /// </summary>
    public partial class dlgHighScores : Window
    {
        public dlgHighScores(string Path2LogFile)
        {
            this.Owner = Application.Current.MainWindow;

            InitializeComponent();

            FillView(Path2LogFile);
        }

        private void FillView(string path)
        {


                HighSoreEntry[] EntryList = readCSV(path);
            ListViewGames.ItemsSource = EntryList;

            ObservableCollection<LineSeries> mydata = new ObservableCollection<LineSeries>();
            WpfSampleBasicChart.LineSeries MySeries = new WpfSampleBasicChart.LineSeries();
           
            ChartControl.ItemsSource = mydata;

            int cnt = 0;
            int iMax = 0;

            if (EntryList.Length > 50)
            {
                cnt = EntryList.Length - 50;
                iMax = 50;
            }
            else
            {
                cnt = EntryList.Length-1;
                iMax = EntryList.Length-1;
            }

           // MySeries.MyData.Add(new WpfSampleBasicChart.DataPoint() { Zuege = 0, Value = 0 });
            MySeries.MyData.Clear();

            for (int i = iMax; i >= 0; i--)
            {
                MySeries.MyData.Add(new WpfSampleBasicChart.DataPoint() { Zuege = cnt, Value = EntryList[i].Züge });
                cnt++;
            }

            if(mydata.Count == 0)
            { 
                mydata.Add(MySeries);
            }

        }

        public HighSoreEntry[] readCSV(string filename)
        {
            string[] GameEntries = System.IO.File.ReadAllLines(filename);

            HighSoreEntry[] EntryList = new HighSoreEntry[GameEntries.Length-1];

            int cnt = 0;

            for (int i = GameEntries.Length-1; i > 0 ; i--)
            {
                string[] columns = GameEntries[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                HighSoreEntry hse = new HighSoreEntry(columns[0], columns[1], columns[2]);

                EntryList[cnt] = hse;

                cnt++;
            }

            return EntryList;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

    }

    public class HighSoreEntry
    {
        public string Spieler { get; set; }
        public DateTime Datum { get; set; }
        public int Züge { get; set; }

        public HighSoreEntry() { }
        public HighSoreEntry(string spieler, string datum, string zuege)
        {
            Spieler = spieler;
            Datum = DateTime.Parse(datum);
            Züge = Convert.ToInt32(zuege);
        }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Memory.Model
{
    class MemoryBoard: INotifyPropertyChanged
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

        private string[] PicturesField;

        public string[] Pictures
        {
            get
            {
                return PicturesField;
            }

            set
            {
                PicturesField = value;
                RaisePropertyChangedEvent(nameof(Pictures));
            }
        }

        private BitmapImage[,] CardsFieldField = new BitmapImage[5,6];

        public BitmapImage[,] CardsField
        {
            get
            {
                return CardsFieldField;
            }

            set
            {
                CardsFieldField = value;
                RaisePropertyChangedEvent(nameof(CardsField));
            }
        }

        private BitmapImage CardBackField;

        public BitmapImage CardBack
        {
            get
            {
                return CardBackField;
            }

            set
            {
                CardBackField = value;
                RaisePropertyChangedEvent(nameof(CardBack));
            }
        }

        private Image FirstSelectedImage { get; set; }
        private Image SecondSelectedImage { get; set; }

        public string PicturePath { get; set; }

        //public Canvas[,] GameBoard
        //{
        //    get
        //    {
        //        return GameBoardField;
        //    }

        //    set
        //    {
        //        GameBoardField = value;
        //        RaisePropertyChangedEvent(nameof(GameBoard));
        //    }
        //}

        //private Canvas[,] GameBoardField = new Canvas[5,6];

        public MemoryBoard()
        {

        }

        public bool LoadPictures(string PathToPicture)
        {
            PicturePath = PathToPicture;

            LoadCardBack();

            DirectoryInfo DInfo = new DirectoryInfo(PathToPicture);

            if (Directory.Exists(PathToPicture))
            {
                Pictures = Directory.GetFiles(PathToPicture, "*.jpg");

                if (Pictures.Length == 15)
                {
                    Array.Sort(Pictures);

                    int PicIndex = 0;

                    if (Pictures.Length % 5 == 0)
                    {
                        for (int y = 0; y < 5; y++)
                        {
                            for (int x = 0; x < 6; x++)
                            {
                                if (x % 2 == 0)
                                {
                                    var path = Path.Combine(Pictures[PicIndex]);
                                    var uri = new Uri(path);
                                    CardsField[y, x] = new BitmapImage(uri);
                                    PicIndex++;
                                }
                                else
                                {
                                    CardsField[y, x] = CardsField[y, x - 1];
                                }
                            }
                        }

                        return true;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Falsche Anzahl Bilder gefunden (15)", "Fehler aufgetreten", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                        return false;
                    }

                }
                else
                {
                    System.Windows.MessageBox.Show("Keine, oder falsche Anzahl Bilder gefunden (15)", "Fehler aufgetreten", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return false;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Das angegebene Verzeichnis existiert nicht", "Fehler aufgetreten", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return false;
            }
        }

        public void LoadCardBack()
        {
            CardBack = new BitmapImage(new Uri(@"/"
    + Assembly.GetExecutingAssembly().GetName().Name
    + ";component/"
    + "Resources/Back.jpg", UriKind.Relative));

            //if (File.Exists(PicturePath))
            //{
            //    var path = Path.Combine(PicturePath);
            //    var uri = new Uri(path);
            //    CardBack = new BitmapImage(uri);
            //}
        }

        public void Clear()
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    CardsField[y, x] = null;
                }
            }

        }

    }
}

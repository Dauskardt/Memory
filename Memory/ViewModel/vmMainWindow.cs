using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory.Model;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Media;
using Microsoft.Win32;

namespace Memory.ViewModel
{
    class vmMainWindow:ViewModelBase
    {
        #region Felder..

        string Path2LogFile { get { return System.IO.Path.Combine(Environment.CurrentDirectory, "GameLog.csv"); } }

        public string Titel { get { return System.Reflection.Assembly.GetExecutingAssembly().FullName; } }

        private System.Windows.Media.Effects.DropShadowEffect DSRasterField = new System.Windows.Media.Effects.DropShadowEffect();

        public DropShadowEffect DSRaster
        {
            get
            {
                return DSRasterField;
            }

            set
            {
                DSRasterField = value;
                RaisePropertyChangedEvent(nameof(DSRaster));
            }
        }

        private Model.MemoryBoard SpielField = new Model.MemoryBoard();

        public MemoryBoard Spiel
        {
            get
            {
                return SpielField;
            }

            set
            {
                SpielField = value;
                RaisePropertyChangedEvent(nameof(Spiel));
            }
        }

        private bool InputLockField = true;

        public bool InputLock
        {
            get
            {
                return InputLockField;
            }

            set
            {
                InputLockField = value;
                RaisePropertyChangedEvent(nameof(InputLock));
            }
        }

        private Canvas[,] GameBoardField = new Canvas[5, 6];

        public Canvas[,] GameBoard
        {
            get
            {
                return GameBoardField;
            }

            set
            {
                GameBoardField = value;
                RaisePropertyChangedEvent(nameof(GameBoard));
            }
        }

        private System.Windows.Size FieldSize { get; set; }

        private Canvas FirstSelected { get; set; }

        private bool SpielfeldEnabledField = true;

        public bool SpielfeldEnabled
        {
            get
            {
                return SpielfeldEnabledField;
            }

            set
            {
                SpielfeldEnabledField = value;
                RaisePropertyChangedEvent(nameof(SpielfeldEnabled));
            }
        }

        private bool ButtonsEnabledField = false;

        public bool ButtonsEnabled
        {
            get
            {
                return ButtonsEnabledField;
            }

            set
            {
                ButtonsEnabledField = value;
                RaisePropertyChangedEvent(nameof(ButtonsEnabled));
            }
        }

        private Visibility UIVisibilityField;

        public Visibility UIVisibility
        {
            get
            {
                return UIVisibilityField;
            }

            set
            {
                UIVisibilityField = value;
                RaisePropertyChangedEvent(nameof(UIVisibility));
            }
        }

        private bool UIanzeigenField = false;

        public bool UIanzeigen
        {
            get
            {
                return UIanzeigenField;
            }

            set
            {
                UIanzeigenField = value;
                if (value) { UIVisibility = Visibility.Collapsed; } else { UIVisibility = Visibility.Visible; }
                RaisePropertyChangedEvent(nameof(UIanzeigenField));
            }
        }

        private int ZuegeField;

        public int Zuege
        {
            get
            {
                return ZuegeField;
            }

            set
            {
                ZuegeField = value;
                RaisePropertyChangedEvent(nameof(Zuege));
            }
        }

        private int BilderGefundenField;

        public int BilderGefunden
        {
            get
            {
                return BilderGefundenField;
            }

            set
            {
                BilderGefundenField = value;
                RaisePropertyChangedEvent(nameof(BilderGefunden));
            }
        }

        private string BilderOrdnerField = System.IO.Path.Combine(Environment.CurrentDirectory, "Bilder\\Default");

        public string BilderOrdner
        {
            get
            {
                return BilderOrdnerField;
            }

            set
            {
                BilderOrdnerField = value;
                RaisePropertyChangedEvent(nameof(BilderOrdner));
            }
        }

        #endregion

        #region Actions..

        public MyCommand ACBilderLaden
        {
            get;
            set;
        }

        public void ACBilderLadenFunc(object parameter)
        {

            View.Dialoge.dlgBilder dlgPicSelector = new View.Dialoge.dlgBilder(System.IO.Path.Combine(Environment.CurrentDirectory, "Bilder"));

            if ((bool)dlgPicSelector.ShowDialog() == true)
            {
                BilderOrdner = dlgPicSelector.FolderPath;

                if (Spiel.LoadPictures(BilderOrdner))
                {
                    GameBoardClear();
                    SpielStarten();
                }

            }
        }

        public MyCommand ACSpielEnde
        {
            get;
            set;
        }

        public void ACSpielEndeFunc(object parameter)
        {
            if (MessageBox.Show("Programm beenden?", "Besutzerabfrage", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
        }

        public MyCommand ACHighScores
        {
            get;
            set;
        }

        public void ACHighScoresFunc(object parameter)
        {

            if (System.IO.File.Exists(Path2LogFile))
            {
                View.Dialoge.dlgHighScores dlgHighscores = new View.Dialoge.dlgHighScores(Path2LogFile);

                dlgHighscores.ShowDialog();
            }
            else
            {
                MessageBox.Show("Noch keine Daten vorhanden.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public MyCommand ACShuffle
        {
            get;
            set;
        }

        public void ACShuffleFunc(object parameter)
        {
            RandomExtension.Randomize<Canvas>(GameBoard);

            RaisePropertyChangedEvent(nameof(GameBoard));
        }

        public MyCommand ACTurnAll
        {
            get;
            set;
        }

        public void ACTurnAllFunc(object parameter)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    Image img = new Image();
                    img.Source = Spiel.CardBack;

                    img.Width = FieldSize.Width;
                    img.Height = FieldSize.Height;
                    //GameBoard[y, x].Children.Clear();

                    if (GameBoard[y, x].Children.Count > 1)
                    {
                        GameBoard[y, x].Children.RemoveAt(1);
                    }
                    else
                    {
                        GameBoard[y, x].Children.Add(img);
                    }
                    //GameBoard[y, x].Children.Insert(0,img);
                }
            }

            RaisePropertyChangedEvent(nameof(GameBoard));
        }

        #endregion

        public vmMainWindow()
        {
            InitControls();

            ACSpielEnde = new MyCommand();
            ACSpielEnde.CanExecuteFunc = obj => true;
            ACSpielEnde.ExecuteFunc = ACSpielEndeFunc;

            ACHighScores = new MyCommand();
            ACHighScores.CanExecuteFunc = obj => true;
            ACHighScores.ExecuteFunc = ACHighScoresFunc;

            ACShuffle = new MyCommand();
            ACShuffle.CanExecuteFunc = obj => true;
            ACShuffle.ExecuteFunc = ACShuffleFunc;

            ACTurnAll = new MyCommand();
            ACTurnAll.CanExecuteFunc = obj => true;
            ACTurnAll.ExecuteFunc = ACTurnAllFunc;

            ACBilderLaden = new MyCommand();
            ACBilderLaden.CanExecuteFunc = obj => true;
            ACBilderLaden.ExecuteFunc = ACBilderLadenFunc;

            DSRaster.BlurRadius = 7;
            DSRaster.Direction = 180;//230
            DSRaster.ShadowDepth = 3;//4
            DSRaster.Opacity = 0.4;//0.5
        }

        #region Ereignisse..

        private void GameBoard_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            FieldSize = e.NewSize;
            RaisePropertyChangedEvent(nameof(GameBoard));
        }

        //private void VmMainWindow_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    Canvas c = (Canvas)sender;
        //    if(c.Children.Count > 0)
        //    { 
        //    Image I = (Image)c.Children[0];
        //        if(I.Source != null)
        //        { 
        //            string Bild = System.IO.Path.GetFileNameWithoutExtension(I.Source.ToString());
        //            System.Diagnostics.Debug.Print(Bild);
        //            //throw new NotImplementedException();
        //        }
        //    }
        //}

        private void VmMainWindow_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SpielfeldEnabled = false;

            Canvas CanvasSel = (Canvas)sender;

            AuswahlSetzen(CanvasSel);

            SpielfeldEnabled = true;
        }

        #endregion

        #region Methoden..

        private void SpielStarten()
        {
            SpielfeldEnabled = false;

            Zuege = 0;
            BilderGefunden = 0;

            if (Spiel.LoadPictures(BilderOrdner))
            {
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        Image img = new Image();
                        img.Source = Spiel.CardsField[y, x];
                        img.Width = FieldSize.Width;
                        img.Height = FieldSize.Height;

                        GameBoard[y, x].Children.Clear();
                        GameBoard[y, x].Children.Add(img);
                        GameBoard[y, x].Tag = img.Source.ToString();
                        GameBoard[y, x].Visibility = Visibility.Visible;
                        GameBoard[y, x].Effect = DSRaster;
                        GameBoard[y, x].InvalidateVisual();
                    }
                }

                SpielfeldEnabled = true;
                ButtonsEnabled = true;
                RaisePropertyChangedEvent(nameof(GameBoard));
            }
        }

        private void InitControls()
        {

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    GameBoard[y, x] = new Canvas();
                    GameBoard[y, x].Name = "Pic" + y.ToString() + x.ToString();
                    GameBoard[y, x].Tag = x;
                    GameBoard[y, x].Margin = new System.Windows.Thickness(5);//6
                    GameBoard[y, x].Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
                    GameBoard[y, x].Effect = DSRaster;

                    GameBoard[y, x].PreviewMouseDown += VmMainWindow_PreviewMouseDown;
                    //GameBoard[y, x].MouseEnter += VmMainWindow_MouseEnter;
                    GameBoard[y, x].SizeChanged += GameBoard_SizeChanged;
                }
            }

            RaisePropertyChangedEvent(nameof(GameBoard));
        }

        private void GameBoardClear()
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    GameBoard[y, x].Visibility = Visibility.Visible;
                    GameBoard[y, x].Effect = DSRaster;
                    GameBoard[y, x].Children.Clear();
                    GameBoard[y, x].InvalidateVisual();
                }
            }
        }

        private void AuswahlSetzen(Canvas CanvasSel)
        {
            
            if (CanvasSel.Children.Count > 1)
            {
                Image SelectedImage = (Image)CanvasSel.Children[0];

                if (FirstSelected == null)
                {
                    FirstSelected = CanvasSel;
                    TurnSelectedCard(CanvasSel);
                    ButtonsEnabled = false;
                    System.Diagnostics.Debug.Print("Erstes Bild " + SelectedImage.Source.ToString());
                }
                else
                {
                    System.Diagnostics.Debug.Print("Zweites Bild " + SelectedImage.Source.ToString());

                    TurnSelectedCard(CanvasSel);

                    Image FirstSelImage = (Image)FirstSelected.Children[0];

                    if (FirstSelImage.Source.ToString() != SelectedImage.Source.ToString())
                    {
                        System.Diagnostics.Debug.Print("Falsches Bild " + SelectedImage.Source.ToString());

                        DoEvents();
                        Thread.Sleep(1000);

                        TurnSelectedCard(FirstSelected);
                        TurnSelectedCard(CanvasSel);

                        Zuege++;
                    }
                    else
                    {
                        Zuege++;
                        BilderGefunden++;
                        DoEvents();

                        BlurEffect BE = new BlurEffect();
                        BE.Radius = 0;

                        FirstSelected.Effect = BE;
                        CanvasSel.Effect = BE;

                        Thread.Sleep(500);

                        for (int i = 0; i < 15; i++)
                        {
                            BE.Radius = i;
                            DoEvents();
                            Thread.Sleep(6);
                        }

                        FirstSelected.Visibility = Visibility.Hidden;
                        CanvasSel.Visibility = Visibility.Hidden;

                        if (BilderGefunden == 15)
                        {
                            DoEvents(); Thread.Sleep(5); MessageBox.Show("Spielende nach " + Zuege + " Zügen.", "Memory");

                            WriteLog();

                            GameBoardClear();

                            Zuege = 0;
                            BilderGefunden = 0;
                        }

                        System.Diagnostics.Debug.Print("Bildpaar gefunden " + SelectedImage.Source.ToString());
                    }

                    FirstSelected = null;
                    DoEvents();
                }
            }
        }

        private void TurnSelectedCard(Canvas SelectedCard)
        {
            Image img = new Image();
            img.Source = Spiel.CardBack;
            img.Width = FieldSize.Width;
            img.Height = FieldSize.Height;

            if (SelectedCard.Children.Count > 1)
            {
                SelectedCard.Children.RemoveAt(1);
            }
            else
            {
                SelectedCard.Children.Add(img);

                //DoEvents();
                //TurnAnimation(img);
                //DoEvents();
            }
        }

        private void WriteLog()
        {
            try
            {
                string log = System.IO.Path.Combine(Environment.CurrentDirectory, "GameLog.csv");
                if (!System.IO.File.Exists(log))
                {
                    System.IO.File.AppendAllLines(log, new string[] { "Spieler;Datum;Zuege"});
                }
                System.IO.File.AppendAllLines(log, new string[] { Environment.UserName + ";" + DateTime.Now + ";" + Zuege});
            }
            catch (Exception){}

        }

        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }

        #endregion

    }

    public static class ArrayHelper
    {
        public static object FindInDimensions(this object[,] target,
          object searchTerm)
        {
            object result = null;
            var rowLowerLimit = target.GetLowerBound(0);
            var rowUpperLimit = target.GetUpperBound(0);

            var colLowerLimit = target.GetLowerBound(1);
            var colUpperLimit = target.GetUpperBound(1);

            for (int row = rowLowerLimit; row < rowUpperLimit; row++)
            {
                for (int col = colLowerLimit; col < colUpperLimit; col++)
                {
                    // you could do the search here...
                }
            }

            return result;
        }
    }
}

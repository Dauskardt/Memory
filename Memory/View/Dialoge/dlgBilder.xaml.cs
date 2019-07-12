using System;
using System.Collections.Generic;
using System.IO;
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

namespace Memory.View.Dialoge
{
    /// <summary>
    /// Interaktionslogik für dlgBilder.xaml
    /// </summary>
    public partial class dlgBilder : Window
    {
        public string FolderPath { get; set; }

        public dlgBilder(string path)
        {
            this.Owner = Application.Current.MainWindow;
            
            InitializeComponent();

            FolderPath = path;
            PopulateTreeView(FolderPath);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnAbbruch_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnExplorer_Click(object sender, RoutedEventArgs e)
        {
            if(Directory.Exists(FolderPath))
            System.Diagnostics.Process.Start(FolderPath);
        }

        private void PopulateTreeView(string Path)
        {
            TreeViewItem rootNode;

            DirectoryInfo info = new DirectoryInfo(FolderPath);

            if (info.Exists)
            {
                rootNode = new TreeViewItem();
                rootNode.Header = info.Name;
                rootNode.Tag = info;
                GetDirectories(info.GetDirectories(), rootNode);
                TreeViewDirectorys.Items.Add(rootNode);
            }
        }

        private void GetDirectories(DirectoryInfo[] subDirs,
            TreeViewItem nodeToAddTo)
        {
            TreeViewItem aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeViewItem();
                aNode.Header = subDir.Name;
                aNode.Tag = subDir;
                //aNode.ImageKey = "folder";
                subSubDirs = subDir.GetDirectories();
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                }

                nodeToAddTo.Items.Add(aNode);
            }
        }

        private void TreeViewDirectorys_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {


            WrapPanelFiles.Children.Clear();

            System.Windows.Media.Effects.DropShadowEffect ds = new System.Windows.Media.Effects.DropShadowEffect();
            ds.BlurRadius = 3;
            ds.ShadowDepth = 4;
            ds.Opacity = 0.5;
            ds.Direction = 240;

            TreeViewItem tvi = (TreeViewItem)e.NewValue;
            DirectoryInfo SelectedPath = (DirectoryInfo)tvi.Tag;
            FolderPath = SelectedPath.FullName;
            FileInfo[] files = SelectedPath.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);


            foreach (FileInfo file in files)
            {
                
                Image image = new Image();
                image.Tag = file.FullName;
                image.Margin = new Thickness(5);
                image.Effect = ds;
                image.PreviewMouseDown += Image_PreviewMouseDown;
                //image.Width = 50;
                //image.Height = 50;
                image.Stretch = Stretch.Uniform;
                image.Source = (new ImageSourceConverter()).ConvertFromString(file.FullName) as ImageSource;

                WrapPanelFiles.Children.Add(image);

                //ListViewItem lvi = new ListViewItem();
                //lvi.Content = image;
                //ListViewFiles.Items.Add(lvi);
            }

            //TreeNode newSelected = e.Node;

            //ListViewFiles.Items.Clear();
            //DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag;
            //ListViewItem.ListViewSubItem[] subItems;
            //ListViewItem item = null;

            //foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())
            //{
            //    item = new ListViewItem(dir.Name, 0);
            //    subItems = new ListViewItem.ListViewSubItem[]
            //        {new ListViewItem.ListViewSubItem(item, "Directory"),
            // new ListViewItem.ListViewSubItem(item,
            //    dir.LastAccessTime.ToShortDateString())};
            //    item.SubItems.AddRange(subItems);
            //    listView1.Items.Add(item);
            //}
            //foreach (FileInfo file in nodeDirInfo.GetFiles())
            //{
            //    item = new ListViewItem(file.Name, 1);
            //    subItems = new ListViewItem.ListViewSubItem[]
            //        { new ListViewItem.ListViewSubItem(item, "File"),
            // new ListViewItem.ListViewSubItem(item,
            //    file.LastAccessTime.ToShortDateString())};

            //    item.SubItems.AddRange(subItems);
            //    listView1.Items.Add(item);
            //}

            //ListViewFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            System.Diagnostics.Process.Start(img.Tag.ToString());
        }

        private void ListViewFiles_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}

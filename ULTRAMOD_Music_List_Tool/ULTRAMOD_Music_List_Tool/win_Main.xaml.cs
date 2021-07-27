using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

using MahApps.Metro.Controls;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using SimpleJSON;
using MP3MetaData;
using Microsoft.Win32;

namespace ULTRAMOD_Music_List_Tool
{
    /// <summary>
    /// Interaction logic for win_Main.xaml
    /// </summary>
    public partial class win_Main : MetroWindow
    {
        private string version = "1.0.0";

        /// <summary>
        /// Window Entry Point
        /// </summary>
        public win_Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Window Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void win_Main_Load(object sender, RoutedEventArgs e)
        {
            this.lbl_version.Content = "ULTRAMOD MusicList Tool " + version;

            //https://stackoverflow.com/questions/291522/wpf-how-can-you-add-a-new-menuitem-to-a-menu-at-runtime
            //https://stackoverflow.com/questions/43547647/how-to-make-right-click-button-context-menu-in-wpf

            var contextMenu = new ContextMenu();
            this.listview_musics.ContextMenu = contextMenu;

            this.listview_musics.MouseDoubleClick += new MouseButtonEventHandler(listview_musics_MouseDoubleClick);

            //contextMenu.Items.Add(new MenuItem
            //{
            //    Header = "Remove",
            //    Name="RemoveNutritionContextMenu",
            //    Click  = "RemoveMusic_OnClick"
            //});

            // Create the new menu item
            MenuItem item = new MenuItem();
            item.Header = "Edit";
            item.Click += new RoutedEventHandler(EditMusicInfo_OnClick);

            // Create the new menu item
            MenuItem item2 = new MenuItem();
            item2.Header = "Remove";
            item2.Click += new RoutedEventHandler(RemoveMusic_OnClick);

            contextMenu.Items.Add(item); // Add menu item as child to pre-defined menu item
            contextMenu.Items.Add(item2); // Add menu item as child to pre-defined menu item

            // Add columns
            var gridView = new GridView();
            this.listview_musics.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "FileName",
                Width = 108,
                DisplayMemberBinding = new Binding("FileName")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "MusicName",
                Width = 108,
                DisplayMemberBinding = new Binding("MusicName")
            });

            // Populate list
            //this.listview_musics.Items.Add(new MusicInfo { FileName = "test file", MusicName = "test name", Source_OFD = false });
        }

        private void lbl_version_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://ultramod.eu/");
        }

        /*private void txt_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txt_name.Text == "")
                lbl_playername.Visibility = Visibility.Hidden;
            else lbl_playername.Visibility = Visibility.Visible;
        }*/

        private void btn_import_files_Click(object sender, RoutedEventArgs e)
        {
            //https://www.wpf-tutorial.com/dialogs/the-openfiledialog/
            //https://www.wpf-tutorial.com/dialogs/the-messagebox/
            //https://stackoverflow.com/questions/938421/getting-the-applications-directory-from-a-wpf-application

            //////Multiple files
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Music files (*.mp3)|*.mp3|All files (*.*)|*.*";
            //openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string strPath in openFileDialog.FileNames)
                {
                    //MP3_MetaData_INFO music_info = MP3_MetaData_Reader.GetMP3MusicTag(filename);
                    //MessageBox.Show(music_info.Artist);
                    //MessageBox.Show(music_info.Title);

                    this.listview_musics.Items.Add(new MusicInfo { FileName = System.IO.Path.GetFileName(strPath), MusicName = "", Source_OFD = true });
                }
            }

            //////Single file
            ////OpenFileDialog openFileDialog = new OpenFileDialog();
            ////if (openFileDialog.ShowDialog() == true)
            ////{
            ////    MessageBox.Show(openFileDialog.FileName);
            ////}
        }

        private void btn_import_json_list_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            //openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            if (openFileDialog.ShowDialog() != true)
                return;

            if (this.listview_musics.Items.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Would you like to Add this List to Previous one or Replace that with this one?\n[YES : ADD(COMBINE) | NO : REPLACE | CANCEL : ABORT]", "ULTRAMOD MusicList Tool", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        break;
                    case MessageBoxResult.No:
                        this.listview_musics.Items.Clear();
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                }
            }

            //C# SimpleJson :https://wiki.unity3d.com/index.php/SimpleJSON
            //C# SimpleJson :https://github.com/Bunny83/SimpleJSON
            //C# Jsons Performance Compare :https://developpaper.com/unity-json-performance-comparison-litjson-newton-softjson-simplejson/
            //C# Json.NET :https://www.technical-recipes.com/2018/how-to-serialize-and-deserialize-objects-using-newtonsoft-json/
            //C# Json.NET :https://www.newtonsoft.com/json/help/html/SerializingJSON.htm

            string ListJSON = File.ReadAllText(openFileDialog.FileName);
            var RESPONSEJSON = JSON.Parse(ListJSON);

            for (int i = 0; string.IsNullOrEmpty(RESPONSEJSON["musics"][i]["filename"].Value) == false; i++)
            {
                this.listview_musics.Items.Add(new MusicInfo { FileName = RESPONSEJSON["musics"][i]["filename"].Value, MusicName = RESPONSEJSON["musics"][i]["name"].Value, Source_OFD = false });
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (this.listview_musics.Items.Count == 0)
            {
                MessageBoxResult result = MessageBox.Show("Export Empty List Really? Are you OK?", "ULTRAMOD MusicList Tool", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            string JSONSerialization = "";
            JSONSerialization += "{ \n   \"musics\":[";
            foreach (MusicInfo music in listview_musics.Items)
            {
                //MessageBox.Show("[DEBUG]" + anItem.FileName + " : " + anItem.MusicName);
                JSONSerialization += "\n      {\n         \"filename\":\"" + music.FileName + "\",\n         \"name\":\"" + music.MusicName + "\"\n      },";
            }

            //https://stackoverflow.com/questions/7901360/delete-last-char-of-string
            JSONSerialization = JSONSerialization.Remove(JSONSerialization.Length - 1);

            JSONSerialization += "\n   ]\n}";

            //MessageBox.Show("[DEBUG]" + JSONSerialization);

            //WPF SaveFileDialog :https://wpf-tutorial.com/dialogs/the-savefiledialog/

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog.FileName = "UltraMusics.json";
            saveFileDialog.Title = "Please Specify Directory you want to save MusicList File";

            //saveFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, JSONSerialization);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Exception Happened During Saving MusicList File : \n" + ex.ToString(), "ULTRAMOD MusicList Tool", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

                    //throw;
                }
            }

        }

        private void RemoveMusic_OnClick(object sender, RoutedEventArgs e)
        {
            listview_musics.Items.Remove(listview_musics.SelectedItem);  // remove the selected Item
        }

        private void EditMusicInfo_OnClick(object sender, RoutedEventArgs e)
        {
            if (listview_musics.SelectedIndex > -1)
            {
                MusicInfo s = (MusicInfo)listview_musics.Items[listview_musics.SelectedIndex];
                //MusicInfo s = listview_musics.Items.GetItemAt(listview_musics.SelectedIndex);
                //listview_musics.SelectedItem

                //MessageBox.Show("[DEBUG]" + s.FileName + ":" + s.MusicName );

                win_Details MusicDetailsForm = new win_Details(this, listview_musics.SelectedIndex, s.FileName, s.MusicName);
                MusicDetailsForm.Show();
                this.IsEnabled = false;
            }
        }

        private void listview_musics_MouseDoubleClick(object sender, MouseButtonEventArgs e) // functions same as contextMenu edit item
        {
            if (listview_musics.SelectedIndex > -1)
            {
                MusicInfo s = (MusicInfo)listview_musics.Items[listview_musics.SelectedIndex];

                win_Details MusicDetailsForm = new win_Details(this, listview_musics.SelectedIndex, s.FileName, s.MusicName);
                MusicDetailsForm.Show();
                this.IsEnabled = false;
            }
        }

        public void EditMusicListRowData(int index, string File_Name, string Music_Name)
        {
            this.listview_musics.Items[index] = new MusicInfo { FileName = File_Name, MusicName = Music_Name, Source_OFD = false };

            //.Add(new MusicInfo { FileName = System.IO.Path.GetFileName(strPath), MusicName = "", Source_OFD = true });
        }

    }

    public class MusicInfo
    {
        //public int ID { get; set; }

        public string FileName { get; set; }
        public string MusicName { get; set; }
        public bool Source_OFD { get; set; }
        //public string Source { get; set; }
    }
}
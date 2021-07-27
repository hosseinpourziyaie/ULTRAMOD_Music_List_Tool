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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace ULTRAMOD_Music_List_Tool
{
    /// <summary>
    /// Interaction logic for win_Details.xaml
    /// </summary>
    public partial class win_Details : MetroWindow
    {
        win_Main Parent;
        int TargetIndex;

        public win_Details(win_Main main, int MusicIndex, string FileName, string MusicName)
        {
            InitializeComponent();
            Parent = main;
            TargetIndex = MusicIndex;
            txt_musicfile.Text = FileName;
            txt_musicname.Text = MusicName;
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (txt_musicfile.Text != "")
            {
                Parent.EditMusicListRowData(TargetIndex, txt_musicfile.Text, txt_musicname.Text);
                Parent.IsEnabled = true;
                this.Close();
            }
            else
                MessageBox.Show("Music File Name Cannot be Empty", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btn_abort_Click(object sender, RoutedEventArgs e)
        {
            //Parent.IsEnabled = true;
            this.Close();
        }

        private void win_Details_Closed(object sender, EventArgs e)
        {
            Parent.IsEnabled = true;
        }
    }
}

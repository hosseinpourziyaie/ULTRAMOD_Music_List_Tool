using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MP3MetaData
{
    //Source: https://stackoverflow.com/questions/68283/view-edit-id3-data-for-mp3-files

    /*public class MP3_MetaData_INFO
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public string Comment { get; set; }
        public string Genre { get; set; }
    }

    class MusicID3Tag
    {
        public byte[] TAGID = new byte[3];      //  3
        public byte[] Title = new byte[30];     //  30
        public byte[] Artist = new byte[30];    //  30 
        public byte[] Album = new byte[30];     //  30 
        public byte[] Year = new byte[4];       //  4 
        public byte[] Comment = new byte[30];   //  30 
        public byte[] Genre = new byte[1];      //  1
    }*/


    public static class MP3_MetaData_Reader
    {
        /*public static MP3_MetaData_INFO GetMP3MusicTag(string file)
        {
            //https://social.msdn.microsoft.com/Forums/vstudio/en-US/3d2e9e48-8a02-419f-91e1-c61faf28bcf7/get-audio-files-info-ie-artist-name-title-album-art

            byte[] b = new byte[128];
            string sTitle = "", sSinger = "", sAlbum = "", sYear = "", sComm = "";

            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                fs.Seek(-128, SeekOrigin.End);
                fs.Read(b, 0, 128);
                String sFlag = System.Text.Encoding.Default.GetString(b, 0, 3);
                if (sFlag.CompareTo("TAG") == 0)
                {
                    //get   title   of   song;
                    sTitle = System.Text.Encoding.Default.GetString(b, 3, 30);
                    //get   singer;
                    sSinger = System.Text.Encoding.Default.GetString(b, 33, 30);
                    //get   album;
                    sAlbum = System.Text.Encoding.Default.GetString(b, 63, 30);
                    //get   Year   of   publish;
                    sYear = System.Text.Encoding.Default.GetString(b, 93, 4);
                    //get   Comment;
                    sComm = System.Text.Encoding.Default.GetString(b, 97, 30);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show( "Exception Happened During MP3 Tag Decode : \n" + ex.ToString(), "ULTRAMOD MusicList Tool", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                //throw;
            }

            return new MP3_MetaData_INFO
            {
                Title = sTitle,
                Artist = sSinger,
                Album = sAlbum,
                Year = sYear,
                Comment = sComm
            };
        }*/

        /*public static MP3_MetaData_INFO GetMP3MusicTag(string file)
        {
            string MusicTitle = "", MusicArtist = "", MusicAlbum = "", MusicYear = "", MusicComment = "", MusicGenre = "";

            System.Windows.MessageBox.Show(file, "[DEBUG]GetMP3MusicTag");
            using (FileStream fs = File.OpenRead(file))
            {
                if (fs.Length >= 128)
                {
                    System.Windows.MessageBox.Show(MusicTitle, "[DEBUG]Here 1");
                    MusicID3Tag tag = new MusicID3Tag();
                    fs.Seek(-128, SeekOrigin.End);
                    fs.Read(tag.TAGID, 0, tag.TAGID.Length);
                    fs.Read(tag.Title, 0, tag.Title.Length);
                    fs.Read(tag.Artist, 0, tag.Artist.Length);
                    fs.Read(tag.Album, 0, tag.Album.Length);
                    fs.Read(tag.Year, 0, tag.Year.Length);
                    fs.Read(tag.Comment, 0, tag.Comment.Length);
                    fs.Read(tag.Genre, 0, tag.Genre.Length);
                    string theTAGID = Encoding.Default.GetString(tag.TAGID);

                    if (theTAGID.Equals("TAG"))
                    {
                        MusicTitle = Encoding.Default.GetString(tag.Title);
                        MusicArtist = Encoding.Default.GetString(tag.Artist);
                        System.Windows.MessageBox.Show(MusicTitle, "[DEBUG]MusicTitle");
                        System.Windows.MessageBox.Show(MusicArtist, "[DEBUG]MusicArtist");
                        MusicAlbum = Encoding.Default.GetString(tag.Album);
                        MusicYear = Encoding.Default.GetString(tag.Year);
                        MusicComment = Encoding.Default.GetString(tag.Comment);
                        MusicGenre = Encoding.Default.GetString(tag.Genre);
                    }
                }
            }

            return new MP3_MetaData_INFO
            {
                Title = MusicTitle,
                Artist = MusicArtist,
                Album = MusicAlbum,
                Year = MusicYear,
                Comment = MusicComment,
                Genre = MusicGenre
            };
        }*/

    }//END of MP3_MetaData_Reader Class

}//END of ULTRAMOD_Music_List_Tool.MP3 namespace

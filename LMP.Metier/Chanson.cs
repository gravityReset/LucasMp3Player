using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using File = TagLib.Aac.File;

namespace LMP.Metier
{
    public class Chanson
    {
        public string Titre { get; set; }
        public string Artiste { get; set; }
        public string Album { get; set; }
        public int NbLecture { get; set; }
        public int NbLectureForce { get; set; }
        public string Path { get; set; }

        public int NumInAlbum { get; set; }
        public uint Annee { get; set; }
        public Chanson()
        { }

        public Chanson(string path)
        {
            Path = path;
            if (System.IO.File.Exists(Path))
            {
                try
                {

                    TagLib.File f = TagLib.File.Create(Path);
                    if (f.Tag.IsEmpty)
                        throw new Exception("Badass exeption");
                    Album = f.Tag.Album;
                    Titre = f.Tag.Title;
                    Annee = f.Tag.Year;
                    NumInAlbum = (int)f.Tag.Track;
                    Artiste = f.Tag.Performers[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    try
                    {
                        Titre = System.IO.Path.GetFileNameWithoutExtension(Path);
                        int conv;
                        string[] name = Titre.Split(' ', '-', '_');
                        if (name.Any())
                        {
                            int.TryParse(name[0], out conv);
                            NumInAlbum = conv;
                            Titre = Titre.Substring(name.Length);
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }

            }
        }

    }
}

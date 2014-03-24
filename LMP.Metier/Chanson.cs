using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TagLib.Aac;

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

        public uint Annee { get; set; }
        public Chanson()
        { }

        public Chanson(string path)
        {
            Path = path;
            TagLib.File f = new File(Path);
            Artiste = f.Tag.FirstAlbumArtist;
            Album = f.Tag.Album;
            Titre = f.Tag.Title;
            Annee = f.Tag.Year;
        }

    }
}

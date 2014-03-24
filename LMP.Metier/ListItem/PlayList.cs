using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace LMP.Metier
{
    public class PlayList:ListElement
    {
        [XmlIgnore]
        private static Geometry ImageGeometry
        { get { return Geometry.Parse("M41.276299,25.435L24.05225,26.817782 24.05225,32.289216 24.05225,33.369702 24.05225,40.485615C22.795097,39.916523 21.095961,40.07542 19.641016,41.036409 17.579193,42.394491 16.790823,44.834463 17.879282,46.486742 18.967842,48.137821 21.521145,48.37612 23.582968,47.016735 25.3556,45.84885 26.18247,43.880074 25.686989,42.300793L25.686989,33.205705 39.641563,31.793122 39.641563,39.635326C38.372009,38.985634 36.596676,39.119633 35.083733,40.114521 33.023212,41.475103 32.23484,43.914074 33.322701,45.567553 34.41256,47.219933 36.965862,47.45693 39.025684,46.098846 40.586227,45.070259 41.415596,43.421879 41.254799,41.964798 41.2677,41.895798 41.276299,41.8242 41.276299,41.7512L41.276299,31.629025 41.277,31.629025z M0,21.806999L64,21.806999 64,51.073999 0,51.073999z M0,0L25.7813,0 25.7813,6.5912857 64,6.5912857 64,14.572 0,14.572 0,7.8972406 0,6.5912857z"); } }

        
        public PlayList(string title)
            : base(title, ImageGeometry)
        {
            
        }

        public void AddSong(Chanson chanson)
        {
            Chansons.Add(chanson);
        }

        public void RemoveSong(Chanson chanson)
        {
            Chansons.Remove(chanson);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;
using LMP.Metier.Annotations;

namespace LMP.Metier.ListItem
{
    [XmlInclude(typeof(AllMusic))]
    [XmlInclude(typeof(PlayList))]
    public class ListElement:INotifyPropertyChanged
    {
        private Geometry _image;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value;OnPropertyChanged(); }
        }

        public ObservableCollection<Chanson> Chansons { get; set; } 

        [XmlIgnore]
        public Geometry Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public ListElement()
        {
            this._image = null;
            this._title = string.Empty;
            Chansons = new ObservableCollection<Chanson>();
        }

        
        public ListElement(string title, Geometry image)
        {
            this._image = image;
            this._title = title;
            Chansons = new ObservableCollection<Chanson>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

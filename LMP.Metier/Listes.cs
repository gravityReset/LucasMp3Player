using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LMP.Metier.ListItem;

namespace LMP.Metier
{
    public static class Listes
    {
        private const string menuFile = "menu.xml";
        public static ObservableCollection<ListElement> MenuList { get; set; }

        private static bool isGenerate = false;

        public static void Generate()
        {
            if (!isGenerate)
            {
                if (File.Exists(menuFile))
                {
                    XmlSerializer xs = new XmlSerializer(typeof (ObservableCollection<ListElement>));
                    using (StreamReader rd = new StreamReader(menuFile))
                    {
                        MenuList = xs.Deserialize(rd) as ObservableCollection<ListElement>;
                    }
                }
                else
                {
                    MenuList = new ObservableCollection<ListElement>
                    {
                        new AllMusic(),
                        new PlayList("Play Liste vide")
                    };
                    
                }
                isGenerate = true;
            }

        }

        public static void Save()
        {
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<ListElement>));
            using (StreamWriter wr = new StreamWriter(menuFile))
            {
                xs.Serialize(wr, MenuList);
            }
        }
    }
}

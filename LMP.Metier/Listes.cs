﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;
using LMP.Metier.Annotations;
using LMP.Metier.ListItem;

namespace LMP.Metier
{
    public class Listes
    {
        private const string menuFile = "menu.xml";

        public static ObservableCollection<ListElement> MenuList { get; set; }

        public static ObservableCollection<Chanson> ActualList { get; set; }


        private static bool isGenerate = false;

        public static void Generate()
        {
            ActualList = new ObservableCollection<Chanson>();
            if (!isGenerate)
            {
                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<ListElement>));
                    using (StreamReader rd = new StreamReader(menuFile))
                    {
                        MenuList = xs.Deserialize(rd) as ObservableCollection<ListElement>;
                    }
                }
                catch (Exception)
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

        public static bool Save()
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<ListElement>));
                using (StreamWriter wr = new StreamWriter(menuFile))
                {
                    xs.Serialize(wr, MenuList);
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return false;
        }

    }
}

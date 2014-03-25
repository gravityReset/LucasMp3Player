using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LMP.Metier;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using MessageBox = System.Windows.MessageBox;

namespace LMP.Vue
{
    /// <summary>
    /// Logique d'interaction pour Settings.xaml
    /// </summary>
    public partial class Settings : MetroWindow
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_OnClosing(object sender, CancelEventArgs e)
        {

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            string filter = "*.mp3";

            try
            {
                var fileList = Directory.GetFiles(PathTextBox.Text, filter);
                foreach (var f in fileList)
                    if (Listes.MenuList.First().Chansons.Count(c => c.Path == f) == 0)
                        Listes.MenuList.First().Chansons.Add(new Chanson(f));
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ButtonParcourir_OnClick(object sender, RoutedEventArgs e)
        {

            string selectedPath;
            var openFileDialog1 = new FolderBrowserDialog();

            openFileDialog1.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            var showDialog = openFileDialog1.ShowDialog();
            if (showDialog == System.Windows.Forms.DialogResult.OK)
            {
                selectedPath = openFileDialog1.SelectedPath;
                PathTextBox.Text = selectedPath;
            }
        }
    }
}

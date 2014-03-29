using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using LMP.Metier;
using LMP.Metier.ListItem;
using LMP.Vue.Annotations;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace LMP.Vue
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        #region propriété et attribut
        DispatcherTimer timer;
        private Chanson _currentTrack;

        public Chanson CurrentTrack
        {
            get { return _currentTrack; }
            set
            {
                _currentTrack = value;
                OnPropertyChanged();
            }
        }
        bool isDragging;

        #endregion

        #region main window
        public MainWindow()
        {
            Listes.Generate();
            CurrentTrack = null;

            isDragging = false;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Tick se dispara cada segundo.
            timer.Tick += new EventHandler(timer_Tick);

            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Listes.MenuList;
            ActualLectureListBox.DataContext = this;
            MediaGrid.DataContext = this;
            
            LbxList.SelectedIndex = 0;

        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Listes.Save();
        }

        #endregion

        #region settings
        /// <summary>
        /// on ouvre la fenetre settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_OnClick(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            var settings = new Settings { Owner = this };
            settings.Show();
            settings.Closing += settings_Closing;
        }

        /// <summary>
        /// si la fenettre settings se ferme on remet la fenetre principale accessible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.IsEnabled = true;
        }

        #endregion

        #region player
        private void timer_Tick(object sender, EventArgs e)
        {
            if (!isDragging)
            {
                TimerSlider.Value = ME_musicPlayer.Position.TotalSeconds;
            }
        }

        private void ME_musicPlayer_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            Suivant();
        }

        private void ME_musicPlayer_OnMediaOpened(object sender, RoutedEventArgs e)
        {
            if (ME_musicPlayer.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = ME_musicPlayer.NaturalDuration.TimeSpan;
                TimerSlider.Maximum = ts.TotalSeconds;
                TimerSlider.SmallChange = 1;
            }
            timer.Start();
        }

        #endregion

        #region slider action
        private void SliderTimeLine_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isDragging = true;
        }

        private void SliderTimeLine_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isDragging = false;
            ME_musicPlayer.Position =
                TimeSpan.FromSeconds(TimerSlider.Value);// La posición del MediaElement se actualiza para que coincida con el progreso del sliderTimeLine.
        }

        private void SliderTimeLine_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ME_musicPlayer.Position =
                TimeSpan.FromSeconds(TimerSlider.Value);
        }

        #endregion

        #region click bouton
        //bouton player
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            ME_musicPlayer.Pause();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (Listes.ActualList.Count == 0 || CurrentTrack != null)
            {
                if (LbxList.ItemsSource != null)
                {
                    foreach (var m in MusicListView.ItemsSource as IEnumerable<Chanson>)
                        Listes.ActualList.Add(m);

                    CurrentTrack = MusicListView.SelectedItem as Chanson;
                    PlayTrack(CurrentTrack.Path);
                }
            }
            else
                PlayTrack();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            ME_musicPlayer.Stop();
            TimerSlider.Value = 0;
            CurrentTrack = null;
            Listes.ActualList.Clear();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Suivant();
        }

        private void btnPrecedent_Click(object sender, RoutedEventArgs e)
        {
            Precedent();
        }

        //bouton liste
        private void bntSaveAsPlayListe_OnClick(object sender, RoutedEventArgs e)
        {
            var elem = LbxList.SelectedItem as ListElement;
            AddNewPlayListe(elem.Chansons);
        }

        private void bntSaveAsPlayListeActual_OnClick(object sender, RoutedEventArgs e)
        {
            AddNewPlayListe(Listes.ActualList);
        }

        private async void bntRenamePlayListe_OnClick(object sender, RoutedEventArgs e)
        {
            var list = LbxList.SelectedItem as PlayList;
            if (list != null)
            {
                var result = await this.ShowInputAsync(string.Format("Renommer la play liste {0}", list.Title), "Nouveau nom:");

                if (result != null)
                    list.Title = result;
            }
        }

        private void btnDeletePlayListe_OnClick(object sender, RoutedEventArgs e)
        {
            Listes.MenuList.RemoveAt(LbxList.SelectedIndex);
        }

        private void btnAddList_OnClick(object sender, RoutedEventArgs e)
        {
            AddNewPlayListe();
        }

        #endregion

        #region double click
       
        private void DoubleClickMusic(object sender, RoutedEventArgs e)
        {
            var item = sender as ListBoxItem;
            var track = item.DataContext as Chanson;
            AddMusicToActualAndPlay(track);
        }

        private void DoubleClickPlayList(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            var playList = item.Content as ListElement;
            PlayPlayListe(playList);
        }

        #endregion

        #region click droit/Menu contextuelle

        private void ContextMenuClickMusic(object sender, RoutedEventArgs e)
        {
            AddMusicToActualAndPlay(MusicListView.SelectedItem as Chanson);
        }
        private void MenuItemDeleteSong_OnClick(object sender, RoutedEventArgs e)
        {
            Listes.MenuList[LbxList.SelectedIndex].Chansons.Remove(MusicListView.SelectedItem as Chanson);
        }
        private void ContextMenuPlayPListeClick(object sender, RoutedEventArgs e)
        {
            PlayPlayListe(LbxList.SelectedItem as ListElement);
        }
        #endregion

        #region action sur music
        private void Suivant()
        {
            int index;
            if (Listes.ActualList.Count > (index = (Listes.ActualList.IndexOf(CurrentTrack) + 1)))
            {
                CurrentTrack = Listes.ActualList[index];
                PlayTrack(CurrentTrack.Path);
            }
            else if (BoucleCheckBox.IsChecked != null && (Listes.ActualList.Count > 0 && (bool)BoucleCheckBox.IsChecked))
            {
                CurrentTrack = Listes.ActualList[0];
                PlayTrack(CurrentTrack.Path);
            }
        }

        private void Precedent()
        {
            int index;
            if ((index = (Listes.ActualList.IndexOf(CurrentTrack) - 1)) >= 0)
            {
                CurrentTrack = Listes.ActualList[index];
                PlayTrack(CurrentTrack.Path);
            }
            else if (BoucleCheckBox.IsChecked != null && (bool)BoucleCheckBox.IsChecked)
            {
                CurrentTrack = Listes.ActualList.Last();
                PlayTrack(CurrentTrack.Path);
            }
        }

        private void PlayPlayListe(ListElement playList)
        {
            Listes.ActualList.Clear();

            foreach (Chanson chanson in playList.Chansons)
                Listes.ActualList.Add(chanson);

            if (Listes.ActualList.Count != 0)
            {
                CurrentTrack = Listes.ActualList.First();
                PlayTrack(CurrentTrack.Path);
            }
        }

        private void AddMusicToActualAndPlay(Chanson track)
        {
            PlayTrack(track.Path);

            if (!Listes.ActualList.Contains(track))
                Listes.ActualList.Add(track);
            CurrentTrack = track;
        }

        private void PlayTrack(string path = "")
        {
            if (path != "")
            {
                if (File.Exists(path))
                {
                    ME_musicPlayer.Source = new Uri(path);
                    TimerSlider.Value = 0;
                    ME_musicPlayer.Play();
                }
                else //si le fichier existe pas ou plus on le supprime
                {
                    if (LbxList.SelectedIndex >= 0)
                    {
                        //on supprime la liste et suivnt précédent pour verifier le current track
                        var l = LbxList.SelectedItem as ListElement;
                        l.Chansons.Remove(CurrentTrack);

                    }
                    Listes.ActualList.Remove(CurrentTrack);
                    Precedent();
                    Suivant();
                }
            }
            else
                ME_musicPlayer.Play();

        }

        #endregion

        #region action sur listes

        private void AddNewPlayListe()
        {
            AddNewPlayListe(new List<Chanson>());
        }

        private async void AddNewPlayListe(IEnumerable<Chanson> chansons)
        {
            var playListe = new PlayList();
            playListe.Chansons = new ObservableCollection<Chanson>();
            foreach (var chanson in chansons)
                playListe.Chansons.Add(chanson);
            var result = await this.ShowInputAsync("Nommer votre liste", "Nom de la nouvelle play liste :");

            if (result != null) //user pressed cancel
                playListe.Title = result;
            else
            {
                int i = 1;
                do
                {
                    playListe.Title = string.Format("Nouvelle liste {0}", i);
                    i++;
                } while (Listes.MenuList.Count(l => l.Title == playListe.Title) != 0);
            }

            Listes.MenuList.Add(playListe);
        }

        private void MenuItemRemoveFromPlay_OnClick(object sender, RoutedEventArgs e)
        {
            var menu = sender as MenuItem;
            var value = menu.DataContext as Chanson;
            if (value != null)
            {
                if (value == CurrentTrack)
                    Suivant();

                Listes.ActualList.Remove(value);
                OnPropertyChanged("CurrentTrack");
            }
        }

        #endregion

        #region propertychange interface
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private void MenuItemAjoutAlaFinClick(object sender, RoutedEventArgs e)
        {
            var song = MusicListView.SelectedItem as Chanson;

            Listes.ActualList.Add(song);
        }
    }
}

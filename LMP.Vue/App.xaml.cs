using System.Windows;
using System.Windows.Controls;
using LMP.Metier;

namespace LMP.Vue
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            ToolTipTimeActualisation();
        }

        private static void ToolTipTimeActualisation()
        {
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof (UIElement),
                new FrameworkPropertyMetadata(UserSettings.Default.DureeMusiquePreview));
        }
    }
}

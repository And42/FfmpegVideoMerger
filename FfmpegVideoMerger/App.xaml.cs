using System.Windows;
using AdonisUI;

namespace FfmpegVideoMerger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            
            ResourceLocator.SetColorScheme(Current.Resources, ResourceLocator.LightColorScheme);
        }
    }
}

using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Presence.Desktop.ViewModels;
using ReactiveUI;

namespace Zurnal_Vizual
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);

        }
    }
}
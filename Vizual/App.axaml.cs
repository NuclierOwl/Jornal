using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using domain.UseCase;
using Microsoft.Extensions.DependencyInjection;
using Zurnal_Vizual.DI;
using Zurnal_Vizual.ViewModels;
using Zurnal_Vizual.Views;

namespace Zurnal_Vizual
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCommonService();

            serviceCollection.AddSingleton<GroupUseCase>();
            serviceCollection.AddSingleton<UserUseCase>();

            var services = serviceCollection.BuildServiceProvider();
            var mainViewModel = services.GetRequiredService<MainWindowViewModel>();

            var groupUseCase = services.GetRequiredService<GroupUseCase>();
            var userUseCase = services.GetRequiredService<UserUseCase>();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow(groupUseCase, userUseCase)
                {
                    DataContext = mainViewModel,
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}

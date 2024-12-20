using Avalonia.Controls;
using Avalonia.Interactivity;
using domain.UseCase;
using Zurnal_Vizual.ViewModels;

namespace Zurnal_Vizual.Views
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel _viewModel;
        public MainWindow(GroupUseCase groupUseCase, UserUseCase userUseCase)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(groupUseCase, userUseCase);
        }

        public void OnDeleteUserClick(object sender, RoutedEventArgs e) => _viewModel.OnDeleteUserClick();

        public void OnEditUserClick(object sender, RoutedEventArgs e) => _viewModel.OnEditUserClick();

    }

}
using domain.UseCase;

namespace Presence.Desktop.ViewModels;

public class MainWindowViewModel: ViewModelBase
{
    private readonly GroupUseCase _groupService;
    public MainWindowViewModel(GroupUseCase groupService)
    {
        _groupService = groupService;
    }

}
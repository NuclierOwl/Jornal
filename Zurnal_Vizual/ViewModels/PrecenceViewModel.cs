using ReactiveUI;

namespace Presence.Desktop.ViewModels;

public class PresenceViewModel: ViewModelBase, IRoutableViewModel
{
    public string? UrlPathSegment { get; }
    public IScreen HostScreen { get; }
}

 
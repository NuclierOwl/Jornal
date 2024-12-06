using data.RemoteData.RemoteDataBase.DAO;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace Zurnal_Vizual.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private ObservableCollection<GroupDao> _groups;
        private GroupDao _selectedGroup;
        private ObservableCollection<UserDao> _students;
        private string _selectedSortOption;

        public ObservableCollection<GroupDao> Groups
        {
            get => _groups;
            set => this.RaiseAndSetIfChanged(ref _groups, value);
        }

        public GroupDao SelectedGroup
        {
            get => _selectedGroup;
            set => this.RaiseAndSetIfChanged(ref _selectedGroup, value);
        }

        public ObservableCollection<UserDao> Students
        {
            get => _students;
            set => this.RaiseAndSetIfChanged(ref _students, value);
        }

        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set => this.RaiseAndSetIfChanged(ref _selectedSortOption, value);
        }

        public ReactiveCommand<Unit, Unit> DeleteAllStudentsCommand { get; }
        public ReactiveCommand<Unit, Unit> ImportStudentsCommand { get; }

        public MainWindowViewModel()
        {
            DeleteAllStudentsCommand = ReactiveCommand.Create(DeleteAllStudents);
            ImportStudentsCommand = ReactiveCommand.Create(ImportStudents);
            Groups = new ObservableCollection<GroupDao>();
            Students = new ObservableCollection<UserDao>();
        }

        private void DeleteAllStudents() 
        { 
        
        }
        private void ImportStudents() 
        { 
        
        }
    }
}
using data.RemoteData.RemoteDataBase.DAO;
using domain.UseCase;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace Presence.Desktop.ViewModels
{
    public class GroupViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly List<GroupDao> _GroupDaosDataSource = new List<GroupDao>();
        private ObservableCollection<GroupDao> _groups;
        public ObservableCollection<GroupDao> Groups => _groups;

        public GroupDao? SelectedGroupItem { 
            get => _selectedGroupItem; 
            set => this.RaiseAndSetIfChanged(ref _selectedGroupItem, value); }

        private GroupDao? _selectedGroupItem;


        private GroupUseCase _groupUseCase;
        public ObservableCollection<UserDao> Users { get => _users;}
        public ObservableCollection<UserDao> _users;
        public GroupViewModel(GroupUseCase groupUseCase)
        {
            _groupUseCase = groupUseCase;
            _users = new ObservableCollection<UserDao>();
            RefreshGroups();
            this.WhenAnyValue(vm => vm.SelectedGroupItem)
                .Subscribe(_ =>
                {   RefreshGroups();
                    SetUsers();
                });

        }

        private void SetUsers()
        {
            if(SelectedGroupItem == null) return;
            Users.Clear();
            var group = _groups.First(it => it.Id == SelectedGroupItem.Id);
            if(group.Users == null) return;
            foreach (var item in group.Users)
            {
                Users.Add(item);
            }
        }

        private void RefreshGroups()
        {
            _GroupDaosDataSource.Clear();
            foreach (var item in _groupUseCase.GetGroupsWithStudents())
            {
                GroupDao GroupDao = new GroupDao
                {
                    Id = item.Id,
                    Name = item.Name,
                    Users = item.Users?.Select(user => new UserDao
                        {
                            FIO = user.FIO,
                            Guid = user.Guid,
                            Group = new GroupDao { Id = item.Id, Name = item.Name }
                        }
                    ).ToList()
                };
                _GroupDaosDataSource.Add(GroupDao);
            }
            _groups = new ObservableCollection<GroupDao>(_GroupDaosDataSource);
        }
        public string? UrlPathSegment { get; }
        public IScreen HostScreen { get; }
    }
}


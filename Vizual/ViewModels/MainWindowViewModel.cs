using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using System.Reactive;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using data.RemoteData.RemoteDataBase.DAO;
using domain.UseCase;
using CsvHelper;
using CsvHelper.Configuration;

namespace Zurnal_Vizual.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public readonly GroupUseCase _groupUseCase;
        public readonly UserUseCase _userUseCase;
        public ObservableCollection<UserDao> _users;
        public ObservableCollection<UserDao> Users => _users;

        public ObservableCollection<UserDao> SelectedUsers { get; set; } = new ObservableCollection<UserDao>();

        public List<GroupDao> GroupDaosDataSource = new List<GroupDao>();
        public ObservableCollection<GroupDao> _groups;
        public ObservableCollection<GroupDao> Groups => _groups;

        public GroupDao? _selectedGroupItem;
        public GroupDao? SelectedGroupItem
        {
            get => _selectedGroupItem;
            set => this.RaiseAndSetIfChanged(ref _selectedGroupItem, value);
        }

        public List<string> SortOptions { get; } = new List<string> { "По фамилии", "По убыванию" };

        public string _selectedSortOption;
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set => this.RaiseAndSetIfChanged(ref _selectedSortOption, value);
        }

        public bool CanDelete => SelectedUsers?.Count > 0;
        public bool CanEdit => SelectedUsers?.Count == 1;

        public ReactiveCommand<Unit, Unit> OnDeleteUserClicks { get; }
        public ReactiveCommand<Unit, Unit> EditUserCommand { get; }
        public ICommand RemoveAllStudentsCommand { get; }
        public ICommand AddStudentCommand { get; }

        public MainWindowViewModel(GroupUseCase groupUseCase, UserUseCase userUseCase)
        {
            OnDeleteUserClicks = ReactiveCommand.Create(OnDeleteUserClick, this.WhenAnyValue(vm => vm.CanDelete));
            EditUserCommand = ReactiveCommand.Create(OnEditUserClick, this.WhenAnyValue(vm => vm.CanEdit));
            _groupUseCase = groupUseCase;
            _userUseCase = userUseCase;

            RefreshGroups();

            _groups = new ObservableCollection<GroupDao>(GroupDaosDataSource);
            _users = new ObservableCollection<UserDao>();

            this.WhenAnyValue(vm => vm.SelectedGroupItem)
                .Subscribe(_ =>
                 {
                     RefreshGroups();
                     SetUsers();
                 });

            this.WhenAnyValue(vm => vm.SelectedGroupItem)
                .Subscribe(vm => SetUsers());

            this.WhenAnyValue(vm => vm.SelectedSortOption)
                .Subscribe(_ => SortUsers());

            RemoveAllStudentsCommand = ReactiveCommand.Create(RemoveAllStudents);
            AddStudentCommand = ReactiveCommand.Create(AddStudent);

            SelectedUsers.CollectionChanged += (s, e) =>
            {
                this.RaisePropertyChanged(nameof(CanDelete));
                this.RaisePropertyChanged(nameof(CanEdit));
            };
        }

        public void SetUsers()
        {
            if (SelectedGroupItem?.Users == null || !SelectedGroupItem.Users.Any())
            {
                Users.Clear();
                return;
            }

            Users.Clear();
            foreach (var item in SelectedGroupItem.Users)
            {
                Users.Add(item);
            }

            SortUsers();
        }

        public void SortUsers()
        {
            if (SelectedGroupItem?.Users == null) return;

            var sortedUsers = SelectedGroupItem.Users.ToList();

            switch (SelectedSortOption)
            {
                case "По фамилии":
                    sortedUsers = sortedUsers.OrderBy(u => u.FIO).ToList();
                    break;
                case "По убыванию":
                    sortedUsers = sortedUsers.OrderByDescending(u => u.FIO).ToList();
                    break;
            }

            Users.Clear();
            foreach (var item in sortedUsers)
            {
                Users.Add(item);
            }
        }

        public void RemoveAllStudents()
        {
            if (SelectedGroupItem == null) return;

            _groupUseCase.RemoveAllStudentsFromGroup(SelectedGroupItem.Id);
            SelectedGroupItem.Users = new List<UserDao>();
            SetUsers();
        }

        public void AddStudent()
        {
            string csvFilePath = @"C:\Users\PC\source\Dopolnenia\Fails\csv.csv";

            List<UserDao> students;
            try
            {
                students = ReadStudentsFromCsv(csvFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении CSV: {ex.Message}");
                return;
            }

            if (SelectedGroupItem == null) return;

            foreach (var student in students)
            {
                _groupUseCase.AddStudentToGroup(SelectedGroupItem.Id, new UserDao
                {
                    GroupID = student.GroupID,
                    FIO = student.FIO
                });

                var newStudent = new UserDao
                {
                    GroupID = student.GroupID,
                    FIO = student.FIO,
                    Group = SelectedGroupItem
                };

                var updatedUsers = SelectedGroupItem.Users?.ToList() ?? new List<UserDao>();
                updatedUsers.Add(newStudent);
                SelectedGroupItem.Users = updatedUsers;
            }

            SetUsers();
        }

        public List<UserDao> ReadStudentsFromCsv(string filePath)
        {
            var students = new List<UserDao>();

            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ","
                }))
                {
                    var records = csv.GetRecords<StudentCsvModel>().ToList();
                    foreach (var record in records)
                    {
                        var student = new UserDao
                        {
                            FIO = record.Name
                        };
                        students.Add(student);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении CSV файла: {ex.Message}");
            }

            return students;
        }

        public void OnDeleteUserClick()
        {
            if (SelectedUsers.Count == 0 || SelectedGroupItem?.Users == null)
                return;

            foreach (var user in SelectedUsers.ToList())
            {
                _userUseCase.RemoveUserByGuid(user.Guid);

                var updatedUsers = SelectedGroupItem.Users.Where(u => u != user).ToList();
                SelectedGroupItem.Users = new List<UserDao>(updatedUsers);
            }

            SetUsers();

            SelectedUsers.Clear();

            this.RaisePropertyChanged(nameof(CanDelete));
            this.RaisePropertyChanged(nameof(CanEdit));
        }

        public void RefreshGroups()
        {
            GroupDaosDataSource.Clear();
            foreach (var item in _groupUseCase.GetAllGroups())
            {
                GroupDao groupPresenter = new GroupDao
                {
                    Id = item.Id,
                    Name = item.Name,
                    Users = item.Users?.Select(user => new UserDao
                    {
                        FIO = user.FIO,
                        Guid = user.Guid,
                        Group = new GroupDao { Id = item.Id, Name = item.Name }
                    }).ToList() ?? new List<UserDao>() // Обеспечиваем инициализацию
                };
                GroupDaosDataSource.Add(groupPresenter);
            }
            _groups = new ObservableCollection<GroupDao>(GroupDaosDataSource);
            if (SelectedGroupItem != null)
            {
                SetUsers();
            }
        }



        public async void OnEditUserClick()
        {
            var user = SelectedUsers.FirstOrDefault();
            if (user == null) return;

            var groups = _groupUseCase.GetAllGroups();
            if (groups == null || !groups.Any()) return;

            var editDialog = new EditUserDialog(user.Guid, user.FIO, user.Group?.Id ?? 0, groups); 

            var mainWindow = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
            if (mainWindow == null) return;

            var result = await editDialog.ShowDialog(mainWindow);

            if (result != (null, null))
            {
                var newName = result.Item1;
                var newGroup = result.Item2;

                user.FIO = newName;
                user.Group = newGroup;

                _userUseCase.UpdateUser(new UserDao
                {
                    Guid = user.Guid,
                    FIO = user.FIO,
                    GroupID = user.Group?.Id ?? 0
                });

                SetUsers();
                SelectedUsers.Clear();
                this.RaisePropertyChanged(nameof(CanEdit));
                this.RaisePropertyChanged(nameof(CanDelete));
            }
            RefreshGroups();
        }

        public void ImportStudents()
        {
            
        }



    }
}

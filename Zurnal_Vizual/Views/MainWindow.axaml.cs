using Avalonia.Controls;
using data.RemoteData.RemoteDataBase.DAO;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;

namespace Views.MainWindow
{
    public class MainViewModel : ReactiveObject
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
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedGroup, value);
                LoadStudents();
            }
        }

        public ObservableCollection<UserDao> Students
        {
            get => _students;
            set => this.RaiseAndSetIfChanged(ref _students, value);
        }

        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedSortOption, value);
                SortStudents();
            }
        }

        public ReactiveCommand<Unit, Unit> DeleteAllStudentsCommand { get; }
        public ReactiveCommand<Unit, Unit> ImportStudentsCommand { get; }
        private ObservableCollection<string> _sortOptions;
        public ObservableCollection<string> SortOptions
        {
            get => _sortOptions;
            set => this.RaiseAndSetIfChanged(ref _sortOptions, value);
        }


        public MainViewModel()
        {
            DeleteAllStudentsCommand = ReactiveCommand.Create(DeleteAllStudents);
            ImportStudentsCommand = ReactiveCommand.Create(ImportStudents);
            Groups = new ObservableCollection<GroupDao>();
            Students = new ObservableCollection<UserDao>();
            SortOptions = new ObservableCollection<string> { "По имени", "По дате" };
        }
        public void LoadStudents()
        {
            //реализовать загрузку студентов из Excel таблицы
        }

        public void SortStudents()
        {
            //реализовать сортеровку пользователей (студентов) по имени и Id
        }

        public void DeleteAllStudents()
        {
            //реализовать удоление студентов по имени или Id
        }

        public void ImportStudents()
        {
            //реализовать добавление студентов по средсвам 2 полей для ввода текста
        }
        public void ShowContextMenu(Window window)
        {
            var contextMenu = new ContextMenu();
            var deleteItem = new MenuItem { Header = "Удалить всех студентов" };
            deleteItem.Click += (sender, e) => DeleteAllStudentsCommand.Execute().Subscribe();
            contextMenu.Items.Add(deleteItem);

            var importItem = new MenuItem { Header = "Импортировать студентов" };
            importItem.Click += (sender, e) => ImportStudentsCommand.Execute().Subscribe();
            contextMenu.Items.Add(importItem);

            window.ContextMenu = contextMenu;
        }
    }
}

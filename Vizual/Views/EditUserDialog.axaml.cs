using Avalonia.Controls;
using Avalonia.Controls.Templates;
using data.RemoteData.RemoteDataBase.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zurnal_Vizual
{
    public partial class EditUserDialog : Window
    {
        public TextBox _nameTextBox;
        public ComboBox _groupComboBox;


        public EditUserDialog(Guid currentGuid, string currentName, int currentGroupId, List<data.RemoteData.RemoteDataBase.DAO.GroupDao> groups)
        {
            _nameTextBox = new TextBox { Text = currentName };

            _groupComboBox = new ComboBox
            {
                ItemsSource = groups, 
                SelectedItem = groups.FirstOrDefault(g => g.Id == currentGroupId),
                ItemTemplate = new FuncDataTemplate<data.RemoteData.RemoteDataBase.DAO.GroupDao>((group, _) =>
                    new TextBlock { Text = group.Name }) 
            };

            var confirmButton = new Button { Content = "OK" };
            confirmButton.Click += (sender, args) =>
            {
                var newFio = _nameTextBox.Text;
                var selectedGroup = (data.RemoteData.RemoteDataBase.DAO.GroupDao)_groupComboBox.SelectedItem;

                if (selectedGroup != null)
                {
                    var newGroupId = selectedGroup.Id; // Получаем ID выбранной группы

                    // Закрываем диалог
                    this.Close();
                }
            };

            Content = new StackPanel
            {
                Children = { _nameTextBox, _groupComboBox, confirmButton }
            };
        }



        // Асинхронный метод для получения результата от пользователя
        public async Task<(string, GroupDao)> ShowDialog(Window parent)
        {
            await base.ShowDialog(parent); // Передаем родительское окно

            var name = _nameTextBox.Text;
            var groupId = (GroupDao)_groupComboBox.SelectedItem;

            return (name, groupId);
        }

    }
}

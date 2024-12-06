using domain.UseCase;
using System.Text;

namespace ui
{
    public class GroupConsoleUI
    {
        private readonly GroupUseCase _groupUseCase;

        public GroupConsoleUI(GroupUseCase groupUseCase)
        {
            _groupUseCase = groupUseCase;
        }

        public void FindGroupById(int groupId)
        {
            try
            {
                var group = _groupUseCase.FindGroupById(groupId);
                Console.WriteLine($"ID группы: {group.Id} Название группы: {group.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        
        public void DisplayAllGroups()
        {
            Console.WriteLine("\n=== Список всех групп ===");
            StringBuilder groupOutput = new StringBuilder();

            foreach (var group in _groupUseCase.GetAllGroups())
            {
                groupOutput.AppendLine($"{group.Id}\t{group.Name}");
            }

            Console.WriteLine(groupOutput);
            Console.WriteLine("===========================\n");
        }

        
        public void AddGroup(string groupName)
        {
            try
            {
                ValidateGroupName(groupName); 
                _groupUseCase.AddGroup(groupName);
                Console.WriteLine($"\nГруппа {groupName} добавлена.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }

        public void RemoveGroup(string groupIdStr)
        {
            try
            {
                int groupId = int.Parse(groupIdStr);
                ValidateGroupId(groupId); 
                _groupUseCase.RemoveGroupById(groupId);
                Console.WriteLine($"Группа с ID: {groupId} удалена");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }


        public void UpdateGroupName(int groupId, string newGroupName)
        {
            var isUpdated = _groupUseCase.UpdateGroup(groupId, newGroupName);

            if (isUpdated)
            {
                Console.WriteLine($"\nНазвание группы с ID {groupId} успешно изменено на {newGroupName}.\n");
            }
            else
            {
                Console.WriteLine($"\nОшибка: Группа с ID {groupId} не существует в базе данных.\n");
            }
        }

        private void ValidateGroupId(int groupId)
        {
            if (groupId < 1)
            {
                throw new ArgumentException("Введите корректный ID группы.");
            }
        }

        private void ValidateGroupName(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new ArgumentException("Имя группы не может быть пустым.");
            }
        }
    }
}

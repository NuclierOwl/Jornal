using data.RemoteData.RemoteDataBase.DAO;
using domain.UseCase;
using System.Text;
using Zurnal.data.Repository;

namespace ui
{
    public class GroupConsoleUI
    {
        public readonly GroupUseCase _groupUseCase;
        public IGroupUseCase _IgroupUseCase;
        public GroupConsoleUI(GroupUseCase groupUseCase)
        {
            _groupUseCase = groupUseCase;
        }

        public void FindGroupById(int GroupID)
        {
            try
            {
                var group = _groupUseCase.FindGroupById(GroupID);
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

        
        public void AddGroup(string Id)
        {
            try
            {
                ValidateId(Id); 
                _groupUseCase.AddGroup(Id);
                Console.WriteLine($"\nГруппа {Id} добавлена.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }

        public void RemoveGroup(string GroupIDStr)
        {
            try
            {
                int GroupID = int.Parse(GroupIDStr);
                ValidateGroupID(GroupID); 
                _groupUseCase.RemoveGroupById(GroupID);
                Console.WriteLine($"Группа с ID: {GroupID} удалена");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }


        public void UpdateId(int GroupID, string newId)
        {
            var isUpdated = _groupUseCase.UpdateGroup(GroupID, newId);

            if (isUpdated)
            {
                Console.WriteLine($"\nНазвание группы с ID {GroupID} успешно изменено на {newId}.\n");
            }
            else
            {
                Console.WriteLine($"\nОшибка: Группа с ID {GroupID} не существует в базе данных.\n");
            }
        }

        public void ValidateGroupID(int GroupID)
        {
            if (GroupID < 1)
            {
                throw new ArgumentException("Введите корректный ID группы.");
            }
        }
        

        public void ValidateId(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                throw new ArgumentException("Имя группы не может быть пустым.");
            }
        }

        public List<GroupDao> GetAllGroups()
        {
            return _IgroupUseCase.GetGroupsWithStudents().ToList();
        }
    }
}

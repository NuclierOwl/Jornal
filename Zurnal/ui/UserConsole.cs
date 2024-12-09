using domain.UseCase;
using System.Text;

namespace ui
{
    public class UserConsoleUI
    {
        private readonly UserUseCase _userUseCase;

        public UserConsoleUI(UserUseCase userUseCase)
        {
            _userUseCase = userUseCase;
        }

        
        public void DisplayAllUsers()
        {
            Console.WriteLine("\n=== Список всех пользователей ===");
            StringBuilder userOutput = new StringBuilder();

            foreach (var user in _userUseCase.GetAllUsers())
            {
                userOutput.AppendLine($"{user.Guid}\t{user.FIO}\t{user.Group.Name}");
            }

            Console.WriteLine(userOutput);
            Console.WriteLine("===============================\n");
        }

        
        public void RemoveUserByGuid(Guid userGuid)
        {
            string output = _userUseCase.RemoveUserByGuid(userGuid) ? "Пользователь удален" : "Пользователь не найден";
            Console.WriteLine($"\n{output}\n");
        }

        public void UpdateUserByGuid(Guid userGuid)
        {
            try
            {
                var user = _userUseCase.FindUserByGuid(userGuid);
                Console.WriteLine($"Текущие данные: {user.FIO}, {user.Group.Name}");
                Console.Write("\nВведите новое ФИО: ");
                string newFIO = Console.ReadLine();
                user.FIO = newFIO;
                _userUseCase.UpdateUser(user);
                Console.WriteLine("\nПользователь обновлен.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }

       
        public void FindUserByGuid(Guid userGuid)
        {
            try
            {
                var user = _userUseCase.FindUserByGuid(userGuid);
                Console.WriteLine($"\nПользователь найден: {user.Guid}, {user.FIO}, {user.Group.Name}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}\n");
            }
        }
    }
}

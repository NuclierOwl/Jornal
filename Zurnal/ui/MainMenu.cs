using data.Repository;
using domain.UseCase;

namespace ui
{
    public class MainMenuUI
    {
        public readonly UserConsoleUI _userConsoleUI;
        public readonly GroupConsoleUI _groupConsoleUI;
        public readonly PresenceConsole _presenceConsoleUI;

        public MainMenuUI(UserUseCase userUseCase, GroupUseCase groupUseCase, UseCaseGeneratePresence presenceUseCase, IPresenceRepository presenceRepository)
        {
            _userConsoleUI = new UserConsoleUI(userUseCase);
            _groupConsoleUI = new GroupConsoleUI(groupUseCase);
            _presenceConsoleUI = new PresenceConsole(presenceUseCase, presenceRepository);
        }


        public void DisplayMenu()
        {
            while (true)
            {
                Console.Write("\nВыберите команду(Help - справка по командам)~> ");
                string command = Console.ReadLine();
                switch (command)
                {
                    case "Help":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n\nМанипуляции с пользователем:");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("SMe User - Показать список всех пользователей; \nDrop User - Удалить пользователя по его Guid;");
                        Console.WriteLine("Update User - Обновить данные пользователя по Guid; \nFind User - Найти пользователя по его Guid;");
                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Манипуляции с групами:");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("SMe Group - Показать список всех групп; \nCr Group - Создать новую группу");
                        Console.WriteLine("Drop Group - Удалить группу по ID; \nRN Group - Изменить название существующей группы; \nFind Group - Найти группу по ее ID;");
                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Манипуляции с Посещаемостью:");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Gen day-pres - Сгенерировать посещаемость на текущий день; \nGen weak-pres - Сгенерировать посещаемость на текущую неделю;");
                        Console.WriteLine("SMe pres - Показать посещаемость всех пользователей; \nMAM - Отметить пользователя как отсутствующего;");
                        Console.WriteLine("SMe pres ForId - Вывести посещаемость группы по ID; \nToXl - Создать Excel файл; \nInfo pres - Информация о посещаемости;");

                        Console.WriteLine("\n\n");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Exit - Выход из программы;");
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.WriteLine();
                        break;

                    case "SMe User":
                        _userConsoleUI.DisplayAllUsers();
                        break;

                    case "Cr User":
                        Console.Write("Введите ID группы пользователя: ");
                        if (int.TryParse(Console.ReadLine(), out int newGuidName))
                        {
                            Console.Write("Введите Name пользователя для добавления: ");
                            string newUserName = Console.ReadLine();
                            _userConsoleUI.AddUser(newUserName, newGuidName);
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат ID группы");
                        }
                        break;

                    case "Drop User":
                        Console.Write("Введите Guid пользователя для удаления: ");
                        if (Guid.TryParse(Console.ReadLine(), out Guid userGuid))
                        {
                            _userConsoleUI.RemoveUserByGuid(userGuid);
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат Guid");
                        }
                        break;

                    case "Update User":
                        Console.Write("Введите Guid для обновления: ");
                        if (Guid.TryParse(Console.ReadLine(), out Guid updateUserGuid))
                        {
                            _userConsoleUI.UpdateUserByGuid(updateUserGuid);
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат Guid");
                        }
                        break;

                    case "Find User":
                        Console.Write("Введите Guid для поиска: ");
                        if (Guid.TryParse(Console.ReadLine(), out Guid findUserGuid))
                        {
                            _userConsoleUI.FindUserByGuid(findUserGuid);
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат Guid");
                        }
                        break;

                    case "SMe Group":
                        _groupConsoleUI.DisplayAllGroups();
                        break;

                    case "Cr Group":
                        Console.Write("Введите название новой группы: ");
                        string newId = Console.ReadLine();
                        _groupConsoleUI.AddGroup(newId);
                        break;

                    case "Drop Group":
                        Console.Write("Введите ID группы для удаления: ");
                        string GroupIDForDeleteStr = Console.ReadLine(); 
                        if (!string.IsNullOrWhiteSpace(GroupIDForDeleteStr) && int.TryParse(GroupIDForDeleteStr, out int GroupIDForDelete))
                        {
                            _groupConsoleUI.RemoveGroup(GroupIDForDeleteStr); 
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат ID группы");
                        }
                        break;

                    case "RN Group":
                        Console.Write("Введите ID группы для изменения: ");
                        if (int.TryParse(Console.ReadLine(), out int GroupIDToUpdate))
                        {
                            Console.Write("Введите новое название группы: ");
                            string newName = Console.ReadLine();
                            _groupConsoleUI.UpdateId(GroupIDToUpdate, newName);
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат ID группы");
                        }
                        break;

                    case "Find Group":
                        Console.Write("Введите ID группы для поиска: ");
                        if (int.TryParse(Console.ReadLine(), out int idGroupToFind))
                        {
                            _groupConsoleUI.FindGroupById(idGroupToFind);
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат ID группы");
                        }
                        break;

                    case "Gen day-pres":
                        Console.Write("Введите номер первого занятия: ");
                        int firstLesson = int.Parse(Console.ReadLine());
                        Console.Write("Введите номер последнего занятия: ");
                        int lastLesson = int.Parse(Console.ReadLine());
                        Console.Write("Введите ID группы: ");
                        int GroupIDForPresence = int.Parse(Console.ReadLine());

                        _presenceConsoleUI.GeneratePresenceForDay(DateTime.Now, GroupIDForPresence, firstLesson, lastLesson);
                        break;

                    case "Gen weak-pres":
                        Console.Write("Введите номер первого занятия: ");
                        int firstLessonForWeek = int.Parse(Console.ReadLine());
                        Console.Write("Введите номер последнего занятия: ");
                        int lastLessonForWeek = int.Parse(Console.ReadLine());
                        Console.Write("Введите ID группы: ");
                        int GroupIDForWeekPresence = int.Parse(Console.ReadLine());

                        _presenceConsoleUI.GeneratePresenceForWeek(DateTime.Now, GroupIDForWeekPresence, firstLessonForWeek, lastLessonForWeek);
                        break;

                    case "SMe pres":
                        Console.Write("Введите дату (гггг-мм-дд): ");
                        DateTime date = DateTime.Parse(Console.ReadLine());
                        Console.Write("Введите ID группы: ");
                        int groupForPresenceView = int.Parse(Console.ReadLine());

                        _presenceConsoleUI.DisplayPresence(date, groupForPresenceView);
                        break;

                    case "MAM":
                        Console.Write("Введите GUID пользователя: ");
                        string userGuidInput = Console.ReadLine();
                        Guid newUserGuid; 
                        if (!Guid.TryParse(userGuidInput, out newUserGuid))
                        {
                            Console.WriteLine("Ошибка: введён некорректный GUID.");
                            break;
                        }

                        Console.Write("Введите номер первого занятия: ");
                        int firstAbsLesson = int.Parse(Console.ReadLine());
                        Console.Write("Введите номер последнего занятия: ");
                        int lastAbsLesson = int.Parse(Console.ReadLine());
                        Console.Write("Введите ID группы: ");
                        int absGroupID = int.Parse(Console.ReadLine());

                        _presenceConsoleUI.MarkUserAsMissing(DateTime.Now, absGroupID, newUserGuid, firstAbsLesson, lastAbsLesson);
                        break;


                    case "SMe pres ForId":
                        Console.Write("Введите ID группы: ");
                        int GroupIDForAllPresence = int.Parse(Console.ReadLine());
                        _presenceConsoleUI.DisplayAllPresenceByGroup(GroupIDForAllPresence);
                        break;



                    case "ToXl":
                        _presenceConsoleUI.ExportAttendanceToExcel();
                        break;

                    case "Info pres":
                        Console.Write("Введите ID группы: ");
                        int searchGroupID = int.Parse(Console.ReadLine());
                        _presenceConsoleUI.DisplayGeneralPresenceForGroup(searchGroupID);
                        break;

                    case "Exit":
                    case "exit":
                        Console.WriteLine("Выход...");
                        return;

                    default:
                        Console.WriteLine("Нет такой команды. Давай по новой");
                        break;
                }
                Console.WriteLine();
            }
        }
    }
}

using data.RemoteData.RemoteDataBase.DAO;
using data.Repository;
using domain.UseCase;
using remoteData.RemoteDataBase;

namespace ui
{
    public class PresenceConsole
    {
        public readonly UseCaseGeneratePresence _presenceUseCase;
        public readonly IPresenceRepository _presenceRepository;
        public readonly RemoteDatabaseContext _remoteDatabaseContext;

        public PresenceConsole(UseCaseGeneratePresence presenceUseCase, IPresenceRepository presenceRepository)
        {
            _presenceUseCase = presenceUseCase;
            _presenceRepository = presenceRepository;
            _remoteDatabaseContext = new RemoteDatabaseContext();
        }

        public void GeneratePresenceForDay(DateTime date, int GroupID, int firstLesson, int lastLesson)
        {
            ExecuteWithExceptionHandling(() =>
                _presenceUseCase.GeneratePresenceForDay(firstLesson, lastLesson, GroupID, date),
                "Посещаемость на день не получилась.",
                "Ошибка. Посещаемость не сгенерирована.");
        }

        public void GeneratePresenceForWeek(DateTime date, int GroupID, int firstLesson, int lastLesson)
        {
            ExecuteWithExceptionHandling(() =>
                _presenceUseCase.GeneratePresenceForWeek(firstLesson, lastLesson, GroupID, date),
                "Посещаемость на неделю не получилась.",
                "Ошибка. Посещаемость не сгенерирована.");
        }

        public void DisplayPresence(DateTime date, int GroupID)
        {
            try
            {
                var presences = _presenceUseCase.GetPresenceByGroupAndDate(GroupID, date);
                if (presences == null || !presences.Any())
                {
                    Console.WriteLine("Нет данных о посещаемости на выбранную дату.");
                    return;
                }

                Console.WriteLine($"\n                Посещаемость на {date:dd.MM.yyyy}                ");
                DisplayPresences(presences);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Что-то пошло не так: {ex.Message}");
            }
        }

        public void MarkUserAsMissing(DateTime date, int GroupID, Guid userGuid, int firstLesson, int lastLesson)
        {
            if (!GroupExists(GroupID) || !UserExists(userGuid)) return;

            _presenceUseCase.MarkUserAsMissing(userGuid, GroupID, firstLesson, lastLesson, date);
            Console.WriteLine("Пользователь отмечен как отсутствующий.");
        }

        public void DisplayAllPresenceByGroup(int GroupID)
        {
            try
            {
                var presences = _presenceUseCase.GetAllPresenceByGroup(GroupID);
                if (presences == null || !presences.Any())
                {
                    Console.WriteLine($"Нет данных о посещаемости для группы с ID: {GroupID}.");
                    return;
                }

                var groupedPresences = presences.GroupBy(p => p.Date);
                foreach (var group in groupedPresences)
                {
                    Console.WriteLine($"                Дата: {group.Key:dd.MM.yyyy}                 ");
                    DisplayPresences(group);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Что-то пошло не так: {ex.Message}");
            }
        }

        public void DisplayGeneralPresenceForGroup(int GroupID)
        {
            var summary = _presenceRepository.GetGeneralPresenceForGroup(GroupID);
            Console.WriteLine($"Человек в группе: {summary.UserCount}, " +
                              $"Количество проведённых занятий: {summary.LessonCount}, " +
                              $"Общий процент посещаемости группы: {summary.TotalAttendancePercentage}%");

            foreach (var user in summary.UserAttendances)
            {
                if (user.AttendanceRate < 40)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.WriteLine($"GUID Пользователя: {user.UserGuid}, " +
                                  $"Посетил: {user.Attended}, " +
                                  $"Пропустил: {user.Missed}, " +
                                  $"Процент посещаемости: {user.AttendanceRate}%");
                Console.ResetColor();
            }
        }

        public void ExportAttendanceToExcel()
        {
            ExecuteWithExceptionHandling(() =>
                _presenceUseCase.ExportAttendanceToExcel(),
                "Данные посещаемости успешно экспортированы в Excel.",
                "Ошибка при экспорте посещаемости.");
        }

        public void UpdateUserAttendance(Guid userGuid, int GroupID, int firstLesson, int lastLesson, DateTime date, bool isAttendance)
        {
            try
            {
                bool result = _presenceRepository.UpdateAttention(userGuid, GroupID, firstLesson, lastLesson, date, isAttendance);
                Console.WriteLine(result
                    ? $"Статус посещаемости для пользователя {userGuid} обновлён."
                    : $"Данные о посещаемости для пользователя ID: {userGuid} не найдены.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении посещаемости: {ex.Message}");
            }
        }

        public void ExecuteWithExceptionHandling(Action action, string successMessage, string errorMessage)
        {
            try
            {
                action();
                Console.WriteLine(successMessage);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"{errorMessage}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
            }
        }

        public void DisplayPresences(IEnumerable<PresenceDao> presences)
        {
            int previousLessonNumber = -1;
            foreach (var presence in presences)
            {
                if (previousLessonNumber != presence.LessonNumber)
                {
                    Console.WriteLine($"                   Занятие: {presence.LessonNumber}                   ");
                    previousLessonNumber = presence.LessonNumber;
                }

                string status = presence.IsAttedance ? "Присутствует" : "Отсутствует";
                Console.WriteLine($"Пользователь (ID: {presence.UserGuid}) - Статус: {status}");
            }
        }

        public bool GroupExists(int GroupID) => _remoteDatabaseContext.groups.Any(g => g.Id == GroupID);
        public bool UserExists(Guid userGuid) => _remoteDatabaseContext.users.Any(u => u.Guid == userGuid);
    }
}
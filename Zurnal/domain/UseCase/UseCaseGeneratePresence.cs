using ClosedXML.Excel;
using data.RemoteData.RemoteDataBase.DAO;
using data.Repository;

namespace domain.UseCase
{
    public class UseCaseGeneratePresence
    {
        public readonly IUserRepository _userRepository;
        public readonly IPresenceRepository _presenceRepository;
        public readonly IGroupRepository _groupRepository;

        public UseCaseGeneratePresence(IUserRepository userRepository, IPresenceRepository presenceRepository, IGroupRepository groupRepository)
        {
            _userRepository = userRepository;
            _presenceRepository = presenceRepository;
            _groupRepository = groupRepository;
        }

        public List<PresenceDao> GetPresenceByGroupAndDate(int GroupID, DateTime date)
        {
            return _presenceRepository.GetPresenceByGroupAndDate(GroupID, date);
        }


        public void GeneratePresenceForDay(int firstLesson, int lastLesson, int GroupID, DateTime currentDate)
        {
            var groupExists = _groupRepository.GetAllGroup().Any(g => g.Id == GroupID);
            if (!groupExists)
            {
                throw new ArgumentException($"Группа с ID {GroupID} не существует.");
            }

            var users = _userRepository.GetAllUsers.Where(u => u.GroupID == GroupID).ToList();
            List<PresenceDao> presences = new List<PresenceDao>();
            for (int lessonNumber = firstLesson; lessonNumber <= lastLesson; lessonNumber++)
            {
                foreach (var user in users)
                {
                    presences.Add(new PresenceDao
                    {
                        UserGuid = user.Guid,
                        Date = currentDate,
                        LessonNumber = lessonNumber,
                        IsAttedance = true
                    });
                }
            }
            _presenceRepository.SavePresence(presences);
        }

        public void GeneratePresenceForWeek(int firstLesson, int lastLesson, int GroupID, DateTime startTime)
        {
            var groupExists = _groupRepository.GetAllGroup().Any(g => g.Id == GroupID);
            if (!groupExists)
            {
                throw new ArgumentException($"Группа с ID {GroupID} не существует.");
            }

            for (int i = 0; i < 7; i++)
            {
                DateTime currentTime = startTime.AddDays(i);
                GeneratePresenceForDay(firstLesson, lastLesson, GroupID, currentTime);
            }
        }




        public void MarkUserAsMissing(Guid userGuid, int GroupID, int firstLesson, int lastLesson, DateTime date)
        {
            var presences = _presenceRepository.GetPresenceByGroupAndDate(GroupID, date);
            foreach (var presence in presences.Where(p => p.UserGuid == userGuid && p.LessonNumber >= firstLesson && p.LessonNumber <= lastLesson))
            {
                presence.IsAttedance = false;
            }
            _presenceRepository.SavePresence(presences);
        }



        public List<PresenceDao> GetAllPresenceByGroup(int GroupID)
        {
            return _presenceRepository.GetPresenceByGroup(GroupID);
        }

        public GroupPresenceSummary GetGeneralPresenceForGroup(int GroupID)
        {
            return _presenceRepository.GetGeneralPresenceForGroup(GroupID);
        }

        public Dictionary<string, List<Excel>> GetAllAttendanceByGroups()
        {
            var attendanceByGroup = new Dictionary<string, List<Excel>>();
            var allGroups = _groupRepository.GetAllGroup();

            foreach (var group in allGroups)
            {
                var groupAttendance = _presenceRepository.GetAttendanceByGroup(group.Id);
                var attendanceRecords = new List<Excel>();

                foreach (var record in groupAttendance)
                {
                    var names = _userRepository.GetUserNames().Where(u => u.Guid == record.UserGuid);
                    foreach (var name in names)
                    {
                        attendanceRecords.Add(new Excel
                        {
                            UserName = name.FIO,
                            UserGuid = name.Guid,
                            Date = record.Date,
                            IsAttedance = record.IsAttedance,
                            LessonNumber = record.LessonNumber,
                            Id = group.Name
                        });
                    }
                }

                attendanceByGroup.Add(group.Name, attendanceRecords);
            }

            return attendanceByGroup;
        }

        public void ExportAttendanceToExcel()
        {
            var attendanceByGroup = GetAllAttendanceByGroups();
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string reportsFolderPath = Path.Combine(projectDirectory, "Reports");
            string filePath = Path.Combine(reportsFolderPath, "AttendanceReport.xlsx");

            if (!Directory.Exists(reportsFolderPath))
            {
                Directory.CreateDirectory(reportsFolderPath);
            }
            using (var workbook = new XLWorkbook())
            {
                foreach (var group in attendanceByGroup)
                {
                    var worksheet = workbook.Worksheets.Add($"{group.Key}");
                    worksheet.Cell(1, 1).Value = "ФИО";
                    worksheet.Cell(1, 2).Value = "Группа";
                    worksheet.Cell(1, 3).Value = "Дата";
                    worksheet.Cell(1, 4).Value = "Занятие";
                    worksheet.Cell(1, 5).Value = "Статус";

                    int row = 2;
                    int lesNum = 1;
                    foreach (var record in group.Value.OrderBy(r => r.Date).ThenBy(r => r.LessonNumber).ThenBy(r => r.UserGuid))
                    {
                        if (lesNum != record.LessonNumber)
                        {
                            row++;
                        }
                        worksheet.Cell(row, 1).Value = record.UserName;
                        worksheet.Cell(row, 2).Value = record.Id;
                        worksheet.Cell(row, 3).Value = record.Date.ToString("dd.MM.yyyy");
                        worksheet.Cell(row, 4).Value = record.LessonNumber.ToString();
                        worksheet.Cell(row, 5).Value = record.IsAttedance ? "Присутствует" : "Отсутствует";
                        row++;



                        lesNum = record.LessonNumber;
                    }

                    worksheet.Columns().AdjustToContents();
                }

                workbook.SaveAs(filePath);
            }
        }


    }
}

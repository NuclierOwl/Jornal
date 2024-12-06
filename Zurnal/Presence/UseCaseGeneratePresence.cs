using data.RemoteData.RemoteDataBase.DAO;

namespace Zurnal.Presence
{
    internal class UseCaseGeneratePresence
    {
        internal class AttendanceRecord
        {
            public int LessonNumber { get; set; }
            public required string GroupNumber { get; set; }
            public DateTime Date { get; set; }
            public bool IsPresent { get; set; }
        }

        private List<GroupDao> groups;

        public UseCaseGeneratePresence(List<GroupDao> groups)
        {
            this.groups = groups;
        }

        public List<AttendanceRecord> GenerateDailyAttendance(int firstLesson, int lastLesson, string groupNumber, DateTime currentDate)
        {
            List<AttendanceRecord> attendanceRecords = new List<AttendanceRecord>();
            var group = groups.FirstOrDefault(g => g.Name == groupNumber);

            if (group != null)
            {
                var users = group.Users;

                foreach (var user in users)
                {
                    for (int lesson = firstLesson; lesson <= lastLesson; lesson++)
                    {
                        attendanceRecords.Add(new AttendanceRecord
                        {
                            LessonNumber = lesson,
                            GroupNumber = groupNumber,
                            Date = currentDate,
                            IsPresent = true
                        });
                    }
                }
            }
            return attendanceRecords;
        }

        public List<AttendanceRecord> GenerateWeeklyAttendance(int firstLesson, int lastLesson, string groupNumber, DateTime startDate)
        {
            List<AttendanceRecord> weeklyAttendanceRecords = new List<AttendanceRecord>();

            for (int day = 0; day < 7; day++)
            {
                DateTime currentDate = startDate.AddDays(day);
                weeklyAttendanceRecords.AddRange(GenerateDailyAttendance(firstLesson, lastLesson, groupNumber, currentDate));
            }

            return weeklyAttendanceRecords;
        }
    }
}
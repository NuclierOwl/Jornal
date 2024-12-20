using data.RemoteData.RemoteDataBase.DAO;
using Microsoft.EntityFrameworkCore;
using remoteData.RemoteDataBase;

namespace data.Repository
{
    public class SQLPresenceRepositoryImpl : IPresenceRepository
    {
        public readonly RemoteDatabaseContext _remoteDatabaseContext;

        public SQLPresenceRepositoryImpl(RemoteDatabaseContext remoteDatabaseContext)
        {
            _remoteDatabaseContext = remoteDatabaseContext;
        }

        public void SavePresence(List<PresenceDao> presences)
        {
            foreach (var presence in presences)
            {
                var existing = _remoteDatabaseContext.presence.FirstOrDefault(p =>
                    p.Date == presence.Date &&
                    p.UserGuid == presence.UserGuid &&
                    p.LessonNumber == presence.LessonNumber);

                if (existing == null)
                {
                    _remoteDatabaseContext.presence.Add(new PresenceDao
                    {
                        Date = presence.Date,
                        IsAttedance = presence.IsAttedance,
                        LessonNumber = presence.LessonNumber,
                        UserGuid = presence.UserGuid
                    });
                }
                else
                {
                    existing.IsAttedance = presence.IsAttedance;
                }
            }

            _remoteDatabaseContext.SaveChanges();
        }

        public void AddPresence(PresenceDao presence)
        {
            if (presence == null) throw new ArgumentNullException(nameof(presence));

            var newPresence = new PresenceDao
            {
                Date = presence.Date,
                UserGuid = presence.UserGuid,
                LessonNumber = presence.LessonNumber,
                IsAttedance = presence.IsAttedance
            };
            _remoteDatabaseContext.presence.Add(newPresence);
        }

        public List<PresenceDao> GetPresenceByGroup(int GroupID)
        {
            return _remoteDatabaseContext.presence.Include(user => user.User)
                .Where(p => p.User != null && p.User.GroupID == GroupID) 
                .Select(p => new PresenceDao
                {
                    Date = p.Date,
                    UserGuid = p.UserGuid,
                    LessonNumber = p.LessonNumber,
                    IsAttedance = p.IsAttedance
                })
                .ToList();
        }

        public List<PresenceDao> GetPresenceByGroupAndDate(int GroupID, DateTime date)
        {
            return _remoteDatabaseContext.presence
                .Where(p => p.User != null && p.User.GroupID == GroupID && p.Date == date.Date)
                .Select(p => new PresenceDao
                {
                    Date = p.Date,
                    UserGuid = p.UserGuid,
                    LessonNumber = p.LessonNumber,
                    IsAttedance = p.IsAttedance
                })
                .ToList();
        }

        public void MarkUserAsMissing(Guid userGuid, int firstLessonNumber, int lastLessonNumber)
        {
            foreach (var lesson in Enumerable.Range(firstLessonNumber, lastLessonNumber - firstLessonNumber + 1))
            {
                var presence = _remoteDatabaseContext.presence.FirstOrDefault(p =>
                    p.UserGuid == userGuid &&
                    p.LessonNumber == lesson);

                if (presence != null)
                {
                    presence.IsAttedance = false;
                }
            }
        }

        public DateTime? GetLastDateByGroupID(int GroupID)
        {
            var lastDate = _remoteDatabaseContext.presence
                .Where(p => p.User.GroupID == GroupID)
                .OrderByDescending(p => p.Date)
                .Select(p => p.Date)
                .FirstOrDefault();

            return lastDate == default ? (DateTime?)null : lastDate;
        }

        public GroupPresenceSummary GetGeneralPresenceForGroup(int GroupID)
        {
            var presences = _remoteDatabaseContext.presence
                .Where(p => p.User.GroupID == GroupID)
                .OrderBy(p => p.Date).ThenBy(p => p.LessonNumber)
                .ToList();


            var distinctLessonDates = presences
                .Select(p => new { p.Date, p.LessonNumber })
                .Distinct()
                .ToList();

            int lessonCount = distinctLessonDates.Count;


            var userGuids = presences
                .Select(p => p.UserGuid)
                .Distinct()
                .ToHashSet();

            double totalAttendance = presences.Count(p => p.IsAttedance);
            double totalPossibleAttendance = userGuids.Count * lessonCount;

            var userAttendances = userGuids.Select(userGuid =>
            {
                var userPresences = presences.Where(p => p.UserGuid == userGuid).ToList();
                double attended = userPresences.Count(p => p.IsAttedance);
                double missed = userPresences.Count(p => !p.IsAttedance);

                return new UserAttendance
                {
                    UserGuid = userGuid,
                    Attended = attended,
                    Missed = missed,
                    AttendanceRate = (attended / (attended + missed)) * 100
                };
            }).ToList();

            double totalAttendancePercentage = (totalAttendance / totalPossibleAttendance) * 100;

            return new GroupPresenceSummary
            {
                UserCount = userGuids.Count,
                LessonCount = lessonCount,
                TotalAttendancePercentage = totalAttendancePercentage,
                UserAttendances = userAttendances
            };
        }





        public bool UpdateAttention(Guid UserGuid, int GroupID, int firstLesson, int lastLesson, DateTime date, bool isAttendance)
        {
            var presences = _remoteDatabaseContext.presence
                .Where(p => p.UserGuid == UserGuid && p.User.GroupID == GroupID &&
                            p.LessonNumber >= firstLesson && p.LessonNumber <= lastLesson && p.Date == date)
                .ToList();

            if (presences.Any())
            {
                foreach (var presence in presences)
                {
                    presence.IsAttedance = isAttendance;
                }
                _remoteDatabaseContext.SaveChanges();
                return true;
            }
            return false;
        }
        public List<PresenceDao> GetAttendanceByGroup(int GroupID)
        {

            var userGuidsInGroup = _remoteDatabaseContext.users
                .Where(u => u.GroupID == GroupID)
                .Select(u => u.Guid)
                .ToList();

            return _remoteDatabaseContext.presence
                .Where(p => userGuidsInGroup.Contains(p.UserGuid))
                .Select(p => new PresenceDao
                {
                    UserGuid = p.UserGuid,
                    Id = p.Id,
                    Date = p.Date,
                    LessonNumber = p.LessonNumber,
                    IsAttedance = p.IsAttedance
                })
                .ToList();
        }


    }
}

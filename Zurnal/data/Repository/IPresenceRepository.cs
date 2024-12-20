using data.RemoteData.RemoteDataBase.DAO;

namespace data.Repository
{
    public interface IPresenceRepository
    {
        void SavePresence(List<PresenceDao> presences);
        List<PresenceDao> GetPresenceByGroup(int GroupID);
        List<PresenceDao> GetPresenceByGroupAndDate(int GroupID, DateTime date);
        DateTime? GetLastDateByGroupID(int GroupID);
        public GroupPresenceSummary GetGeneralPresenceForGroup(int GroupID);
        bool UpdateAttention(Guid UserGuid, int GroupID, int firstLesson, int lastLesson, DateTime date, bool isAttendance);

        void MarkUserAsMissing(Guid userGuid, int firstLessonNumber, int lastLessonNumber);
        void AddPresence(PresenceDao presence);
        List<PresenceDao> GetAttendanceByGroup(int GroupID);
    }
}

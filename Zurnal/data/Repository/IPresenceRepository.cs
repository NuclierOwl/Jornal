using data.RemoteData.RemoteDataBase.DAO;

namespace data.Repository
{
    public interface IPresenceRepository
    {
        void SavePresence(List<PresenceDao> presences);
        List<PresenceDao> GetPresenceByGroup(int groupId);
        List<PresenceDao> GetPresenceByGroupAndDate(int groupId, DateTime date);
        DateTime? GetLastDateByGroupId(int groupId);
        public GroupPresenceSummary GetGeneralPresenceForGroup(int groupId);
        bool UpdateAttention(Guid UserGuid, int groupId, int firstLesson, int lastLesson, DateTime date, bool isAttendance);

        void MarkUserAsMissing(Guid userGuid, int firstLessonNumber, int lastLessonNumber);
        void AddPresence(PresenceDao presence);
        List<PresenceDao> GetAttendanceByGroup(int groupId);
    }
}

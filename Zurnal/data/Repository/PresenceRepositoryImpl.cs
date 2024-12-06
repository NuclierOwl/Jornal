using data.RemoteData.RemoteDataBase.DAO;
using domain.Models;

namespace data.Repository
{
    public class PresenceRepositoryImpl
    {
        private readonly List<PresenceDao> _presences = new List<PresenceDao>();

        public void SavePresence(List<PresenceDao> presences)
        {
            foreach (var presence in presences)
            {
                var existing = _presences.FirstOrDefault(p =>
                    p.Date == presence.Date &&
                    p.UserGuid == presence.UserGuid && 
                    p.LessonNumber == presence.LessonNumber);

                if (existing == null)
                {
                    _presences.Add(presence);
                }
                else
                {
                    existing.IsAttedance = presence.IsAttedance;
                }
            }
        }

        public void AddPresence(PresenceDao presence)
        {
            if (presence == null) throw new ArgumentNullException(nameof(presence));

            _presences.Add(presence);
        }

        public List<PresenceDao> GetPresenceByGroup(int groupId)
        {
            return _presences.Where(p => p.Group.Id == groupId).ToList();
        }

        public List<PresenceDao> GetPresenceByGroupAndDate(int groupId, DateTime date)
        {
            return _presences.Where(p => p.Group.Id == groupId && p.Date == date).ToList();
        }

        public void MarkUserAsMissing(Guid userGuid, int firstLessonNumber, int lastLessonNumber)
        {
            foreach (var lesson in Enumerable.Range(firstLessonNumber, lastLessonNumber - firstLessonNumber + 1))
            {
                var presence = _presences.FirstOrDefault(p => p.UserGuid == userGuid && p.LessonNumber == lesson);
                if (presence != null)
                {
                    presence.IsAttedance = false;
                }
            }
        }
    }
}

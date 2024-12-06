public class UserAttendance
{
    public Guid UserGuid { get; set; }
    public double Attended { get; set; }
    public double Missed { get; set; }
    public double AttendanceRate { get; set; }
}

public class GroupPresenceSummary
{
    public int UserCount { get; set; }
    public int LessonCount { get; set; }
    public double TotalAttendancePercentage { get; set; }
    public List<UserAttendance> UserAttendances { get; set; }
    public int TotalCount { get; set; }
    public int AttendedCount { get; set; }
    public int AbsentCount { get; set; }

}
namespace domain.Request
{
    public class AddGroupWithStudentsRequest
    {
        public AddGroupRequest addGroupRequest { get; set; }
        public IEnumerable<AddStudentRequest> AddStudentRequests { get; set; }
    }
}



namespace ResidentSupervision.Application.Entities
{
    public class Profile 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }        
        public string TitleType { get; set; }

        public Supervisor Supervisor { get; set; }
        public Settings Settings { get; set; }
        public Account Account { get; set; }
    }
}
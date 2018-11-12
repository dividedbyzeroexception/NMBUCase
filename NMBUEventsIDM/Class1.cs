using System;

namespace NMBUIDM
{
    public enum PersonEventAction {
        PersonNew,
        PersonUpdated,
        PersonDeleted
    };

    public class Person {
        public int PersonID { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Mobile { get; set; }
    };

    public class User : Person {
        public int UserID { get; set; }
        public string UserName { get; set; }        
    };

    public class PersonEvent {
        public string EventSource { get; set; }
        public int EventID { get; set; }
        public Guid EventUniqueID { get; set; }
        public PersonEventAction EventAction { get; set; }
        public Person OldPerson { get; set; }
        public Person NewPerson { get; set; }
    };
        

}

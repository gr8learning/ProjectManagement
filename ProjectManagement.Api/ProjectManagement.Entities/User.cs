using System;
using System.Collections.Generic;

namespace ProjectManagement.Entities
{
    public class User : BaseEntity
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public virtual IEnumerable<Task> Tasks { get; set; }
    }
}

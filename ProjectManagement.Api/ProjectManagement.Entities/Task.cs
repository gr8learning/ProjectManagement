using ProjectManagement.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Entities
{
    public class Task : BaseEntity
    {

        public long ProjectID { get; set; }

        public string Detail { get; set; }

        public TaskStatus Status { get; set; }

        public long? AssignedToUserID { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual User AssignedToUser { get; set; }

        public virtual Project Project { get; set; }
    }
}

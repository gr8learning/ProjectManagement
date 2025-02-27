﻿using ProjectManagement.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectManagement.Entities
{
    public class Task : BaseEntity
    {

        public long ProjectID { get; set; }

        public string Detail { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskStatus Status { get; set; }

        public long? AssignedToUserID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }

        [ForeignKey("AssignedToUserID")]
        public virtual UserDto AssignedToUser { get; set; }

    }
}

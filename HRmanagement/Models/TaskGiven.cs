﻿using HRmanagement.Data.enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRmanagement.Models
{
	public class TaskGiven
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

		[Required]
		public Severity severity { get; set; }

		[Required]
		[DefaultValue(TasksStatus.Pending)]
		public TasksStatus taskStatus { get; set; }

		[Required]
		public string AssignedTO { get; set; }

		[Required]
		public string CreatedBy { get; set; }

		[ForeignKey("CreatedBy")]
		public EmployeeUser Creator { get; set; }

		[ForeignKey("AssignedTO")]
		public EmployeeUser Assignee { get; set; }
	}
}
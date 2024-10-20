using HRmanagement.Data.enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRmanagement.Models
{
    public class EmployeeUser : IdentityUser
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Discriminator { get; set; } = string.Empty;

        [Required]
        public decimal BaseSalary { get; set; } = 0;

        [Required]
        public EmpStatus Status { get; set; } = EmpStatus.Active;

        [Required]
        public MaritialStatus MaritalStatus { get; set; } = MaritialStatus.Unmarrired;

        [Required]
        public EmpTypes Type { get; set; } = EmpTypes.FullTime;

        public string? PAN { get; set; }
        public string? CitizenshipNumber { get; set; }
        public string? AccountNumber { get; set; }

        [Required]
        public int DesignationId { get; set; } = 1;

        [ForeignKey("DesignationId")]
        public Designation designation { get; set; }
    }
}
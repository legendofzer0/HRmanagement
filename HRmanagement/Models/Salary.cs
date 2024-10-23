using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRmanagement.Models
{
    public class Salary
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Phone Number")]
        public string? PhoneNumber { get; set; }

        [DisplayName("Account Number")]
        public string? AccountNumber { get; set; }

        [DisplayName("Base Salary")]
        public decimal BaseSalary { get; set; }

        public decimal? Bonus { get; set; }
        public decimal? Deduction { get; set; }

        [DisplayName("Total Salary")]
        public decimal TotalSalary { get; set; }

        public string? Description { get; set; }

        [DisplayName("Leave Taken")]
        public int? LeaveTaken { get; set; }

        [Required]
        [DisplayName("Date Created")]
        public DateTime Created { get; set; }

        [Required]
        public string userId { get; set; }

        [ForeignKey("userId")]
        public EmployeeUser User { get; set; }

        // Constructor to set the default Created date
        public Salary()
        {
            Created = DateTime.Now;
        }
    }
}
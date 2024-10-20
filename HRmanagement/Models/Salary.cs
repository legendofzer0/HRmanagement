using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRmanagement.Models
{
    public class Salary
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        public string PhoneNumber { get; set; }
        public string AccountNumber { get; set; }

        public decimal TotalSalary { get; set; }

        public decimal Bonus { get; set; }

        public string userId { get; set; }

        [ForeignKey("userId")]
        public EmployeeUser User { get; set; }
    }
}
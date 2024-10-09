using HRmanagement.Data.enums;
using System.ComponentModel.DataAnnotations;

namespace HRmanagement.Models
{
    public class Designation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(10,ErrorMessage ="Minimum length of Name is 10")]
        [MaxLength(100, ErrorMessage = "Maximum length of Name is 100")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public EmployeeLevel Level { get; set; } = EmployeeLevel.Junior;
    }
}
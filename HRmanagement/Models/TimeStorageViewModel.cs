namespace HRmanagement.Models
{
	public class TimeStorageViewModel
	{
		public TimeStorage? TimeStorage { get; set; }
		public IEnumerable<EmployeeUser>? Employees { get; set; }
	}
}
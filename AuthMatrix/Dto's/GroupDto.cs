using AuthMatrix.Models.UserModel;

namespace AuthMatrix.Dto_s
{
	public class GroupDto
	{
		public int Id { get; set; }
		public string Name { get; set; } 
		public int Priority { get; set; }
		public int TotalUsers { get; set; } // count of users on group 
		public int TotalPermissions { get; set; } // count of permissions on group	
	}
}

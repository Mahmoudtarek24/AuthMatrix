namespace AuthMatrix.Models.UserModel
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Password { get; set; } // for only demo purpose 
		public bool IsSuperAdmin { get; set; }
		public string Department { get; set; }	
		public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
	}
}

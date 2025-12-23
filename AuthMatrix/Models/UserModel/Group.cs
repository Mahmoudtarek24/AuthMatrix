namespace AuthMatrix.Models.UserModel
{
	public class Group
	{
		public int Id { get; set; }
		public string Name { get; set; }
		//public string Description { get; set; }
		public int Priority { get; set; }
		public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
		public ICollection<GroupPermission> GroupPermissions { get; set; } = new List<GroupPermission>();

	}
}

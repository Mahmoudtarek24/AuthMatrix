namespace AuthMatrix.Models.UserModel
{
	public class Permission
	{
		public int Id { get; set; }

		public string Action { get; set; }     // View, Create, Approve, Delete

		public int ResourceId { get; set; }
		public Resource Resource { get; set; }

		public PermissionScope Scope { get; set; } = PermissionScope.All;
		
		public string? ScopeFilter { get; set; } // "Department == 'Sales'"
		 
		public ICollection<GroupPermission> GroupPermissions { get; set; } = new List<GroupPermission>();
	}
}

namespace AuthMatrix.Models.UserModel
{
	public class GroupPermission
	{
		public int GroupId { get; set; }
		public Group Group { get; set; }

		public int PermissionId { get; set; }
		public Permission Permission { get; set; }

		public DateTime? ExpiresAt { get; set; }
		public PermissionType Type { get; set; } = PermissionType.Allow;
	}
}

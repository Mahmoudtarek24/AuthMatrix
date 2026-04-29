using AuthMatrix.Models.UserModel;

namespace AuthMatrix.Dto_s
{
	public class PermissionDto
	{
		public int PermissionId { get; set; }
		public string Action { get; set; }
		public string ResourceName { get; set; }
		public int ResourceId { get; set; }
		public PermissionScope PermissionScope { get; set; }
		public string ScopeFilter { get; set; }
		public string GroupName { get; set; }
		public int GroupPriority { get; set; }
		public PermissionType Type { get; set; }
		public DateTime? ExpiresAt { get; set; }

		public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value <= DateTime.UtcNow;
		public bool IsActive => !IsExpired && Type == PermissionType.Allow;
	}
}

using AuthMatrix.Models.UserModel;

namespace AuthMatrix.Dto_s
{
	public class AssignPermissionDto
	{
		public int PermissionId { get; set; }
		public PermissionType Type { get; set; }
		public DateTime? ExpiresAt { get; set; }
	}
}

using AuthMatrix.Dto_s;

namespace AuthMatrix.Services.Interfaces
{
	public interface IGroupServices
	{
		Task<GroupDto> CreateGroupAsync(CreateGroupDto dto);
		Task<List<GroupDto>> GetAllGroupsAsync();
		Task<GroupDto> GetGroupByIdAsync(int id);
		Task<GroupDto> UpdateGroupAsync(int id, UpdateGroupDto dto);
		Task<bool> DeleteGroupAsync(int id);
		Task<bool> AssignPermissionAsync(int groupId, AssignPermissionDto dto);
		Task<bool> RevokePermissionAsync(int groupId, int permissionId);
		Task<List<PermissionDto>> GetGroupPermissionsAsync(int groupId);
	}
}

using AuthMatrix.Dto_s;

namespace AuthMatrix.Services.Interfaces
{
	public interface IPermissionService
	{
		Task<bool> HasPermissionAsync(int userId, string action, string resourceName);
		Task<List<PermissionDto>> GetUserPermissionsAsync(int userId);
		Task<IQueryable<T>>  GetFilteredDataAsync<T>(int userId, string resourceName, IQueryable<T> query) where T : class;		
	}
}

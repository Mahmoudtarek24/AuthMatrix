using AuthMatrix.Dto_s;
using AuthMatrix.Models;
using AuthMatrix.Models.UserModel;
using AuthMatrix.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace AuthMatrix.Services
{
	public class PermissionService : IPermissionService
	{
		private readonly ApplicationDbContext context;
		public PermissionService(ApplicationDbContext context)
		{
			this.context = context;
		}

		public async Task<IQueryable<T>> GetFilteredDataAsync<T>(int userId, string resourceName, IQueryable<T> query) where T : class
		{
			var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
			
			if (user is null)
				return query.Where(e => false);
			
			if (user.IsSuperAdmin)
				return query;

			var userGroupIds = await context.UserGroups
				.Where(ug => ug.UserId == userId)
				.Select(ug => ug.GroupId)
				.ToListAsync();

			if (!userGroupIds.Any())
				return query.Where(x => false);

			var highestPriorityGroup = await context.Groups
				.Where(g => userGroupIds.Contains(g.Id))
				.OrderByDescending(g => g.Priority)
				.FirstOrDefaultAsync();

			if (highestPriorityGroup == null)
				return query.Where(x => false);

			// ✅ Step 3: Get GroupPermission
			var groupPermission = await context.GroupPermissions
				.Include(gp => gp.Permission)
					.ThenInclude(p => p.Resource)
				.Where(gp =>
					gp.GroupId == highestPriorityGroup.Id &&
					gp.Permission.Resource.Name == resourceName)
				.FirstOrDefaultAsync();

			if (groupPermission == null)
				return query.Where(x => false);

			if (IsExpired(groupPermission.ExpiresAt)|| groupPermission.Type == PermissionType.Deny)
				return query.Where(x => false);


			var permission = groupPermission.Permission;

			return permission.Scope switch
			{
				PermissionScope.All => query,	
				PermissionScope.Own => ApplyOwnScope(query, userId),	
				PermissionScope.Department => ApplyDepartmentScope(query, user),		
				PermissionScope.Custom => ApplyCustomScope(query, permission.ScopeFilter),	
				_ => query.Where(e => false),	
			};
		}

		private bool IsExpired(DateTime? expiresAt)
		{
			return expiresAt.HasValue && expiresAt.Value <= DateTime.UtcNow;
		}
		private IQueryable<T> ApplyOwnScope<T>(IQueryable<T> query, int userId) where T : class
		{
			var property = typeof(T).GetProperty("CreatedBy");

			if (property == null)
				return query.Where(x => false);

			return query.Where(e => EF.Property<int>(e, "CreatedBy") == userId);
		}
		private IQueryable<T> ApplyDepartmentScope<T>(IQueryable<T> query ,User user) where T : class
		{
			var property = typeof(T).GetProperty("Department");

			if (property is null)
				return query.Where(x => false);		

			var userDepartment = user.Department ?? string.Empty;

			return query.Where(e => EF.Property<string>(e, "Department") == userDepartment);
		}
		private IQueryable<T> ApplyCustomScope<T>(IQueryable<T> query, string scopeFilter) where T : class
		{
			if (string.IsNullOrWhiteSpace(scopeFilter))
				return query;

			try
			{
				return query.Where(scopeFilter);
			}
			catch
			{
				return query.Where(x => false);
			}
		}

		public async Task<List<PermissionDto>> GetUserPermissionsAsync(int userId)
		{
			var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
			if (user is null)
				return new List<PermissionDto>();

			// will return all permissions
			if (user.IsSuperAdmin)
			{
				var allPermissions = await context.Permissions.Include(e => e.Resource)
								.Select(e => new PermissionDto
								{
									Action = e.Action,
									ScopeFilter = e.ScopeFilter,
									PermissionScope = e.Scope,
									ResourceId = e.ResourceId,
									PermissionId = e.Id,
									ResourceName = e.Resource.Name,
									Type = PermissionType.Allow,
									ExpiresAt = null
								}).ToListAsync();
				return allPermissions;
			}

			var userPermissions = await context.UserGroups
					   .AsNoTracking()
					   .Where(ug => ug.UserId == userId)
					   .SelectMany(ug => ug.Group.GroupPermissions)
					   .Include(gp => gp.Group)
					   .Include(gp => gp.Permission)
						   .ThenInclude(p => p.Resource)
					   .Where(gp => gp.ExpiresAt == null || gp.ExpiresAt > DateTime.UtcNow)
					   .Select(gp => new PermissionDto
					   {
						   PermissionId = gp.PermissionId,
						   Action = gp.Permission.Action,
						   ResourceName = gp.Permission.Resource.Name,
						   ResourceId = gp.Permission.ResourceId,
						   PermissionScope = gp.Permission.Scope,
						   ScopeFilter = gp.Permission.ScopeFilter,
						   GroupName = gp.Group.Name,
						   GroupPriority = gp.Group.Priority,
						   Type = gp.Type,
						   ExpiresAt = gp.ExpiresAt
					   })
					   .ToListAsync();

			return userPermissions;
		}

		public async Task<bool> HasPermissionAsync(int userId, string action, string resourceName)
		{
			var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

			if (user == null)
				return false;

			if (user.IsSuperAdmin)
				return true;

			//  Get Valid Permissions
			var groupPermissions = await context.GroupPermissions.AsNoTracking()
				.Include(gp => gp.Group).Include(gp => gp.Permission)
				.ThenInclude(p => p.Resource)
				.Where(gp =>
					gp.Group.UserGroups.Any(ug => ug.UserId == userId) &&
					gp.Permission.Action == action &&
					gp.Permission.Resource.Name == resourceName &&
					(gp.ExpiresAt == null || gp.ExpiresAt > DateTime.UtcNow))
				.ToListAsync();

			//  No Permissions
			if (!groupPermissions.Any())
				return false;

			//  Get Highest Priority
			var highestPriority = groupPermissions
				.OrderByDescending(gp => gp.Group.Priority)
				.First();

			//  Check Type
			return highestPriority.Type == PermissionType.Allow;
		}
	}
}

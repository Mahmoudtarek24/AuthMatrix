using AuthMatrix.Dto_s;
using AuthMatrix.Models;
using AuthMatrix.Models.UserModel;
using AuthMatrix.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuthMatrix.Services
{
	public class GroupServices : IGroupServices
	{
		private readonly ApplicationDbContext context;
		public GroupServices(ApplicationDbContext context)
		{
			this.context = context;
		}

		public async Task<bool> AssignPermissionAsync(int groupId, AssignPermissionDto dto)
		{
			var group = await context.Groups.FindAsync(groupId);
			if (group is null)
				return false;

			var permission = await context.Permissions.FindAsync(dto.PermissionId);
			if (permission is null)
				return false;

			var existingGroupPermission = await context.GroupPermissions
				.FirstOrDefaultAsync(gp =>
					gp.GroupId == groupId &&
					gp.PermissionId == dto.PermissionId);

			if (existingGroupPermission != null)
			{
				existingGroupPermission.Type = dto.Type;
				existingGroupPermission.ExpiresAt = dto.ExpiresAt;
			}
			else
			{
				var groupPermission = new GroupPermission
				{
					GroupId = groupId,
					PermissionId = dto.PermissionId,
					Type = dto.Type,
					ExpiresAt = dto.ExpiresAt
				};
				await context.GroupPermissions.AddAsync(groupPermission);
			}

			await context.SaveChangesAsync();
			return true;
		}
		public async Task<GroupDto> CreateGroupAsync(CreateGroupDto dto)
		{
			var group = new Group
			{
				Name = dto.Name,
				Priority = dto.Priority,
			};

			context.Groups.Add(group);
			await context.SaveChangesAsync();

			var groupData = await GetGroupData(group.Id);

			return groupData;
		}

		public async Task<bool> DeleteGroupAsync(int id)
		{
			var group = await context.Groups
				.Include(g => g.UserGroups)  // ✅ Include للتحقق
				.FirstOrDefaultAsync(g => g.Id == id);

			if (group is null)
				return false;

			// ✅ Check: هل فيه Users؟
			if (group.UserGroups.Any())
				throw new ValidationException("Cannot delete group with users");

			context.Groups.Remove(group);
			await context.SaveChangesAsync();
			return true;
		}
		public async Task<List<GroupDto>> GetAllGroupsAsync()
		{
			var groupData = await context.Groups
				.Select(e => new GroupDto
				{
					Id = e.Id,
					Name = e.Name,
					Priority = e.Priority,
					TotalUsers = e.UserGroups.Count(),
					TotalPermissions = e.GroupPermissions.Count(),
				}).ToListAsync();
			return groupData;
		}

		public async Task<GroupDto> GetGroupByIdAsync(int id)
		{
			var groupData = await GetGroupData(id);

			return groupData;
		}

		public async Task<List<PermissionDto>> GetGroupPermissionsAsync(int groupId)
		{
			var permissions = await context.GroupPermissions
				.AsNoTracking()
				.Where(gp => gp.GroupId == groupId)
				.Include(gp => gp.Permission)
					.ThenInclude(p => p.Resource)
				.Select(gp => new PermissionDto
				{
					PermissionId = gp.PermissionId,
					Action = gp.Permission.Action,
					ResourceName = gp.Permission.Resource.Name,
					ResourceId = gp.Permission.ResourceId,
					PermissionScope = gp.Permission.Scope,
					ScopeFilter = gp.Permission.ScopeFilter,
					Type = gp.Type,
					ExpiresAt = gp.ExpiresAt
				})
				.ToListAsync();

			return permissions;
		}

		public async Task<bool> RevokePermissionAsync(int groupId, int permissionId)
		{
			var groupPermission = await context.GroupPermissions.FirstOrDefaultAsync(e => e.GroupId == groupId && e.PermissionId == permissionId);

			if (groupPermission is null)
				return false;

			context.GroupPermissions.Remove(groupPermission);
			await context.SaveChangesAsync();

			return true;
		}

		public async Task<GroupDto> UpdateGroupAsync(int id, UpdateGroupDto dto)
		{
			var group = await context.Groups.FindAsync(id);
			if (group is null)
				return null;
			
			group.Name = dto.Name;
			group.Priority = dto.Priority;
			context.Groups.Update(group);
			await context.SaveChangesAsync();

			var groupData = await GetGroupData(group.Id);
			return groupData;		
		}

		private async Task<GroupDto> GetGroupData(int id)
		{
			var groupData = await context.Groups.Where(e => e.Id == id)
					.Select(e => new GroupDto
					{
						Id = e.Id,
						Name = e.Name,
						Priority = e.Priority,
						TotalUsers = e.UserGroups.Count(),
						TotalPermissions = e.GroupPermissions.Count(),
					}).FirstOrDefaultAsync();
			return groupData;

		}
	}
}

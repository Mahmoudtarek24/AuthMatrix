using AuthMatrix.Dto_s;
using AuthMatrix.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AuthMatrix.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GroupsController : ControllerBase
	{
		private readonly IGroupServices _groupService;

		public GroupsController(IGroupServices groupService)
		{
			_groupService = groupService;
		}

		[HttpPost]
		public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto dto)
		{
			try
			{
				var result = await _groupService.CreateGroupAsync(dto);
				return CreatedAtAction(nameof(GetGroupById), new { id = result.Id }, result);
			}
			catch (ValidationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Internal server error" });
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllGroups()
		{
			var groups = await _groupService.GetAllGroupsAsync();
			return Ok(groups);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetGroupById(int id)
		{
			var group = await _groupService.GetGroupByIdAsync(id);
			if (group == null)
				return NotFound();

			return Ok(group);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateGroup(int id, [FromBody] UpdateGroupDto dto)
		{
			try
			{
				var result = await _groupService.UpdateGroupAsync(id, dto);
				if (result == null)
					return NotFound();

				return Ok(result);
			}
			catch (ValidationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteGroup(int id)
		{
			try
			{
				var result = await _groupService.DeleteGroupAsync(id);
				if (!result)
					return NotFound();

				return NoContent();
			}
			catch (ValidationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("{id}/permissions")]
		public async Task<IActionResult> GetGroupPermissions(int id)
		{
			var permissions = await _groupService.GetGroupPermissionsAsync(id);
			return Ok(permissions);
		}

		[HttpPost("{groupId}/permissions")]
		public async Task<IActionResult> AssignPermission(int groupId, [FromBody] AssignPermissionDto dto)
		{
			var result = await _groupService.AssignPermissionAsync(groupId, dto);
			if (!result)
				return NotFound();

			return Ok();
		}

		[HttpDelete("{groupId}/permissions/{permissionId}")]
		public async Task<IActionResult> RevokePermission(int groupId, int permissionId)
		{
			var result = await _groupService.RevokePermissionAsync(groupId, permissionId);
			if (!result)
				return NotFound();

			return NoContent();
		}
	}
}

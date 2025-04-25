using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services
{
    public class ParentService
    {
        private readonly UserService _userService;
        private readonly ParentRepository _parentRepo;
        private readonly EmailService _emailService;

        public ParentService(UserService userService, ParentRepository repo, EmailService emailService)
        {
            _userService = userService;
            _parentRepo = repo;
            _emailService = emailService;
        }
        public async Task<List<ParentDTO>> GetAllParentsAsync() => await _parentRepo.GetAllParentsAsync();
        public async Task<ParentDTO?> GetParentByIdAsync(int id) => await _parentRepo.GetParentByIdAsync(id);
        public async Task<ParentDTO> AddParentAsync(CreateParentDTO dto)
        {
            int insertedId = await _parentRepo.AddParentAsync(dto);
            var parent = await _parentRepo.GetParentByIdAsync(insertedId);
            return new ParentDTO
            {
                Id = insertedId,
                Address = parent?.Address ?? string.Empty,
                UserId = parent?.UserId ?? 0,
                User = parent?.User
            };
        }

        public async Task<ParentDTO> CreateParentWithUserAsync(CreateUserWithParentDTO dto)
        {
            int parentRoleId = 3;

            var createdUser = await _userService.AddUserAsync(new CreateUserDTO
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
                RoleId = parentRoleId
            });


            var createdParent = await this.AddParentAsync(new CreateParentDTO
            {
                UserId = createdUser.Id,
                Address = dto.Address
            });

            return new ParentDTO
            {
                Id = createdParent.Id,
                UserId = createdParent.UserId,
                Address = createdParent.Address,
                User = new UserDTO
                {
                    Id = createdUser.Id,
                    FirstName = createdUser.FirstName,
                    LastName = createdUser.LastName,
                    Email = createdUser.Email,
                    RoleName = createdUser.RoleName
                }
            };
        }
        public async Task<ParentDTO?> UpdateParentAsync(int id, UpdateParentDTO dto)
        {
            var updated = await _parentRepo.UpdateParentAsync(id, dto);
            if (!updated) return null;

            var updatedParent = await _parentRepo.GetParentByIdAsync(id);
            return updatedParent;
        }

        public async Task<ParentDTO?> UpdateParentWithUserAsync(int id, UpdateParentWithUserDTO dto)
        {
            var parent = await this.GetParentByIdAsync(id);

            await _userService.UpdateUserAsync(parent.UserId, new UpdateUserDTO
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
            });

            var updatedParent = await this.UpdateParentAsync(id, new UpdateParentDTO
            {
                Address = dto.Address,
            });
            return updatedParent;

        }
        public async Task<bool> DeleteParentAsync(int id)
        {
            var parent = await this.GetParentByIdAsync(id);
            bool isParentDeleted = await _parentRepo.DeleteParentAsync(id);
            if (isParentDeleted)
            {
                return await _userService.DeleteUserAsync(parent.UserId);
            }
            return false;
        }
        public async Task<List<StudentInfoDTO>> GetParentChildrenByIdAsync(int id)
            => await _parentRepo.GetParentChildrenByIdAsync(id);
    }
}

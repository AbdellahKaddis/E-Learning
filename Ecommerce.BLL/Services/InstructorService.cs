using Ecommerce.DAL.Repositories;
using Ecommerce.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services
{
    public class InstructorService
    {
        private readonly UserService _userService;
        private readonly InstructorRepository _instructorRepo;


        public InstructorService(UserService userService, InstructorRepository repo)
        {
            _userService = userService;
            _instructorRepo = repo;
        }
        public async Task<List<InstructorDTO>> GetAllInstructorsAsync() => await _instructorRepo.GetAllInstructorsAsync();
        public async Task<InstructorDTO?> GetInstructorByIdAsync(int id) => await _instructorRepo.GetInstructorByIdAsync(id);
        public async Task<InstructorDTO> AddInstructorAsync(CreateInstructorDTO dto)
        {
            int insertedId = await _instructorRepo.AddInstructorAsync(dto);
            var instructor = await _instructorRepo.GetInstructorByIdAsync(insertedId);
            return new InstructorDTO
            {
                Id = insertedId,
                Address = instructor?.Address ?? string.Empty,
                Cin = instructor?.Cin ?? string.Empty,
                Specialite = instructor?.Specialite ?? string.Empty,
                Telephone = instructor?.Telephone ?? string.Empty,
                User = instructor?.User
            };
        }

        public async Task<InstructorDTO> CreateInstructorWithUserAsync(CreateUserWithInstructorDTO dto)
        {
            int parentRoleId = 2;

            var createdUser = await _userService.AddUserAsync(new CreateUserDTO
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
                RoleId = parentRoleId
            });


            var createdInstructor = await this.AddInstructorAsync(new CreateInstructorDTO
            {
                UserId = createdUser.Id,
                Address = dto.Address,
                Cin = dto.Cin,
                Telephone = dto.Telephone,
                Specialite = dto.Specialite
            });

            return new InstructorDTO
            {
                Id = createdInstructor.Id,
                Cin = createdInstructor.Cin,
                Address = createdInstructor.Address,
                Telephone = createdInstructor.Telephone,
                Specialite = createdInstructor.Specialite,
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
        public async Task<InstructorDTO?> UpdateInstructorAsync(int id, UpdateInstructorDTO dto)
        {
            var updated = await _instructorRepo.UpdateInstructorAsync(id, dto);
            if (!updated) return null;

            var updatedInstructor = await _instructorRepo.GetInstructorByIdAsync(id);
            return updatedInstructor;
        }

        public async Task<InstructorDTO?> UpdateParentWithUserAsync(int id, UpdateInstructorWithUserDTO dto)
        {
            var instructor = await this.GetInstructorByIdAsync(id);

            await _userService.UpdateUserAsync(instructor.User.Id, new UpdateUserDTO
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
            });

            var updatedInstructor = await this.UpdateInstructorAsync(id, new UpdateInstructorDTO
            {
                Address = dto.Address,
                Cin = dto.Cin,
                Telephone = dto.Telephone,
                Specialite = dto.Specialite
            });
            return updatedInstructor;

        }
        public async Task<bool> DeleteInstructorAsync(int id)
        {
            var instructor = await this.GetInstructorByIdAsync(id);
            bool isInstructorDeleted = await _instructorRepo.DeleteInstructorAsync(id);
            if (isInstructorDeleted)
            {
                return await _userService.DeleteUserAsync(instructor.User.Id);
            }
            return false;
        }

        public async Task CreateInstructorOnlyAsync(CreateInstructorDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateInstructorWithUserAsync(int id, UpdateInstructorWithUserDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}

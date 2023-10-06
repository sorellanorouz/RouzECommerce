using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rouz.API.DTOs;
using Rouz.API.Entities;

namespace Rouz.API.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UserService(ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            List<User> users = await context.Users.ToListAsync();
            return mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserAsync(int id)
        {
            User user = await context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            return mapper.Map<UserDTO>(user);
        }

        public async Task<int> CreateUserAsync(UserDTO userDTO)
        {
            User newUser = mapper.Map<User>(userDTO);
            newUser.RegistrationDate = DateTime.Now;

            context.Add(newUser);
            await context.SaveChangesAsync();

            return newUser.Id;
        }

        public async Task<bool> ModifyUserAsync(int id, UserDTO userDTO)
        {
            User user = await context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }

            user.Name = userDTO.Name;
            user.Email = userDTO.Email;
            user.Password = userDTO.Password;

            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            User user = await context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return true;
        }

        private bool UserExists(int id)
        {
            return context.Users.Any(e => e.Id == id);
        }
    }
}

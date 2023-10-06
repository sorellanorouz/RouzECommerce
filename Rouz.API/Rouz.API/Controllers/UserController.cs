using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rouz.API.DTOs;
using Rouz.API.Entities;
using Rouz.API.Services;

namespace Rouz.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserService userService;

        public UserController(IConfiguration configuration,
            ApplicationDbContext context,
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            UserService userService)
        {
            this.configuration = configuration;
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            List<UserDTO> users = await userService.GetUsersAsync();
            return users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            User? user = await context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return mapper.Map<UserDTO>(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                int newUserId = await userService.CreateUserAsync(userDTO);

                return CreatedAtAction(nameof(GetUser), new { id = newUserId }, await userService.GetUserAsync(newUserId));
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyUser(int id, UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                bool success = await userService.ModifyUserAsync(id, userDTO);

                if (success)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool success = await userService.DeleteUserAsync(id);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}

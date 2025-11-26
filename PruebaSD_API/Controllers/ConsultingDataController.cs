using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // 1. CREATE: POST api/User
    [HttpPost]
    public async Task<ActionResult<User>> AddUser(User user)
    {
        
        await _userRepository.AddUserAsync(user);

        await _userRepository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.UsuId }, user);
    }

    // 2. READ: Búsqueda y Obtener Todos
    // GET api/User?search=...
    // GET api/User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers(
        [FromQuery] string? search = null) // Parámetro opcional de búsqueda
    {
        IEnumerable<User> users;

        if (!string.IsNullOrWhiteSpace(search))
        {
            // Búsqueda por ID o Nombre
            users = await _userRepository.SearchUsersAsync(search);
        }
        else
        {
            // Obtener todos
            users = await _userRepository.GetUsersAsync();
        }

        return Ok(users);
    }

    // 3. READ: Obtener por ID
    // GET api/User/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    // 4. UPDATE: PUT api/User/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        if (id != user.UsuId)
        {
            return BadRequest(new { message = "El ID de la ruta no coincide con el ID del usuario." });
        }

        var existingUser = await _userRepository.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        existingUser.Nombre = user.Nombre;
        existingUser.Apellido = user.Apellido;

        try
        {
            await _userRepository.UpdateUserAsync(existingUser);
            await _userRepository.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _userRepository.GetUserByIdAsync(id) == null)
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        await _userRepository.DeleteUserAsync(id);
        await _userRepository.SaveChangesAsync();

        return NoContent();
    }
}
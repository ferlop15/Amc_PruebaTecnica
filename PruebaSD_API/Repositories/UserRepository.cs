using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly PruebaSDContext _context;

    public UserRepository(PruebaSDContext context)
    {
        _context = context;
    }

  
    public async Task AddUserAsync(User user)
    {
      
        var maxId = await _context.User
            .MaxAsync(u => (int?)u.UsuId) ?? 0; 

        user.UsuId = maxId + 1;

        await _context.User.AddAsync(user);
    }

    // READ: Obtener todos
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _context.User.ToListAsync();
    }

    // READ: Obtener por ID
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.User.FindAsync(id);
    }

    // READ: Búsqueda (ID o Nombre)
    public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetUsersAsync();
        }

        bool isNumeric = int.TryParse(searchTerm, out int id);

        var query = _context.User.AsQueryable();

        if (isNumeric)
        {
            query = query.Where(u => u.UsuId == id);
        }
        else
        {
            query = query.Where(u => u.Nombre.Contains(searchTerm) || u.Apellido.Contains(searchTerm));
        }

        return await query.ToListAsync();
    }

    // UPDATE
    public async Task UpdateUserAsync(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        await Task.CompletedTask; 
    }

    // DELETE
    public async Task DeleteUserAsync(int id)
    {
        var userToDelete = await _context.User.FindAsync(id);
        if (userToDelete != null)
        {
            _context.User.Remove(userToDelete);
        }
    }

    // Guarda los cambios en la base de datos
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
public interface IUserRepository
{
    // READ (Lectura)
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);

    // CREATE (Creación)
    Task AddUserAsync(User user);

    // UPDATE (Actualización)
    Task UpdateUserAsync(User user);

    // DELETE (Eliminación)
    Task DeleteUserAsync(int id);

    // Método para guardar cambios
    Task<bool> SaveChangesAsync();
}
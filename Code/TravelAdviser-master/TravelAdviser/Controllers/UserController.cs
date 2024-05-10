
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TravelAdviser.Entities;

namespace TravelAdviser;

public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        var users = new List<User>();

        // SQL query to select all users
        var sqlQuery = "SELECT id, admin_id, email, first_name, last_name, password FROM Users";

        using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
        {
            connection.Open();

            using (var command = new SqlCommand(sqlQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User
                        {
                            id = (int)reader["id"],
                            admin_id = (int)reader["admin_id"],
                            email = (string)reader["email"],
                            first_name = (string)reader["first_name"],
                            last_name = (string)reader["last_name"],
                            password = (string)reader["password"]
                        };
                        users.Add(user);
                    }
                }
            }
        }

        return users;
    }
    [HttpGet("GetUsersByAdminId/{admin_id}")]
    public async Task<IEnumerable<User>> GetUsersByAdminId(int admin_id)
    {
        var users = new List<User>();

        var sqlQuery = $"Select * from users WHERE admin_id = {admin_id}";

        using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
        {
            connection.Open();

            using (var command = new SqlCommand(sqlQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User
                        {
                            id = (int)reader["id"],
                            admin_id = (int)reader["admin_id"],
                            email = (string)reader["email"],
                            first_name = (string)reader["first_name"],
                            last_name = (string)reader["last_name"],
                            password = (string)reader["password"]
                        };
                        users.Add(user);
                    }
                }
            }
        }

        return users;
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        if (ModelState.IsValid)
        {
            var sqlQuery = "INSERT INTO Users (admin_id, email, first_name, last_name, password) " +
                           "VALUES (@admin_id, @email, @first_name, @last_name, @password)";

            await _context.Database.ExecuteSqlRawAsync(sqlQuery,
                new SqlParameter("@admin_id", user.admin_id),
                new SqlParameter("@email", user.email),
                new SqlParameter("@first_name", user.first_name),
                new SqlParameter("@last_name", user.last_name),
                new SqlParameter("@password", user.password));

            return CreatedAtAction(nameof(GetUsers), user);
        }
        return BadRequest(ModelState);
    }

    [HttpPut("UpdateUser/{id}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        if (id != user.id)
        {
            return BadRequest();
        }

        var sqlQuery = "UPDATE Users " +
                       "SET admin_id = @admin_id, " +
                       "email = @email, " +
                       "first_name = @first_name, " +
                       "last_name = @last_name, " +
                       "password = @password " +
                       "WHERE id = @id";

        await _context.Database.ExecuteSqlRawAsync(sqlQuery,
            new SqlParameter("@admin_id", user.admin_id),
            new SqlParameter("@email", user.email),
            new SqlParameter("@first_name", user.first_name),
            new SqlParameter("@last_name", user.last_name),
            new SqlParameter("@password", user.password),
            new SqlParameter("@id", id));

        return NoContent();
    }

    [HttpDelete("DeleteUser/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var sqlQuery = "DELETE FROM Users WHERE id = @id";

        await _context.Database.ExecuteSqlRawAsync(sqlQuery,
            new SqlParameter("@id", id));

        return NoContent();
    }
}
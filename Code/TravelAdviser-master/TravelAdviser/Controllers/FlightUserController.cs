
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TravelAdviser.Entities;
using static TravelAdviser.FlightController;

namespace TravelAdviser;

public class FlightUserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FlightUserController(ApplicationDbContext context)
    {
        _context = context;
    }

    //[HttpGet("GetFlightUsers")]
    //public async Task<IEnumerable<FlightUser>> GetFlightUsers()
    //{
    //    var flightUsers = new List<FlightUser>();

    //    var sqlQuery = "SELECT * FROM flight_user";

    //    using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
    //    {
    //        connection.Open();

    //        using (var command = new SqlCommand(sqlQuery, connection))
    //        {
    //            using (var reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    var flightUser = new FlightUser
    //                    {
    //                        flight_id = (int)reader["flight_id"],
    //                        user_id = (int)reader["user_id"],
    //                        email = (string)reader["email"],
    //                        travellers_number = (int)reader["travellers_number"]
    //                    };
    //                    flightUsers.Add(flightUser);
    //                }
    //            }
    //        }
    //    }

    //    return flightUsers;
    //}

    [HttpPost("CreateFlightUser")]
    public async Task<ActionResult<FlightUser>> CreateFlightUser(FlightUser flightUser)
    {
        if (ModelState.IsValid)
        {
            var sqlQuery = "INSERT INTO flight_user (flight_id, user_id, email, travellers_number) " +
                           "VALUES (@flight_id, @user_id, @email, @travellers_number)";

            await _context.Database.ExecuteSqlRawAsync(sqlQuery,
                new SqlParameter("@flight_id", flightUser.flight_id),
                new SqlParameter("@user_id", flightUser.user_id),
                new SqlParameter("@email", flightUser.email),
                new SqlParameter("@travellers_number", flightUser.travellers_number));

            return CreatedAtAction(nameof(GetFlightUsers), new { id = flightUser.flight_id }, flightUser);
        }
        return BadRequest(ModelState);
    }

    [HttpPut("UpdateFlightUser/{flightId}/{userId}")]
    public async Task<IActionResult> UpdateFlightUser(int flightId, int userId, FlightUser flightUser)
    {
        if (flightId != flightUser.flight_id || userId != flightUser.user_id)
        {
            return BadRequest();
        }

        var sqlQuery = "UPDATE flight_user " +
                       "SET email = @email, " +
                       "travellers_number = @travellers_number " +
                       "WHERE flight_id = @flight_id AND user_id = @user_id";

        await _context.Database.ExecuteSqlRawAsync(sqlQuery,
            new SqlParameter("@email", flightUser.email),
            new SqlParameter("@travellers_number", flightUser.travellers_number),
            new SqlParameter("@flight_id", flightId),
            new SqlParameter("@user_id", userId));

        return NoContent();
    }

    [HttpDelete("DeleteFlightUser/{flightId}/{userId}")]
    public async Task<IActionResult> DeleteFlightUser(int flightId, int userId)
    {
        var sqlQuery = "DELETE FROM flight_user WHERE flight_id = @flight_id AND user_id = @user_id";

        await _context.Database.ExecuteSqlRawAsync(sqlQuery,
            new SqlParameter("@flight_id", flightId),
            new SqlParameter("@user_id", userId));

        return NoContent();
    }

    [HttpGet("GetFlightUsers")]
    public IEnumerable<FlightUserData> GetFlightUsers()
    {
        var flightUsersData = new List<FlightUserData>();

        // SQL query to select users associated with flights
        var sqlQuery = @"
        SELECT u.*, fu.travellers_number, f.*
        FROM users u
        JOIN flight_user fu ON u.id = fu.user_id
        JOIN flight f ON f.flight_id = fu.flight_id";

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
                            email = (string)reader["email"],
                            // Populate other properties of the User object
                        };

                        var flight = new Flight
                        {
                            flight_id = (int)reader["flight_id"],
                            location = (string)reader["location"],
                            // Populate other properties of the Flight object
                        };

                        var flightUserData = new FlightUserData
                        {
                            User = user,
                            Flight = flight,
                            TravellersNumber = (int)reader["travellers_number"],
                            // Populate other properties of the FlightUserData object
                        };

                        flightUsersData.Add(flightUserData);
                    }
                }
            }
        }

        return flightUsersData;
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TravelAdviser.Entities;

namespace TravelAdviser;

public partial class FlightController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FlightController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("GetFlights")]
    public IEnumerable<Flight> GetFlights()
    {
        var flights = new List<Flight>();

        // SQL query to select all users
        var sqlQuery = "SELECT * FROM flight";

        using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
        {
            connection.Open();

            using (var command = new SqlCommand(sqlQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new Flight
                        {
                            flight_id = (int)reader["flight_id"],
                            location = (string)reader["location"],
                            price = (double)reader["price"],
                            arrive_date = (DateTime)reader["arrive_date"],
                            departure_date = (DateTime)reader["departure_date"],
                            photo = reader["photo"] == DBNull.Value ? null : (byte[])reader["photo"],
                            status = (string)reader["status"],
                        };
                        flights.Add(user);
                    }
                }
            }
        }

        return flights;
    }

    [HttpPost("CreateFlight")]
    public async Task<ActionResult<Flight>> CreateFlight(Flight flight)
    {
        if (ModelState.IsValid)
        {
            var sqlQuery = "INSERT INTO flight (location, price, arrive_date, departure_date, photo, status) " +
                           "VALUES (@location, @price, @arrive_date, @departure_date, @photo, @status)";

            await _context.Database.ExecuteSqlRawAsync(sqlQuery,
                new SqlParameter("@location", flight.location),
                new SqlParameter("@price", flight.price),
                new SqlParameter("@arrive_date", flight.arrive_date),
                new SqlParameter("@departure_date", flight.departure_date),
                new SqlParameter("@photo", flight.photo),
                new SqlParameter("@status", flight.status));

            return CreatedAtAction(nameof(GetFlights), new { id = flight.flight_id }, flight);
        }
        return BadRequest(ModelState);
    }

    [HttpPut("UpdateFlight/{id}")]
    public async Task<IActionResult> UpdateFlight(int id, Flight flight)
    {
        if (id != flight.flight_id)
        {
            return BadRequest();
        }

        var sqlQuery = "UPDATE flight " +
                       "SET location = @location, " +
                       "price = @price, " +
                       "arrive_date = @arrive_date, " +
                       "departure_date = @departure_date, " +
                       "photo = @photo, " +
                       "status = @status " +
                       "WHERE flight_id = @id";

        await _context.Database.ExecuteSqlRawAsync(sqlQuery,
            new SqlParameter("@location", flight.location),
            new SqlParameter("@price", flight.price),
            new SqlParameter("@arrive_date", flight.arrive_date),
            new SqlParameter("@departure_date", flight.departure_date),
            new SqlParameter("@photo", flight.photo),
            new SqlParameter("@status", flight.status),
            new SqlParameter("@id", id));

        return NoContent();
    }

    [HttpDelete("DeleteFlight/{id}")]
    public async Task<IActionResult> DeleteFlight(int id)
    {
        var sqlQuery = "DELETE FROM flight WHERE flight_id = @id";

        await _context.Database.ExecuteSqlRawAsync(sqlQuery,
            new SqlParameter("@id", id));

        return NoContent();
    }    

}
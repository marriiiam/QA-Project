using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TravelAdviser.Entities;

namespace TravelAdviser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RatingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetRatings")]
        public async Task<IEnumerable<Rating>> GetRatings()
        {
            var ratings = new List<Rating>();

            var sqlQuery = "SELECT * FROM rating";

            using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var rating = new Rating
                            {
                                id = (int)reader["id"],
                                user_id = (int)reader["user_id"],
                                flight_id = (int)reader["flight_id"],
                                rating = reader["rating"] == DBNull.Value ? null : (int?)reader["rating"],
                                feedback = reader["feedback"] == DBNull.Value ? null : (string)reader["feedback"]
                            };
                            ratings.Add(rating);
                        }
                    }
                }
            }

            return ratings;
        }

        [HttpGet("GetRatingsByFlightId/{flight_id}")]
        public async Task<IEnumerable<Rating>> GetRatingsByFlightId(int flight_id)
        {
            var ratings = new List<Rating>();

            var sqlQuery = $"Select * from rating WHERE flight_id = {flight_id}";

            using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(sqlQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var rating = new Rating
                            {
                                id = (int)reader["id"],
                                user_id = (int)reader["user_id"],
                                flight_id = (int)reader["flight_id"],
                                rating = reader["rating"] == DBNull.Value ? null : (int?)reader["rating"],
                                feedback = reader["feedback"] == DBNull.Value ? null : (string)reader["feedback"]
                            };
                            ratings.Add(rating);
                        }
                    }
                }
            }

            return ratings;
        }

        [HttpPost("CreateRating")]
        public async Task<ActionResult<Rating>> CreateRating(Rating rating)
        {
            if (ModelState.IsValid)
            {
                var sqlQuery = "INSERT INTO rating (user_id, flight_id, rating, feedback) " +
                               "VALUES (@user_id, @flight_id, @rating, @feedback)";

                await _context.Database.ExecuteSqlRawAsync(sqlQuery,
                    new SqlParameter("@user_id", rating.user_id),
                    new SqlParameter("@flight_id", rating.flight_id),
                    new SqlParameter("@rating", rating.rating ?? (object)DBNull.Value),
                    new SqlParameter("@feedback", rating.feedback ?? (object)DBNull.Value));

                return CreatedAtAction(nameof(GetRatings), new { id = rating.id }, rating);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("UpdateRating/{id}")]
        public async Task<IActionResult> UpdateRating(int id, Rating rating)
        {
            if (id != rating.id)
            {
                return BadRequest();
            }

            var sqlQuery = "UPDATE rating " +
                           "SET user_id = @user_id, " +
                           "flight_id = @flight_id, " +
                           "rating = @rating, " +
                           "feedback = @feedback " +
                           "WHERE id = @id";

            await _context.Database.ExecuteSqlRawAsync(sqlQuery,
                new SqlParameter("@user_id", rating.user_id),
                new SqlParameter("@flight_id", rating.flight_id),
                new SqlParameter("@rating", rating.rating ?? (object)DBNull.Value),
                new SqlParameter("@feedback", rating.feedback ?? (object)DBNull.Value),
                new SqlParameter("@id", id));

            return NoContent();
        }

        [HttpDelete("DeleteRating/{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            var sqlQuery = "DELETE FROM rating WHERE id = @id";

            await _context.Database.ExecuteSqlRawAsync(sqlQuery,
                new SqlParameter("@id", id));

            return NoContent();
        }
    }
}

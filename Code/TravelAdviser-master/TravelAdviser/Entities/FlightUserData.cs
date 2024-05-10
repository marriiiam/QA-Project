using TravelAdviser.Entities;

namespace TravelAdviser;

public partial class FlightController
{
    public class FlightUserData
    {
        public User User { get; set; }
        public Flight Flight { get; set; }
        public int TravellersNumber { get; set; }
        // Add any other properties you need from the joined tables
    }

}
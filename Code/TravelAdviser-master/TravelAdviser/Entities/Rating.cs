namespace TravelAdviser.Entities;

public class Rating
{
    public int id { get; set; }
    public int user_id { get; set; }
    public int flight_id { get; set; }
    public int? rating { get; set; }
    public string feedback { get; set; }
}

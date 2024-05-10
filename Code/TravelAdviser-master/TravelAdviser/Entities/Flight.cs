namespace TravelAdviser.Entities;

public class Flight
{
    public int flight_id { get; set; }
    public string location { get; set; }
    public double price { get; set; }
    public DateTime arrive_date { get; set; }
    public DateTime departure_date { get; set; }
    public byte[]? photo { get; set; } // Assuming you store photos as binary data
    public string status { get; set; }
}

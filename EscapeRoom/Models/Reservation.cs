namespace EscapeRoom.Models;

public class Reservation
{
    public int ID { get; set; }
    public string Name { get; set; }
    public DateTime ReservationStart { get; set; }
    public DateTime ReservationEnd { get; set; }
    public int RoomID { get; set; }
    public Room Room { get; set;}
    public IEnumerable<User> Workers { get; set; }

    public string ClientID { get; set; }
    public User Client { get; set; }
    public int NumberOfPeople { get; set; }

}

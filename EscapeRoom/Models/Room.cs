namespace EscapeRoom.Models;

public class Room
{
    public int ID { get; set; }

    public string RoomName { get; set; }

    public bool IsOccupied { get; set;}

    public string LongDescription { get; set; }

    public string Image {  get; set; }
}

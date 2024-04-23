namespace EscapeRoom.Models;

public class User
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserType UserType { get; set; }
}

public enum UserType
{
    Admin, 
    RoomWorker, 
    DeskWorker,
    Client
}

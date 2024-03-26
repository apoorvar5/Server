namespace Server.DTO
{
    public class PlayerCount
    {
        public int ClubId { get; set; }
        public required string ClubName { get; set; }
        public int CountPlayer { get; set; }
    }
}
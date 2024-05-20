namespace EventTicketAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public DateTime DateRegistered { get; set; }
        public string? Verify { get; set; }

        public DateTime? VerifiedAt { get; set; }
        public string? PasswordChangeVerification {  get; set; }
        public DateTime? ExpiresPasswordChangeDate { get; set; }
        

        public ICollection<TicketSale> TicketSale  { get; set; }
        public ICollection<Favorite> Favorite { get; set; }
    }
}

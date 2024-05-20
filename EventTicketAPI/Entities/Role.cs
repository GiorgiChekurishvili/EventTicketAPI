﻿namespace EventTicketAPI.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> User { get; set; }
    }
}

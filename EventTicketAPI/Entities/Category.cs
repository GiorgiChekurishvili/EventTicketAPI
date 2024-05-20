﻿namespace EventTicketAPI.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public ICollection<Event> Event { get; set; }
    }
}

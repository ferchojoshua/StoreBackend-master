﻿namespace Store.Entities
{
    public class Almacen
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}

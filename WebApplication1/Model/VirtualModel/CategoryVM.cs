﻿namespace WebApplication1.Model.VirtualModel
{
    public class CategoryVM
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
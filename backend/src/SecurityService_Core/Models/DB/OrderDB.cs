﻿namespace SecurityService_Core.Models.DB
{
    public class OrderDB : BaseEntity
    {
        public DateTime? Date { get; set; }
        public int State { get; set; }
        public string? Status { get; set; }
        public string? SNILS { get; set; }
        public string? FIO { get; set; }
        public string? ContactData { get; set; }
        public string? Type { get; set; }
        public string? Body { get; set; }
        public string? SupportMeasures { get; set; }
    }
}

﻿namespace WebApplication1.Model.VirtualModel
{
    public class Response
    {
        public int resultCd { get; set; }
        public string? Error { get; set; }
        public string? MessageCode { get; set; }

        public object? Data { get; set; }
    }
}

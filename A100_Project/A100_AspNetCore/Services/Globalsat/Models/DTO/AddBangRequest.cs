﻿using System;

namespace A100_AspNetCore.Services.Globalsat.Models.DTO
{
    public class AddBangRequest
    {
        public string SensorId { get; set; }
        public float? Strength { get; set; }
        public string Status { get; set; }
        public DateTime BangDate { get; set; }
        public int ResoultID { get; set; }
    }
}
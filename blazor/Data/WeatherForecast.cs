using System;

namespace tmp91.Data
{
    public class WeatherForecast
    {
        public string Id { get; } = $"id{Guid.NewGuid().ToString().Substring(4)}";
        
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}

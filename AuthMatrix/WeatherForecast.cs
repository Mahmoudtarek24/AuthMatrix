namespace AuthMatrix
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    
        public void Add(int x =10)
        {
            int y = 10;// update line one one , update this line 
            int a = 10;
        }
    }
}

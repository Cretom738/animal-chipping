namespace Core.Exceptions
{
    public class AreaBordersIntersectException : BadRequestException
    {
        public AreaBordersIntersectException(
            string areaName, double longitude, double latitude) 
            : base($"Borders of area with name {areaName} intersect "
                  + $"at longitude {longitude} and latitude {latitude}")
        {
        }

        public AreaBordersIntersectException(
            string firstAreaName, string secondAreaName, double longitude, double latitude)
            : base($"Borders of areas {firstAreaName} and {secondAreaName} intersect" 
                  + $" at longitude {longitude} and latitude {latitude}")
        {
        }
    }
}

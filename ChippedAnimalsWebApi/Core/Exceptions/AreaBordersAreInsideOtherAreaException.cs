namespace Core.Exceptions
{
    public class AreaBordersAreInsideOtherAreaException : BadRequestException
    {
        public AreaBordersAreInsideOtherAreaException(string areaName) 
            : base($"Area {areaName} borders are inside or contain borders of other area")
        {
        }
    }
}

namespace Core.Exceptions
{
    public class AreaConsistsOfPartOfOtherAreaPointsException : BadRequestException
    {
        public AreaConsistsOfPartOfOtherAreaPointsException(string subArea, string fullArea) 
            : base($"Area {subArea} consists of the part of {fullArea} points")
        {
        }
    }
}

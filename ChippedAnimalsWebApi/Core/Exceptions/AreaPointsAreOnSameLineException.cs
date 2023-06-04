namespace Core.Exceptions
{
    public class AreaPointsAreOnSameLineException : BadRequestException
    {
        public AreaPointsAreOnSameLineException(string name) 
            : base($"Points of area {name} are on the same line")
        {
        }
    }
}

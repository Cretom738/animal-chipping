namespace Services.Processing
{
    public interface IGeohashService
    {
        string Encode(double latitude, double longitude, int precision);
    }
}

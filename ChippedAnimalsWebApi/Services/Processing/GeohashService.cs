using System.Text;

namespace Services.Processing
{
    public class GeohashService : IGeohashService
    {
        const string Base32Geohash = "0123456789bcdefghjkmnpqrstuvwxyz";

        public string Encode(double latitude, double longitude, int precision)
        {
            if (precision < 1)
            {
                return string.Empty;
            }
            double maxLongitude = 180D;
            double minLongitude = -180D;
            double maxLatitude = 90D;
            double minLatitude = -90D;
            int bitCount = 0;
            int base32CharacterIndex = 0;
            int base32CharacterBitCount = 0;
            StringBuilder geohashbuilder = new StringBuilder();
            while (geohashbuilder.Length < precision)
            {
                double middleCoordinate;
                if (bitCount % 2 == 0)
                {
                    middleCoordinate = (maxLongitude + minLongitude) / 2;
                    if (longitude > middleCoordinate)
                    {
                        base32CharacterIndex = (base32CharacterIndex << 1) | 1;
                        minLongitude = middleCoordinate;
                    }
                    else
                    {
                        base32CharacterIndex <<= 1;
                        maxLongitude = middleCoordinate;
                    }
                }
                else
                {
                    middleCoordinate = (maxLatitude + minLatitude) / 2;
                    if (latitude > middleCoordinate)
                    {
                        base32CharacterIndex = (base32CharacterIndex << 1) | 1;
                        minLatitude = middleCoordinate;
                    }
                    else
                    {
                        base32CharacterIndex <<= 1;
                        maxLatitude = middleCoordinate;
                    }
                }
                bitCount++;
                base32CharacterBitCount++;
                if (base32CharacterBitCount == 5)
                {
                    geohashbuilder.Append(Base32Geohash[base32CharacterIndex]);
                    base32CharacterBitCount = 0;
                    base32CharacterIndex = 0;
                }
            }
            return geohashbuilder.ToString();
        }
    }
}

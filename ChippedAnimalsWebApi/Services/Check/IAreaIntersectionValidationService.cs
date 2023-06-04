using Core.Models;

namespace Services.Check
{
    public interface IAreaIntersectionValidationService
    {
        void Validate(Area newArea, IList<Area> allAreas);
    }
}

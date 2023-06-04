using Core.Models;

namespace Services.Check
{
    public interface IAreaPointsCoincidenceValidationService
    {
        void Validate(Area newArea, IList<Area> allAreas);
    }
}

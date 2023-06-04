using Core.Exceptions;
using Core.Models;

namespace Services.Check
{
    public class AreaPointsCoincidenceValidationService : IAreaPointsCoincidenceValidationService
    {
        public void Validate(Area newArea, IList<Area> allAreas)
        {
            foreach (Area currentArea in allAreas)
            {
                if (currentArea.Id == newArea.Id)
                {
                    continue;
                }
                CheckNewAreaPointsForEqualtyWithOtherAreaPoints(newArea, currentArea);
                CheckNewAreaPointsForCoincidenceWithOtherAreaPoints(newArea, currentArea);
            }
        }

        void CheckNewAreaPointsForCoincidenceWithOtherAreaPoints(Area newArea, Area otherArea)
        {
            if (DoesOtherAreaContainsAllPointOfNewArea(newArea, otherArea))
            {
                throw new AreaConsistsOfPartOfOtherAreaPointsException(
                    newArea.Name, otherArea.Name);
            }
        }

        void CheckNewAreaPointsForEqualtyWithOtherAreaPoints(Area newArea, Area otherArea)
        {
            IList<AreaPoint> newAreaPoints = newArea.AreaPoints.ToList();
            IList<AreaPoint> otherAreaPoints = otherArea.AreaPoints.ToList();
            if (newAreaPoints.Count != otherAreaPoints.Count)
            {
                return;
            }
            for (int i = 0; i < otherAreaPoints.Count; i++)
            {
                if (newAreaPoints[0].Equals(otherAreaPoints[i]))
                {
                    int otherAreaIndex = i;
                    int newAreaIndex = 0;
                    do
                    {
                        otherAreaIndex++;
                        newAreaIndex++;
                        if (otherAreaIndex >= otherAreaPoints.Count)
                        {
                            otherAreaIndex = 0;
                        }
                    }
                    while (newAreaPoints.Count > newAreaIndex 
                        && newAreaPoints[newAreaIndex].Equals(otherAreaPoints[otherAreaIndex]));
                    if (newAreaIndex == newAreaPoints.Count)
                    {
                        throw new AreaWithPointCombinationAlreadyExistsException();
                    }
                }
            }
        }

        bool DoesOtherAreaContainsAllPointOfNewArea(Area newArea, Area otherArea)
        {
            return !newArea.AreaPoints
                .Except(otherArea.AreaPoints)
                .Any();
        }
    }
}

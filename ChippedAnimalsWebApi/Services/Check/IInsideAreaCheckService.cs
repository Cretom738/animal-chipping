using Core.Models;

namespace Services.Check
{
    public interface IInsideAreaCheckService
    {
        bool Check(Location location, Area area);
        bool Check(Area firstArea, Area secondArea);
        bool Check(Area area, IList<Area> areas);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Repos.Interfaces
{
    public interface IPointRepo
    {
        Task InsertAsync(Point p);
        Task InsertSelfieAsync(UriTime uriTime);
        Task<IEnumerable<Point>> GetActivityPointsAsync(int activityId);
        Task InsertSharedPointAsync(Point p);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITSSelfRunning.Services
{
    public interface IEventHubService
    {
        Task SendMsg(string msg);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;

namespace ITSSelfRunning.Services
{
    /// <summary>
    /// Class to manage upload to eventHub
    /// </summary>
    public class EventHubService : IEventHubService
    {
        private readonly EventHubClient _evc;

        public EventHubService(EventHubsConnectionStringBuilder cs)
        {
            _evc = EventHubClient.CreateFromConnectionString(cs.ToString());
        }



        /// <summary>
        /// Upload string msg to eventhub
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task SendMsg(string msg)
        {
            await _evc.SendAsync(new EventData(Encoding.UTF8.GetBytes(msg)));
        }
        
    }
}

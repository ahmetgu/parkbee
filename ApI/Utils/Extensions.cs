using ApI.Data;
using System.Net.NetworkInformation;

namespace ApI.Utils
{
    public static class Extensions
    {
        public static async Task<bool> CheckAvailability(this Door door) => await Ping(door.IP);

        public static async Task<bool> Open(this Door door) => await Ping(door.IP);

        public static async Task<bool> Ping(string ipAddress)
        {
            try
            {
                var ping = new Ping();
                var reply = await ping.SendPingAsync(ipAddress, 3000);

                return reply.Status == IPStatus.Success;
            }
            catch 
            { 
                return false; 
            }
        }
    }
}

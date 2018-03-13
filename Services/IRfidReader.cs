using System;

namespace IotTest.Services
{
    public interface IRfidReader
    {
        void Start();

        void Stop();

        event EventHandler<RfidTagEventArgs> TagRead;

        (byte, byte, byte, byte)? GetLastReadTag();
    }
}

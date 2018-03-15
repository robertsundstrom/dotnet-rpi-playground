using System;

namespace IotTest.Services
{
    public interface IRfidReader
    {
        void Start();

        void Stop();

        event EventHandler<RfidTagEventArgs> TagRead;

        IObservable<RfidTagEventArgs> WhenTagRead { get; }

        (byte, byte, byte, byte)? GetLastReadTag();
    }
}

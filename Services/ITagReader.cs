using System;

namespace IotTest.Services
{
    public interface ITagReader
    {
        void Start();

        void Stop();

        event EventHandler<TagEventArgs> TagRead;

        (byte, byte, byte, byte)? GetLastReadTag();
    }
}

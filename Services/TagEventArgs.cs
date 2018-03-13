using System;

namespace IotTest.Services
{
    public class TagEventArgs : EventArgs
    {
        public TagEventArgs((byte, byte, byte, byte) tag) 
        {
            Tag = tag;
        }

        public  (byte, byte, byte, byte) Tag { get; }
    }
}

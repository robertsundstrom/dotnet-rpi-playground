using System;

namespace IotTest.Services
{
    public class RfidTagEventArgs : EventArgs
    {
        public RfidTagEventArgs((byte, byte, byte, byte) tag) 
        {
            Tag = tag;
        }

        public  (byte, byte, byte, byte) Tag { get; }
    }
}

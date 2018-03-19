using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Unosquare.Swan;
using IotTest.MessageHandlers;
using System.Reactive;
using System.Reactive.Linq;

namespace IotTest.Services
{
    public class RfidReader : IRfidReader
    {
        private CancellationTokenSource cts;
        private Task task;
        private (byte, byte, byte, byte)? lastReadTag;

        public event EventHandler<RfidTagEventArgs> TagRead;

        public (byte, byte, byte, byte)? GetLastReadTag() => lastReadTag;

        public IObservable<RfidTagEventArgs> WhenTagRead => Observable
            .FromEventPattern<RfidTagEventArgs>(
                handler => TagRead += handler,
                handler => TagRead -= handler)
            .Select(x => x.EventArgs);

        public void Start()
        {
            cts = new CancellationTokenSource();
            task = Task.Run(() =>
            {
                var mfrc522 = new RFIDControllerMfrc522();

                while (true)
                {
                    // Scan for cards
                    var response = mfrc522.DetectCard();

                    // If a card is found
                    if (response == RFIDControllerMfrc522.Status.AllOk)
                    {
                        if (cts.IsCancellationRequested)
                            return;

                        "Card detected".Info();
                    }

                    // Get the UID of the card
                    var response2 = mfrc522.ReadCardUniqueId();
                    var uid = response2.Data;

                    if (response2.Status == RFIDControllerMfrc522.Status.AllOk)
                    {
                        var readTag = (uid[0], uid[1], uid[2], uid[3]);

                        // if (!readTag.Equals(lastReadTag))
                        // {
                        lastReadTag = readTag;

                        // Print UID
                        $"Card read UID: {uid[0]}, {uid[1]}, {uid[2]}, {uid[3]}".Info();

                        TagRead?.Invoke(this, new RfidTagEventArgs(readTag));
                        // }
                    }
                }
            }, cts.Token);
        }

        public void Stop()
        {
            cts.Cancel();
        }
    }
}

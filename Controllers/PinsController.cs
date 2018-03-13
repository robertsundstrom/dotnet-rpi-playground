using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IotTest.MessageHandlers;
using Unosquare.RaspberryIO;
using Newtonsoft.Json;

namespace IotTest.Controllers
{
    [Produces("application/json")]  
    [Route("[controller]")]
    public class PinsController : Controller
    {
        private NotificationsMessageHandler _notificationsMessageHandler { get; set; }

        public PinsController(NotificationsMessageHandler notificationsMessageHandler)
        {
            _notificationsMessageHandler = notificationsMessageHandler;
        }

        [Route(":id")]  
        [HttpGet]
        public Pin GetPin(int id)
        {
            var pin = Pi.Gpio[id];
            return new Pin {
                PinNumber = pin.PinNumber,
                WiringPiPinNumber = (long)pin.WiringPiPinNumber,
                BcmPinNumber = pin.BcmPinNumber,
                HeaderPinNumber = pin.HeaderPinNumber
            };
        }

        [Route("~/api/Pins/HelloWorld")] 
        public async Task HelloWorld([FromQueryAttribute]string message)
        {
            await _notificationsMessageHandler.InvokeClientMethodToAllAsync("receiveMessage", message);
        }
    }

    public partial class Pin
    {
        [JsonProperty("PinNumber")]
        public long PinNumber { get; set; }

        [JsonProperty("WiringPiPinNumber")]
        public long WiringPiPinNumber { get; set; }

        [JsonProperty("BcmPinNumber")]
        public long BcmPinNumber { get; set; }

        [JsonProperty("HeaderPinNumber")]
        public long HeaderPinNumber { get; set; }

        [JsonProperty("Header")]
        public long Header { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Capabilities")]
        public long[] Capabilities { get; set; }

        [JsonProperty("PinMode")]
        public long PinMode { get; set; }

        [JsonProperty("InputPullMode")]
        public long InputPullMode { get; set; }

        [JsonProperty("PwmRegister")]
        public long PwmRegister { get; set; }

        [JsonProperty("PwmMode")]
        public long PwmMode { get; set; }

        [JsonProperty("PwmRange")]
        public long PwmRange { get; set; }

        [JsonProperty("PwmClockDivisor")]
        public long PwmClockDivisor { get; set; }

        [JsonProperty("IsInSoftPwmMode")]
        public bool IsInSoftPwmMode { get; set; }

        [JsonProperty("SoftPwmValue")]
        public long SoftPwmValue { get; set; }

        [JsonProperty("SoftPwmRange")]
        public long SoftPwmRange { get; set; }

        [JsonProperty("IsInSoftToneMode")]
        public bool IsInSoftToneMode { get; set; }

        [JsonProperty("SoftToneFrequency")]
        public long SoftToneFrequency { get; set; }

        [JsonProperty("InterruptCallback")]
        public object InterruptCallback { get; set; }

        [JsonProperty("InterruptEdgeDetection")]
        public long InterruptEdgeDetection { get; set; }
    }
}
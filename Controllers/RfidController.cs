using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IotTest.MessageHandlers;
using Unosquare.RaspberryIO;
using Newtonsoft.Json;
using IotTest.Services;

namespace IotTest.Controllers
{
    [Produces("application/json")]  
    [Route("[controller]")]
    public class RfidController : Controller
    {
        private IRfidReader RfidReader { get; set; }

        public RfidController(IRfidReader rfidReader)
        {
            RfidReader = rfidReader;
        }

        [HttpGet("GetLastReadTag")]
        public IActionResult GetLastReadTag()
        {
            var tag = RfidReader.GetLastReadTag();
            if(tag != null) 
            {
                var (a, b, c, d) = ((byte, byte, byte, byte))tag;
                return Content($"{a},{b},{c},{d}");
            } 
            else 
            {
                return NoContent();
            }
        }
    }
}
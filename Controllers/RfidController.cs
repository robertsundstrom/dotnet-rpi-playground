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
        private ITagReader TagReader { get; set; }

        public RfidController(ITagReader tagReader)
        {
            TagReader = tagReader;
        }

        [HttpGet("GetLastReadTag")]
        public IActionResult GetLastReadTag()
        {
            var tag = TagReader.GetLastReadTag();
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
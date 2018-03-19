using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Unosquare.RaspberryIO.Gpio;

namespace IotTest.Controllers
{
    public class RelaysController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
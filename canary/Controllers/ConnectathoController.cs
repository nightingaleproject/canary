using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VRDR;
using canary.Models;

namespace canary.Controllers
{
    [ApiController]
    public class ConnectathonController : ControllerBase
    {

        /// <summary>
        /// Returns all Connectathon test messages.
        /// GET /connectathon
        /// </summary>
        [HttpGet("Connectathon")]
        [HttpGet("Connectathon/Index")]
        public DeathRecord[] Index()
        {
            return Connectathon.Records;
        }
    }
}

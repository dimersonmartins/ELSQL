using LaraNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using Vendor.VDataAnnotations;
using Vendor.VMigrations.Struct;

namespace LaraNetCore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
       
        [HttpGet("dubug")]
        public IActionResult Cadastrar()
        {
            ModelMigrate modelMigrate = new ModelMigrate();
            modelMigrate.ModelsToMigrate.Add(new Person());
            modelMigrate.ModelsToMigrate.Add(new Work());
            modelMigrate.GenerateSchema();
            modelMigrate.Create();

            return Ok("");
        }

    }
}

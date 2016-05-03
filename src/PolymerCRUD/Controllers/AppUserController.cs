using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PolymerCRUD.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PolymerCRUD.Controllers
{
    [Route("api/[controller]")]
    public class AppUserController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<AppUser> Get()
        {
            using (AppUser user = new AppUser())
            {
                return user.Get();
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public AppUser Get(int id)
        {
            return ModelBase<AppUser>.Get<AppUser>(id);
        }

        // POST api/values
        [HttpPost]
        public JsonResult Post([FromBody]AppUser value)
        {
            return value.Save();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]AppUser value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

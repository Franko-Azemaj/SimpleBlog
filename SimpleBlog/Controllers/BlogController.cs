using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {

        [HttpPost]
        public IEnumerable<string> RegisterUser()
        {
            throw new NotImplementedException();
        }
    }
}

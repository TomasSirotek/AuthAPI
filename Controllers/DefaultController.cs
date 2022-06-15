using Microsoft.AspNetCore.Mvc;

namespace ProductAPI.Controllers {
    // missing versioning add later 
    [Route("[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase {
        public DefaultController() {
        }
    }
}
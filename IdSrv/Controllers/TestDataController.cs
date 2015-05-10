using System.Web.Http;

namespace IdSrv.Controllers
{
    public class TestDataController : ApiController
    {
        [Authorize]
        public IHttpActionResult Get()
        {
            return Json(this.User.Identity.IsAuthenticated);
        }
    }
}

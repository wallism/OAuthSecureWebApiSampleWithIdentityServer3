using System.Collections.Generic;
using System.IdentityModel;
using System.Web.Http;

namespace SecureWebApi.Controllers
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Authorize]
        public string GetId(int id)
        {
            return id.ToString();
        }

        [Route("NewPath")]
        public string GetValue(string value)
        {
            return value;
        }

        [Route("scopetest")]
        [Scope("SecureWebApiSampleScope")]
        public string GetScopeTest(string value)
        {
            return value;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

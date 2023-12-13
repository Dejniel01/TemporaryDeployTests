using System.Collections;

namespace TestApi
{
    public class TestResponse
    {
        public IDictionary? Variables {  get; set; }
        public string? Rdb {  get; set; }
        public string? Issuer {  get; set; }
        public string? Audience {  get; set; }
    }
}

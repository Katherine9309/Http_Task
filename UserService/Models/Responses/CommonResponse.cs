using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Models.Responses
{
    public class CommonResponse<T>
    {
        public HttpStatusCode StatusCode;

        public string Content;

        public T Body;
    
    }
}

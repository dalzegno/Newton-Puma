using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class SuccessResponse<T> : PumaResponse
    {
        public T Data { get; set; }
    }
}

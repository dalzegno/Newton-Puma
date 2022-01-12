using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class PumaResponse
    {
        public static SuccessResponse<T> CreateSuccessResponse<T>(T data)
        {
            return new SuccessResponse<T>
            {
                Data = data
            };
        }
    }
}

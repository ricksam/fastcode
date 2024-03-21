using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services.Models
{
    public class DefaultResponse
    {
        public DefaultResponse()
        {
            this.Success = true;
            this.Message = "";
        }

        public DefaultResponse(bool Success, Exception ex)
        {
            this.Success = Success;
            this.Message = ex.Message;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}

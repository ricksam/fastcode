using WebApplication1.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers.Filters
{
    public class RequestToken : ActionFilterAttribute
    {
        public RequestToken(bool Admin = false, bool Dev = false)
        {
            this.Admin = Admin;
            this.Dev = Dev;
        }

        public bool Admin { get; set; }
        public bool Dev { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string token = HttpUtils.GetToken(context.HttpContext.Request);
            //if (jwtUsuario.Usuario == null || jwtUsuario.Usuario.Id == 0)
            //{
            //    throw new Exception("O token do usuário é inválido");
            //}

            //if (Admin && !jwtUsuario.Admin)
            //{
            //    throw new Exception("O usuário não é admin");
            //}

            //if (Dev && !jwtUsuario.Dev)
            //{
            //    throw new Exception("O usuário não é desenvolvedor");
            //}

            base.OnActionExecuting(context);
        }
    }
}

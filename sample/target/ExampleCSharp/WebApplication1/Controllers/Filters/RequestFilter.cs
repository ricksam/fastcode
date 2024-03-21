using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Repositories.Generics;
using WebApplication1.Repositories.Models;
using WebApplication1.Services.Models;

namespace WebApplication1.Controllers.Filters
{
    public class RequestFilter : ActionFilterAttribute
    {
        private long TimeTicks { get; set; }
        private string postData { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            TimeTicks = System.DateTime.UtcNow.Ticks;

            if (context.ActionArguments.Count > 0)
            {
                postData = System.Text.Json.JsonSerializer.Serialize(context.ActionArguments);
            }
            else
            {
                postData = "";
            }

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            LogApi logApi = new LogApi();

            logApi.Milissegundos = (System.DateTime.UtcNow.Ticks - TimeTicks) / System.TimeSpan.TicksPerMillisecond;
            logApi.Metodo = context.HttpContext.Request.Method;
            logApi.Url = string.Format("{0}/{1}", context.RouteData.Values["Controller"].ToString(), context.RouteData.Values["Action"].ToString());

            try
            {
                logApi.Resposta = System.Text.Json.JsonSerializer.Serialize(((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value);
            }
            catch
            {
                logApi.Resposta = "";
            }

            logApi.Autorizacao = Helpers.HttpUtils.GetAuthorization(context.HttpContext.Request);
            logApi.Ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
            logApi.Requisicao = postData;

            if (!string.IsNullOrEmpty(logApi.Resposta) && logApi.Resposta.Contains("Success"))
            {
                DefaultResponse baseResponse = System.Text.Json.JsonSerializer.Deserialize<DefaultResponse>(logApi.Resposta);
                logApi.Erro = !baseResponse.Success;
            }

            //(new LogApiRepository(new Context(new DatabaseConfiguration()))).Insert(logApi);

            base.OnResultExecuted(context);
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            return base.OnResultExecutionAsync(context, next);
        }
    }
}

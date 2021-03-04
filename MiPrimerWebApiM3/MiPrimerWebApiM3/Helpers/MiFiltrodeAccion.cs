using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApiM3.Helpers
{
    public class MiFiltrodeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltrodeAccion> logger;

        public MiFiltrodeAccion(ILogger<MiFiltrodeAccion> logger)
        {
            this.logger = logger;
        }
        //antes de la accion
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogError("OnActionExecuting");
        }

        //despues de la accion
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogError("OnActionExecuted");
        }

    }
}

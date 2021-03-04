using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IHostedServiceDemo.Services
{
    public class WriteToFileHostedService : Microsoft.Extensions.Hosting.IHostedService
    {
        //para obtener el directorio de donde se encuentra nuestra aplicacion ejecutandose
        private readonly IHostingEnvironment enviroment;
        private readonly string fileName = "File 1.txt";
        public WriteToFileHostedService(IHostingEnvironment enviroment)
        {
            this.enviroment = enviroment;
        }
        //cuando se inicia la aplicacion se ejecutará
        public Task StartAsync(CancellationToken cancellationToken)
        {
            WritetoFile("WriteToFileHostedService: Proceso iniciado");
            return Task.CompletedTask;
        }
        // cuando finalize la aplicacion se ejecutará
        public Task StopAsync(CancellationToken cancellationToken)
        {
            WritetoFile("WriteToFileHostedService: Proceso finalizado");
            return Task.CompletedTask;
        }

        private void WritetoFile(string mensaje)
        {
            var path = $@"{enviroment.ContentRootPath}\wwwroot\{fileName}";
            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine(mensaje);
            }

        }
    }
}

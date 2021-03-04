using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IHostedServiceDemo.Services
{
    public class WriteToFileHostedService : Microsoft.Extensions.Hosting.IHostedService, IDisposable
    {
        //para obtener el directorio de donde se encuentra nuestra aplicacion ejecutandose
        private readonly IHostingEnvironment enviroment;
        private readonly string fileName = "File 1.txt";
        private Timer timer;
        public WriteToFileHostedService(IHostingEnvironment enviroment)
        {
            this.enviroment = enviroment;
        }

        public void Dispose()
        {
            //limpiar los recursos del timer
            timer?.Dispose();
        }

        //cuando se inicia la aplicacion se ejecutará
        public Task StartAsync(CancellationToken cancellationToken)
        {
            WritetoFile("WriteToFileHostedService: Proceso iniciado");
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }
        // cuando finalize la aplicacion se ejecutará
        public Task StopAsync(CancellationToken cancellationToken)
        {
            WritetoFile("WriteToFileHostedService: Proceso finalizado");
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        private void DoWork(object state)
        {
            WritetoFile($"WriteToFileHostedService: A las" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
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

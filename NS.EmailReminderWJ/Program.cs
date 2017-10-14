using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using EmailReminderWJ.BL;
using EmailReminderWJ.BE;

namespace EmailReminderWJ
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var config = new JobHostConfiguration();

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            Console.WriteLine("Se inicia job de envío de alertas de cobros pendientes");

            Bll bll = new Bll();

            //bll.DeletePlanAlert();
            //bll.initPlanAlerts();
            bll.SendAlerts();
            
            //TODO: FALTA PANTALLA QUE REGISTRE PAGO YA REALIZADO POR CLIENTE
            Console.WriteLine("Finaliza job de envío de alertas de cobros pendientes");

            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}

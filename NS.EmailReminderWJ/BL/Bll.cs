using EmailReminderWJ.BE;
using NS.ERWJ.DA;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace EmailReminderWJ.BL
{
    public class Bll
    {
        public void DeletePlanAlert()
        {
            new Dal().DeletePlanAlert("009db29f-76f1-47d1-9689-a7ff015e6005");
        }

        public void SendAlerts()
        {
            List<PlanAlertBE> lst = new Dal().GetAllEnabledPlanAlerts();
            try
            {
                foreach (PlanAlertBE plan in lst)
                {

                    string message = plan.Concept + " por un total de S/" + plan.Charge + " sin IGV. <br>" +
                        "Fecha de vencimiento: " + plan.DueDate.ToString("d MMMM", CultureInfo.GetCultureInfo("es"));
#if !DEBUG
                Int32 dif = Convert.ToInt32(String.Format("{0:MMdd}", plan.AlertDate)) -
                    Convert.ToInt32(String.Format("{0:MMdd}", DateTime.Now));

                Int32 dif2 = Convert.ToInt32(String.Format("{0:MMdd}", plan.DueDate)) -
                Convert.ToInt32(String.Format("{0:MMdd}", DateTime.Now));

                if ((dif >= 0 && dif < 3) || (dif2 >= 2 && dif2 < 4))
                {
                    SendEmail(plan.Email, plan.Customer, plan.Concept, message, message);
                }
#else
                    SendEmail(plan.Email, plan.Customer, plan.Concept, message, message);
#endif
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("ERROR-SendAlerts: " + exc.Message);
            }
        }

        private void SendEmail(string toEmail, string toName, string subject, string text, string htmlText)
        {
            MailMessage mailMsg = new MailMessage();
#if DEBUG
            mailMsg.To.Add(new MailAddress(ConfigurationManager.AppSettings["adminEmail"], ConfigurationManager.AppSettings["company"]));
#else
            mailMsg.To.Add(new MailAddress(toEmail, toName));
#endif
            mailMsg.From = new MailAddress(ConfigurationManager.AppSettings["noReplyEmail"], ConfigurationManager.AppSettings["company"]);
            mailMsg.Bcc.Add(new MailAddress(ConfigurationManager.AppSettings["adminEmail"], "ALERTA DE COBROS PENDIENTES"));

            mailMsg.Subject = subject;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(@htmlText, null, MediaTypeNames.Text.Html));
            mailMsg.Priority = MailPriority.High;

            SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["sendGridUser"], ConfigurationManager.AppSettings["sendGridPass"]);
            smtpClient.Credentials = credentials;

            smtpClient.Send(mailMsg);
        }

        public void initPlanAlerts()
        {
            Dal dal = new Dal();

            dal.CreatePlanAlert(new PlanAlertBE()
            {
                Customer = "INDUMET GRUPO ORTIZ S.A.C.",
                AlertDate = new DateTime(2012, 10, 30),
                Concept = "Recordatorio de pago por el servicio anual de mantenimiento de la web indumet.pe",
                CreatedDate = DateTime.Now,
                DueDate = new DateTime(2012, 12, 6),
                Charge = 200.00M,
                Email = "sortiz@indumet.pe",
                Comment = "(custodia de fuentes, problemas menores, etc)",
                Enabled = false
            });

            dal.CreatePlanAlert(new PlanAlertBE()
            {
                Customer = "INDUMET GRUPO ORTIZ S.A.C.",
                AlertDate = new DateTime(2012, 10, 30),
                Concept = "Recordatorio de pago por el servicio anual de mantenimiento de la web indumet.pe",
                CreatedDate = DateTime.Now,
                DueDate = new DateTime(2012, 12, 6),
                Charge = 250.00M,
                Email = "sortiz@indumet.pe",
                Comment = "(custodia de fuentes, problemas menores, etc) (incremento del 25%)",
                Enabled = true
            });

            dal.CreatePlanAlert(new PlanAlertBE()
            {
                Customer = "M & S PIMA COTTON S.A.C.",
                AlertDate = new DateTime(2015, 9, 30),
                Concept = "Recordatorio de pago por renovación anual del dominio myspimacotton.pe",
                CreatedDate = DateTime.Now,
                DueDate = new DateTime(2015, 11, 4),
                Charge = 240.00M,
                Email = "sortiz@myspimacotton.pe",
                Enabled = false
            });

            dal.CreatePlanAlert(new PlanAlertBE()
            {
                Customer = "M & S PIMA COTTON S.A.C.",
                AlertDate = new DateTime(2015, 9, 30),
                Concept = "Recordatorio de pago por renovación anual del dominio myspimacotton.pe",
                CreatedDate = DateTime.Now,
                DueDate = new DateTime(2015, 11, 4),
                Charge = 300.00M,
                Email = "sortiz@myspimacotton.pe",
                Comment = "(incremento del 25%)",
                Enabled = true
            });

            dal.CreatePlanAlert(new PlanAlertBE()
            {
                Customer = "M & S PIMA COTTON S.A.C.",
                AlertDate = new DateTime(2015, 9, 30),
                Concept = "Recordatorio de pago por el servicio anual de correo/s y administración de correo/s de dominio myspimacotton.pe",
                CreatedDate = DateTime.Now,
                DueDate = new DateTime(2015, 11, 7),
                Charge = 200.00M,
                Email = "sortiz@myspimacotton.pe",
                Comment = "S/100 por cuenta de correo y S/100 por administración general (se tiene sólo una cuenta)",
                Enabled = false
            });

            dal.CreatePlanAlert(new PlanAlertBE()
            {
                Customer = "M & S PIMA COTTON S.A.C.",
                AlertDate = new DateTime(2015, 9, 30),
                Concept = "Recordatorio de pago por el servicio anual de correo/s y administración de correo/s de dominio myspimacotton.pe",
                CreatedDate = DateTime.Now,
                DueDate = new DateTime(2015, 11, 7),
                Charge = 250.00M,
                Email = "sortiz@myspimacotton.pe",
                Comment = "S/100 por cuenta de correo y S/100 por administración general (se tiene sólo una cuenta) (incremento del 25%)",
                Enabled = true
            });

            dal.CreatePlanAlert(new PlanAlertBE()
            {
                Customer = "M & S PIMA COTTON S.A.C.",
                AlertDate = new DateTime(2015, 2, 28),
                Concept = "Recordatorio de pago por el servicio anual de mantenimiento y hosting de la web myspimacotton.pe",
                CreatedDate = DateTime.Now,
                DueDate = new DateTime(2015, 4, 5),
                Charge = 1250.00M,
                Email = "sortiz@myspimacotton.pe",
                Comment = "S/1000 por hosting y S/250 por mantenimiento web (custodia de fuentes, problemas menores, etc)",
                Enabled = true
            });
        }
    }
}

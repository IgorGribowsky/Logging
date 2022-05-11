using Serilog.Core;
using Serilog.Events;
using System;
using System.Net;
using System.Net.Mail;

namespace BrainstormSessions.Logging
{
    public class EmailSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;
        private readonly string _sendTo;

        public EmailSink(IFormatProvider formatProvider, string sendTo)
        {
            _formatProvider = formatProvider;
            _sendTo = sendTo;
        }

        public void Emit(LogEvent logEvent)
        {
            var message = logEvent.RenderMessage(_formatProvider);
            Console.WriteLine(DateTimeOffset.Now.ToString() + " " + message);

            MailAddress from = new MailAddress("northwind20211203@gmail.com", "BrainstormSessions");
            MailAddress to = new MailAddress(_sendTo);
            MailMessage m = new MailMessage(from, to);
            m.Subject = logEvent.Level.ToString();
            m.Body = message;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("northwind20211203@gmail.com", "hardpassword123");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
}

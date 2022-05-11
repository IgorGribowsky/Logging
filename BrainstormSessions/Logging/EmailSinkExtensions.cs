using System;
using Serilog;
using Serilog.Configuration;

namespace BrainstormSessions.Logging
{
    public static class EmailSinkExtensions
    {
        public static LoggerConfiguration EmailSink(
                  this LoggerSinkConfiguration loggerConfiguration,
                  string sendTo,
                  IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new EmailSink(formatProvider, sendTo));
        }
    }
}

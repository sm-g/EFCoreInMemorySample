using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL
{
    internal class DbLoggerProvider : ILoggerProvider
    {
        private readonly bool _onlySql;

        public DbLoggerProvider(bool onlySql)
        {
            _onlySql = onlySql;
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (_onlySql && categoryName != DbLoggerCategory.Database.Command.Name)
            {
                return new NullLogger();
            }
            return new DbLogger();
        }

        public void Dispose()
        {
            // interface
        }

        private class DbLogger : ILogger
        {
            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                Debug.WriteLine(formatter(state, exception));
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }

        private class NullLogger : ILogger
        {
            public bool IsEnabled(LogLevel logLevel)
            {
                return false;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                // interface
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}
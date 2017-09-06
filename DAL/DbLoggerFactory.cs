using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DAL
{
    public class DbLoggerFactory : ILoggerFactory
    {
        private readonly ILoggerProvider _provider;

        public DbLoggerFactory(bool onlySql)
        {
            _provider = new DbLoggerProvider(onlySql);
        }

        public void AddProvider(ILoggerProvider provider)
        {
            throw new NotImplementedException();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _provider.CreateLogger(categoryName);
        }

        public void Dispose()
        {
            // interface
        }
    }
}
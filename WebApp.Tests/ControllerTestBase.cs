using System;
using DAL;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace WebApp.Tests
{
    public abstract class ControllerTestBase<TStartup> : IntegrationTestFixture<TStartup>
    {
        [SetUp]
        public void OnStart()
        {
            using (var db = new MyContext(options))
            {
                db.ResetValueGenerators();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var db = new MyContext(options))
            {
                db.ResetValueGenerators();
                db.Database.EnsureDeleted();
            }
        }

        protected override void InitializeServices(IServiceCollection services)
        {
        }
    }
}
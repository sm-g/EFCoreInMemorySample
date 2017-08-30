using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DAL;

namespace WebApp.Tests
{
    [TestFixture]
    public class SampleDataControllerTests : ControllerTestBase<Startup>
    {
        [Test]
        public async Task IndexPost_ValidModelForAtUser_ReturnsOK()
        {
            using (var db = new MyContext(options))
            {
                db.Foos.Add(new Foo() { Value = 4 });
                db.SaveChanges();
            }

            var response = await Client.GetAsync("api/SampleData/test");

            var str = await response.Content.ReadAsStringAsync();

            Assert.AreEqual("4", str);
        }
    }
}
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
        }
    }
}
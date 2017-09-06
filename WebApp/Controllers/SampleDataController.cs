using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private readonly MyContext _db;

        public SampleDataController(MyContext db)
        {
            _db = db;
        }

        [HttpGet("[action]")]
        public int? Test()
        {
            var result = 0;
            DAL.Models.Import import;
            using (var tr = _db.Database.BeginTransaction())
            {
                import = new DAL.Models.Import();
                _db.Imports.Add(import);
                _db.SaveChanges();
                tr.Commit();
            }

            using (var tr = _db.Database.BeginTransaction())
            {
                var group1 = new DAL.Models.ImportGroup() { GroupId = 1, Name = "cat1", ImportId = import.Id };

                var zone1 = new DAL.Models.ImportZone() { ZoneId = 1, Name = "zone1", ImportId = import.Id };

                var users = new[]
                {
                    new DAL.Models.ImportUser {
                        ImportId = import.Id,
                        UserId = 1,
                        FirstName = "fn",
                        MiddleName = "",
                        LastName = "ln",
                        ImportZone = zone1,
                        ImportGroup = group1,
                    }
                };

                var customer = new DAL.Models.ImportCustomer { ImportId = import.Id, Name = "cust", CustomerId = 1 };
                var projects = new[]
                {
                    new DAL.Models.ImportProject{ Name = "proj1", ProjectId = 1, ImportId = import.Id, ImportCustomer = customer },
                    new DAL.Models.ImportProject{ Name = "proj2", ProjectId = 2, ImportId = import.Id, ImportCustomer = customer }
                };

                _db.Users.AddRange(users);
                _db.Projects.AddRange(projects);
                result = _db.SaveChanges();
                tr.Commit();
            }
            return result;
        }
    }
}
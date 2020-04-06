using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using VRDR;
using canary.Models;

namespace canary.Controllers
{
    [ApiController]
    public class TestsController : ControllerBase
    {
        /// <summary>
        /// Returns all tests.
        /// GET /api/tests
        /// </summary>
        [HttpGet("Tests")]
        [HttpGet("Tests/Index")]
        public Dictionary<string, string>[] Index()
        {
            using (var db = new RecordContext())
            {
                List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();
                foreach(Test test in db.Tests.Take(20))
                {
                    Dictionary<string, string> result = new Dictionary<string, string>();
                    result.Add("testId", test.TestId.ToString());
                    result.Add("created", test.Created.ToString());
                    result.Add("type", test.Type != null ? test.Type.ToString() : "");
                    result.Add("completedDateTime", test.CompletedDateTime != null ? test.CompletedDateTime.ToString() : "");
                    result.Add("correct", test.Correct.ToString());
                    result.Add("total", test.Total.ToString());
                    results.Add(result);
                }
                return results.ToArray();
            }
        }

        /// <summary>
        /// Gets a test by id.
        /// GET /api/tests/1
        /// </summary>
        [HttpGet("Tests/{id:int}")]
        public Test GetTest(int id)
        {
            using (var db = new RecordContext())
            {
                return db.Tests.Where(t => t.TestId == id).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets a pre-defined connectathon test by id.
        /// GET /api/tests/connectathon/1
        /// </summary>
        [HttpGet("Tests/Connectathon/{id:int}")]
        public Test GetTestConnectathon(int id)
        {
            using (var db = new RecordContext())
            {
                Test test = new Test(Connectathon.FromId(id));
                db.Tests.Add(test);
                db.SaveChanges();
                return test;
            }
        }

        /// <summary>
        /// Perform a connectathon test by id.
        /// POST /api/tests/connectathon/1
        /// </summary>
        [HttpPost("Tests/Connectathon/{id:int}")]
        public int PerformTestConnectathon(int id)
        {
            using (var db = new RecordContext())
            {
                Test test = new Test(Connectathon.FromId(id));
                string input;
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    input = reader.ReadToEnd();
                }
                if (!String.IsNullOrEmpty(input))
                {
                    test = test.Run(new DeathRecord(input));
                }
                return test.Incorrect;
            }
        }

        /// <summary>
        /// Starts a new test.
        /// GET /api/tests/new
        /// </summary>
        [HttpGet("Tests/New")]
        public Test NewTest()
        {
            using (var db = new RecordContext())
            {
                Test test = new Test();
                db.Tests.Add(test);
                db.SaveChanges();
                return test;
            }
        }

        /// <summary>
        /// Calculates test results.
        /// POST /api/tests/consume/run
        /// </summary>
        [HttpPost("Tests/Consume/Run/{id:int}")]
        public Test RunConsumeTest(int id) // TODO: These test routes can probably be combined.
        {
            using (var db = new RecordContext())
            {
                Test test = db.Tests.Where(t => t.TestId == id).FirstOrDefault();
                string input;
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    input = reader.ReadToEnd();
                }
                if (!String.IsNullOrEmpty(input))
                {
                   test.Run(input);
                   test.Type = "Consume";
                }
                db.Tests.Remove(test);
                db.SaveChanges();
                return test;
            }
        }

        /// <summary>
        /// Calculates test results.
        /// POST /api/tests/produce/run
        /// </summary>
        [HttpPost("Tests/Produce/Run/{id:int}")]
        public Test RunProduceTest(int id) // TODO: These test routes can probably be combined.
        {
            using (var db = new RecordContext())
            {
                Test test = db.Tests.Where(t => t.TestId == id).FirstOrDefault();
                string input;
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    input = reader.ReadToEnd();
                }
                if (!String.IsNullOrEmpty(input))
                {
                   test.Run(input);
                   test.Type = "Produce";
                }
                db.Tests.Remove(test);
                db.SaveChanges();
                return test;
            }
        }

        /// <summary>
        /// Calculates test results.
        /// POST /api/tests/roundtrip/consuming/run
        /// </summary>
        [HttpPost("Tests/Roundtrip/Consuming/Run/{id:int}")]
        public Test RunRoundtripConsumingTest(int id) // TODO: These test routes can probably be combined.
        {
            using (var db = new RecordContext())
            {
                Test test = db.Tests.Where(t => t.TestId == id).FirstOrDefault();
                string input;
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    input = reader.ReadToEnd();
                }
                if (!String.IsNullOrEmpty(input))
                {
                    test.Type = "RoundtripConsuming";
                    test.Run(input);
                }
                db.Tests.Remove(test);
                db.SaveChanges();
                return test;
            }
        }

        /// <summary>
        /// Calculates test results.
        /// POST /api/tests/roundtrip/producing/run
        /// </summary>
        [HttpPost("Tests/Roundtrip/Producing/Run/{id:int}")]
        public Test RunRoundtripProducingTest(int id) // TODO: These test routes can probably be combined.
        {
            using (var db = new RecordContext())
            {
                Test test = db.Tests.Where(t => t.TestId == id).FirstOrDefault();
                string input;
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    input = reader.ReadToEnd();
                }
                if (!String.IsNullOrEmpty(input))
                {
                    test.Type = "RoundtripProducing";
                    test.Run(input);
                }
                db.Tests.Remove(test);
                db.SaveChanges();
                return test;
            }
        }
    }
}

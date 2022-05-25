using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
                    result.Add("completedDateTime", test.CompletedDateTime.ToString());
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
        /// Gets a pre-defined connectathon test by id with a certificate
        /// number and an optional state parameter which sets the placeOfDeath
        /// to the provided state.
        /// GET /api/tests/connectathon/1 or /api/tests/connectathon/1/AK
        /// </summary>
        [HttpGet("Tests/Connectathon/{id:int}/{certificateNumber:int}/{state?}")]
        public Test GetTestConnectathon(int id, int certificateNumber, string state)
        {
            using (var db = new RecordContext())
            {
                Test test = new Test(Connectathon.FromId(id, certificateNumber, state));
                db.Tests.Add(test);
                db.SaveChanges();
                return test;
            }
        }

        [HttpPost("Tests/Validator")]
        public async Task<Test> GetTestIJEValidator(int id)
        {
            using (var db = new RecordContext())
            {
                string input = await new StreamReader(Request.Body, Encoding.UTF8).ReadToEndAsync();
                if (String.IsNullOrEmpty(input))
                {
                    return null;
                }
                DeathRecord record = DeathRecord.FromDescription(input);
                Test test = new Test(record);
                db.Tests.Add(test);
                db.SaveChanges();
                return test;
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
        /// POST /api/tests/<type>/run/<id>
        /// </summary>
        [HttpPost("Tests/{type}/Run/{id:int}")]
        public async Task<Test> RunTest(int id, string type)
        {
            using (var db = new RecordContext())
            {
                Test test = db.Tests.Where(t => t.TestId == id).FirstOrDefault();
                string input = await new StreamReader(Request.Body, Encoding.UTF8).ReadToEndAsync();
                if (!String.IsNullOrEmpty(input))
                {
                    test.Type = type;
                    test.Run(input);
                }
                db.Tests.Remove(test);
                db.SaveChanges();
                return test;
            }
        }

        [HttpPost("Tests/{type}/Response")]
        public async Task<Dictionary<string, Message>> GetTestResponse(int id, string type)
        {
            using (var db = new RecordContext())
            {
                string input = await new StreamReader(Request.Body, Encoding.UTF8).ReadToEndAsync();
                if (String.IsNullOrEmpty(input))
                {
                    return null;
                }

                // get the responses for the submitted message
                Message msg = new Message(input);             
                Dictionary<string, Message> result = msg.GetResponsesFor(type);
                
                return result;
            }
        }
    }
}

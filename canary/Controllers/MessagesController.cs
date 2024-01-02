using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VRDR;
using canary.Models;
using Microsoft.Extensions.Primitives;
using System.Reflection;
using Hl7.Fhir.Model;

namespace canary.Controllers
{
    [ApiController]
    public class MessagesController : ControllerBase
    {
        /// <summary>
        /// Inspects a message using the contents provided. Returns the message + record and any validation issues.
        /// POST Messages/Inspect
        /// </summary>
        [HttpPost("Messages/Inspect")]
        public async Task<(Record record, List<Dictionary<string, string>> issues)> NewPost()
        {
            string input = await new StreamReader(Request.Body, Encoding.UTF8).ReadToEndAsync();

            if (!String.IsNullOrEmpty(input))
            {
                if (input.Trim().StartsWith("<") || input.Trim().StartsWith("{")) // XML or JSON?
                {
                    BaseMessage message = BaseMessage.Parse(input, false);


                    DeathRecord extracted = new DeathRecord();
                    foreach (PropertyInfo property in message.GetType().GetProperties())
                    {
                        if (property.PropertyType == typeof(DeathRecord))
                        {
                            extracted = (DeathRecord)property.GetValue(message);
                        }
                    }
                    string deathRecordString = extracted.ToJSON();
                    var messageInspectResults = Record.CheckGet(deathRecordString, false);

                    return messageInspectResults;
                }
                else
                {
                    return (null, new List<Dictionary<string, string>> 
                        { new Dictionary<string, string> { { "severity", "error" }, 
                            { "message", "The given input is not JSON or XML." } } }
                    );
                }
            }
            else
            {
                return (null, new List<Dictionary<string, string>> { new Dictionary<string, string> { { "severity", "error" }, { "message", "The given input appears to be empty." } } });
            }
        }

        /// <summary>
        /// Creates a new message using the contents provided. Returns the message and any validation issues.
        /// POST /api/messages/new
        /// </summary>
        [HttpPost("Messages/New")]
        public async Task<(Message message, List<Dictionary<string, string>> issues)> NewMessagePost()
        {
            string input = await new StreamReader(Request.Body, Encoding.UTF8).ReadToEndAsync();

            try {
                BaseMessage message = BaseMessage.Parse(input, false);
                // If we were to return the Message here, the controller would automatically
                // serialize the message into a JSON object. Since that would happen outside of this
                // try/catch block, this would mean any errors would return a 500 and not display
                // them nicely to the user. By doing the serialization here and returning a string
                // we can nicely display any deserialization errors to the user.
                // One such error can be caused by removing the `<source>` endpoint from a Submission
                // message and then trying to validate it.
                JsonConvert.SerializeObject(message);
                return (message: new Message(message), issues: new List<Dictionary<string, string>>());
            }
            catch (Exception e)
            {
                return (message: null, issues: Record.DecorateErrors(e));
            }
        }

        /// <summary>
        /// Creates a new message of the provided type using record provided
        /// POST /api/messages/create?type={type}
        /// </summary>
        [HttpPost("Messages/Create")]
        public async Task<(Message message, List<Dictionary<string, string>> issues)> NewMessageRecordPost(String type)
        {
            string input = await new StreamReader(Request.Body, Encoding.UTF8).ReadToEndAsync();

            (Record record, List< Dictionary<string, string> > _) = Record.CheckGet(input, false);
            try {
                return (new Message(record, type), null);
            }
            catch (ArgumentException e) {
                return (null, new List<Dictionary<string, string>> { new Dictionary<string, string> { { "severity", "error" }, { "message", e.Message } } });
            }
        }
  }
}

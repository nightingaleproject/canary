using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VRDR;
using Newtonsoft.Json;
using System.IO;
using System.Text.Json.Nodes;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net.Http.Headers;
using RestSharp;
using RestSharp.Authenticators;
using System.Text;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Hl7.FhirPath.Sprache;
using Newtonsoft.Json.Linq;
using Bogus.Bson;

namespace canary.Models
{
    public class RecordContext : DbContext
    {
        public DbSet<Record> Records { get; set; }

        public DbSet<Endpoint> Endpoints { get; set; }

        public DbSet<Test> Tests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=canary.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Endpoint>().Property(b => b.Issues).HasConversion(v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(v));

            modelBuilder.Entity<Endpoint>().Property(b => b.Record).HasConversion(v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<Record>(v));

            modelBuilder.Entity<Record>().Property(b => b.IjeInfo).HasConversion(v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(v));

            modelBuilder.Entity<Test>().Property(t => t.ReferenceRecord).HasConversion(t => t.GetRecord().ToXML(),
                t => new Record(t));

            modelBuilder.Entity<Test>().Property(t => t.TestRecord).HasConversion(t => t.GetRecord().ToXML(),
                t => new Record(t));
        }
    }

    public class Record
    {
        private DeathRecord record { get; set; }

        private string ije { get; set; }

        private string fsh { get; set; }

        public int RecordId { get; set; }

        public string Xml
        {
            get
            {
                if (record == null)
                {
                    return null;
                }
                return record.ToXML();
            }
            set
            {
                record = new DeathRecord(value);
                ije = new IJEMortality(record).ToString();
            }
        }

        public string Json
        {
            get
            {
                if (record == null)
                {
                    return null;
                }
                return record.ToJSON();
            }
            set
            {
                record = new DeathRecord(value);
                ije = new IJEMortality(record).ToString();
            }
        }

        public string Ije
        {
            get
            {
                if (record == null)
                {
                    return null;
                }
                return ije;
            }
            set
            {
                record = new IJEMortality(value).ToDeathRecord();
                ije = value;
            }
        }

        public string Fsh
        {
            get
            {
                if (record == null)
                {
                    return null;
                }
                return fsh;
            }

            set { fsh = value; }

        }

        public DeathRecord GetRecord()
        {
            return record;
        }

        public List<Dictionary<string, string>> IjeInfo
        {
            get
            {
                string ijeString = ije;
                List<PropertyInfo> properties = typeof(IJEMortality).GetProperties().ToList().OrderBy(p => p.GetCustomAttribute<IJEField>().Field).ToList();
                List<Dictionary<string, string>> propList = new List<Dictionary<string, string>>();
                foreach (PropertyInfo property in properties)
                {
                    IJEField info = property.GetCustomAttribute<IJEField>();
                    string field = ijeString.Substring(info.Location - 1, info.Length);
                    Dictionary<string, string> propInfo = new Dictionary<string, string>();
                    propInfo.Add("number", info.Field.ToString());
                    propInfo.Add("location", info.Location.ToString());
                    propInfo.Add("length", info.Length.ToString());
                    propInfo.Add("contents", info.Contents);
                    propInfo.Add("name", info.Name);
                    propInfo.Add("value", field.Trim());
                    propList.Add(propInfo);
                }
                return propList;
            }
            set
            {
                // NOOP
            }
        }

        public string FhirInfo
        {
            get
            {
                if (record == null)
                {
                    return null;
                }
                return record.ToDescription();
            }
            set
            {
                record = DeathRecord.FromDescription(value);
                ije = new IJEMortality(record).ToString();
            }
        }

        public Record()
        {
            record = new DeathRecord();
            ije = new IJEMortality(record, false).ToString();
        }

        public Record(DeathRecord record)
        {
            this.record = record;
            ije = new IJEMortality(this.record, false).ToString();
        }

        public Record(string record)
        {
            this.record = new DeathRecord(record);
            ije = new IJEMortality(this.record, false).ToString();
        }

        public Record(string record, bool permissive)
        {
            this.record = new DeathRecord(record, permissive);
            ije = new IJEMortality(this.record).ToString();
        }

        public static List<Dictionary<string, string>> ParseSushiErrorsAndWarnings(string sushiResults)
        {
            var issueList = new List<Dictionary<string, string>>();

            JToken jToken = JToken.Parse(sushiResults);

            var errorList = jToken["errors"].Select(z => z).ToList();

            foreach (var errorData in errorList)
            {
                StringBuilder messageInfo = new StringBuilder();
                messageInfo.Append((string)errorData["message"]);
                if (errorData["location"] != null)
                {
                    messageInfo.Append(" Location: ");
                    messageInfo.Append(" start col: ");
                    messageInfo.Append((string)errorData["location"]["startColumn"]);
                    messageInfo.Append(" end col: ");
                    messageInfo.Append((string)errorData["location"]["endColumn"]);
                    messageInfo.Append(" start line: ");
                    messageInfo.Append((string)errorData["location"]["startLine"]);
                    messageInfo.Append(" end line: ");
                    messageInfo.Append((string)errorData["location"]["endLine"]);
                }

                issueList.Add(
                    new Dictionary<string, string> { { "severity", "error" },
                        { "message", messageInfo.ToString() } } 
                );
            }

            var warningList = jToken["warnings"].Select(z => z).ToList();

            foreach (var warningData in warningList)
            {
                StringBuilder messageInfo = new StringBuilder();
                messageInfo.Append((string)warningData["message"]);
                if (warningData["location"] != null)
                {
                    messageInfo.Append(" Location: ");
                    messageInfo.Append(" start col: ");
                    messageInfo.Append((string)warningData["location"]["startColumn"]);
                    messageInfo.Append(" end col: ");
                    messageInfo.Append((string)warningData["location"]["endColumn"]);
                    messageInfo.Append(" start line: ");
                    messageInfo.Append((string)warningData["location"]["startLine"]);
                    messageInfo.Append(" end line: ");
                    messageInfo.Append((string)warningData["location"]["endLine"]);
                }

                issueList.Add(
                    new Dictionary<string, string> { { "severity", "warning" },
                        { "message", messageInfo.ToString() } }
                );
            }

            return issueList;
        }

        public static string ValidateFshSushi(string fshInput)
        {
            string resultData = string.Empty;

            System.Threading.Tasks.Task<string> task =
                System.Threading.Tasks.Task.Run<string>(async () => await getSushResults(fshInput));

            resultData = task.Result;
            return resultData;
        }

        /// <summary>Check the given FHIR record string and return a list of issues. Also returned
        /// the parsed record if parsing was successful.</summary>
        public static (Record record, List<Dictionary<string, string>> issues) CheckGet(string record, bool permissive, string originalFhirData = "", bool useFsh = false)
        {
            Record newRecord = null;
            List<Dictionary<string, string>> entries = new List<Dictionary<string, string>>();
            try
            {
                Record recordToSerialize = new Record(new DeathRecord(record, permissive));
                // If the object fails to serialize then it will not be possible for canary to return it to the user
                // (since canary has to serialize it to JSON in order to do so). This is why serialization is tested
                // here and if it passes then the record is considered "safe" to return.
                JsonConvert.SerializeObject(recordToSerialize);
                newRecord = recordToSerialize;
                if (!String.IsNullOrWhiteSpace(originalFhirData) && useFsh)
                {
                    System.Threading.Tasks.Task<string> task =
                        System.Threading.Tasks.Task.Run<string>(async () => await getFshData(record));
                    newRecord.Fsh = task.Result;
                }

                validateRecordType(newRecord);
                return (record: newRecord, issues: entries);
            }
            catch (Exception e)
            {
                entries = DecorateErrors(e);
            }
            return (record: newRecord, issues: entries);
        }

        private async static Task<string> getSushResults(string fshInput)
        {
            string ret = string.Empty;

            ret = await getFshInspection(fshInput);

            return ret;
        }

        private async static Task<string> getFshData(string fhirMessage)
        {
            string ret = string.Empty;

            string rawFsh = await getRawFshData(fhirMessage);

            ret = await convertFshDataProfileNames(rawFsh);

            return ret;

        }

        private async static Task<string> getFshInspection(string fshData)
        {
            string ret = string.Empty;

            try
            {
                JsonObject fshJson = new JsonObject();
                fshJson.Add("fsh", fshData);
                string convertedFshData = fshJson.ToString();
                
                var options = new RestClientOptions("https://cte-nvss-canary-a213fdc38384.azurewebsites.net")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/api/FshToFhir", Method.Post);
                request.AddHeader("Host", "cte-nvss-canary-a213fdc38384.azurewebsites.net");
                request.AddJsonBody(fshJson);
                RestResponse response = await client.ExecuteAsync(request);
                ret = response.Content;

            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }
            return ret;
        }

        private async static Task<string> getRawFshData(string fhirMessage)
        {
            string ret = string.Empty;

            try
            {

                byte[] bytes = Encoding.ASCII.GetBytes(fhirMessage);

                var fhrContent = Regex.Replace(fhirMessage, @"(""[^""\\]*(?:\\.[^""\\]*)*"")|\s+", "$1");

                var options = new RestClientOptions("https://cte-nvss-canary-a213fdc38384.azurewebsites.net")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/api/FhirToFsh", Method.Post);
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Host", "cte-nvss-canary-a213fdc38384.azurewebsites.net");
                request.AddJsonBody(fhirMessage);
                RestResponse response = await client.ExecuteAsync(request);
                ret = response.Content;

            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }
            return ret;
        }


        private async static Task<string> convertFshDataProfileNames(string rawFshData)
        {
            string ret = string.Empty;

            try
            {

                byte[] bytes = Encoding.ASCII.GetBytes(rawFshData);

                var fhrContent = Regex.Replace(rawFshData, @"(""[^""\\]*(?:\\.[^""\\]*)*"")|\s+", "$1");

                var options = new RestClientOptions("https://cte-nvss-canary-a213fdc38384.azurewebsites.net")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/api/ConvertInstanceOf", Method.Post);
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Host", "cte-nvss-canary-a213fdc38384.azurewebsites.net");
                request.AddJsonBody(rawFshData);
                RestResponse response = await client.ExecuteAsync(request);
                ret = response.Content;

            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }
            return ret;
        }



        /// <summary>Recursively call InnerException and add all errors to the list until we reach the BaseException.</summary>
        public static List<Dictionary<string, string>> DecorateErrors(Exception e)
        {
            List<Dictionary<string, string>> entries = new List<Dictionary<string, string>>();

            return DecorateErrorsRecursive(e, entries);
        }

        private static void validateRecordType(Record record)
        {
            List<string> acceptedPayloadTypes = new List<string>() { "document" };  //Add to this with "document", "newtype", "etc"

            if (String.IsNullOrWhiteSpace(record.Json))
            {
                return;
            }

            var jsonData = JsonObject.Parse(record.Json);
            if (jsonData["type"] == null)
            {
                throw new Exception("No type key in JSON data");
            }
            string payloadType = jsonData["type"].ToString();
            if (!acceptedPayloadTypes.Contains(payloadType))
            {
                throw new Exception("JSON input type not equal to " + string.Join(",", acceptedPayloadTypes.ToArray()));
            }
        }

        private static List<Dictionary<string, string>> DecorateErrorsRecursive(Exception exception, List<Dictionary<string, string>> entries)
        {
            if (exception != null && exception.Message != null)
            {
                foreach (string er in exception.Message.Split(";"))
                {
                    Dictionary<string, string> entry = new Dictionary<string, string>();
                    // targetSite contains the information required to show the function class and function that
                    // the error occurred in
                    var targetSite = exception.TargetSite;
                    string erString = er.Trim();
                    // Ensure the original error string always ends in a period.
                    if (!erString.EndsWith('.')) erString += '.';
                    string errorWithLocation = $"{erString} Error occurred";
                    if (targetSite.ReflectedType != null) errorWithLocation += $" at {targetSite.ReflectedType}";
                    if (targetSite.Name != null) errorWithLocation += $" in function {targetSite.Name}";
                    errorWithLocation += ".";
                    entry.Add("message", errorWithLocation.Replace("Parser:", "").Trim());
                    entry.Add("severity", "error");
                    entries.Add(entry);
                }
                return DecorateErrorsRecursive(exception.InnerException, entries);
            }
            return entries;
        }

        /// <summary>Populate this record with synthetic data.</summary>
        public void Populate(string state = "MA", string type = "Natural", string sex = "Male")
        {
            DeathRecordFaker faker = new DeathRecordFaker(state, type, sex);
            record = faker.Generate(true);
            ije = new IJEMortality(record).ToString();
        }
    }

}

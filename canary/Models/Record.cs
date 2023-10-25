using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VRDR;
using Newtonsoft.Json;
using System.IO;
using System.Text.Json.Nodes;

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
                foreach(PropertyInfo property in properties)
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

        /// <summary>Check the given FHIR record string and return a list of issues. Also returned
        /// the parsed record if parsing was successful.</summary>
        public static (Record record, List<Dictionary<string, string>> issues) CheckGet(string record, bool permissive)
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
                validateRecordType(newRecord);
                return (record: newRecord, issues: entries);
            }
            catch (Exception e)
            {
                entries = DecorateErrors(e);
            }
            return (record: newRecord, issues: entries);
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
            if(jsonData["type"] == null)
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

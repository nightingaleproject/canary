using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VRDR;
using Newtonsoft.Json;
using Hl7.Fhir.Model;

namespace canary.Models
{
    public class Message
    {
        private BaseMessage message { get; set; }

        private static Dictionary<string, string> messageDescription = new Dictionary<string, string>()
        {
            { "MessageTimestamp", "The Timestamp of the Message" },
            { "MessageId", "The Message Identifier" },
            { "MessageType", "The NCHS Message Type" },
            { "MessageSource", "The Jurisdiction Message Source" },
            { "MessageDestination", "The NCHS Message Endpoint" },
            { "CertificateNumber", "Death Certificate Number (DeathRecord Identifier)" },
            { "StateAuxiliaryIdentifier", "Auxiliary State File Number (DeathRecord BundleIdentifier)" },
            { "NCHSIdentifier", "The NCHS compound identifier for the supplied DeathRecord" },
            { "DeathRecord", "A VRDR Death Record Object" },
            { "DeathYear", "The year in which the death occurred" },
            { "DeathJurisdictionID", "Two character identifier of the jurisdiction in which the death occurred" },
        };

        private static String[] validateForPresence = new String[] {
            "MessageTimestamp",
            "MessageId",
            "MessageSource"
        };

        public int MessageId { get; set; }

        public Message() {}

        public Message(string message)
        {
            this.message = BaseMessage.Parse(message, false);
        }

        public Message(BaseMessage message)
        {
            this.message = message;
        }

        public Message(Record record, String type)
        {
            DeathRecord dr = record.GetRecord();
            switch (type)
            {
                case "Submission": case "http://nchs.cdc.gov/vrdr_submission":
                    message = new DeathRecordSubmission(dr);
                    message.MessageSource = "https://example.com/jurisdiction/message/endpoint";
                    break;
                case "Update": case "http://nchs.cdc.gov/vrdr_submission_update":
                    message = new DeathRecordUpdate(dr);
                    message.MessageSource = "https://example.com/jurisdiction/message/endpoint";
                    break;
                case "Void": case "http://nchs.cdc.gov/vrdr_submission_void":
                    message = new VoidMessage(dr);
                    message.MessageSource = "https://example.com/jurisdiction/message/endpoint";
                    break;
                default:
                    throw new ArgumentException($"The given message type {type} is not valid.", "type");
            }
        }

        public BaseMessage GetMessage()
        {
            return message;
        }

        public string MessageType
        {
            get
            {
                return message.MessageType;
            }
        }

        public static string GetDescriptionFor(string entry)
        {
            return messageDescription.GetValueOrDefault(entry, "Unknown Property");
        }

        public Dictionary<string, Message> GetResponsesFor(String type)
        {
            Dictionary<string, Message> result = new Dictionary<string, Message>();
            // Create acknowledgement
            AckMessage ack = new AckMessage(message);
            Message ackMsg = new Message(ack);
            result.Add("ACK", ackMsg);

            // Create the extraction error
            ExtractionErrorMessage err = new ExtractionErrorMessage(message);
            // Add the issues found during processing
            var issues = new List<Issue>();
            var issue = new Issue(OperationOutcome.IssueSeverity.Fatal, OperationOutcome.IssueType.Invalid, "This is a fake message");
            issues.Add(issue);
            err.Issues = issues;
            Message errMsg = new Message(err);
            result.Add("Error", errMsg);

            switch (type)
            {
                case "Submission": case "http://nchs.cdc.gov/vrdr_submission": case "Update": case "http://nchs.cdc.gov/vrdr_submission_update":
                    // Create the ethnicity coding
                    CodingResponseMessage mre = new CodingResponseMessage(message);
                    var ethnicity = new Dictionary<CodingResponseMessage.HispanicOrigin, string>();
                    ethnicity.Add(CodingResponseMessage.HispanicOrigin.DETHNICE, "222");
                    ethnicity.Add(CodingResponseMessage.HispanicOrigin.DETHNIC5C, "222");
                    mre.Ethnicity = ethnicity;

                    // Create the race coding
                    var race = new Dictionary<CodingResponseMessage.RaceCode, string>();
                    race.Add(CodingResponseMessage.RaceCode.RACE1E, "500");
                    race.Add(CodingResponseMessage.RaceCode.RACE17C, "A09");
                    race.Add(CodingResponseMessage.RaceCode.RACEBRG, "03");
                    mre.Race = race;

                    Message mreMsg = new Message(mre);
                    result.Add("MRE", mreMsg);

                    CodingResponseMessage trx = new CodingResponseMessage(message);
                    
                    // Create the cause of death coding
                    trx.UnderlyingCauseOfDeath = "A04.7";

                    // Assign the record axis codes
                    var recordAxisCodes = new List<string>();
                    recordAxisCodes.Add("A04.7");
                    recordAxisCodes.Add("A41.9");
                    recordAxisCodes.Add("J18.9");
                    recordAxisCodes.Add("J96.0");
                    trx.CauseOfDeathRecordAxis = recordAxisCodes;

                    // Assign the entity axis codes
                    var builder = new CauseOfDeathEntityAxisBuilder();
                    builder.Add("1", "1", "A04.7");
                    builder.Add("1", "2", "A41.9");
                    builder.Add("2", "1", "J18.9");
                    trx.CauseOfDeathEntityAxis = builder.ToCauseOfDeathEntityAxis();

                    Message trxMsg = new Message(trx);
                    result.Add("TRX", trxMsg);
                    break;
                case "Void": case "http://nchs.cdc.gov/vrdr_submission_void":
                    break;
                default:
                    throw new ArgumentException($"The given message type {type} is not valid.", "type");
            }     
            return result;

        }

        // Returns whether or not we should only validate presence for these fields and not their values
        public static Boolean validatePresenceOnly(string field)
        {
            return Array.Exists(validateForPresence, element => element == field);
        }

        public string Xml
        {
            get
            {
                if (message == null)
                {
                    return null;
                }
                return message.ToXML();
            }
        }

        public string Json
        {
            get
            {
                if (message == null)
                {
                    return null;
                }
                return message.ToJSON();
            }
        }
    }
}

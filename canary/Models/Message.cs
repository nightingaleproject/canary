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
            { "MessageDestinations", "The NCHS Message Endpoints" },
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

        public Message() { }

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
                case "Submission":
                case "http://nchs.cdc.gov/vrdr_submission":
                    message = new DeathRecordSubmissionMessage(dr);
                    break;
                case "Update":
                case "http://nchs.cdc.gov/vrdr_submission_update":
                    message = new DeathRecordUpdateMessage(dr);
                    break;
                case "Void":
                case "http://nchs.cdc.gov/vrdr_submission_void":
                    message = new DeathRecordVoidMessage(dr);
                    break;
                case "Alias":
                case "http://nchs.cdc.gov/vrdr_alias":
                    message = new DeathRecordAliasMessage(dr);
                    break;
                default:
                    throw new ArgumentException($"The given message type {type} is not valid.", "type");
            }
            message.MessageSource = "https://example.com/jurisdiction/message/endpoint";
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
            AcknowledgementMessage ack = new AcknowledgementMessage(message);
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
                case "Submission":
                case "http://nchs.cdc.gov/vrdr_submission":
                case "Update":
                case "http://nchs.cdc.gov/vrdr_submission_update":
                    DeathRecordSubmissionMessage drsm = message as DeathRecordSubmissionMessage;
                    if (drsm == null)
                    {
                        throw new ArgumentException($"The given message type {type} requires a DeathRecordSubmissionMessage.", "type");
                    }
                    // Create the ethnicity coding
                    DemographicsCodingMessage mre = new DemographicsCodingMessage(drsm.DeathRecord);
                    mre.DeathRecord.EthnicityLiteral = "222";
                    mre.DeathRecord.HispanicCodeHelper = "222";

                    // Create the race coding
                    mre.DeathRecord.FirstEditedRaceCodeHelper = "500";
                    mre.DeathRecord.SecondAmericanIndianRaceCodeHelper = "A09"; ;

                    Message mreMsg = new Message(mre);
                    result.Add("MRE", mreMsg);

                    CauseOfDeathCodingMessage trx = new CauseOfDeathCodingMessage(drsm.DeathRecord);

                    // Create the cause of death coding
                    trx.DeathRecord.AutomatedUnderlyingCOD = "A04.7";

                    // Assign the record axis codes
                    // These leave out "position" and "pregnancy", which weren't specified previously (before IG 1.3 update)
                    // It appears (from VRDR library) that these values are only used if pregnancy is true ("1") and position is "2"
                    IList<(int, string, bool)> causeOfDeathRecordAxis = new List<(int, string, bool)>();
                    causeOfDeathRecordAxis.Add((0, "A04.7", false));
                    causeOfDeathRecordAxis.Add((0, "A41.9", false));
                    causeOfDeathRecordAxis.Add((0, "J18.9", false));
                    causeOfDeathRecordAxis.Add((0, "J96.0", false));
                    trx.DeathRecord.RecordAxisCauseOfDeath = causeOfDeathRecordAxis;

                    // Assign the entity axis codes
                    IList<(int, int, string, bool)> causeOfDeathEntityAxis = new List<(int, int, string, bool)>();
                    causeOfDeathEntityAxis.Add((1, 1, "A04.7", false));
                    causeOfDeathEntityAxis.Add((1, 2, "A41.9", false));
                    causeOfDeathEntityAxis.Add((2, 1, "J18.9", false));
                    trx.DeathRecord.EntityAxisCauseOfDeath = causeOfDeathEntityAxis;

                    Message trxMsg = new Message(trx);
                    result.Add("TRX", trxMsg);
                    break;
                case "Void":
                case "http://nchs.cdc.gov/vrdr_submission_void":
                case "Alias":
                case "http://nchs.cdc.gov/vrdr_alias":
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

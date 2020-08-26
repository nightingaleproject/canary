using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VRDR;
using Newtonsoft.Json;

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

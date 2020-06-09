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

        public Message(Record record, String type)
        {
            DeathRecord dr = record.GetRecord();
            switch (type)
            {
                case "Submission":
                    message = new DeathRecordSubmission(dr);
                    message.MessageSource = "https://example.com/juristdiction/message/endpoint";
                    break;
                case "Update":
                    message = new DeathRecordUpdate(dr);
                    message.MessageSource = "https://example.com/juristdiction/message/endpoint";
                    break;
                case "Void":
                    message = new VoidMessage(dr);
                    message.MessageSource = "https://example.com/juristdiction/message/endpoint";
                    break;
                default:
                    throw new ArgumentException($"The given message type {type} is not valid.", "type");
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Hl7.Fhir.Serialization;
using Hl7.FhirPath;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VRDR;

namespace canary.Models
{
    public class Test
    {
        public int TestId { get; set; }

        public DateTime Created { get; set; }

        public DateTime CompletedDateTime { get; set; }

        public bool CompletedBool { get; set; }

        public int Total { get; set; }

        public int Correct { get; set; }

        public int Incorrect { get; set; }

        public Record ReferenceRecord { get; set; }

        public Record TestRecord { get; set; }

        public Message TestMessage { get; set; }

        public string Results { get; set; }

        public string Type { get; set; }

        public Test()
        {
            Created = DateTime.Now;
            Total = 0;
            Correct = 0;
            Incorrect = 0;
            CompletedBool = false;
            ReferenceRecord = new Record();
            ReferenceRecord.Populate();
        }

        public Test(DeathRecord record)
        {
            Created = DateTime.Now;
            Total = 0;
            Correct = 0;
            Incorrect = 0;
            CompletedBool = false;
            ReferenceRecord = new Record(record);
        }

        public Test Run(string description)
        {
            if (Type.Contains("Message"))
            {
                TestMessage = new Message(description);
                Results = JsonConvert.SerializeObject(MessageCompare());
            }
            else
            {
                TestRecord = new Record(DeathRecord.FromDescription(description));
                Results = JsonConvert.SerializeObject(RecordCompare());
            }
            CompletedDateTime = DateTime.Now;
            CompletedBool = true;
            return this;
        }

        public Test Run(DeathRecord record)
        {
            TestRecord = new Record(record);
            Results = JsonConvert.SerializeObject(RecordCompare());
            CompletedDateTime = DateTime.Now;
            CompletedBool = true;
            return this;
        }

        public Dictionary<string, Dictionary<string, dynamic>> MessageCompare()
        {
            Dictionary<string, Dictionary<string, dynamic>> description = new Dictionary<string, Dictionary<string, dynamic>>();
            BaseMessage bundle = TestMessage.GetMessage();
            DeathRecord record = ReferenceRecord.GetRecord();
            BaseMessage referenceBundle = new Message(ReferenceRecord, bundle.MessageType).GetMessage();
            // 
            // On the frontend this shares the same view as the RecordCompare below. This heading
            // is shown above the results in the app.
            string heading = "Message Validation Results";
            description.Add(heading, new Dictionary<string, dynamic>());
            Dictionary<string, dynamic> category = description[heading];
            foreach (PropertyInfo property in bundle.GetType().GetProperties())
            {
                // Add the new property to the category
                category[property.Name] = new Dictionary<string, dynamic>();
                category[property.Name]["Name"] = property.Name;
                category[property.Name]["Type"] = (Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType).Name;
                category[property.Name]["Description"] = Message.GetDescriptionFor(property.Name);
                category[property.Name]["Value"] = property.GetValue(referenceBundle);
                category[property.Name]["FoundValue"] = property.GetValue(bundle);
                // The record should be valid since we check its validity elsewhere.
                // Here we just check to make sure the record is properly embedded
                // into the message bundle.
                if (property.PropertyType == typeof(DeathRecord))
                {
                    DeathRecord extracted = (DeathRecord)property.GetValue(bundle);
                    TestRecord = new Record(extracted);
                    int previousIncorrect = Incorrect;
                    // Add the record comparison results to the message comparison results
                    Dictionary<string, Dictionary<string, dynamic>> recordCompare = RecordCompare();
                    foreach (KeyValuePair<string, Dictionary<string, dynamic>> entry in recordCompare)
                    {
                        description.Add(entry.Key, entry.Value);
                    }
                    category.Remove(property.Name);
                    //category[property.Name]["SnippetJSON"] = JsonConvert.SerializeObject(recordCompare);
                    // // See if the value of Incorrect changed in 'RecordCompare' and use that to determine if the
                    // // Record matches or not.
                    // category[property.Name]["Match"] = previousIncorrect.Equals(Incorrect) ? "true" : "false";                    
                }
                else if (Message.validatePresenceOnly(property.Name))
                {
                    // Basic message validation ensures that these fields are present if they are required
                    // These values do not have to match our reference record and are just therefore
                    // set to valid here.
                    MarkCorrect();
                    category[property.Name]["Match"] = "true";
                    // Override the displayed value to be equal to what the user provided on the UI
                    category[property.Name]["Value"] = category[property.Name]["FoundValue"];
                }
                else
                {
                    // Using == here seems to be checking ReferenceEquals and not Equals, causing the equality to return false.
                    // Calling Prop1.Equals(Prop2) here raises an error if the value is null in the ReferenceBundle.
                    // The best option here is to just use the object.Equals operator.
                    // JToken.DeepEquals(property.GetValue(referenceBundle), property.GetValue(bundle)))

                    // if (Equals(property.GetValue(referenceBundle), property.GetValue(bundle)))

                    var a = property.GetValue(referenceBundle);
                    var b = property.GetValue(bundle);

                    if (a == null && b == null)
                    {
                        MarkCorrect();
                        category[property.Name]["Match"] = "true";
                    }
                    else if (a == null || b == null)
                    {
                        MarkIncorrect();
                        category[property.Name]["Match"] = "false";
                    }
                    else if (JToken.DeepEquals(JToken.FromObject(a), JToken.FromObject(b)))
                    {
                        MarkCorrect();
                        category[property.Name]["Match"] = "true";
                    }
                    else
                    {
                        MarkIncorrect();
                        category[property.Name]["Match"] = "false";
                    }
                }
            }

            return description;
        }

        public Dictionary<string, Dictionary<string, dynamic>> RecordCompare()
        {
            Dictionary<string, Dictionary<string, dynamic>> description = new Dictionary<string, Dictionary<string, dynamic>>();
            foreach (PropertyInfo property in typeof(DeathRecord).GetProperties().OrderBy(p => p.GetCustomAttribute<Property>().Priority))
            {
                // Grab property annotation for this property
                Property info = property.GetCustomAttribute<Property>();

                // Skip properties that shouldn't be serialized.
                if (!info.Serialize)
                {
                    continue;
                }

                // Skip properties that are lost in the IJE format (if the test is a roundtrip).
                if (!info.CapturedInIJE && (Type != null && Type.Contains("Roundtrip")))
                {
                    continue;
                }

                // Add category if it doesn't yet exist
                if (!description.ContainsKey(info.Category))
                {
                    description.Add(info.Category, new Dictionary<string, dynamic>());
                }

                // Add the new property to the category
                Dictionary<string, dynamic> category = description[info.Category];
                category[property.Name] = new Dictionary<string, dynamic>();

                // Add the attributes of the property
                category[property.Name]["Name"] = info.Name;
                category[property.Name]["Type"] = info.Type.ToString();
                category[property.Name]["Description"] = info.Description;

                // Add snippets for reference
                FHIRPath path = property.GetCustomAttribute<FHIRPath>();
                var matches = ReferenceRecord.GetRecord().GetITypedElement().Select(path.Path);
                if (matches.Count() > 0)
                {
                    if (info.Type == Property.Types.TupleCOD)
                    {
                        // Make sure to grab all of the Conditions for COD
                        string xml = "";
                        string json = "";
                        foreach (var match in matches)
                        {
                            xml += match.ToXml();
                            json += match.ToJson() + ",";
                        }
                        category[property.Name]["SnippetXML"] = xml;
                        category[property.Name]["SnippetJSON"] = "[" + json + "]";
                    }
                    else if (!String.IsNullOrWhiteSpace(path.Element))
                    {
                        // Since there is an "Element" for this path, we need to be more
                        // specific about what is included in the snippets.
                        XElement root = XElement.Parse(matches.First().ToXml());
                        XElement node = root.DescendantsAndSelf("{http://hl7.org/fhir}" + path.Element).FirstOrDefault();
                        if (node != null)
                        {
                            node.Name = node.Name.LocalName;
                            category[property.Name]["SnippetXML"] = node.ToString();
                        }
                        else
                        {
                            category[property.Name]["SnippetXML"] = "";
                        }
                        Dictionary<string, dynamic> jsonRoot =
                           JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(matches.First().ToJson(),
                               new JsonSerializerSettings() { DateParseHandling = DateParseHandling.None });
                        if (jsonRoot != null && jsonRoot.Keys.Contains(path.Element))
                        {
                            category[property.Name]["SnippetJSON"] = "{" + $"\"{path.Element}\": \"{jsonRoot[path.Element]}\"" + "}";
                        }
                        else
                        {
                            category[property.Name]["SnippetJSON"] = "";
                        }
                    }
                    else
                    {
                        category[property.Name]["SnippetXML"] = matches.First().ToXml();
                        category[property.Name]["SnippetJSON"] = matches.First().ToJson();
                    }

                }
                else
                {
                    category[property.Name]["SnippetXML"] = "";
                    category[property.Name]["SnippetJSON"] = "";
                }

                // Add snippets for test
                FHIRPath pathTest = property.GetCustomAttribute<FHIRPath>();
                var matchesTest = TestRecord.GetRecord().GetITypedElement().Select(pathTest.Path);
                if (matchesTest.Count() > 0)
                {
                    if (info.Type == Property.Types.TupleCOD)
                    {
                        // Make sure to grab all of the Conditions for COD
                        string xml = "";
                        string json = "";
                        foreach (var match in matchesTest)
                        {
                            xml += match.ToXml();
                            json += match.ToJson() + ",";
                        }
                        category[property.Name]["SnippetXMLTest"] = xml;
                        category[property.Name]["SnippetJSONTest"] = "[" + json + "]";
                    }
                    else if (!String.IsNullOrWhiteSpace(pathTest.Element))
                    {
                        // Since there is an "Element" for this path, we need to be more
                        // specific about what is included in the snippets.
                        XElement root = XElement.Parse(matchesTest.First().ToXml());
                        XElement node = root.DescendantsAndSelf("{http://hl7.org/fhir}" + pathTest.Element).FirstOrDefault();
                        if (node != null)
                        {
                            node.Name = node.Name.LocalName;
                            category[property.Name]["SnippetXMLTest"] = node.ToString();
                        }
                        else
                        {
                            category[property.Name]["SnippetXMLTest"] = "";
                        }
                        Dictionary<string, dynamic> jsonRoot =
                           JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(matchesTest.First().ToJson(),
                               new JsonSerializerSettings() { DateParseHandling = DateParseHandling.None });
                        if (jsonRoot != null && jsonRoot.Keys.Contains(pathTest.Element))
                        {
                            category[property.Name]["SnippetJSONTest"] = "{" + $"\"{pathTest.Element}\": \"{jsonRoot[pathTest.Element]}\"" + "}";
                        }
                        else
                        {
                            category[property.Name]["SnippetJSONTest"] = "";
                        }
                    }
                    else
                    {
                        category[property.Name]["SnippetXMLTest"] = matchesTest.First().ToXml();
                        category[property.Name]["SnippetJSONTest"] = matchesTest.First().ToJson();
                    }

                }
                else
                {
                    category[property.Name]["SnippetXMLTest"] = "";
                    category[property.Name]["SnippetJSONTest"] = "";
                }

                // RecordCompare values
                if (info.Type == Property.Types.Dictionary)
                {
                    // Special case for Dictionary; we want to be able to describe what each key means
                    Dictionary<string, string> valueReference = (Dictionary<string, string>)property.GetValue(ReferenceRecord.GetRecord());

                    if (valueReference == null || valueReference.Values.All(vRV => String.IsNullOrWhiteSpace(vRV)))
                    {
                        continue;
                    }

                    // If this is a coded value, the comparison should be case sensitive
                    StringComparison caseSensitivity = StringComparison.OrdinalIgnoreCase;
                    if (valueReference.ContainsKey("code"))
                    {
                        caseSensitivity = StringComparison.Ordinal;
                    }
                    Dictionary<string, string> valueTest = (Dictionary<string, string>)property.GetValue(TestRecord.GetRecord());
                    Dictionary<string, Dictionary<string, string>> moreInfo = new Dictionary<string, Dictionary<string, string>>();
                    bool match = true;
                    foreach (PropertyParam parameter in property.GetCustomAttributes<PropertyParam>())
                    {
                        moreInfo[parameter.Key] = new Dictionary<string, string>();
                        moreInfo[parameter.Key]["Description"] = parameter.Description;
                        if (valueReference != null && valueReference.ContainsKey(parameter.Key))
                        {
                            moreInfo[parameter.Key]["Value"] = valueReference[parameter.Key];
                        }
                        else
                        {
                            moreInfo[parameter.Key]["Value"] = null;
                        }
                        if (valueTest != null && valueTest.ContainsKey(parameter.Key))
                        {
                            moreInfo[parameter.Key]["FoundValue"] = valueTest[parameter.Key];
                        }
                        else
                        {
                            moreInfo[parameter.Key]["FoundValue"] = null;
                        }
                        // Check for match
                        if ((valueReference.ContainsKey(parameter.Key) && valueTest.ContainsKey(parameter.Key)) &&
                            (String.Equals((string)valueReference[parameter.Key], (string)valueTest[parameter.Key], caseSensitivity)))
                        {
                            // Equal
                            MarkCorrect();
                            moreInfo[parameter.Key]["Match"] = "true";
                        }
                        else if ((valueReference.ContainsKey(parameter.Key) && valueTest.ContainsKey(parameter.Key)) &&
                                    String.IsNullOrWhiteSpace((string)valueReference[parameter.Key]) &&
                                    String.IsNullOrWhiteSpace((string)valueTest[parameter.Key]))
                        {
                            // Equal
                            MarkCorrect();
                            moreInfo[parameter.Key]["Match"] = "true";
                        }
                        else if (!valueReference.ContainsKey(parameter.Key) && !valueTest.ContainsKey(parameter.Key))
                        {
                            // Both null, equal
                            MarkCorrect();
                            moreInfo[parameter.Key]["Match"] = "true";
                        }
                        else if (!valueReference.ContainsKey(parameter.Key) || (valueReference.ContainsKey(parameter.Key) && String.IsNullOrWhiteSpace(valueReference[parameter.Key])))
                        {
                            // Source is empty, so no need to punish test
                            MarkCorrect();
                            moreInfo[parameter.Key]["Match"] = "true";
                        }
                        else if (parameter.Key == "display" && (!valueTest.ContainsKey(parameter.Key) || String.IsNullOrWhiteSpace(valueTest[parameter.Key])))
                        {
                            // Test record had nothing for display, equal
                            MarkCorrect();
                            moreInfo[parameter.Key]["Match"] = "true";
                        }
                        else
                        {
                            // Not equal
                            MarkIncorrect();
                            moreInfo[parameter.Key]["Match"] = "false";
                            match = false;
                        }
                    }
                    category[property.Name]["Match"] = match ? "true" : "false";
                    category[property.Name]["Value"] = moreInfo;
                }
                else
                {
                    category[property.Name]["Value"] = property.GetValue(ReferenceRecord.GetRecord());
                    category[property.Name]["FoundValue"] = property.GetValue(TestRecord.GetRecord());

                    // RecordCompare values
                    if (info.Type == Property.Types.String)
                    {
                        if (String.Equals((string)property.GetValue(ReferenceRecord.GetRecord()), (string)property.GetValue(TestRecord.GetRecord()), StringComparison.OrdinalIgnoreCase))
                        {
                            MarkCorrect();
                            category[property.Name]["Match"] = "true";
                        }
                        else if (String.IsNullOrWhiteSpace((string)property.GetValue(ReferenceRecord.GetRecord())))
                        {
                            MarkCorrect();
                            category[property.Name]["Match"] = "true";
                        }
                        else if (!String.IsNullOrWhiteSpace((string)property.GetValue(TestRecord.GetRecord())) &&
                                   !String.IsNullOrWhiteSpace((string)property.GetValue(ReferenceRecord.GetRecord())) &&
                                   ((string)property.GetValue(TestRecord.GetRecord())).ToLower().Contains(((string)property.GetValue(ReferenceRecord.GetRecord())).ToLower()))
                        {
                            MarkCorrect();
                            category[property.Name]["Match"] = "true";
                        }
                        else
                        {
                            MarkIncorrect();
                            category[property.Name]["Match"] = "false";
                        }
                    }
                    else if (info.Type == Property.Types.StringDateTime)
                    {
                        DateTimeOffset referenceDateTime;
                        DateTimeOffset testDateTime;
                        if (property.GetValue(ReferenceRecord.GetRecord()) == null && property.GetValue(TestRecord.GetRecord()) == null)
                        {
                            MarkCorrect();
                            category[property.Name]["Match"] = "true";
                        }
                        else if (String.IsNullOrWhiteSpace((string)property.GetValue(ReferenceRecord.GetRecord())))
                        {
                            MarkCorrect();
                            category[property.Name]["Match"] = "true";
                        }
                        else if (DateTimeOffset.TryParse((string)property.GetValue(ReferenceRecord.GetRecord()), out referenceDateTime))
                        {
                            if (DateTimeOffset.TryParse((string)property.GetValue(TestRecord.GetRecord()), out testDateTime))
                            {
                                if (DateTimeOffset.Compare(referenceDateTime, testDateTime) == 0)
                                {
                                    MarkCorrect();
                                    category[property.Name]["Match"] = "true";
                                }
                                else if (DateTimeOffset.Compare(new DateTimeOffset(referenceDateTime.Date, TimeSpan.Zero), new DateTimeOffset(testDateTime.Date, TimeSpan.Zero)) == 0)
                                {
                                    MarkCorrect();
                                    category[property.Name]["Match"] = "true";
                                }
                                else
                                {
                                    MarkIncorrect();
                                    category[property.Name]["Match"] = "false";
                                }
                            }
                            else
                            {
                                MarkIncorrect();
                                category[property.Name]["Match"] = "false";
                            }
                        }
                        else
                        {
                            MarkIncorrect();
                            category[property.Name]["Match"] = "false";
                        }
                    }
                    else if (info.Type == Property.Types.StringArr)
                    {
                        string[] referenceArr = (string[])property.GetValue(ReferenceRecord.GetRecord());
                        string[] testArr = (string[])property.GetValue(TestRecord.GetRecord());
                        if (referenceArr != null)
                        {
                            if (testArr != null)
                            {
                                if (String.Equals(String.Join(",", referenceArr.ToList().OrderBy(s => s, StringComparer.OrdinalIgnoreCase).ToArray()), String.Join(",", testArr.ToList().OrderBy(s => s, StringComparer.OrdinalIgnoreCase).ToArray()), StringComparison.OrdinalIgnoreCase))
                                {
                                    MarkCorrect();
                                    category[property.Name]["Match"] = "true";
                                }
                                else if (referenceArr.ToList().Count == 0)
                                {
                                    MarkCorrect();
                                    category[property.Name]["Match"] = "true";
                                }
                                else
                                {
                                    MarkIncorrect();
                                    category[property.Name]["Match"] = "false";
                                }
                            }
                            else
                            {
                                MarkIncorrect();
                                category[property.Name]["Match"] = "false";
                            }
                        }
                        else
                        {
                            MarkCorrect();
                            category[property.Name]["Match"] = "true";
                        }
                    }
                    else if (info.Type == Property.Types.Bool)
                    {
                        if (bool.Equals(property.GetValue(ReferenceRecord.GetRecord()), property.GetValue(TestRecord.GetRecord())))
                        {
                            MarkCorrect();
                            category[property.Name]["Match"] = "true";
                        }
                        else if (property.GetValue(ReferenceRecord.GetRecord()) == null)
                        {
                            MarkCorrect();
                            category[property.Name]["Match"] = "true";
                        }
                        else
                        {
                            MarkIncorrect();
                            category[property.Name]["Match"] = "false";
                        }
                    }
                    else if (info.Type == Property.Types.TupleArr)
                    {
                        Tuple<string, string>[] referenceArr = (Tuple<string, string>[])property.GetValue(ReferenceRecord.GetRecord());
                        Tuple<string, string>[] testArr = (Tuple<string, string>[])property.GetValue(TestRecord.GetRecord());
                        if (referenceArr != null)
                        {
                            if (testArr != null)
                            {
                                if (String.Equals(String.Join(",", referenceArr.ToList().OrderBy(s => s.Item1 + s.Item2, StringComparer.OrdinalIgnoreCase)), String.Join(",", testArr.ToList().OrderBy(s => s.Item1 + s.Item2, StringComparer.OrdinalIgnoreCase)), StringComparison.OrdinalIgnoreCase))
                                {
                                    MarkCorrect();
                                    category[property.Name]["Match"] = "true";
                                }
                                else if (referenceArr.ToList().Count == 0)
                                {
                                    MarkCorrect();
                                    category[property.Name]["Match"] = "true";
                                }
                                else
                                {
                                    MarkIncorrect();
                                    category[property.Name]["Match"] = "false";
                                }
                            }
                            else
                            {
                                MarkIncorrect();
                                category[property.Name]["Match"] = "false";
                            }
                        }
                        else if (testArr != null)
                        {
                            MarkIncorrect();
                            category[property.Name]["Match"] = "false";
                        }
                        else
                        {
                            MarkCorrect();
                            category[property.Name]["Match"] = "true";
                        }
                    }
                    else if (info.Type == Property.Types.TupleCOD)
                    {
                        Console.WriteLine(property.Name);
                        Tuple<string, string, Dictionary<string, string>>[] referenceArr = (Tuple<string, string, Dictionary<string, string>>[])property.GetValue(ReferenceRecord.GetRecord());
                        Tuple<string, string, Dictionary<string, string>>[] testArr = (Tuple<string, string, Dictionary<string, string>>[])property.GetValue(TestRecord.GetRecord());
                        if (referenceArr != null)
                        {
                            if (testArr != null)
                            {
                                string lower1 = String.Join(",", referenceArr.ToList().OrderBy(s => s.Item1 + s.Item2, StringComparer.Ordinal));
                                string lower2 = String.Join(",", testArr.ToList().OrderBy(s => s.Item1 + s.Item2, StringComparer.Ordinal));
                                if (String.Equals(lower1, lower2, StringComparison.Ordinal))
                                {
                                    MarkCorrect();
                                    category[property.Name]["Match"] = "true";
                                }
                                else
                                {
                                    MarkIncorrect();
                                    category[property.Name]["Match"] = "false";
                                }
                            }
                            else
                            {
                                MarkIncorrect();
                                category[property.Name]["Match"] = "false";
                            }
                        }
                        else if (testArr != null)
                        {
                            MarkIncorrect();
                            category[property.Name]["Match"] = "false";
                        }
                        else
                        {
                            MarkCorrect();
                            category[property.Name]["Match"] = "true";
                        }
                    }
                    else if (info.Type == Property.Types.Int32)
                    {
                        if (uint.Equals(property.GetValue(ReferenceRecord.GetRecord()), property.GetValue(TestRecord.GetRecord())))
                        {
                            MarkCorrect();
                            category[property.Name]["Match"] = "true";
                        }
                        else if (property.GetValue(ReferenceRecord.GetRecord()) == null)
                        {
                            MarkCorrect();
                            category[property.Name]["Match"] = "true";
                        }
                        else
                        {
                            MarkIncorrect();
                            category[property.Name]["Match"] = "false";
                        }
                    }
                }
            }
            return description;
        }

        private void MarkCorrect()
        {
            Total += 1;
            Correct += 1;
        }
        private void MarkIncorrect()
        {
            Total += 1;
            Incorrect += 1;
        }
    }
}

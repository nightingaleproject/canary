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
            #region Death Record items
            {"AUTOP","Was Autopsy performed" },
            { "AUTOPF","Were Autopsy Findings Available to Complete the Cause of Death?" }, 
            { "BCNO","Infant Death/Birth Linking - birth certificate number" },
            { "IDOB_YR","Infant Death/Birth Linking - year of birth" },
            { "BSTATE","Infant Death/Birth Linking - State, U.S. Territory or Canadian Province of Birth - code" },
            { "COD1A","Cause of Death Part I Line a" },
            { "INTERVAL1A","Cause of Death Part I Interval, Line a" },
            { "COD1B","Cause of Death Part I Line b" },
            { "INTERVAL1B","Cause of Death Part I Interval, Line b" },
            { "COD1C","Cause of Death Part I Line c" },
            { "INTERVAL1C","Cause of Death Part I Interval, Line c" },
            { "COD1D","Cause of Death Part I Line d" },
            { "INTERVAL1D","Cause of Death Part I Interval, Line d" },
            { "OTHERCONDITION","Cause of Death Part II" },
            { "CERTFIRST","Certifier?s First Name" },
            { "CERTMIDDLE","Certifier?s Middle Name" },
            { "CERTLAST","Certifier?s Last Name" },
            { "CERTSUFFIX","Certifier?s Suffix Name" },
            { "CERTSTNUM","Certifier - Street number" },
            { "CERTPREDIR","Certifier - Pre Directional" },
            { "CERTSTRNAME","Certifier - Street name" },
            { "CERTSTRDESIG","Certifier - Street designator" },
            { "CERTPOSTDIR","Certifier - Post Directional" },
            { "CERTUNITNUM","Certifier - Unit or apt number" },
            { "CERTADDRESS","Long string address for Certifier same as above but allows states to choose the way they capture information." },
            { "CERTCITYTEXT","Certifier - City or Town name" },
            { "CERTSTATECD","State, U.S. Territory or Canadian Province of Certifier - code" },
            { "CERTSTATE","State, U.S. Territory or Canadian Province of Certifier - literal" },
            { "CERTZIP","Certifier - Zip" },
            { "MFILED","Source flag: paper/electronic" },
            { "DOR_YR","Date of Registration?Year" },
            { "DOR_MO","Date of Registration?Month" },
            { "DOR_DY","Date of Registration?Day" },
            { "STATESP","State Specific Data" },
            { "REPLACE (deprecated)","Replacement Record" },
            { "DOD_YR","Date of Death?Year" },
            { "DPLACE","Place of Death" },
            { "DOD_MO","Date of Death?Month" },
            { "DOD_DY","Date of Death?Day" },
            { "TOD","Time of Death" },
            { "PPDATESIGNED","Person Pronouncing Date Signed" },
            { "PPTIME","Person Pronouncing Time Pronounced" },
            { "CERTL","Title of Certifier" },
            { "CERTDATE","Certifier Date Signed" },
            { "DSTATE","State, U.S. Territory or Canadian Province of Death - code" },
            { "COD","County of Death Occurrence" },
            { "DINSTI","Death Institution name" },
            { "ADDRESS_D","Long String address for place of death" },
            { "STNUM_D","Place of death. Street number" },
            { "PREDIR_D","Place of death. Pre Directional" },
            { "STNAME_D","Place of death. Street name" },
            { "STDESIG_D","Place of death. Street designator" },
            { "POSTDIR_D","Place of death. Post Directional" },
            { "CITYTEXT_D","Place of death. City or Town name" },
            { "STATETEXT_D","Place of death. State name literal" },
            { "ZIP9_D","Place of death. Zip code" },
            { "COUNTYTEXT_D","Place of death. County of Death" },
            { "CITYCODE_D","Place of death. City FIPS code" },
            { "LONG_D","Place of death. Longitude" },
            { "LAT_D","Place of Death. Latitude" },
            { "DTHCOUNTRYCD","Country of Death - Code" },
            { "DTHCOUNTRY","Country of Death - Literal" },
            { "GNAME","Decedent?s Legal Name?Given" },
            { "MNAME","Decedent?s Legal Name?Middle" },
            { "LNAME","Decedent?s Legal Name?Last" },
            { "SUFF","Decedent?s Legal Name?Suffix" },
            { "SEX","Sex" },
            { "SSN","Social Security Number" },
            { "DOB_YR","Date of Birth?Year" },
            { "DOB_MO","Date of Birth?Month" },
            { "DOB_DY","Date of Birth?Day" },
            { "BPLACE_CNT","Birthplace?Country" },
            { "BPLACE_ST","State, U.S. Territory or Canadian Province of Birth - code" },
            { "CITYC","Decedent?s Residence?City" },
            { "COUNTYC","Decedent?s Residence?County" },
            { "STATEC","State, U.S. Territory or Canadian Province of Decedent?s residence - code" },
            { "COUNTRYC","Decedent?s Residence?Country" },
            { "LIMITS","Decedent?s Residence?Inside City Limits" },
            { "MARITAL","Marital Status" },
            { "MARITAL_BYPASS","Marital Status?Edit Flag" },
            { "SPOUSELV","Decedent?s spouse living at decedent?s DOD?" },
            { "STNUM_R","Decedent?s Residence - Street number" },
            { "PREDIR_R","Decedent?s Residence - Pre Directional" },
            { "STNAME_R","Decedent?s Residence - Street name" },
            { "STDESIG_R","Decedent?s Residence - Street designator" },
            { "POSTDIR_R","Decedent?s Residence - Post Directional" },
            { "UNITNUM_R","Decedent?s Residence - Unit or apt number" },
            { "CITYTEXT_R","Decedent?s Residence - City or Town name" },
            { "ZIP9_R","Decedent?s Residence - ZIP code" },
            { "COUNTYTEXT_R","Decedent?s Residence - County" },
            { "STATETEXT_R","Decedent?s Residence - State name" },
            { "COUNTRYTEXT_R","Decedent?s Residence - COUNTRY name" },
            { "ADDRESS_R","Long string address for decedent?s place of residence same as above but allows states to choose the way they capture information." },
            { "DMIDDLE","Middle Name of Decedent" },
            { "DMAIDEN","Decedent?s Maiden Name" },
            { "DBPLACECITYCODE","Decedent?s Birth Place City - Code" },
            { "DBPLACECITY","Decedent?s Birth Place City - Literal" },
            { "INFORMRELATE","Informant?s Relationship" },
            { "STATEBTH","State, U.S. Territory or Canadian Province of Birth - literal" },
            { "MARITAL_DESCRIP","Marital Descriptor" },
            { "AGETYPE","Decedent?s Age?Type" },
            { "AGE","Decedent?s Age?Units" },
            { "AGE_BYPASS","Decedent?s Age?Edit Flag" },
            { "DISP","Method of Disposition" },
            { "DEDUC","Decedent?s Education" },
            { "DEDUC_BYPASS","Decedent?s Education?Edit Flag" },
            { "FLNAME","Father?s Surname" },
            { "DDADF","Father?s First Name" },
            { "DDADMID","Father?s Middle Name" },
            { "FATHERSUFFIX","Father?s Suffix" },
            { "ARMEDF","Decedent ever served in Armed Forces?" },
            { "DMOMF","Mother?s First Name" },
            { "DMOMMID","Mother?s Middle Name" },
            { "DMOMMDN","Mother?s Maiden Surname" },
            { "MOTHERSSUFFIX","Mother?s Suffix" },
            { "SPOUSEF","Spouse?s First Name" },
            { "SPOUSEL","Husband?s Surname/Wife?s Maiden Last Name" },
            { "SPOUSEMIDNAME","Spouse?s Middle Name" },
            { "SPOUSESUFFIX","Spouse?s Suffix" },
            { "PREG","Pregnancy" },
            { "PREG_BYPASS","If Female?Edit Flag: From EDR only" },
            { "OCCUP","Occupation ? Literal" },
            { "INDUST","Industry ? Literal" },
            { "OCCUPC4","Occupation ? 4 digit Code" },
            { "INDUSTC4","Industry ? 4 digit Code" },
            { "DISPSTATECD","State, U.S. Territory or Canadian Province of Disposition - code" },
            { "DISPSTATE","Disposition State or Territory - Literal" },
            { "DISPCITYCODE","Disposition City - Code" },
            { "DISPCITY","Disposition City - Literal" },
            { "REFERRED","Was case Referred to Medical Examiner/Coroner?" },
            { "FUNFACNAME","Funeral Facility Name" },
            { "FUNFACSTNUM","Funeral Facility - Street number" },
            { "FUNFACPREDIR","Funeral Facility - Pre Directional" },
            { "FUNFACSTRNAME","Funeral Facility - Street name" },
            { "FUNFACSTRDESIG","Funeral Facility - Street designator" },
            { "FUNPOSTDIR","Funeral Facility - Post Directional" },
            { "FUNUNITNUM","Funeral Facility - Unit or apt number" },
            { "FUNFACADDRESS","Long string address for Funeral Facility same as above but allows states to choose the way they capture information." },
            { "FUNCITYTEXT","Funeral Facility - City or Town name" },
            { "FUNSTATECD","State, U.S. Territory or Canadian Province of Funeral Facility - code" },
            { "FUNSTATE","State, U.S. Territory or Canadian Province of Funeral Facility - literal" },
            { "FUNZIP","Funeral Facility - ZIP" },
            { "DOI_MO","Date of injury?month" },
            { "DOI_DY","Date of injury?day" },
            { "DOI_YR","Date of injury?year" },
            { "TOI_HR","Time of injury" },
            { "WORKINJ","Injury at work" },
            { "TOI_UNIT","Time of Injury Unit" },
            { "POILITRL","Place of Injury- literal" },
            { "HOWINJ","Describe How Injury Occurred" },
            { "TRANSPRT","If Transportation Accident, Specify" },
            { "COUNTYTEXT_I","County of Injury - literal" },
            { "COUNTYCODE_I","County of Injury code" },
            { "CITYTEXT_I","Town/city of Injury - literal" },
            { "CITYCODE_I","Town/city of Injury code" },
            { "STATECODE_I","State, U.S. Territory or Canadian Province of Injury - code" },
            { "LONG_I","Place of injury. Longitude" },
            { "LAT_I","Place of injury. Latitude" },
            { "STINJURY","State, U.S. Territory or Canadian Province of Injury - literal" },
            { "DETHNIC1","Decedent of Hispanic Origin??Mexican" },
            { "DETHNIC2","Decedent of Hispanic Origin??Puerto Rican" },
            { "DETHNIC3","Decedent of Hispanic Origin??Cuban" },
            { "DETHNIC4","Decedent of Hispanic Origin??Other" },
            { "DETHNIC5","Decedent of Hispanic Origin??Other, Literal" },
            { "RACE1","Decedent?s Race?White" },
            { "RACE2","Decedent?s Race?Black or African American" },
            { "RACE3","Decedent?s Race?American Indian or Alaska Native" },
            { "RACE4","Decedent?s Race?Asian Indian" },
            { "RACE5","Decedent?s Race?Chinese" },
            { "RACE6","Decedent?s Race?Filipino" },
            { "RACE7","Decedent?s Race?Japanese" },
            { "RACE8","Decedent?s Race?Korean" },
            { "RACE9","Decedent?s Race?Vietnamese" },
            { "RACE10","Decedent?s Race?Other Asian" },
            { "RACE11","Decedent?s Race?Native Hawaiian" },
            { "RACE12","Decedent?s Race?Guamanian or Chamorro" },
            { "RACE13","Decedent?s Race?Samoan" },
            { "RACE14","Decedent?s Race?Other Pacific Islander" },
            { "RACE15","Decedent?s Race?Other" },
            { "RACE16","Decedent?s Race?First American Indian or Alaska Native Literal" },
            { "RACE17","Decedent?s Race?Second American Indian or Alaska Native Literal" },
            { "RACE18","Decedent?s Race?First Other Asian Literal" },
            { "RACE19","Decedent?s Race?Second Other Asian Literal" },
            { "RACE20","Decedent?s Race?First Other Pacific Islander Literal" },
            { "RACE21","Decedent?s Race?Second Other Pacific Islander Literal" },
            { "RACE22","Decedent?s Race?First Other Literal" },
            { "RACE23","Decedent?s Race?Second Other Literal" },
            { "RACE_MVR","Decedent?s Race?Missing" },
            { "MANNER","Manner of Death" },
            { "PLACE1_1","Blank for One-Byte Field 1" },
            { "PLACE1_2","Blank for One-Byte Field 2" },
            { "PLACE1_3","Blank for One-Byte Field 3" },
            { "PLACE1_4","Blank for One-Byte Field 4" },
            { "PLACE1_5","Blank for One-Byte Field 5" },
            { "PLACE1_6","Blank for One-Byte Field 6" },
            { "PLACE8_1","Blank for Eight-Byte Field 1" },
            { "PLACE8_2","Blank for Eight-Byte Field 2" },
            { "PLACE8_3","Blank for Eight-Byte Field 3" },
            { "PLACE20","Blank for Twenty-Byte Field" },
            { "SUR_MO","Surgery Date?month" },
            { "SUR_DY","Surgery Date?day" },
            { "SUR_YR","Surgery Date?year" },
            { "TOBAC","Did Tobacco Use Contribute to Death?" },
            { "INACT","Activity at time of death (computer generated)" },
            { "ACME_UC","ACME Underlying Cause" },
            { "MAN_UC","Manual Underlying Cause" },
            { "RAC","Record-axis codes" },
            { "EAC","Entity-axis codes" },
            { "INJPL","Place of Injury (computer generated)" },
            { "RACE1E","First Edited Code" },
            { "RACE2E","Second Edited Code" },
            { "RACE3E","Third Edited Code" },
            { "RACE4E","Fourth Edited Code" },
            { "RACE5E","Fifth Edited Code" },
            { "RACE6E","Sixth Edited Code" },
            { "RACE7E","Seventh Edited Code" },
            { "RACE8E","Eighth Edited Code" },
            { "RACE16C","First American Indian Code" },
            { "RACE17C","Second American Indian Code" },
            { "RACE18C","First Other Asian Code" },
            { "RACE19C","Second Other Asian Code" },
            { "RACE20C","First Other Pacific Islander Code" },
            { "RACE21C","Second Other Pacific Islander Code" },
            { "RACE22C","First Other Race Code" },
            { "RACE23C","Second Other Race Code" },
            { "DETHNICE","Hispanic" },
            { "DETHNIC5C","Hispanic Code for Literal" },
            { "R_YR","Receipt date ? Year" },
            { "R_MO","Receipt date ? Month" },
            { "R_DY","Receipt date ? Day" },
            { "CS (TRX Field, no IJE Mapping)","coder status" },
            { "SHIP (TRX Field, no IJE Mapping)","shipment number" },
            { "INT_REJ","Intentional Reject" },
            { "SYS_REJ","Acme System Reject Codes" },
            { "TRX_FLG","Transax conversion flag: Computer Generated" },
            { "FILENO","Certificate Number" },
            { "AUXNO","Auxiliary State file number" },
            { "AUXNO2","Auxiliary State file number" },
            #endregion
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

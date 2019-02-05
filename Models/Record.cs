using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using FhirDeathRecord;
using Bogus;
using Bogus.Extensions.UnitedStates;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Introspection;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Specification;
using Hl7.Fhir.Utility;
using Hl7.Fhir.Model;
using Hl7.FhirPath;
using Newtonsoft.Json;

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

        public int RecordId { get; set; }

        public string Xml
        {
            get
            {
                return record.ToXML();
            }
            set
            {
                record = new DeathRecord(value);
            }
        }

        public string Json
        {
            get
            {
                return record.ToJSON();
            }
            set
            {
                record = new DeathRecord(value);
            }
        }

        public string Ije
        {
            get
            {
                return new IJEMortality(record).ToString();
            }
            set
            {
                record = new IJEMortality(value).ToDeathRecord();
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
                string ijeString = new IJEMortality(record).ToString();
                List<PropertyInfo> properties = typeof(IJEMortality).GetProperties().ToList().OrderBy(p => ((IJEField)p.GetCustomAttributes().First()).Field).ToList();
                List<Dictionary<string, string>> propList = new List<Dictionary<string, string>>();
                foreach(PropertyInfo property in properties)
                {
                    IJEField info = (IJEField)property.GetCustomAttributes().First();
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
                return record.ToDescription();
            }
            set
            {
                record = DeathRecord.FromDescription(value);
            }
        }

        public Record()
        {
            record = new DeathRecord();
        }

        public Record(DeathRecord record)
        {
            this.record = record;
        }

        public Record(string record)
        {
            // TODO: Handle "from" IJE here
            this.record = new DeathRecord(record, true);
        }

        /// <summary>Check the given FHIR XML record string and return a list of issues. Also returned
        /// the parsed record if parsing was successful.</summary>
        public static (Record record, List<Dictionary<string, string>> issues) CheckGetXml(string record, bool strict)
        {
            List<Dictionary<string, string>> entries = new List<Dictionary<string, string>>();
            Record newRecord = null;
            try
            {
                ParserSettings parserSettings = new ParserSettings { AcceptUnknownMembers = !strict, AllowUnrecognizedEnums = !strict };
                FhirXmlParser parser = new FhirXmlParser(parserSettings);
                Bundle bundle = parser.Parse<Bundle>(record);
                ITypedElement node = bundle.ToTypedElement();
                List<Hl7.Fhir.Utility.ExceptionNotification> problems = node.VisitAndCatch();
                foreach (Hl7.Fhir.Utility.ExceptionNotification problem in problems)
                {
                    Dictionary<string, string> entry = new Dictionary<string, string>();
                    entry.Add("message", problem.Message);
                    entry.Add("severity", problem.Severity.ToString());
                    entries.Add(entry);
                }
                newRecord = new Record(record);
            }
            catch (Exception e)
            {
                Dictionary<string, string> entry = new Dictionary<string, string>();
                entry.Add("message", e.Message);
                entry.Add("severity", Hl7.Fhir.Utility.ExceptionSeverity.Error.ToString());
                entries.Add(entry);
            }
            return (record: newRecord, issues: entries);
        }

        /// <summary>Check the given FHIR XML record string and return a list of issues. Also returned
        /// the parsed record if parsing was successful.</summary>
        public static (Record record, List<Dictionary<string, string>> issues) CheckGetJson(string record, bool strict)
        {
            List<Dictionary<string, string>> entries = new List<Dictionary<string, string>>();
            Record newRecord = null;
            try
            {
                ParserSettings parserSettings = new ParserSettings { AcceptUnknownMembers = !strict, AllowUnrecognizedEnums = !strict };
                FhirJsonParser parser = new FhirJsonParser(parserSettings);
                Bundle bundle = parser.Parse<Bundle>(record);
                ITypedElement node = bundle.ToTypedElement();
                List<Hl7.Fhir.Utility.ExceptionNotification> problems = node.VisitAndCatch();
                foreach (Hl7.Fhir.Utility.ExceptionNotification problem in problems)
                {
                    Dictionary<string, string> entry = new Dictionary<string, string>();
                    entry.Add("message", problem.Message);
                    entry.Add("severity", problem.Severity.ToString());
                    entries.Add(entry);
                }
                newRecord = new Record(record);
            }
            catch (Exception e)
            {
                Dictionary<string, string> entry = new Dictionary<string, string>();
                entry.Add("message", e.Message);
                entry.Add("severity", Hl7.Fhir.Utility.ExceptionSeverity.Error.ToString());
                entries.Add(entry);
            }
            return (record: newRecord, issues: entries);
        }

        /// <summary>Populate this record with synthetic data.</summary>
        public void Populate(string state = "MA", string type = "Natural", string sex = "Male")
        {
            Random random = new Random();
            Faker faker = new Faker("en");
            DataHelper dataHelper = DataHelper.Instance;

            // Grab Gender enum value
            Bogus.DataSets.Name.Gender gender = sex == "Male" ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female;

            // Was married?
            bool wasMarried = faker.Random.Bool();

            // Full state name for reference
            string stateName = dataHelper.StateCodeToStateName(state);

            ///////////////////////////////////////////////////////////////////
            // Composition
            ///////////////////////////////////////////////////////////////////

            record.Id = Convert.ToString(faker.Random.Number(999999));
            record.DateOfRegistration = faker.Date.Recent().ToString("o");


            ///////////////////////////////////////////////////////////////////
            // Decedent
            ///////////////////////////////////////////////////////////////////

            // Basic Decedent information

            record.GivenNames = new string[] { faker.Name.FirstName(gender), faker.Name.FirstName(gender) };
            record.FamilyName = faker.Name.LastName(gender);
            if (gender == Bogus.DataSets.Name.Gender.Female && wasMarried)
            {
                record.MaidenName = faker.Name.LastName(gender);
            }
            record.Gender = gender.ToString().ToLower();
            record.SSN = faker.Person.Ssn().Replace("-", string.Empty);
            DateTime birth = faker.Date.Past(123, DateTime.Today.AddYears(-18));
            DateTime death = faker.Date.Recent();
            DateTimeOffset birthUtc = new DateTimeOffset(birth.Year, birth.Month, birth.Day, birth.Hour, birth.Minute, birth.Second, TimeSpan.Zero);
            DateTimeOffset deathUtc = new DateTimeOffset(death.Year, death.Month, death.Day, death.Hour, death.Minute, death.Second, TimeSpan.Zero);
            record.DateOfBirth = birthUtc.ToString("o");
            record.DateOfDeath = deathUtc.ToString("o");
            int age = death.Year - birth.Year;
            if (birthUtc > deathUtc.AddYears(-age)) age--;
            record.Age = age.ToString();

            // Birthsex

            Dictionary<string, string> birthSex = new Dictionary<string, string>();
            birthSex.Add("code", gender.ToString()[0].ToString());
            birthSex.Add("system", "http://hl7.org/fhir/us/core/ValueSet/us-core-birthsex");
            birthSex.Add("display", gender.ToString());
            record.BirthSex = birthSex;

            // Place of residence

            Dictionary<string, string> residence = new Dictionary<string, string>();
            Tuple<string, string, string, string, string, string> residencePlace = dataHelper.StateCodeToRandomPlace(state);
            residence.Add("residenceLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            residence.Add("residenceCity", residencePlace.Item4);
            residence.Add("residenceCounty", residencePlace.Item2);
            residence.Add("residenceState", stateName);
            residence.Add("residenceCountry", "United States");
            residence.Add("residenceInsideCityLimits", "True");
            record.Residence = residence;

            // Place of birth

            Dictionary<string, string> placeOfBirth = new Dictionary<string, string>();
            Tuple<string, string, string, string, string, string> placeOfBirthPlace = dataHelper.StateCodeToRandomPlace(state);
            placeOfBirth.Add("placeOfBirthLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            placeOfBirth.Add("placeOfBirthCity", placeOfBirthPlace.Item4);
            placeOfBirth.Add("placeOfBirthCounty", placeOfBirthPlace.Item2);
            placeOfBirth.Add("placeOfBirthState", stateName);
            placeOfBirth.Add("placeOfBirthCountry", "United States");
            record.PlaceOfBirth = placeOfBirth;

            // Place of death

            Dictionary<string, string> placeOfDeath = new Dictionary<string, string>();
            Tuple<string, string, string, string, string, string> placeOfDeathPlace = dataHelper.StateCodeToRandomPlace(state);
            Tuple<string, string>[] placeOfDeathTypeCodes =
            {
                Tuple.Create("63238001", "Dead on arrival at hospital"),
                Tuple.Create("16983000", "Death in hospital"),
                Tuple.Create("450391000124102", "Death in hospital-based emergency department or outpatient department"),
                Tuple.Create("450381000124100", "Death in nursing home or long term care facility"),
            };
            Tuple<string, string> placeOfDeathType = faker.Random.ArrayElement<Tuple<string, string>>(placeOfDeathTypeCodes);
            placeOfDeath.Add("placeOfDeathTypeSystem", "http://snomed.info/sct");
            placeOfDeath.Add("placeOfDeathTypeCode", placeOfDeathType.Item1);
            placeOfDeath.Add("placeOfDeathTypeDisplay", placeOfDeathType.Item2);
            placeOfDeath.Add("placeOfDeathFacilityName", $"{placeOfDeathPlace.Item4} Hospital");
            placeOfDeath.Add("placeOfDeathLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            placeOfDeath.Add("placeOfDeathCity", placeOfDeathPlace.Item4);
            placeOfDeath.Add("placeOfDeathCounty", placeOfDeathPlace.Item2);
            placeOfDeath.Add("placeOfDeathState", stateName);
            placeOfDeath.Add("placeOfDeathCountry", "United States");
            record.PlaceOfDeath = placeOfDeath;

            // Marital status

            Dictionary<string, string> maritalStatus = new Dictionary<string, string>();
            Tuple<string, string>[] maritalStatusCodes =
            {
                Tuple.Create("M", "Married"),
                Tuple.Create("D", "Divorced"),
                Tuple.Create("W", "Widowed"),
            };
            Tuple<string, string> maritalStatusCode = faker.Random.ArrayElement<Tuple<string, string>>(maritalStatusCodes);
            if (!wasMarried)
            {
                maritalStatusCode = Tuple.Create("S", "Never Married");
            }
            maritalStatus.Add("code", maritalStatusCode.Item1);
            maritalStatus.Add("system", "http://hl7.org/fhir/v3/MaritalStatus");
            maritalStatus.Add("display", maritalStatusCode.Item2);
            record.MaritalStatus = maritalStatus;

            // Ethnicity

            if (faker.Random.Bool())
            {
                Tuple<string, string> ethnicityDetailed = dataHelper.CDCEthnicityCodes[2 + faker.Random.Number(15)];
                Tuple<string, string>[] ethnicity = { Tuple.Create("Hispanic or Latino", "2135-2"), Tuple.Create(ethnicityDetailed.Item2, ethnicityDetailed.Item1) };
                record.Ethnicity = ethnicity;
            }
            else
            {
                Tuple<string, string>[] ethnicity = { Tuple.Create("Not Hispanic or Latino", "2186-5") };
                record.Ethnicity = ethnicity;
            }

            // Race

            Tuple<string, string>[] ombRaces =
            {
                Tuple.Create("Black or African American", "2054-5"),
                Tuple.Create("Asian", "2028-9"),
                Tuple.Create("American Indian or Alaska Native", "1002-5"),
                Tuple.Create("Native Hawaiian or Other Pacific Islander", "2076-8")
            };
            Tuple<string, string> ombRace = faker.Random.ArrayElement<Tuple<string, string>>(ombRaces);
            Tuple<string, string> cdcRaceW = dataHelper.CDCRaceWCodes[1 + faker.Random.Number(10)];
            Tuple<string, string>[] race = { Tuple.Create("White", "2106-3"), Tuple.Create(cdcRaceW.Item2, cdcRaceW.Item1), ombRace };
            record.Race = race;

            // Education level

            Dictionary<string, string> education = new Dictionary<string, string>();
            Tuple<string, string>[] educationCodes =
            {
                Tuple.Create("PHC1453", "Bachelor's Degree"),
                Tuple.Create("PHC1454", "Master's Degree"),
                Tuple.Create("PHC1448", "8th grade or less"),
                Tuple.Create("PHC1451", "Some college credit, but no degree"),
            };
            Tuple<string, string> educationCode = faker.Random.ArrayElement<Tuple<string, string>>(educationCodes);
            education.Add("code", educationCode.Item1);
            education.Add("system", "http://github.com/nightingaleproject/fhirDeathRecord/sdr/decedent/cs/EducationCS");
            education.Add("display", educationCode.Item2);
            record.Education = education;

            // Occupation; Industry; Military Service

            Dictionary<string, string> occupation = new Dictionary<string, string>();
            string[] industries = { "Manufacturing", "Telecommunications", "Mining", "Information Technology" };
            occupation.Add("jobDescription", faker.Name.JobType());
            occupation.Add("industryDescription", faker.Random.ArrayElement<string>(industries));
            record.Occupation = occupation;
            record.ServedInArmedForces = faker.Random.Bool();

            // Disposition

            Dictionary<string, string> disposition = new Dictionary<string, string>();
            Tuple<string, string>[] dispositionTypeCodes =
            {
                Tuple.Create("449971000124106", "Burial"),
                Tuple.Create("449961000124104", "Cremation"),
                Tuple.Create("449931000124108", "Entombment"),
            };
            Tuple<string, string> dispositionTypeCode = faker.Random.ArrayElement<Tuple<string, string>>(dispositionTypeCodes);
            disposition.Add("dispositionTypeCode", dispositionTypeCode.Item1);
            disposition.Add("dispositionTypeSystem", "http://snomed.info/sct");
            disposition.Add("dispositionTypeDisplay", dispositionTypeCode.Item2);
            Tuple<string, string, string, string, string, string> dispositionPlacePlace = dataHelper.StateCodeToRandomPlace(state);
            disposition.Add("dispositionPlaceName", $"{dispositionPlacePlace.Item4} Cemetery");
            disposition.Add("dispositionPlaceLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            disposition.Add("dispositionPlaceCity", dispositionPlacePlace.Item4);
            disposition.Add("dispositionPlaceCounty", dispositionPlacePlace.Item2);
            disposition.Add("dispositionPlaceState", stateName);
            disposition.Add("dispositionPlaceCountry", "United States");
            Tuple<string, string, string, string, string, string> funeralFacilityPlace = dataHelper.StateCodeToRandomPlace(state);
            disposition.Add("funeralFacilityName", $"{faker.Name.LastName()} Funeral Home");
            disposition.Add("funeralFacilityLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            disposition.Add("funeralFacilityCity", funeralFacilityPlace.Item4);
            disposition.Add("funeralFacilityCounty", funeralFacilityPlace.Item2);
            disposition.Add("funeralFacilityState", stateName);
            disposition.Add("funeralFacilityCountry", "United States");
            record.Disposition = disposition;


            ///////////////////////////////////////////////////////////////////
            // Certifier
            ///////////////////////////////////////////////////////////////////

            // Basic Certifier information

            record.CertifierFamilyName = faker.Name.LastName();
            record.CertifierGivenNames = new string[] { faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female), faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female) };
            record.CertifierSuffix = "MD";

            Dictionary<string, string> certifierAddress = new Dictionary<string, string>();
            Tuple<string, string, string, string, string, string> certifierAddressPlace = dataHelper.StateCodeToRandomPlace(state);
            certifierAddress.Add("certifierAddressLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            certifierAddress.Add("certifierAddressCity", certifierAddressPlace.Item4);
            certifierAddress.Add("certifierAddressCounty", certifierAddressPlace.Item2);
            certifierAddress.Add("certifierAddressState", stateName);
            certifierAddress.Add("certifierAddressCountry", "United States");
            record.CertifierAddress = certifierAddress;

            // Certifier type

            Dictionary<string, string> certifierType = new Dictionary<string, string>();
            certifierType.Add("system", "http://snomed.info/sct");
            if (type == "Injury")
            {
                certifierType.Add("code", "440051000124108");
                certifierType.Add("display", "Medical Examiner");
            }
            else
            {
                certifierType.Add("code", "434651000124107");
                certifierType.Add("display", "Physician (Pronouncer and Certifier)");
            }
            record.CertifierType = certifierType;

            // Certifier qualification

            Dictionary<string, string> qualification = new Dictionary<string, string>();
            qualification.Add("code", "MD");
            qualification.Add("system", "http://hl7.org/fhir/v2/0360/2.7");
            qualification.Add("display", "Doctor of Medicine");
            record.CertifierQualification = qualification;


            ///////////////////////////////////////////////////////////////////
            // Conditions and Observations
            ///////////////////////////////////////////////////////////////////

            if (type == "Natural")
            {
                Dictionary<string, string> mannerOfDeath = new Dictionary<string, string>();
                mannerOfDeath.Add("code", "38605008");
                mannerOfDeath.Add("system", "http://snomed.info/sct");
                mannerOfDeath.Add("display", "Natural");
                record.MannerOfDeath = mannerOfDeath;

                record.ActualOrPresumedDateOfDeath = deathUtc.ToString("o");
                record.DatePronouncedDead = deathUtc.AddHours(1).ToString("o");

                // Randomly pick one of four possible natural causes
                int choice = faker.Random.Number(3);
                if (choice == 0)
                {
                    Tuple<string, string, Dictionary<string, string>>[] causes =
                    {
                        Tuple.Create("Pulmonary embolism", "30 minutes", new Dictionary<string, string>()),
                        Tuple.Create("Deep venuous thrombosis in left thigh", "3 days", new Dictionary<string, string>()),
                        Tuple.Create("Acute hepatic failure", "3 days", new Dictionary<string, string>()),
                        Tuple.Create("Moderately differentiated hepatocellular carcinoma", "over 3 months", new Dictionary<string, string>())
                    };
                    record.CausesOfDeath = causes;

                    record.AutopsyPerformed = false;
                    record.AutopsyResultsAvailable = false;
                    record.MedicalExaminerContacted = false;

                    Dictionary<string, string> tobaccoUseContributedToDeath = new Dictionary<string, string>();
                    tobaccoUseContributedToDeath.Add("code", "UNK");
                    tobaccoUseContributedToDeath.Add("system", "http://hl7.org/fhir/v3/NullFlavor");
                    tobaccoUseContributedToDeath.Add("display", "Unknown");
                    record.TobaccoUseContributedToDeath = tobaccoUseContributedToDeath;
                }
                else if (choice == 1)
                {
                    Tuple<string, string, Dictionary<string, string>>[] causes =
                    {
                        Tuple.Create("Acute myocardial infarction", "2 days", new Dictionary<string, string>()),
                        Tuple.Create("Arteriosclerotic heart disease", "10 years", new Dictionary<string, string>())
                    };
                    record.CausesOfDeath = causes;

                    record.ContributingConditions = "Carcinoma of cecum, Congestive heart failure";

                    record.AutopsyPerformed = false;
                    record.AutopsyResultsAvailable = false;
                    record.MedicalExaminerContacted = false;

                    Dictionary<string, string> tobaccoUseContributedToDeath = new Dictionary<string, string>();
                    tobaccoUseContributedToDeath.Add("code", "373067005");
                    tobaccoUseContributedToDeath.Add("system", "http://snomed.info/sct");
                    tobaccoUseContributedToDeath.Add("display", "No");
                    record.TobaccoUseContributedToDeath = tobaccoUseContributedToDeath;
                }
                else if (choice == 2)
                {
                    Tuple<string, string, Dictionary<string, string>>[] causes =
                    {
                        Tuple.Create("Pulmonary embolism", "1 hour", new Dictionary<string, string>()),
                        Tuple.Create("Acute myocardial infarction", "7 days", new Dictionary<string, string>()),
                        Tuple.Create("Chronic ischemic heart disease", "8 years", new Dictionary<string, string>())
                    };
                    record.CausesOfDeath = causes;

                    record.ContributingConditions = "Non-insulin-dependent diabetes mellitus, Obesity, Hypertension, Congestive heart failure";

                    record.AutopsyPerformed = false;
                    record.AutopsyResultsAvailable = false;
                    record.MedicalExaminerContacted = false;

                    Dictionary<string, string> tobaccoUseContributedToDeath = new Dictionary<string, string>();
                    tobaccoUseContributedToDeath.Add("code", "373066001");
                    tobaccoUseContributedToDeath.Add("system", "http://snomed.info/sct");
                    tobaccoUseContributedToDeath.Add("display", "Yes");
                    record.TobaccoUseContributedToDeath = tobaccoUseContributedToDeath;
                }
                else if (choice == 3)
                {
                    Tuple<string, string, Dictionary<string, string>>[] causes =
                    {
                        Tuple.Create("Rupture of left ventricle", "Minutes", new Dictionary<string, string>()),
                        Tuple.Create("Myocardial infarction", "2 Days", new Dictionary<string, string>()),
                        Tuple.Create("Coronary atherosclerosis", "2 Years", new Dictionary<string, string>())
                    };
                    record.CausesOfDeath = causes;

                    record.ContributingConditions = "Non-insulin-dependent diabetes mellitus, Cigarette smoking, Hypertension, Hypercholesterolemia, Coronary bypass surgery";

                    record.AutopsyPerformed = true;
                    record.AutopsyResultsAvailable = true;
                    record.MedicalExaminerContacted = true;

                    Dictionary<string, string> tobaccoUseContributedToDeath = new Dictionary<string, string>();
                    tobaccoUseContributedToDeath.Add("code", "373066001");
                    tobaccoUseContributedToDeath.Add("system", "http://snomed.info/sct");
                    tobaccoUseContributedToDeath.Add("display", "Yes");
                    record.TobaccoUseContributedToDeath = tobaccoUseContributedToDeath;
                }
            }
            if (type == "Injury")
            {
                // Randomly pick one of three possible injury causes
                int choice = faker.Random.Number(2);
                if (choice == 0)
                {
                    Tuple<string, string, Dictionary<string, string>>[] causes =
                    {
                        Tuple.Create("Carbon monoxide poisoning", "Unkown", new Dictionary<string, string>()),
                        Tuple.Create("Inhalation of automobile exhaust fumes", "Unkown", new Dictionary<string, string>())
                    };
                    record.CausesOfDeath = causes;

                    record.ContributingConditions = "Terminal gastric adenocarcinoma, depression";

                    record.AutopsyPerformed = true;
                    record.AutopsyResultsAvailable = true;
                    record.MedicalExaminerContacted = true;

                    Dictionary<string, string> tobaccoUseContributedToDeath = new Dictionary<string, string>();
                    tobaccoUseContributedToDeath.Add("code", "UNK");
                    tobaccoUseContributedToDeath.Add("system", "http://hl7.org/fhir/v3/NullFlavor");
                    tobaccoUseContributedToDeath.Add("display", "Unknown");
                    record.TobaccoUseContributedToDeath = tobaccoUseContributedToDeath;

                    Dictionary<string, string> mannerOfDeath = new Dictionary<string, string>();
                    mannerOfDeath.Add("code", "44301001");
                    mannerOfDeath.Add("system", "http://snomed.info/sct");
                    mannerOfDeath.Add("display", "Suicide");
                    record.MannerOfDeath = mannerOfDeath;

                    Dictionary<string, string> detailsOfInjury = new Dictionary<string, string>();
                    detailsOfInjury.Add("detailsOfInjuryPlaceDescription", "Own home garage");
                    detailsOfInjury.Add("detailsOfInjuryEffectiveDateTime", new DateTimeOffset(deathUtc.Year, deathUtc.Month, deathUtc.Day, 0, 0, 0, TimeSpan.Zero).ToString("o"));
                    detailsOfInjury.Add("detailsOfInjuryDescription", "Inhaled carbon monoxide from auto exhaust through hose in an enclosed garage");
                    detailsOfInjury.Add("detailsOfInjuryLine1", residence["residenceLine1"]);
                    detailsOfInjury.Add("detailsOfInjuryCity", residencePlace.Item4);
                    detailsOfInjury.Add("detailsOfInjuryCounty", residencePlace.Item2);
                    detailsOfInjury.Add("detailsOfInjuryState", stateName);
                    detailsOfInjury.Add("detailsOfInjuryCountry", "United States");
                    record.DetailsOfInjury = detailsOfInjury;

                    record.ActualOrPresumedDateOfDeath = new DateTimeOffset(deathUtc.Year, deathUtc.Month, deathUtc.Day, 0, 0, 0, TimeSpan.Zero).ToString("o");
                    record.DatePronouncedDead = deathUtc.AddHours(1).ToString("o");

                    record.DeathFromWorkInjury = false;
                }
                else if (choice == 1)
                {
                    Tuple<string, string, Dictionary<string, string>>[] causes =
                    {
                        Tuple.Create("Cardiac tamponade", "15 minutes", new Dictionary<string, string>()),
                        Tuple.Create("Perforation of heart", "20 minutes", new Dictionary<string, string>()),
                        Tuple.Create("Gunshot wound to thorax", "20 minutes", new Dictionary<string, string>())
                    };
                    record.CausesOfDeath = causes;

                    record.AutopsyPerformed = true;
                    record.AutopsyResultsAvailable = true;
                    record.MedicalExaminerContacted = true;

                    Dictionary<string, string> tobaccoUseContributedToDeath = new Dictionary<string, string>();
                    tobaccoUseContributedToDeath.Add("code", "373067005");
                    tobaccoUseContributedToDeath.Add("system", "http://snomed.info/sct");
                    tobaccoUseContributedToDeath.Add("display", "No");
                    record.TobaccoUseContributedToDeath = tobaccoUseContributedToDeath;

                    Dictionary<string, string> mannerOfDeath = new Dictionary<string, string>();
                    mannerOfDeath.Add("code", "27935005");
                    mannerOfDeath.Add("system", "http://snomed.info/sct");
                    mannerOfDeath.Add("display", "Homicide");
                    record.MannerOfDeath = mannerOfDeath;

                    Dictionary<string, string> detailsOfInjury = new Dictionary<string, string>();
                    Tuple<string, string, string, string, string, string> detailsOfInjuryPlace = dataHelper.StateCodeToRandomPlace(state);
                    detailsOfInjury.Add("detailsOfInjuryPlaceDescription", "restaurant");
                    detailsOfInjury.Add("detailsOfInjuryEffectiveDateTime", deathUtc.AddMinutes(-20).ToString("o"));
                    detailsOfInjury.Add("detailsOfInjuryDescription", "Shot by another person using a handgun");
                    detailsOfInjury.Add("detailsOfInjuryLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
                    detailsOfInjury.Add("detailsOfInjuryCity", detailsOfInjuryPlace.Item4);
                    detailsOfInjury.Add("detailsOfInjuryCounty", detailsOfInjuryPlace.Item2);
                    detailsOfInjury.Add("detailsOfInjuryState", stateName);
                    detailsOfInjury.Add("detailsOfInjuryCountry", "United States");
                    record.DetailsOfInjury = detailsOfInjury;

                    record.ActualOrPresumedDateOfDeath = deathUtc.ToString("o");
                    record.DatePronouncedDead = deathUtc.AddHours(1).ToString("o");

                    record.DeathFromWorkInjury = false;
                }
                else if (choice == 2)
                {
                    Tuple<string, string, Dictionary<string, string>>[] causes =
                    {
                        Tuple.Create("Cerebral contusion", "minutes", new Dictionary<string, string>()),
                        Tuple.Create("Fractured skull", "minutes", new Dictionary<string, string>()),
                        Tuple.Create("Blunt impact to head", "minutes", new Dictionary<string, string>())
                    };
                    record.CausesOfDeath = causes;

                    record.AutopsyPerformed = false;
                    record.AutopsyResultsAvailable = false;
                    record.MedicalExaminerContacted = true;

                    Dictionary<string, string> tobaccoUseContributedToDeath = new Dictionary<string, string>();
                    tobaccoUseContributedToDeath.Add("code", "373067005");
                    tobaccoUseContributedToDeath.Add("system", "http://snomed.info/sct");
                    tobaccoUseContributedToDeath.Add("display", "No");
                    record.TobaccoUseContributedToDeath = tobaccoUseContributedToDeath;

                    Dictionary<string, string> mannerOfDeath = new Dictionary<string, string>();
                    mannerOfDeath.Add("code", "7878000");
                    mannerOfDeath.Add("system", "http://snomed.info/sct");
                    mannerOfDeath.Add("display", "Accident");
                    record.MannerOfDeath = mannerOfDeath;

                    Dictionary<string, string> detailsOfInjury = new Dictionary<string, string>();
                    Tuple<string, string, string, string, string, string> detailsOfInjuryPlace = dataHelper.StateCodeToRandomPlace(state);
                    detailsOfInjury.Add("detailsOfInjuryPlaceDescription", "Highway");
                    detailsOfInjury.Add("detailsOfInjuryEffectiveDateTime", deathUtc.ToString("o"));
                    detailsOfInjury.Add("detailsOfInjuryDescription", "Automobile accident. Car slid off wet road and struck tree.");
                    detailsOfInjury.Add("detailsOfInjuryCity", detailsOfInjuryPlace.Item4);
                    detailsOfInjury.Add("detailsOfInjuryCounty", detailsOfInjuryPlace.Item2);
                    detailsOfInjury.Add("detailsOfInjuryState", stateName);
                    detailsOfInjury.Add("detailsOfInjuryCountry", "United States");
                    record.DetailsOfInjury = detailsOfInjury;

                    record.DeathFromWorkInjury = false;

                    record.ActualOrPresumedDateOfDeath = deathUtc.ToString("o");
                    record.DatePronouncedDead = deathUtc.AddHours(2).ToString("o");

                    Dictionary<string, string> deathFromTransportInjury = new Dictionary<string, string>();
                    deathFromTransportInjury.Add("code", "236320001");
                    deathFromTransportInjury.Add("system", "http://github.com/nightingaleproject/fhirDeathRecord/sdr/causeOfDeath/vs/TransportRelationshipsVS");
                    deathFromTransportInjury.Add("display", "Vehicle driver");
                    record.DeathFromTransportInjury = deathFromTransportInjury;
                }
            }

            if (gender == Bogus.DataSets.Name.Gender.Female)
            {
                Dictionary<string, string> timingOfRecentPregnancyInRelationToDeath = new Dictionary<string, string>();
                timingOfRecentPregnancyInRelationToDeath.Add("code", "PHC1260");
                timingOfRecentPregnancyInRelationToDeath.Add("system", "http://github.com/nightingaleproject/fhirDeathRecord/sdr/causeOfDeath/vs/PregnancyStatusVS");
                timingOfRecentPregnancyInRelationToDeath.Add("display", "Not pregnant within past year");
                record.TimingOfRecentPregnancyInRelationToDeath = timingOfRecentPregnancyInRelationToDeath;
            }
        }
    }

}

using System;
using System.Collections.Generic;
using Bogus.Extensions.UnitedStates;
using System.Linq;
using VRDR;

namespace canary.Models
{
    /// <summary>Class <c>Faker</c> can be used to generate synthetic <c>DeathRecord</c>s. Various
    /// options are available to tailoring the records generated to specific use case by the class.
    /// </summary>
    public class DeathRecordFaker
    {
        /// <summary>Mortality data for code translations.</summary>
        private MortalityData MortalityData = MortalityData.Instance;

        /// <summary>State to use when generating records.</summary>
        private string state;

        /// <summary>Type of Cause of Death; Natural or Injury.</summary>
        private string type;

        /// <summary>Decedent Sex to use.</summary>
        private string sex;

        /// <summary>Constructor with optional arguments to customize the records generated.</summary>
        public DeathRecordFaker(string state = "MA", string type = "Natural", string sex = "Male")
        {
            this.state = state;
            this.type = type;
            this.sex = sex;
        }

        /// <summary>Return a new record populated with fake data.</summary>
        public DeathRecord Generate(bool simple = false)
        {
            DeathRecord record = new DeathRecord();

            Random random = new Random();
            Bogus.Faker faker = new Bogus.Faker("en");
            MortalityData dataHelper = MortalityData;

            // Grab Gender enum value
            Bogus.DataSets.Name.Gender gender = sex == "Male" ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female;

            // Was married?
            bool wasMarried = faker.Random.Bool();

            record.Identifier = Convert.ToString(faker.Random.Number(999999));
            // record.BundleIdentifier = Convert.ToString(faker.Random.Number(999999));
            DateTime date = faker.Date.Recent();
            record.CertifiedTime = date.ToString("s");
            record.RegisteredTime = new DateTimeOffset(date.AddDays(-1).Year, date.AddDays(-1).Month, date.AddDays(-1).Day, 0, 0, 0, TimeSpan.Zero).ToString("s");
            record.StateLocalIdentifier1 = Convert.ToString(faker.Random.Number(999999));

            // Basic Decedent information

            record.GivenNames = new string[] { faker.Name.FirstName(gender), faker.Name.FirstName(gender) };
            record.FamilyName = faker.Name.LastName(gender);
            record.Suffix = faker.Name.Suffix();
            if (gender == Bogus.DataSets.Name.Gender.Female && wasMarried)
            {
                record.MaidenName = faker.Name.LastName(gender);
            }

            record.FatherFamilyName = record.FamilyName;
            record.FatherGivenNames = new string[] { faker.Name.FirstName(Bogus.DataSets.Name.Gender.Male), faker.Name.FirstName(Bogus.DataSets.Name.Gender.Male) };
            record.FatherSuffix = faker.Name.Suffix();

            record.MotherGivenNames = new string[] { faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female), faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female) };
            record.MotherMaidenName = faker.Name.LastName(Bogus.DataSets.Name.Gender.Female);
            record.MotherSuffix = faker.Name.Suffix();

            record.SpouseGivenNames = new string[] { faker.Name.FirstName(), faker.Name.FirstName() };
            record.SpouseFamilyName = record.FamilyName;
            record.SpouseSuffix = faker.Name.Suffix();

            record.BirthRecordId = Convert.ToString(faker.Random.Number(999999));


            record.Gender = gender.ToString().ToLower();
            record.SSN = faker.Person.Ssn().Replace("-", string.Empty);
            DateTime birth = faker.Date.Past(123, DateTime.Today.AddYears(-18));
            DateTime death = faker.Date.Recent();
            DateTimeOffset birthUtc = new DateTimeOffset(birth.Year, birth.Month, birth.Day, 0, 0, 0, TimeSpan.Zero);
            DateTimeOffset deathUtc = new DateTimeOffset(death.Year, death.Month, death.Day, 0, 0, 0, TimeSpan.Zero);
            record.DateOfBirth = birthUtc.ToString("yyyy-MM-dd");
            record.DateOfDeath = deathUtc.ToString("yyyy-MM-dd");
            int age = death.Year - birth.Year;
            if (birthUtc > deathUtc.AddYears(-age)) age--;
            record.AgeAtDeath = new Dictionary<string, string>() { { "value", age.ToString() }, { "unit", "a" } };

            // Birthsex

            record.BirthSex = gender.ToString()[0].ToString();

            // Place of residence

            Dictionary<string, string> residence = new Dictionary<string, string>();
            PlaceCode residencePlace = dataHelper.StateCodeToRandomPlace(state);
            residence.Add("addressLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            residence.Add("addressCity", residencePlace.City);
            residence.Add("addressCounty", residencePlace.County);
            residence.Add("addressState", state);
            residence.Add("addressCountry", "US");
            record.Residence = residence;

            // Residence Within City Limits
            record.ResidenceWithinCityLimitsBoolean = true;

            // Place of birth

            Dictionary<string, string> placeOfBirth = new Dictionary<string, string>();
            PlaceCode placeOfBirthPlace = dataHelper.StateCodeToRandomPlace(state);
            placeOfBirth.Add("addressCity", placeOfBirthPlace.City);
            placeOfBirth.Add("addressCounty", placeOfBirthPlace.County);
            placeOfBirth.Add("addressState", state);
            placeOfBirth.Add("addressCountry", "US");
            record.PlaceOfBirth = placeOfBirth;
            record.BirthRecordState = state;
            // Place of death

            PlaceCode placeOfDeathPlace = dataHelper.StateCodeToRandomPlace(state);
            record.DeathLocationName = placeOfDeathPlace.City + " Hospital";

            Dictionary<string, string> deathLocationType = new Dictionary<string, string>();
            deathLocationType.Add("code", "450391000124102");
            deathLocationType.Add("system", VRDR.CodeSystems.SCT);
            deathLocationType.Add("display", "Death in emergency Room/Outpatient");
            record.DeathLocationType = deathLocationType;

            Dictionary<string, string> placeOfDeath = new Dictionary<string, string>();
            placeOfDeath.Add("addressLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            placeOfDeath.Add("addressCity", placeOfDeathPlace.City);
            placeOfDeath.Add("addressCounty", placeOfDeathPlace.County);
            placeOfDeath.Add("addressState", state);
            placeOfDeath.Add("addressCountry", "US");
            record.DeathLocationAddress = placeOfDeath;
            record.DeathLocationJurisdiction = state;

            record.DeathLocationJurisdiction = state;

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
            maritalStatus.Add("system", VRDR.CodeSystems.PH_MaritalStatus_HL7_2x);
            maritalStatus.Add("display", maritalStatusCode.Item2);
            record.MaritalStatus = maritalStatus;

            // Ethnicity
            if (faker.Random.Bool() && !simple)
            {
                record.Ethnicity1Helper = "N";
                record.Ethnicity2Helper = "N";
                record.Ethnicity3Helper = "N";
                record.Ethnicity4Helper = "Y";
                record.EthnicityLiteral = "";
            }
            else
            {
                record.Ethnicity1Helper = "N";
                record.Ethnicity2Helper = "N";
                record.Ethnicity3Helper = "N";
                record.Ethnicity4Helper = "N";
            }

            // Race
            Tuple<string, string>[] nvssRaces =
            {
                Tuple.Create(NvssRace.AmericanIndianOrAlaskaNative, "Y"),
                Tuple.Create(NvssRace.AsianIndian, "Y"),
                Tuple.Create(NvssRace.BlackOrAfricanAmerican, "Y"),
                Tuple.Create(NvssRace.Chinese, "Y"),
                Tuple.Create(NvssRace.Filipino, "Y"),
                Tuple.Create(NvssRace.GuamanianOrChamorro, "Y"),
                Tuple.Create(NvssRace.Japanese, "Y"),
                Tuple.Create(NvssRace.Korean, "Y"),
                Tuple.Create(NvssRace.NativeHawaiian, "Y"),
                Tuple.Create(NvssRace.OtherAsian, "Y"),
                Tuple.Create(NvssRace.OtherPacificIslander, "Y"),
                Tuple.Create(NvssRace.OtherRace, "Y"),
                Tuple.Create(NvssRace.Samoan, "Y"),
                Tuple.Create(NvssRace.Vietnamese, "Y"),
                Tuple.Create(NvssRace.White, "Y"),
            };
            if (!simple)
            {
                Tuple<string, string> race1 = faker.Random.ArrayElement<Tuple<string, string>>(nvssRaces);
                Tuple<string, string> race2 = faker.Random.ArrayElement<Tuple<string, string>>(nvssRaces);
                Tuple<string, string> race3 = faker.Random.ArrayElement<Tuple<string, string>>(nvssRaces);
                Tuple<string, string>[] race = { race1, race2, race3 };
                record.Race = race;
            }
            else
            {
                Tuple<string, string> race1 = faker.Random.ArrayElement<Tuple<string, string>>(nvssRaces);
                record.Race = new Tuple<string, string>[] { race1 };
            }

            // Education level

            Dictionary<string, string> education = new Dictionary<string, string>();
            Tuple<string, string>[] educationCodes =
            {
                Tuple.Create("PHC1453", "College or baccalaureate degree complete"),
                Tuple.Create("PHC1454", "Graduate or professional Degree complete"),
                Tuple.Create("PHC1449", "Some secondary or high school education"),
                Tuple.Create("PHC1451", "Some College education"),
            };
            Tuple<string, string> educationCode = faker.Random.ArrayElement<Tuple<string, string>>(educationCodes);
            education.Add("code", educationCode.Item1);
            education.Add("system", VRDR.CodeSystems.PH_PHINVS_CDC);
            education.Add("display", educationCode.Item2);
            record.EducationLevel = education;

            Tuple<string, string>[] occupationIndustries =
            {
                Tuple.Create("secretary", "State agency"),
                Tuple.Create("carpenter", "construction"),
                Tuple.Create("Programmer", "Health Insurance"),
            };
            Tuple<string, string> occInd = faker.Random.ArrayElement<Tuple<string, string>>(occupationIndustries);

            // Occupation

            record.UsualOccupation = occInd.Item1;
            DateTime usualOccupationEnd = faker.Date.Past(18, deathUtc.DateTime.AddYears(0));
            record.UsualOccupationEnd = usualOccupationEnd.ToString("yyyy-MM-dd");

            // Industry

            record.UsualIndustry = occInd.Item2;

            // Military Service

            Dictionary<string, string> military = new Dictionary<string, string>();
            Tuple<string, string>[] militaryCodes =
            {
                Tuple.Create("Y", "Yes"),
                Tuple.Create("N", "No"),
            };
            Tuple<string, string> militaryCode = faker.Random.ArrayElement<Tuple<string, string>>(militaryCodes);
            military.Add("code", militaryCode.Item1);
            military.Add("system", VRDR.CodeSystems.PH_YesNo_HL7_2x);
            military.Add("display", militaryCode.Item2);
            record.MilitaryService = military;

            // Funeral Home Name

            record.FuneralHomeName = faker.Name.LastName() + " Funeral Home";

            // Funeral Home Address

            Dictionary<string, string> fha = new Dictionary<string, string>();
            PlaceCode fhaPlace = dataHelper.StateCodeToRandomPlace(state);
            fha.Add("addressLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            fha.Add("addressCity", fhaPlace.City);
            fha.Add("addressCounty", fhaPlace.County);
            fha.Add("addressState", state);
            fha.Add("addressCountry", "US");
            record.FuneralHomeAddress = fha;

            // Disposition Location Name

            record.DispositionLocationName = faker.Name.LastName() + " Cemetery";

            // Disposition Location Address

            Dictionary<string, string> dispLoc = new Dictionary<string, string>();
            PlaceCode dispLocPlace = dataHelper.StateCodeToRandomPlace(state);
            dispLoc.Add("addressCity", dispLocPlace.City);
            dispLoc.Add("addressCounty", dispLocPlace.County);
            dispLoc.Add("addressState", state);
            dispLoc.Add("addressCountry", "US");
            record.DispositionLocationAddress = dispLoc;

            // Disposition Method

            Dictionary<string, string> disposition = new Dictionary<string, string>();
            Tuple<string, string>[] dispositionTypeCodes =
            {
                Tuple.Create("449971000124106", "Patient status determination, deceased and buried"),
                Tuple.Create("449961000124104", "Patient status determination, deceased and cremated"),
                Tuple.Create("449931000124108", "Patient status determination, deceased and entombed"),
            };
            Tuple<string, string> dispositionTypeCode = faker.Random.ArrayElement<Tuple<string, string>>(dispositionTypeCodes);
            disposition.Add("code", dispositionTypeCode.Item1);
            disposition.Add("system", VRDR.CodeSystems.SCT);
            disposition.Add("display", dispositionTypeCode.Item2);
            record.DecedentDispositionMethod = disposition;

            // Basic Certifier information

            Dictionary<string, string> certifierIdentifier = new Dictionary<string, string>();
            certifierIdentifier.Add("system", "http://hl7.org/fhir/sid/us-npi");
            certifierIdentifier.Add("value", Convert.ToString(faker.Random.Number(999999)));
            record.CertifierIdentifier = certifierIdentifier;
            
            record.CertifierFamilyName = faker.Name.LastName();
            record.CertifierGivenNames = new string[] { faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female), faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female) };
            record.CertifierSuffix = "MD";

            Dictionary<string, string> certifierAddress = new Dictionary<string, string>();
            PlaceCode certifierAddressPlace = dataHelper.StateCodeToRandomPlace(state);
            certifierAddress.Add("addressLine1", $"{faker.Random.Number(999) + 1} {faker.Address.StreetName()}");
            certifierAddress.Add("addressCity", certifierAddressPlace.City);
            certifierAddress.Add("addressCounty", certifierAddressPlace.County);
            certifierAddress.Add("addressState", state);
            certifierAddress.Add("addressCountry", "US");
            record.CertifierAddress = certifierAddress;

            // Certifier type
            Dictionary<string, string> certificationType = new Dictionary<string, string>();
            certificationType.Add("system", VRDR.CodeSystems.SCT);
            certificationType.Add("code", "434641000124105");
            certificationType.Add("display", "Death certification and verification by physician");
            record.CertificationRole = certificationType;

            // TODO Add these fields
            Dictionary<string, string> relationship = new Dictionary<string, string>();
            relationship["text"] = "Spouse";
            record.ContactRelationship = relationship;

            if (type == "Natural")
            {
                Dictionary<string, string> mannerOfDeath = new Dictionary<string, string>();
                mannerOfDeath.Add("code", "38605008");
                mannerOfDeath.Add("system", VRDR.CodeSystems.SCT);
                mannerOfDeath.Add("display", "Natural death");
                record.MannerOfDeathType = mannerOfDeath;

                record.DateOfDeath = deathUtc.ToString("o");
                record.DateOfDeathPronouncement = deathUtc.AddHours(1).ToString("o");

                // Randomly pick one of four possible natural causes
                int choice = faker.Random.Number(3);
                if (choice == 0)
                {
                    Tuple<string, string>[] causes =
                    {
                        Tuple.Create("Pulmonary embolism", "30 minutes"),
                        Tuple.Create("Deep venuous thrombosis in left thigh", "3 days"),
                        Tuple.Create("Acute hepatic failure", "3 days"),
                        Tuple.Create("Moderately differentiated hepatocellular carcinoma", "over 3 months")
                    };
                    record.CausesOfDeath = causes;

                    record.AutopsyPerformedIndicator = new Dictionary<string, string>() { { "code", "N" }, { "system", VRDR.CodeSystems.PH_YesNo_HL7_2x }, { "display", "No" } };
                    record.AutopsyResultsAvailable = new Dictionary<string, string>() { { "code", "N" }, { "system", VRDR.CodeSystems.PH_YesNo_HL7_2x }, { "display", "No" } }; ;
                    record.ExaminerContactedHelper = ValueSets.YesNoUnknown.No;

                    record.TobaccoUse = new Dictionary<string, string>() { { "code", "373067005" }, { "system", VRDR.CodeSystems.SCT }, { "display", "No" } };
                }
                else if (choice == 1)
                {
                    Tuple<string, string>[] causes =
                    {
                        Tuple.Create("Acute myocardial infarction", "2 days"),
                        Tuple.Create("Arteriosclerotic heart disease", "10 years")
                    };
                    record.CausesOfDeath = causes;

                    record.ContributingConditions = "Carcinoma of cecum, Congestive heart failure";

                    record.AutopsyPerformedIndicator = new Dictionary<string, string>() { { "code", "N" }, { "system", VRDR.CodeSystems.PH_YesNo_HL7_2x }, { "display", "No" } };
                    record.AutopsyResultsAvailable = new Dictionary<string, string>() { { "code", "N" }, { "system", VRDR.CodeSystems.PH_YesNo_HL7_2x }, { "display", "No" } }; ;
                    record.ExaminerContactedHelper = ValueSets.YesNoUnknown.No;

                    record.TobaccoUse = new Dictionary<string, string>() { { "code", "373067005" }, { "system", VRDR.CodeSystems.SCT }, { "display", "No" } };
                }
                else if (choice == 2)
                {
                    Tuple<string, string>[] causes =
                    {
                        Tuple.Create("Pulmonary embolism", "1 hour"),
                        Tuple.Create("Acute myocardial infarction", "7 days"),
                        Tuple.Create("Chronic ischemic heart disease", "8 years")
                    };
                    record.CausesOfDeath = causes;

                    record.ContributingConditions = "Non-insulin-dependent diabetes mellitus, Obesity, Hypertension, Congestive heart failure";

                    record.AutopsyPerformedIndicator = new Dictionary<string, string>() { { "code", "N" }, { "system", VRDR.CodeSystems.PH_YesNo_HL7_2x }, { "display", "No" } };
                    record.AutopsyResultsAvailable = new Dictionary<string, string>() { { "code", "N" }, { "system", VRDR.CodeSystems.PH_YesNo_HL7_2x }, { "display", "No" } }; ;
                    record.ExaminerContactedHelper = ValueSets.YesNoUnknown.No;

                    record.TobaccoUse = new Dictionary<string, string>() { { "code", "373066001" }, { "system", VRDR.CodeSystems.SCT }, { "display", "Yes" } };
                }
                else if (choice == 3)
                {
                    Tuple<string, string>[] causes =
                    {
                        Tuple.Create("Rupture of left ventricle", "Minutes"),
                        Tuple.Create("Myocardial infarction", "2 Days"),
                        Tuple.Create("Coronary atherosclerosis", "2 Years")
                    };
                    record.CausesOfDeath = causes;

                    record.ContributingConditions = "Non-insulin-dependent diabetes mellitus, Cigarette smoking, Hypertension, Hypercholesterolemia, Coronary bypass surgery";

                    record.AutopsyPerformedIndicator = new Dictionary<string, string>() { { "code", "Y" }, { "system", VRDR.CodeSystems.PH_YesNo_HL7_2x }, { "display", "Yes" } };
                    record.AutopsyResultsAvailable = new Dictionary<string, string>() { { "code", "Y" }, { "system", VRDR.CodeSystems.PH_YesNo_HL7_2x }, { "display", "Yes" } }; ;
                    record.ExaminerContactedHelper = ValueSets.YesNoUnknown.Yes;

                    record.TobaccoUse = new Dictionary<string, string>() { { "code", "373066001" }, { "system", VRDR.CodeSystems.SCT }, { "display", "Yes" } };
                }
            }
            if (type == "Injury")
            {
                // Randomly pick one of three possible injury causes
                int choice = faker.Random.Number(2);
                if (choice == 0)
                {
                    Tuple<string, string>[] causes =
                    {
                        Tuple.Create("Carbon monoxide poisoning", "Unkown"),
                        Tuple.Create("Inhalation of automobile exhaust fumes", "Unkown")
                    };
                    record.CausesOfDeath = causes;

                    record.ContributingConditions = "Terminal gastric adenocarcinoma, depression";

                    record.AutopsyPerformedIndicator = new Dictionary<string, string>() { { "code", "Y" }, { "system", VRDR.CodeSystems.PH_YesNo_HL7_2x }, { "display", "Yes" } };
                    record.AutopsyResultsAvailable = new Dictionary<string, string>() { { "code", "Y" }, { "system", VRDR.CodeSystems.PH_YesNo_HL7_2x }, { "display", "Yes" } }; ;
                    record.ExaminerContactedHelper = ValueSets.YesNoUnknown.Yes;

                    record.TobaccoUse = new Dictionary<string, string>() { { "code", "UNK" }, { "system", VRDR.CodeSystems.PH_NullFlavor_HL7_V3 }, { "display", "unknown" } };

                    Dictionary<string, string> mannerOfDeath = new Dictionary<string, string>();
                    mannerOfDeath.Add("code", "44301001");
                    mannerOfDeath.Add("system", VRDR.CodeSystems.SCT);
                    mannerOfDeath.Add("display", "Suicide");
                    record.MannerOfDeathType = mannerOfDeath;

                    Dictionary<string, string> detailsOfInjury = new Dictionary<string, string>();
                    record.InjuryLocationName = "Own home garage";
                    record.InjuryDate = new DateTimeOffset(deathUtc.Year, deathUtc.Month, deathUtc.Day, 0, 0, 0, TimeSpan.Zero).ToString("s");
                    record.InjuryLocationDescription = "Inhaled carbon monoxide from auto exhaust through hose in an enclosed garage";

                    Dictionary<string, string> detailsOfInjuryAddr = new Dictionary<string, string>();
                    detailsOfInjuryAddr.Add("addressLine1", residence["addressLine1"]);
                    detailsOfInjuryAddr.Add("addressCity", residencePlace.City);
                    detailsOfInjuryAddr.Add("addressCounty", residencePlace.County);
                    detailsOfInjuryAddr.Add("addressState", state);
                    detailsOfInjuryAddr.Add("addressCountry", "US");
                    record.InjuryLocationAddress = detailsOfInjuryAddr;

                    record.InjuryPlaceDescription = "Home";

                    record.DateOfDeath = new DateTimeOffset(deathUtc.Year, deathUtc.Month, deathUtc.Day, 0, 0, 0, TimeSpan.Zero).ToString("s");
                }
                else if (choice == 1)
                {
                    Tuple<string, string>[] causes =
                    {
                        Tuple.Create("Cardiac tamponade", "15 minutes"),
                        Tuple.Create("Perforation of heart", "20 minutes"),
                        Tuple.Create("Gunshot wound to thorax", "20 minutes")
                    };
                    record.CausesOfDeath = causes;

                    record.AutopsyPerformedIndicator = new Dictionary<string, string>() { { "code", "Y" }, { "system", VRDR.CodeSystems.PH_YesNo_HL7_2x }, { "display", "Yes" } };
                    record.AutopsyResultsAvailable = new Dictionary<string, string>() { { "code", "Y" }, { "system", VRDR.CodeSystems.PH_YesNo_HL7_2x }, { "display", "Yes" } };
                    record.ExaminerContactedHelper = ValueSets.YesNoUnknown.No;

                    record.TobaccoUse = new Dictionary<string, string>() { { "code", "373067005" }, { "system", VRDR.CodeSystems.SCT }, { "display", "No" } };

                    Dictionary<string, string> mannerOfDeath = new Dictionary<string, string>();
                    mannerOfDeath.Add("code", "27935005");
                    mannerOfDeath.Add("system", VRDR.CodeSystems.SCT);
                    mannerOfDeath.Add("display", "Homicide");
                    record.MannerOfDeathType = mannerOfDeath;

                    Dictionary<string, string> detailsOfInjury = new Dictionary<string, string>();
                    record.InjuryLocationName = "restaurant";
                    record.InjuryDate = deathUtc.AddMinutes(-20).ToString("s");
                    record.InjuryLocationDescription = "Shot by another person using a handgun";

                    Dictionary<string, string> detailsOfInjuryAddr = new Dictionary<string, string>();
                    PlaceCode detailsOfInjuryPlace = dataHelper.StateCodeToRandomPlace(state);
                    detailsOfInjuryAddr.Add("addressLine1", residence["addressLine1"]);
                    detailsOfInjuryAddr.Add("addressCity", detailsOfInjuryPlace.City);
                    detailsOfInjuryAddr.Add("addressCounty", detailsOfInjuryPlace.County);
                    detailsOfInjuryAddr.Add("addressState", state);
                    detailsOfInjuryAddr.Add("addressCountry", "US");
                    record.InjuryLocationAddress = detailsOfInjuryAddr;
                    
                    record.InjuryPlaceDescription = "Trade and Service Area";
                }
                else if (choice == 2)
                {
                    Tuple<string, string >[] causes =
                    {
                        Tuple.Create("Cerebral contusion", "minutes"),
                        Tuple.Create("Fractured skull", "minutes"),
                        Tuple.Create("Blunt impact to head", "minutes")
                    };
                    record.CausesOfDeath = causes;

                    record.AutopsyPerformedIndicator = new Dictionary<string, string>() { { "code", "N" }, { "system", VRDR.CodeSystems.PH_YesNo_HL7_2x }, { "display", "No" } };
                    record.AutopsyResultsAvailable = new Dictionary<string, string>() { { "code", "N" }, { "system", VRDR.CodeSystems.PH_YesNo_HL7_2x }, { "display", "No" } };
                    record.ExaminerContactedHelper = ValueSets.YesNoUnknown.Yes;

                    record.TobaccoUse = new Dictionary<string, string>() { { "code", "373067005" }, { "system", VRDR.CodeSystems.SCT }, { "display", "No" } };

                    Dictionary<string, string> mannerOfDeath = new Dictionary<string, string>();
                    mannerOfDeath.Add("code", "7878000");
                    mannerOfDeath.Add("system", VRDR.CodeSystems.SCT);
                    mannerOfDeath.Add("display", "Accidental death");
                    record.MannerOfDeathType = mannerOfDeath;

                    Dictionary<string, string> detailsOfInjury = new Dictionary<string, string>();
                    record.InjuryLocationName = "Highway";
                    record.InjuryDate = deathUtc.ToString("s");
                    record.InjuryLocationDescription = "Automobile accident. Car slid off wet road and struck tree.";

                    Dictionary<string, string> detailsOfInjuryAddr = new Dictionary<string, string>();
                    PlaceCode detailsOfInjuryPlace = dataHelper.StateCodeToRandomPlace(state);
                    detailsOfInjuryAddr.Add("addressLine1", residence["addressLine1"]);
                    detailsOfInjuryAddr.Add("addressCity", detailsOfInjuryPlace.City);
                    detailsOfInjuryAddr.Add("addressCounty", detailsOfInjuryPlace.County);
                    detailsOfInjuryAddr.Add("addressState", state);
                    detailsOfInjuryAddr.Add("addressCountry", "US");
                    record.InjuryLocationAddress = detailsOfInjuryAddr;

                    record.InjuryPlaceDescription = "Street/Highway";

                    // TransportationRole
                    record.TransportationRoleHelper = VRDR.ValueSets.TransportationIncidentRole.Passenger;
                }
            }

            if (gender == Bogus.DataSets.Name.Gender.Female)
            {
                Dictionary<string, string> pregnancyStatus = new Dictionary<string, string>();
                pregnancyStatus.Add("code", "PHC1260");
                pregnancyStatus.Add("system", VRDR.CodeSystems.PH_PHINVS_CDC);
                pregnancyStatus.Add("display", "Not pregnant within the past year");
                record.PregnancyStatus = pregnancyStatus;
            }
            else
            {
                Dictionary<string, string> pregnanacyStatus = new Dictionary<string, string>();
                pregnanacyStatus.Add("code", "NA");
                pregnanacyStatus.Add("system", VRDR.CodeSystems.PH_NullFlavor_HL7_V3);
                pregnanacyStatus.Add("display", "not applicable");
                record.PregnancyStatus = pregnanacyStatus;
            }

            return record;
        }
    }
}

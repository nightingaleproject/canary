using VRDR;
using System;
using System.Collections.Generic;

namespace canary.Models
{
    /// <summary>vrdr-dotnet versions of connectathon records.</summary>
    public class Connectathon
    {
        public Connectathon() {}

        public static DeathRecord FromId(int id)
        {
            switch (id)
            {
                case 1:
                    return JanetPage();
                case 2:
                    return MadelynPatel();
                case 3:
                    return VivienneLeeWright();
                case 4:
                    return JavierLuisPerezFull();
                case 5:
                    return JavierLuisPerezPartial();
            }
            return null;
        }

        public static DeathRecord JanetPage()
        {
            DeathRecord record = new DeathRecord();

            record.Identifier = "111111";

            record.RegisteredTime = "2019-09-02";

            record.GivenNames = new string[] { "Janet" };

            record.FamilyName = "Page";

            record.Race = new Tuple<string, string>[] { Tuple.Create("White", "2106-3") };

            record.Ethnicity = new Tuple<string, string>[] { Tuple.Create("Puerto Rican", "2180-8") };

            record.BirthSex = "F";

            record.SSN = "555111234";

            Dictionary<string, string> age = new Dictionary<string, string>();
            age.Add("value", "70");
            age.Add("unit", "a");
            record.AgeAtDeath = age;

            record.DateOfBirth = "1949-01-15";

            Dictionary<string, string> addressB = new Dictionary<string, string>();
            addressB.Add("addressCity", "Atlanta");
            addressB.Add("addressState", "Georgia");
            addressB.Add("addressCountry", "United States");
            record.PlaceOfBirth = addressB;

            record.BirthRecordId = "515151";

            record.MotherGivenNames = new String[] { "Linda" };
            record.MotherMaidenName = "Shay";

            record.FatherFamilyName = "Page";

            Dictionary<string, string> code = new Dictionary<string, string>();
            code.Add("code", "D");
            code.Add("system", "http://terminology.hl7.org/CodeSystem/v3-MaritalStatus");
            code.Add("display", "Divorced");
            record.MaritalStatus = code;

            Dictionary<string, string> addressR = new Dictionary<string, string>();
            addressR.Add("addressCity", "Atlanta");
            addressR.Add("addressCounty", "Fulton");
            addressR.Add("addressState", "Georgia");
            addressR.Add("addressCountry", "United States");
            record.Residence = addressR;
            record.ResidenceWithinCityLimits = true;

            Dictionary<string, string> elevel = new Dictionary<string, string>();
            elevel.Add("code", "POSTG");
            elevel.Add("system", "http://terminology.hl7.org/CodeSystem/v3-EducationLevel");
            elevel.Add("display", "Doctoral or post graduate education");
            record.EducationLevel = elevel;

            record.UsualOccupation = "Programmer";

            record.UsualIndustry = "Health Insurance";

            record.DateOfDeath = "2019-09-01T05:30:00";

            record.DeathLocationName = "Pecan Grove Nursing Home";

            record.DeathLocationDescription = "nursing home";

            Dictionary<string, string> role = new Dictionary<string, string>();
            role.Add("code", "434641000124105");
            role.Add("system", "http://snomed.info/sct");
            role.Add("display", "Death certification and verification by physician");
            record.CertificationRole = role;

            record.CertifierGivenNames = new string[] { "Sam" };
            record.CertifierFamilyName = "Jones";

            Dictionary<string, string> address = new Dictionary<string, string>();
            address.Add("addressLine1", "1 Main Street");
            address.Add("addressCity", "Atlanta");
            address.Add("addressState", "Georgia");
            address.Add("addressZip", "30303");
            address.Add("addressCountry", "United States");
            record.CertifierAddress = address;

            record.CertifiedTime = "2019-09-02";

            record.DateOfDeathPronouncement = "2019-09-01T05:30:00";

            record.COD1A = "Congestive heart failure";
            record.INTERVAL1A = "1 hour";

            record.COD1B = "breast cancer";
            record.INTERVAL1B = "20 years";

            record.ExaminerContacted = false;

            Dictionary<string, string> manner = new Dictionary<string, string>();
            manner.Add("code", "38605008");
            manner.Add("system", "http://snomed.info/sct");
            manner.Add("display", "Natural death");
            record.MannerOfDeathType = manner;

            Dictionary<string, string> codeA = new Dictionary<string, string>();
            codeA.Add("code", "N");
            codeA.Add("system", "http://terminology.hl7.org/CodeSystem/v2-0136");
            codeA.Add("display", "No");
            record.AutopsyPerformedIndicator = codeA;

            Dictionary<string, string> dmethod = new Dictionary<string, string>();
            dmethod.Add("code", "449961000124104");
            dmethod.Add("system", "http://snomed.info/sct");
            dmethod.Add("display", "Patient status determination, deceased and cremated");
            record.DecedentDispositionMethod = dmethod;

            Dictionary<string, string> pregnanacyStatus = new Dictionary<string, string>();
            pregnanacyStatus.Add("code", "NA");
            pregnanacyStatus.Add("system", "http://hl7.org/fhir/v3/NullFlavor");
            pregnanacyStatus.Add("display", "not applicable");
            record.PregnancyStatus = pregnanacyStatus;

            record.TobaccoUse = new Dictionary<string, string>() { { "code", "UNK" }, { "system", "http://hl7.org/fhir/v3/NullFlavor" }, { "display", "unknown" } };

            return record;
        }

        public static DeathRecord MadelynPatel()
        {
            DeathRecord record = new DeathRecord();

            record.Identifier = "222222";

            record.RegisteredTime = "2019-11-06";

            record.GivenNames = new string[] { "Madelyn" };

            record.FamilyName = "Patel";

            record.Race = new Tuple<string, string>[] { Tuple.Create("Asian Indian", "2029-7") };

            record.BirthSex = "F";

            record.SSN = "987654321";

            Dictionary<string, string> age = new Dictionary<string, string>();
            age.Add("value", "41");
            age.Add("unit", "a");
            record.AgeAtDeath = age;

            record.DateOfBirth = "1978-03-12";

            Dictionary<string, string> addressB = new Dictionary<string, string>();
            addressB.Add("addressCity", "Roanoke");
            addressB.Add("addressState", "Virginia");
            addressB.Add("addressCountry", "United States");
            record.PlaceOfBirth = addressB;

            record.BirthRecordId = "616161";

            record.MotherGivenNames = new String[] { "Jennifer" };
            record.MotherMaidenName = "May";

            record.FatherFamilyName = "Patel";

            Dictionary<string, string> code = new Dictionary<string, string>();
            code.Add("code", "S");
            code.Add("system", "http://terminology.hl7.org/CodeSystem/v3-MaritalStatus");
            code.Add("display", "Never Married");
            record.MaritalStatus = code;

            Dictionary<string, string> addressR = new Dictionary<string, string>();
            addressR.Add("addressCity", "Atlanta");
            addressR.Add("addressCounty", "Fulton");
            addressR.Add("addressState", "Georgia");
            addressR.Add("addressCountry", "United States");
            record.Residence = addressR;
            record.ResidenceWithinCityLimits = true;

            Dictionary<string, string> elevel = new Dictionary<string, string>();
            elevel.Add("code", "ASSOC");
            elevel.Add("system", "http://terminology.hl7.org/CodeSystem/v3-EducationLevel");
            elevel.Add("display", "Associate's or technical degree complete");
            record.EducationLevel = elevel;

            record.UsualOccupation = "Food Prep";

            record.UsualIndustry = "Fast food";

            record.DateOfDeath = "2019-11-02T14:04:00";

            record.DeathLocationName = "home";

            record.DeathLocationDescription = "home";

            Dictionary<string, string> role = new Dictionary<string, string>();
            role.Add("code", "440051000124108");
            role.Add("system", "http://snomed.info/sct");
            role.Add("display", "Medical Examiner");
            record.CertificationRole = role;

            record.CertifierGivenNames = new string[] { "Constance" };
            record.CertifierFamilyName = "Green";

            Dictionary<string, string> address = new Dictionary<string, string>();
            address.Add("addressLine1", "123 12th St.");
            address.Add("addressCity", "Danville");
            address.Add("addressState", "Virginia");
            address.Add("addressCountry", "United States");
            record.CertifierAddress = address;

            record.CertifiedTime = "2019-11-05";

            record.DateOfDeathPronouncement = "2019-11-02T15:30:00";

            record.COD1A = "Cocaine toxicity";
            record.INTERVAL1A = "1 hour";

            record.ContributingConditions = "hypertensive heart disease";

            record.ExaminerContacted = true;

            Dictionary<string, string> manner = new Dictionary<string, string>();
            manner.Add("code", "7878000");
            manner.Add("system", "http://snomed.info/sct");
            manner.Add("display", "Accidental death");
            record.MannerOfDeathType = manner;

            record.InjuryDate = "2019-11-02T13:00:00";

            Dictionary<string, string> codeIAW = new Dictionary<string, string>();
            codeIAW.Add("code", "N");
            codeIAW.Add("system", "http://terminology.hl7.org/CodeSystem/v2-0136");
            codeIAW.Add("display", "No");
            record.InjuryAtWork = codeIAW;

            Dictionary<string, string> codeI = new Dictionary<string, string>();
            codeI.Add("code", "0");
            codeI.Add("system", "urn:oid:2.16.840.1.114222.4.11.7374");
            codeI.Add("display", "Home");
            record.InjuryPlace = codeI;

            record.InjuryLocationName = "home";

            record.InjuryDescription = "drug toxicity";

            Dictionary<string, string> codeA = new Dictionary<string, string>();
            codeA.Add("code", "Y");
            codeA.Add("system", "http://terminology.hl7.org/CodeSystem/v2-0136");
            codeA.Add("display", "Yes");
            record.AutopsyPerformedIndicator = codeA;

            Dictionary<string, string> codeAP = new Dictionary<string, string>();
            codeAP.Add("code", "Y");
            codeAP.Add("system", "http://terminology.hl7.org/CodeSystem/v2-0136");
            codeAP.Add("display", "Yes");
            record.AutopsyResultsAvailable = codeAP;

            Dictionary<string, string> dmethod = new Dictionary<string, string>();
            dmethod.Add("code", "449971000124106");
            dmethod.Add("system", "http://snomed.info/sct");
            dmethod.Add("display", "Patient status determination, deceased and buried");
            record.DecedentDispositionMethod = dmethod;

            Dictionary<string, string> pregnanacyStatus = new Dictionary<string, string>();
            pregnanacyStatus.Add("code", "PHC1260");
            pregnanacyStatus.Add("system", "urn:oid:2.16.840.1.114222.4.5.274");
            pregnanacyStatus.Add("display", "Not pregnant within the past year");
            record.PregnancyStatus = pregnanacyStatus;

            record.TobaccoUse = new Dictionary<string, string>() { { "code", "373067005" }, { "system", "http://snomed.info/sct" }, { "display", "No" } };

            return record;
        }

        public static DeathRecord VivienneLeeWright()
        {
            DeathRecord record = new DeathRecord();

            record.Identifier = "333333";

            record.RegisteredTime = "2019-10-14";

            record.GivenNames = new string[] { "Vivienne", "Lee" };

            record.FamilyName = "Wright";

            record.Race = new Tuple<string, string>[] { Tuple.Create("White", "2106-3"), Tuple.Create("American Indian or Alaska Native", "1002-5") };

            record.Ethnicity = new Tuple<string, string>[] { Tuple.Create("Salvadoran", "2161-8") };

            record.BirthSex = "F";

            record.SSN = "789456123";

            Dictionary<string, string> age = new Dictionary<string, string>();
            age.Add("value", "18");
            age.Add("unit", "a");
            record.AgeAtDeath = age;

            record.DateOfBirth = "2001-04-11";

            Dictionary<string, string> addressB = new Dictionary<string, string>();
            addressB.Add("addressCity", "Hinsdale");
            addressB.Add("addressState", "Illinois");
            addressB.Add("addressCountry", "United States");
            record.PlaceOfBirth = addressB;

            record.BirthRecordId = "717171";

            record.MotherGivenNames = new String[] { "Martha" };
            record.MotherMaidenName = "White";

            record.FatherFamilyName = "Wright";

            Dictionary<string, string> code = new Dictionary<string, string>();
            code.Add("code", "M");
            code.Add("system", "http://terminology.hl7.org/CodeSystem/v3-MaritalStatus");
            code.Add("display", "Married");
            record.MaritalStatus = code;

            Dictionary<string, string> addressR = new Dictionary<string, string>();
            addressR.Add("addressCity", "Atlanta");
            addressR.Add("addressCounty", "Fulton");
            addressR.Add("addressState", "Georgia");
            addressR.Add("addressCountry", "United States");
            record.Residence = addressR;
            record.ResidenceWithinCityLimits = true;

            Dictionary<string, string> elevel = new Dictionary<string, string>();
            elevel.Add("code", "SEC");
            elevel.Add("system", "http://terminology.hl7.org/CodeSystem/v3-EducationLevel");
            elevel.Add("display", "Some secondary or high school education");
            record.EducationLevel = elevel;

            record.UsualOccupation = "secretary";

            record.UsualIndustry = "State agency";

            record.DateOfDeath = "2019-10-10T21:00:00";

            record.DeathLocationName = "Mt. Olive Hospital";

            record.DeathLocationDescription = "Emergency room";

            Dictionary<string, string> role = new Dictionary<string, string>();
            role.Add("code", "310193003");
            role.Add("system", "http://snomed.info/sct");
            role.Add("display", "Coroner");
            record.CertificationRole = role;

            record.CertifierGivenNames = new string[] { "Jim" };
            record.CertifierFamilyName = "Black";

            Dictionary<string, string> address = new Dictionary<string, string>();
            address.Add("addressCity", "Bird in Hand");
            address.Add("addressCounty", "Lancaster");
            address.Add("addressState", "Pennsylvania");
            address.Add("addressCountry", "United States");
            record.CertifierAddress = address;

            record.CertifiedTime = "2019-10-14";

            record.DateOfDeathPronouncement = "2019-10-10T21:00:00";

            record.COD1A = "Cardiopulmonary arrest";
            record.INTERVAL1A = "4 hours";

            record.COD1B = "Eclampsia";
            record.INTERVAL1B = "3 months";

            record.ExaminerContacted = true;

            Dictionary<string, string> manner = new Dictionary<string, string>();
            manner.Add("code", "38605008");
            manner.Add("system", "http://snomed.info/sct");
            manner.Add("display", "Natural death");
            record.MannerOfDeathType = manner;

            Dictionary<string, string> codeA = new Dictionary<string, string>();
            codeA.Add("code", "Y");
            codeA.Add("system", "http://terminology.hl7.org/CodeSystem/v2-0136");
            codeA.Add("display", "Yes");
            record.AutopsyPerformedIndicator = codeA;

            Dictionary<string, string> codeAP = new Dictionary<string, string>();
            codeAP.Add("code", "Y");
            codeAP.Add("system", "http://terminology.hl7.org/CodeSystem/v2-0136");
            codeAP.Add("display", "Yes");
            record.AutopsyResultsAvailable = codeAP;

            Dictionary<string, string> dmethod = new Dictionary<string, string>();
            dmethod.Add("code", "449961000124104");
            dmethod.Add("system", "http://snomed.info/sct");
            dmethod.Add("display", "Patient status determination, deceased and cremated");
            record.DecedentDispositionMethod = dmethod;

            Dictionary<string, string> pregnanacyStatus = new Dictionary<string, string>();
            pregnanacyStatus.Add("code", "PHC1261");
            pregnanacyStatus.Add("system", "urn:oid:2.16.840.1.114222.4.5.274");
            pregnanacyStatus.Add("display", "Pregnant at the time of death");
            record.PregnancyStatus = pregnanacyStatus;

            record.TobaccoUse = new Dictionary<string, string>() { { "code", "373066001" }, { "system", "http://snomed.info/sct" }, { "display", "Yes" } };

            return record;
        }

        public static DeathRecord JavierLuisPerezFull()
        {
            DeathRecord record = new DeathRecord();

            record.Identifier = "444444";

            record.RegisteredTime = "2020-01-15";

            record.GivenNames = new string[] { "Javier", "Luis" };

            record.FamilyName = "Perez";

            record.Race = new Tuple<string, string>[] { Tuple.Create("White", "2106-3"), Tuple.Create("Black", "2054-5") };

            record.Ethnicity = new Tuple<string, string>[] { Tuple.Create("Cuban", "2182-4") };

            record.BirthSex = "M";

            record.SSN = "456123789";

            Dictionary<string, string> age = new Dictionary<string, string>();
            age.Add("value", "55");
            age.Add("unit", "a");
            record.AgeAtDeath = age;

            record.DateOfBirth = "1964-02-24";

            Dictionary<string, string> addressB = new Dictionary<string, string>();
            addressB.Add("addressCity", "San Antonio");
            addressB.Add("addressState", "Texas");
            addressB.Add("addressCountry", "United States");
            record.PlaceOfBirth = addressB;

            record.BirthRecordId = "818181";

            record.MotherGivenNames = new String[] { "Lilliana" };
            record.MotherMaidenName = "Jones";

            record.FatherFamilyName = "Perez";

            Dictionary<string, string> code = new Dictionary<string, string>();
            code.Add("code", "M");
            code.Add("system", "http://terminology.hl7.org/CodeSystem/v3-MaritalStatus");
            code.Add("display", "Married");
            record.MaritalStatus = code;

            Dictionary<string, string> addressR = new Dictionary<string, string>();
            addressR.Add("addressCity", "Annapolis");
            addressR.Add("addressCounty", "Anne Arundel");
            addressR.Add("addressState", "Maryland");
            addressR.Add("addressCountry", "United States");
            record.Residence = addressR;
            record.ResidenceWithinCityLimits = false;

            Dictionary<string, string> elevel = new Dictionary<string, string>();
            elevel.Add("code", "GD");
            elevel.Add("system", "http://terminology.hl7.org/CodeSystem/v3-EducationLevel");
            elevel.Add("display", "Graduate or professional Degree complete");
            record.EducationLevel = elevel;

            record.UsualOccupation = "carpenter";

            record.UsualIndustry = "construction";

            record.DateOfDeath = "2019-12-20T11:25:00";

            record.DeathLocationName = "County Hospital";

            record.DeathLocationDescription = "DOA";

            Dictionary<string, string> role = new Dictionary<string, string>();
            role.Add("code", "434641000124105");
            role.Add("system", "http://snomed.info/sct");
            role.Add("display", "Death certification and verification by physician");
            record.CertificationRole = role;

            record.CertifierGivenNames = new string[] { "Hope" };
            record.CertifierFamilyName = "Lost";

            Dictionary<string, string> address = new Dictionary<string, string>();
            address.Add("addressLine1", "RR1");
            address.Add("addressCity", "Dover");
            address.Add("addressState", "Delaware");
            address.Add("addressCountry", "United States");
            record.CertifierAddress = address;

            record.CertifiedTime = "2020-01-10";

            record.DateOfDeathPronouncement = "2019-12-20T11:35:00";

            record.COD1A = "blunt head trauma";

            record.INTERVAL1A = "unknown";

            record.COD1B = "Automobile accident";

            record.INTERVAL1B = "unknown";

            record.COD1C = "Epilepsy";

            record.INTERVAL1C = "unknown";

            record.ExaminerContacted = true;

            Dictionary<string, string> manner = new Dictionary<string, string>();
            manner.Add("code", "7878000");
            manner.Add("system", "http://snomed.info/sct");
            manner.Add("display", "Accidental death");
            record.MannerOfDeathType = manner;

            record.InjuryDate = "2019-12-20T11:15:00";

            Dictionary<string, string> codeIAW = new Dictionary<string, string>();
            codeIAW.Add("code", "Y");
            codeIAW.Add("system", "http://terminology.hl7.org/CodeSystem/v2-0136");
            codeIAW.Add("display", "Yes");
            record.InjuryAtWork = codeIAW;

            Dictionary<string, string> codeI = new Dictionary<string, string>();
            codeI.Add("code", "4");
            codeI.Add("system", "urn:oid:2.16.840.1.114222.4.11.7374");
            codeI.Add("display", "Street/Highway");
            record.InjuryPlace = codeI;

            record.InjuryLocationName = "street";

            record.InjuryDescription = "unrestrained ejected driver in rollover motor vehicle accident";

            Dictionary<string, string> codeT = new Dictionary<string, string>();
            codeT.Add("code", "236320001");
            codeT.Add("system", "urn:oid:2.16.840.1.114222.4.11.6005");
            codeT.Add("display", "Vehicle driver");
            record.TransportationRole = codeT;

            Dictionary<string, string> codeA = new Dictionary<string, string>();
            codeA.Add("code", "N");
            codeA.Add("system", "http://terminology.hl7.org/CodeSystem/v2-0136");
            codeA.Add("display", "No");
            record.AutopsyPerformedIndicator = codeA;

            Dictionary<string, string> dmethod = new Dictionary<string, string>();
            dmethod.Add("code", "449941000124103");
            dmethod.Add("system", "http://snomed.info/sct");
            dmethod.Add("display", "Patient status determination, deceased and removed from state");
            record.DecedentDispositionMethod = dmethod;

            Dictionary<string, string> pregnanacyStatus = new Dictionary<string, string>();
            pregnanacyStatus.Add("code", "NA");
            pregnanacyStatus.Add("system", "http://hl7.org/fhir/v3/NullFlavor");
            pregnanacyStatus.Add("display", "not applicable");
            record.PregnancyStatus = pregnanacyStatus;

            record.TobaccoUse = new Dictionary<string, string>() { { "code", "373067005" }, { "system", "http://snomed.info/sct" }, { "display", "No" } };

            return record;
        }

        public static DeathRecord JavierLuisPerezPartial()
        {
            DeathRecord record = new DeathRecord();

            record.Identifier = "444444";

            record.RegisteredTime = "2020-01-15";

            record.GivenNames = new string[] { "Javier", "Luis" };

            record.FamilyName = "Perez";

            record.Race = new Tuple<string, string>[] { Tuple.Create("White", "2106-3"), Tuple.Create("Black", "2054-5") };

            record.Ethnicity = new Tuple<string, string>[] { Tuple.Create("Cuban", "2182-4") };

            record.BirthSex = "M";

            record.SSN = "456123789";

            Dictionary<string, string> age = new Dictionary<string, string>();
            age.Add("value", "55");
            age.Add("unit", "a");
            record.AgeAtDeath = age;

            record.FatherFamilyName = "Perez";

            Dictionary<string, string> code = new Dictionary<string, string>();
            code.Add("code", "M");
            code.Add("system", "http://terminology.hl7.org/CodeSystem/v3-MaritalStatus");
            code.Add("display", "Married");
            record.MaritalStatus = code;

            Dictionary<string, string> addressR = new Dictionary<string, string>();
            addressR.Add("addressCity", "Annapolis");
            addressR.Add("addressCounty", "Anne Arundel");
            addressR.Add("addressState", "Maryland");
            addressR.Add("addressCountry", "United States");
            record.Residence = addressR;
            record.ResidenceWithinCityLimits = false;

            Dictionary<string, string> elevel = new Dictionary<string, string>();
            elevel.Add("code", "GD");
            elevel.Add("system", "http://terminology.hl7.org/CodeSystem/v3-EducationLevel");
            elevel.Add("display", "Graduate or professional Degree complete");
            record.EducationLevel = elevel;

            record.UsualOccupation = "carpenter";

            record.UsualIndustry = "construction";

            record.DateOfDeath = "2019-12-20T11:25:00";

            record.DeathLocationName = "County Hospital";

            record.DeathLocationDescription = "DOA";

            Dictionary<string, string> role = new Dictionary<string, string>();
            role.Add("code", "434641000124105");
            role.Add("system", "http://snomed.info/sct");
            role.Add("display", "Death certification and verification by physician");
            record.CertificationRole = role;

            record.CertifierGivenNames = new string[] { "Hope" };
            record.CertifierFamilyName = "Lost";

            Dictionary<string, string> address = new Dictionary<string, string>();
            address.Add("addressLine1", "RR1");
            address.Add("addressCity", "Dover");
            address.Add("addressState", "Delaware");
            address.Add("addressCountry", "United States");
            record.CertifierAddress = address;

            record.CertifiedTime = "2020-01-10";

            record.DateOfDeathPronouncement = "2019-12-20T11:35:00";

            record.COD1A = "blunt head trauma";

            record.INTERVAL1A = "unknown";

            record.COD1B = "Automobile accident";

            record.INTERVAL1B = "unknown";

            record.ExaminerContacted = true;

            Dictionary<string, string> manner = new Dictionary<string, string>();
            manner.Add("code", "7878000");
            manner.Add("system", "http://snomed.info/sct");
            manner.Add("display", "Accidental death");
            record.MannerOfDeathType = manner;

            record.InjuryDate = "2019-12-20T11:15:00";

            Dictionary<string, string> codeIAW = new Dictionary<string, string>();
            codeIAW.Add("code", "Y");
            codeIAW.Add("system", "http://terminology.hl7.org/CodeSystem/v2-0136");
            codeIAW.Add("display", "Yes");
            record.InjuryAtWork = codeIAW;

            Dictionary<string, string> codeI = new Dictionary<string, string>();
            codeI.Add("code", "4");
            codeI.Add("system", "urn:oid:2.16.840.1.114222.4.11.7374");
            codeI.Add("display", "Street/Highway");
            record.InjuryPlace = codeI;

            record.InjuryLocationName = "street";

            record.InjuryDescription = "unrestrained ejected driver in rollover motor vehicle accident";

            Dictionary<string, string> codeT = new Dictionary<string, string>();
            codeT.Add("code", "236320001");
            codeT.Add("system", "urn:oid:2.16.840.1.114222.4.11.6005");
            codeT.Add("display", "Vehicle driver");
            record.TransportationRole = codeT;

            Dictionary<string, string> codeA = new Dictionary<string, string>();
            codeA.Add("code", "N");
            codeA.Add("system", "http://terminology.hl7.org/CodeSystem/v2-0136");
            codeA.Add("display", "No");
            record.AutopsyPerformedIndicator = codeA;

            Dictionary<string, string> dmethod = new Dictionary<string, string>();
            dmethod.Add("code", "449941000124103");
            dmethod.Add("system", "http://snomed.info/sct");
            dmethod.Add("display", "Removal from State");
            record.DecedentDispositionMethod = dmethod;

            Dictionary<string, string> pregnanacyStatus = new Dictionary<string, string>();
            pregnanacyStatus.Add("code", "NA");
            pregnanacyStatus.Add("system", "http://hl7.org/fhir/v3/NullFlavor");
            pregnanacyStatus.Add("display", "not applicable");
            record.PregnancyStatus = pregnanacyStatus;

            return record;
        }
    }
}

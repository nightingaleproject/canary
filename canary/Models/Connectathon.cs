using VRDR;
using System;
using System.Collections.Generic;

namespace canary.Models
{
    /// <summary>vrdr-dotnet versions of connectathon records.</summary>
    public class Connectathon
    {
        public static string fhirVersion = "R4";    // used to generate files
        public Connectathon() { }

        public static DeathRecord FromId(int id, string state = null)
        {
            DeathRecord record = null;
            switch (id)
            {
                case 1:
                    record = JanetPage();
                    break;
                case 2:
                    record = MadelynPatel();
                    break;
                case 3:
                    record = VivienneLeeWright();
                    break;
                case 4:
                    record = JavierLuisPerezFull();
                    break;
                case 5:
                    record = JavierLuisPerezPartial();
                    break;
            }

            if (record != null && state != null)
            {
                MortalityData dataHelper = MortalityData.Instance;

                Dictionary<string, string> placeOfDeath = new Dictionary<string, string>();
                placeOfDeath.Add("addressState", dataHelper.StateCodeToStateName(state));
                placeOfDeath.Add("addressCountry", "United States");
                record.DeathLocationAddress = placeOfDeath;
            }
            return record;
        }

        public static DeathRecord JanetPage()
        {
            DeathRecord record = new DeathRecord();

            record.BundleIdentifier = "2019000211";

            record.Identifier = "111115";

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
            addressB.Add("addressState", "GA");
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
            addressR.Add("addressLine1", "25 Hope Street");
            addressR.Add("addressCity", "Atlanta");
            addressR.Add("addressCounty", "Fulton");
            addressR.Add("addressState", "GA");
            addressR.Add("addressCountry", "United States");
            record.Residence = addressR;
            record.ResidenceWithinCityLimitsBoolean = true;

            Dictionary<string, string> elevel = new Dictionary<string, string>();
            elevel.Add("code", "PHC1455");
            elevel.Add("system", "urn:oid:2.16.840.1.114222.4.11.7385");
            elevel.Add("display", "Doctorate Degree or Professional Degree");
            elevel.Add("text", "Doctorate");
            record.EducationLevel = elevel;

            Dictionary<string, string> uocc = new Dictionary<string, string>();
            uocc.Add("code", "1010");
            uocc.Add("system", "urn:oid:2.16.840.1.114222.4.11.7186");
            uocc.Add("display", "Computer Programmers");
            uocc.Add("text", "Programmer");
            record.UsualOccupationCode = uocc;

            Dictionary<string, string> uind = new Dictionary<string, string>();
            uind.Add("code", "6990");
            uind.Add("system", "urn:oid:2.16.840.1.114222.4.11.7187");
            uind.Add("display", "Insurance carriers and related activities");
            uind.Add("text", "Health Insurance");
            record.UsualIndustryCode = uind;

            record.DateOfDeath = "2019-09-01T05:30:00";

            Dictionary<string, string> deathLoc = new Dictionary<string, string>();
            deathLoc.Add("addressCity", "Atlanta");
            deathLoc.Add("addressCountry", "United States");
            record.DeathLocationAddress = deathLoc;

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
            address.Add("addressState", "GA");
            address.Add("addressZip", "30303");
            address.Add("addressCountry", "United States");
            record.CertifierAddress = address;

            record.CertifiedTime = "2019-09-02";

            record.DateOfDeathPronouncement = "2019-09-01T05:30:00";
            record.PronouncerGivenNames = new string[] { "Sam" };
            record.PronouncerFamilyName = "Jones";

            Tuple<string, string, Dictionary<string, string>>[] causes = {
                Tuple.Create("Congestive heart failure", "1 hour",
                    new Dictionary<string, string>(){
                            {"code", "I50.0"},
                            {"system", "urn:oid:2.16.840.1.114222.4.11.3593"},
                            {"display", "Congestive heart failure"}}),
                Tuple.Create("breast cancer", "20 years",
                    new Dictionary<string, string>(){
                            {"code", "D05.7"},
                            {"system", "urn:oid:2.16.840.1.114222.4.11.3593"},
                            {"display", "Other carcinoma in situ of breast"}}),
            };
            record.CausesOfDeath = causes;

            record.ExaminerContactedBoolean = false;

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

            Dictionary<string, string> fdaddress = new Dictionary<string, string>();
            fdaddress.Add("addressLine1", "15 Pecan Street");
            fdaddress.Add("addressLine2", "Line 2");
            fdaddress.Add("addressCity", "Atlanta");
            fdaddress.Add("addressState", "GA");
            fdaddress.Add("addressZip", " 30301");
            fdaddress.Add("addressCountry", "United States");
            record.FuneralHomeAddress = fdaddress;
            record.FuneralHomeName = "Pecan Street Funeral Home and Crematory";
            // record.FuneralDirectorPhone = "000-000-0000";    // unknown property?????

            Dictionary<string, string> morticianId = new Dictionary<string, string>();
            morticianId.Add("system", "http://hl7.org/fhir/sid/us-npi");
            morticianId.Add("value", "111111AB");
            record.MorticianIdentifier = morticianId;

            record.MorticianGivenNames = new string[] { "Maureen", "P" };
            record.MorticianFamilyName = "Winston";

            Dictionary<string, string> dladdress = new Dictionary<string, string>();
            dladdress.Add("addressLine1", "15 Pecan Street");
            dladdress.Add("addressLine2", "Line 2");
            dladdress.Add("addressCity", "Atlanta");
            dladdress.Add("addressState", "GA");
            dladdress.Add("addressZip", " 30301");
            dladdress.Add("addressCountry", "United States");
            record.DispositionLocationAddress = dladdress;
            record.DispositionLocationName = "Pecan Street Funeral Home and Crematory";

            Dictionary<string, string> dmethod = new Dictionary<string, string>();
            dmethod.Add("code", "449961000124104");
            dmethod.Add("system", "http://snomed.info/sct");
            dmethod.Add("display", "Patient status determination, deceased and cremated");
            record.DecedentDispositionMethod = dmethod;

            Dictionary<string, string> pregnanacyStatus = new Dictionary<string, string>();
            pregnanacyStatus.Add("code", "NA");
            pregnanacyStatus.Add("system", "http://terminology.hl7.org/CodeSystem/v3-NullFlavor");
            pregnanacyStatus.Add("display", "not applicable");
            record.PregnancyStatus = pregnanacyStatus;

            record.TobaccoUse = new Dictionary<string, string>() {
                { "code", "UNK" },
                { "system", "http://terminology.hl7.org/CodeSystem/v3-NullFlavor" },
                { "display", "unknown" } };

            // uncomment to generate file
            string filename = "1_janet_page_cancer_" + fhirVersion + ".xml";
            WriteRecordAsXml(record, filename);

            return record;
        }

        public static DeathRecord MadelynPatel()
        {
            DeathRecord record = new DeathRecord();

            record.BundleIdentifier = "2019000213";

            record.Identifier = "222225";

            record.RegisteredTime = "2019-11-06";

            record.GivenNames = new string[] { "Madelyn" };

            record.FamilyName = "Patel";

            record.Race = new Tuple<string, string>[] { Tuple.Create("Asian Indian", "2029-7") };

            record.Ethnicity = new Tuple<string, string>[] { Tuple.Create("Not Hispanic or Latino", "2186-5") };

            record.BirthSex = "F";

            record.SSN = "987654321";

            Dictionary<string, string> age = new Dictionary<string, string>();
            age.Add("value", "41");
            age.Add("unit", "a");
            record.AgeAtDeath = age;

            record.DateOfBirth = "1978-03-12";

            Dictionary<string, string> addressB = new Dictionary<string, string>();
            addressB.Add("addressCity", "Roanoke");
            addressB.Add("addressState", "VA");
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
            addressR.Add("addressLine1", "5590 Lockwood Drive");
            addressR.Add("addressCity", "Danville");
            addressR.Add("addressState", "VA");
            addressR.Add("addressCountry", "United States");
            record.Residence = addressR;
            record.ResidenceWithinCityLimitsBoolean = true;

            Dictionary<string, string> elevel = new Dictionary<string, string>();
            elevel.Add("code", "PHC1452");
            elevel.Add("system", "2.16.840.1.114222.4.11.7385");
            elevel.Add("display", "Associate Degree");
            record.EducationLevel = elevel;

            Dictionary<string, string> uocc = new Dictionary<string, string>();
            uocc.Add("code", "4030");
            uocc.Add("system", "urn:oid:2.16.840.1.114222.4.11.7186");
            uocc.Add("display", "Food preparation workers");
            uocc.Add("text", "Food Prep");
            record.UsualOccupationCode = uocc;

            Dictionary<string, string> uind = new Dictionary<string, string>();
            uind.Add("code", "8680");
            uind.Add("system", "urn:oid:2.16.840.1.114222.4.11.7187");
            uind.Add("display", "Restaurants and other food services");
            uind.Add("text", "Fast food");
            record.UsualIndustryCode = uind;

            record.DateOfDeath = "2019-11-02T14:04:00";

            Dictionary<string, string> deathLoc = new Dictionary<string, string>();
            deathLoc.Add("addressCity", "Danville");
            deathLoc.Add("addressCountry", "United States");
            record.DeathLocationAddress = deathLoc;

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
            address.Add("addressState", "VA");
            address.Add("addressCountry", "United States");
            record.CertifierAddress = address;

            record.CertifiedTime = "2019-11-05";

            record.DateOfDeathPronouncement = "2019-11-02T15:30:00";
            record.PronouncerGivenNames = new string[] { "Constance" };
            record.PronouncerFamilyName = "Green";

            Tuple<string, string, Dictionary<string, string>>[] causes = {
                Tuple.Create("Cocaine toxicity", "1 hour",
                    new Dictionary<string, string>(){
                            {"code", "T40.5"},
                            {"system", "urn:oid:2.16.840.1.114222.4.11.3593"},
                            {"display", "Poisoning by cocaine"}})
            };
            record.CausesOfDeath = causes;

            record.ContributingConditions = "hypertensive heart disease";

            record.ExaminerContactedBoolean = true;

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

            // @check
            Dictionary<string, string> injuryPlace = new Dictionary<string, string>();
            injuryPlace.Add("code", "0");
            injuryPlace.Add("system", "urn:oid:2.16.840.1.114222.4.5.320");
            injuryPlace.Add("display", "Home");
            record.InjuryPlace = injuryPlace;

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



            Dictionary<string, string> fdaddress = new Dictionary<string, string>();
            fdaddress.Add("addressLine1", "303 Rosewood Ave");
            fdaddress.Add("addressCity", "Danville");
            fdaddress.Add("addressState", "VA");
            fdaddress.Add("addressZip", " 24541");
            fdaddress.Add("addressCountry", "United States");
            record.FuneralHomeAddress = fdaddress;
            record.FuneralHomeName = "Rosewood Funeral Home";

            Dictionary<string, string> morticianId = new Dictionary<string, string>();
            morticianId.Add("system", "http://hl7.org/fhir/sid/us-npi");
            morticianId.Add("value", "212222AB");
            record.MorticianIdentifier = morticianId;

            record.MorticianGivenNames = new string[] { "Ronald", "Q" };
            record.MorticianFamilyName = "Smith";

            Dictionary<string, string> dladdress = new Dictionary<string, string>();
            dladdress.Add("addressLine1", "303 Rosewood Ave");
            dladdress.Add("addressCity", "Danville");
            dladdress.Add("addressState", "VA");
            dladdress.Add("addressZip", " 24541");
            dladdress.Add("addressCountry", "United States");
            record.DispositionLocationAddress = dladdress;
            record.DispositionLocationName = "Rosewood Cemetary";

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

            record.TobaccoUse = new Dictionary<string, string>() {
                { "code", "373067005" },
                { "system", "http://snomed.info/sct" },
                { "display", "No" } };

            // uncomment to generate file
            string filename = "2_madelyn_patel_opiod_" + fhirVersion + ".xml";
            WriteRecordAsXml(record, filename);

            return record;
        }

        public static DeathRecord VivienneLeeWright()
        {
            DeathRecord record = new DeathRecord();

            record.BundleIdentifier = "2019000215";

            record.Identifier = "333335";

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
            addressB.Add("addressState", "IL");
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
            addressR.Add("addressLine1", "101 Liberty Lane");
            addressR.Add("addressCity", "Harrisburg ");
            addressR.Add("addressState", "PA");
            addressR.Add("addressCountry", "United States");
            record.Residence = addressR;
            record.ResidenceWithinCityLimitsBoolean = true;

            Dictionary<string, string> elevel = new Dictionary<string, string>();
            elevel.Add("code", "PHC1449");
            elevel.Add("system", "urn:oid:2.16.840.1.114222.4.11.7385");
            elevel.Add("display", "9th through 12th grade; no diploma");
            elevel.Add("text", "11th grade");
            record.EducationLevel = elevel;

            Dictionary<string, string> uocc = new Dictionary<string, string>();
            uocc.Add("code", "5700");
            uocc.Add("system", "urn:oid:2.16.840.1.114222.4.11.7186");
            uocc.Add("display", "Secretaries and administrative assistants");
            uocc.Add("text", "secretary");
            record.UsualOccupationCode = uocc;

            Dictionary<string, string> uind = new Dictionary<string, string>();
            uind.Add("code", "9390");
            uind.Add("system", "urn:oid:2.16.840.1.114222.4.11.7187");
            uind.Add("display", "Other general government and support");
            uind.Add("text", "State agency");
            record.UsualIndustryCode = uind;

            record.DateOfDeath = "2019-10-10T21:00:00";

            Dictionary<string, string> deathLoc = new Dictionary<string, string>();
            deathLoc.Add("addressCity", "Lancaster");
            deathLoc.Add("addressCounty", "Lancaster");
            deathLoc.Add("addressState", "PA");
            deathLoc.Add("addressCountry", "United States");
            record.DeathLocationAddress = deathLoc;

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
            address.Add("addressLine1", "44 South Street");
            address.Add("addressCity", "Bird in Hand");
            address.Add("addressState", "PA");
            address.Add("addressZip", "17505");
            address.Add("addressCountry", "United States");
            record.CertifierAddress = address;

            record.CertifiedTime = "2019-10-14";

            record.DateOfDeathPronouncement = "2019-10-10T21:00:00";
            record.PronouncerGivenNames = new string[] { "Jim" };
            record.PronouncerFamilyName = "Black";

            Tuple<string, string, Dictionary<string, string>>[] causes = {
                Tuple.Create("Cardiopulmonary arrest", "4 hours",
                    new Dictionary<string, string>(){
                            {"code", "I46"},
                            {"system", "urn:oid:2.16.840.1.114222.4.11.3593"},
                            {"display", "Cardiac arrest"}}),
                Tuple.Create("Eclampsia", "3 months",
                    new Dictionary<string, string>(){
                            {"code", "O15"},
                            {"system", "urn:oid:2.16.840.1.114222.4.11.3593"},
                            {"display", "Eclampsia"}}),
            };
            record.CausesOfDeath = causes;

            record.ExaminerContactedBoolean = true;

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

            Dictionary<string, string> morticianId = new Dictionary<string, string>();
            morticianId.Add("system", "http://hl7.org/fhir/sid/us-npi");
            morticianId.Add("value", "414444AB");
            record.MorticianIdentifier = morticianId;

            record.MorticianGivenNames = new string[] { "Joseph", "M" };
            record.MorticianFamilyName = "Clark";

            Dictionary<string, string> fdaddress = new Dictionary<string, string>();
            fdaddress.Add("addressLine1", "211 High Street");
            fdaddress.Add("addressCity", "Lancaster");
            fdaddress.Add("addressState", "PA");
            fdaddress.Add("addressZip", " 17573");
            fdaddress.Add("addressCountry", "United States");
            record.FuneralHomeAddress = fdaddress;
            record.FuneralHomeName = "Lancaster Funeral Home and Crematory";

            Dictionary<string, string> dladdress = new Dictionary<string, string>();
            dladdress.Add("addressLine1", "211 High Street");
            dladdress.Add("addressCity", "Lancaster");
            dladdress.Add("addressState", "PA");
            dladdress.Add("addressZip", " 17573");
            dladdress.Add("addressCountry", "United States");
            record.FuneralHomeAddress = dladdress;
            record.FuneralHomeName = "Lancaster Funeral Home and Crematory";
            record.DispositionLocationAddress = dladdress;
            record.DispositionLocationName = "Lancaster Funeral Home and Crematory";

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

            record.TobaccoUse = new Dictionary<string, string>() {
                { "code", "373066001" },
                { "system", "http://snomed.info/sct" },
                { "display", "Yes" } };

            // uncomment to generate file
            string filename = "3_vivienne_wright_pregnant_" + fhirVersion + ".xml";
            WriteRecordAsXml(record, filename);

            return record;
        }

        public static DeathRecord JavierLuisPerezFull()
        {
            DeathRecord record = new DeathRecord();

            record.BundleIdentifier = "2019000217";

            record.Identifier = "444445";

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
            addressB.Add("addressState", "TX");
            addressB.Add("addressCountry", "United States");
            record.PlaceOfBirth = addressB;

            record.BirthRecordId = "818181";

            record.MotherGivenNames = new String[] { "Liliana" };
            record.MotherMaidenName = "Jones";

            record.FatherFamilyName = "Perez";

            Dictionary<string, string> code = new Dictionary<string, string>();
            code.Add("code", "M");
            code.Add("system", "http://terminology.hl7.org/CodeSystem/v3-MaritalStatus");
            code.Add("display", "Married");
            record.MaritalStatus = code;

            Dictionary<string, string> addressR = new Dictionary<string, string>();
            addressR.Add("addressLine1", "143 Taylor Street");
            addressR.Add("addressCity", "Annapolis");
            addressR.Add("addressCounty", "Anne Arundel");
            addressR.Add("addressState", "MD");
            addressR.Add("addressCountry", "United States");
            record.Residence = addressR;
            record.ResidenceWithinCityLimitsBoolean = false;

            Dictionary<string, string> elevel = new Dictionary<string, string>();
            elevel.Add("code", "PHC1454");
            elevel.Add("system", "urn:oid:2.16.840.1.114222.4.11.7385");
            elevel.Add("display", "Master's Degree");
            record.EducationLevel = elevel;

            Dictionary<string, string> uocc = new Dictionary<string, string>();
            uocc.Add("code", "6230");
            uocc.Add("system", "urn:oid:2.16.840.1.114222.4.11.7186");
            uocc.Add("display", "carpenters");
            uocc.Add("text", "carpenter");
            record.UsualOccupationCode = uocc;

            Dictionary<string, string> uind = new Dictionary<string, string>();
            uind.Add("code", "0770");
            uind.Add("system", "urn:oid:2.16.840.1.114222.4.11.7187");
            uind.Add("display", "Construction");    // actual text is "Construction (the cleaning of buildings and dwellings is incidental during construction and immediately after construction)"
            uind.Add("text", "construction");
            record.UsualIndustryCode = uind;

            record.DateOfDeath = "2019-12-20T11:25:00";

            record.DeathLocationName = "County Hospital";

            record.DeathLocationDescription = "Dead On Arrival";

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
            address.Add("addressState", "DE");
            address.Add("addressCountry", "United States");
            record.CertifierAddress = address;

            record.CertifiedTime = "2020-01-10";

            record.DateOfDeathPronouncement = "2019-12-20T11:35:00";
            record.PronouncerGivenNames = new string[] { "Hope" };
            record.PronouncerFamilyName = "Lost";

            Tuple<string, string, Dictionary<string, string>>[] causes = {
                Tuple.Create("blunt head trauma", "30 min",
                    new Dictionary<string, string>(){
                            {"code", "T90.9"},
                            {"system", "urn:oid:2.16.840.1.114222.4.11.3593"},
                            {"display", "Sequelae of unspecified injury of head"}}),
                Tuple.Create("Automobile accident", "30 min",
                    new Dictionary<string, string>(){
                            {"code", "Y85"},
                            {"system", "urn:oid:2.16.840.1.114222.4.11.3593"},
                            {"display", "Sequelae of transport accidents"}}),
                Tuple.Create("Epilepsy", "20 years",
                    new Dictionary<string, string>(){
                            {"code", "G40"},
                            {"system", "urn:oid:2.16.840.1.114222.4.11.3593"},
                            {"display", "Epilepsy"}}),
            };
            record.CausesOfDeath = causes;

            record.ExaminerContactedBoolean = true;

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

            // @check
            Dictionary<string, string> injuryPlace = new Dictionary<string, string>();
            injuryPlace.Add("code", "0");// @check
            injuryPlace.Add("system", "urn:oid:2.16.840.1.114222.4.5.320");// @check
            injuryPlace.Add("display", "street");
            record.InjuryPlace = injuryPlace;

            record.InjuryDescription = "unrestrained ejected driver in rollover motor vehicle accident";

            Dictionary<string, string> codeT = new Dictionary<string, string>();
            codeT.Add("code", "236320001");
            codeT.Add("system", "http://snomed.info/sct");
            codeT.Add("display", "Vehicle driver");
            record.TransportationRole = codeT;

            Dictionary<string, string> codeA = new Dictionary<string, string>();
            codeA.Add("code", "N");
            codeA.Add("system", "http://terminology.hl7.org/CodeSystem/v2-0136");
            codeA.Add("display", "No");
            record.AutopsyPerformedIndicator = codeA;

            Dictionary<string, string> fdaddress = new Dictionary<string, string>();
            fdaddress.Add("addressLine1", "15 Furnace Drive");
            fdaddress.Add("addressLine2", "Line 2");
            fdaddress.Add("addressCity", "San Antonio");
            fdaddress.Add("addressState", "TX");
            fdaddress.Add("addressZip", " 78201");
            fdaddress.Add("addressCountry", "United States");
            record.FuneralHomeAddress = fdaddress;
            record.FuneralHomeName = "River Funeral Home";

            Dictionary<string, string> morticianId = new Dictionary<string, string>();
            morticianId.Add("system", "http://hl7.org/fhir/sid/us-npi");
            morticianId.Add("value", "313333AB");
            record.MorticianIdentifier = morticianId;

            record.MorticianGivenNames = new string[] { "Pedro", "A" };
            record.MorticianFamilyName = "Jimenez";

            Dictionary<string, string> dladdress = new Dictionary<string, string>();
            dladdress.Add("addressLine1", "15 Furnace Drive");
            dladdress.Add("addressLine2", "Line 2");
            dladdress.Add("addressCity", "San Antonio");
            dladdress.Add("addressState", "TX");
            dladdress.Add("addressZip", " 78201");
            dladdress.Add("addressCountry", "United States");
            record.DispositionLocationAddress = dladdress;
            record.DispositionLocationName = "River Cemetary";

            Dictionary<string, string> dmethod = new Dictionary<string, string>();
            dmethod.Add("code", "449941000124103");
            dmethod.Add("system", "http://snomed.info/sct");
            dmethod.Add("display", "Patient status determination, deceased and removed from state");
            record.DecedentDispositionMethod = dmethod;

            Dictionary<string, string> pregnanacyStatus = new Dictionary<string, string>();
            pregnanacyStatus.Add("code", "NA");
            pregnanacyStatus.Add("system", "http://terminology.hl7.org/CodeSystem/v3-NullFlavor");
            pregnanacyStatus.Add("display", "not applicable");
            record.PregnancyStatus = pregnanacyStatus;

            record.TobaccoUse = new Dictionary<string, string>() {
                { "code", "373067005" },
                { "system", "http://snomed.info/sct" },
                { "display", "No" } };

            // uncomment to generate file
            string filename = "4_javier_perez_accident_full_" + fhirVersion + ".xml";
            WriteRecordAsXml(record, filename);

            return record;
        }

        public static DeathRecord JavierLuisPerezPartial()
        {
            DeathRecord record = new DeathRecord();

            // record.Identifier = "444444";

            // record.RegisteredTime = "2020-01-15";

            // record.GivenNames = new string[] { "Javier", "Luis" };

            // record.FamilyName = "Perez";

            // record.Race = new Tuple<string, string>[] { Tuple.Create("White", "2106-3"), Tuple.Create("Black", "2054-5") };

            // record.Ethnicity = new Tuple<string, string>[] { Tuple.Create("Cuban", "2182-4") };

            // record.BirthSex = "M";

            // record.SSN = "456123789";

            // Dictionary<string, string> age = new Dictionary<string, string>();
            // age.Add("value", "55");
            // age.Add("unit", "a");
            // record.AgeAtDeath = age;

            // record.FatherFamilyName = "Perez";

            // Dictionary<string, string> code = new Dictionary<string, string>();
            // code.Add("code", "M");
            // code.Add("system", "http://terminology.hl7.org/CodeSystem/v3-MaritalStatus");
            // code.Add("display", "Married");
            // record.MaritalStatus = code;

            // Dictionary<string, string> addressR = new Dictionary<string, string>();
            // addressR.Add("addressCity", "Annapolis");
            // addressR.Add("addressCounty", "Anne Arundel");
            // addressR.Add("addressState", "Maryland");
            // addressR.Add("addressCountry", "United States");
            // record.Residence = addressR;
            // record.ResidenceWithinCityLimitsBoolean = false;

            // Dictionary<string, string> elevel = new Dictionary<string, string>();
            // elevel.Add("code", "GD");
            // elevel.Add("system", "http://terminology.hl7.org/CodeSystem/v3-EducationLevel");
            // elevel.Add("display", "Graduate or professional Degree complete");
            // record.EducationLevel = elevel;

            // record.UsualOccupation = "carpenter";

            // record.UsualIndustry = "construction";

            // record.DateOfDeath = "2019-12-20T11:25:00";

            // record.DeathLocationName = "County Hospital";

            // record.DeathLocationDescription = "Dead On Arrival";

            // Dictionary<string, string> role = new Dictionary<string, string>();
            // role.Add("code", "434641000124105");
            // role.Add("system", "http://snomed.info/sct");
            // role.Add("display", "Death certification and verification by physician");
            // record.CertificationRole = role;

            // record.CertifierGivenNames = new string[] { "Hope" };
            // record.CertifierFamilyName = "Lost";

            // Dictionary<string, string> address = new Dictionary<string, string>();
            // address.Add("addressLine1", "RR1");
            // address.Add("addressCity", "Dover");
            // address.Add("addressState", "Delaware");
            // address.Add("addressCountry", "United States");
            // record.CertifierAddress = address;

            // record.CertifiedTime = "2020-01-10";

            // record.DateOfDeathPronouncement = "2019-12-20T11:35:00";

            // record.COD1A = "blunt head trauma";

            // record.INTERVAL1A = "unknown";

            // record.COD1B = "Automobile accident";

            // record.INTERVAL1B = "unknown";

            // record.ExaminerContactedBoolean = true;

            // Dictionary<string, string> manner = new Dictionary<string, string>();
            // manner.Add("code", "7878000");
            // manner.Add("system", "http://snomed.info/sct");
            // manner.Add("display", "Accidental death");
            // record.MannerOfDeathType = manner;

            // record.InjuryDate = "2019-12-20T11:15:00";

            // Dictionary<string, string> codeIAW = new Dictionary<string, string>();
            // codeIAW.Add("code", "Y");
            // codeIAW.Add("system", "http://terminology.hl7.org/CodeSystem/v2-0136");
            // codeIAW.Add("display", "Yes");
            // record.InjuryAtWork = codeIAW;

            // // @check
            // Dictionary<string, string> injuryPlace = new Dictionary<string, string>();
            // injuryPlace.Add("code", "0");// @check
            // injuryPlace.Add("system", "urn:oid:2.16.840.1.114222.4.5.320");// @check
            // injuryPlace.Add("display", "street");
            // record.InjuryPlace = injuryPlace;


            // record.InjuryDescription = "unrestrained ejected driver in rollover motor vehicle accident";

            // Dictionary<string, string> codeT = new Dictionary<string, string>();
            // codeT.Add("code", "236320001");
            // codeT.Add("system", "http://snomed.info/sct");
            // codeT.Add("display", "Vehicle driver");
            // record.TransportationRole = codeT;

            // Dictionary<string, string> codeA = new Dictionary<string, string>();
            // codeA.Add("code", "N");
            // codeA.Add("system", "http://terminology.hl7.org/CodeSystem/v2-0136");
            // codeA.Add("display", "No");
            // record.AutopsyPerformedIndicator = codeA;

            // Dictionary<string, string> dmethod = new Dictionary<string, string>();
            // dmethod.Add("code", "449941000124103");
            // dmethod.Add("system", "http://snomed.info/sct");
            // dmethod.Add("display", "Patient status determination, deceased and removed from state");
            // record.DecedentDispositionMethod = dmethod;

            // Dictionary<string, string> pregnanacyStatus = new Dictionary<string, string>();
            // pregnanacyStatus.Add("code", "NA");
            // pregnanacyStatus.Add("system", "http://terminology.hl7.org/CodeSystem/v3-NullFlavor");
            // pregnanacyStatus.Add("display", "not applicable");
            // record.PregnancyStatus = pregnanacyStatus;

            return record;
        }

        // Writes record to a file named filename in a subdirectory of the current working directory
        //  Note that you do this with docker, you will have to set a bind mount on the container
        public static string WriteRecordAsXml(DeathRecord record, string filename)
        {
            string parentPath = System.IO.Directory.GetCurrentDirectory() + "/connectathon_files";
            System.IO.Directory.CreateDirectory(parentPath);  // in case the directory does not exist
            string fullPath = parentPath + "/" + filename;
            Console.WriteLine("writing record to " + fullPath + " as XML");
            string xml = record.ToXml();
            System.IO.File.WriteAllText(@fullPath, xml);
            // Console.WriteLine(xml);
            return xml;
        }

    }
}

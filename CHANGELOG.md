## Change Log

### v4.0.0-preview1 - 2022-05-25

* Updated the VRDR .NET library dependency for the project to use V4.0.0-preview3
    - Aligns Canary with the latest VRDR IG at http://build.fhir.org/ig/HL7/vrdr/artifacts.html
    - Aligns Canary with the latest VRDR Messaging IG at http://build.fhir.org/ig/nightingaleproject/vital_records_fhir_messaging_ig/branches/main/index.html
    - Reduces test clutter by excluding VRDR DeathRecord Helper properties from tests
* Updated the Connectathon test records to use the 2022 test records provided by NCHS
* Addressed an issue with test counts not totaling accurately
* Updated Canary to use .NET 6.0

### v3.1.0 - 2021-10-01

* Updated VRDR .NET library dependency to v3.2.1
* Updated Connectathon test cases

### v3.1.0-preview2 - 2021-09-13

* Updated the VRDR-dotnet dependency for the project to use V3.2.0-preview5, which fixes bugs
    - Fixed bug in how nulls are interpreted when loading description files that caused segments of records to be dropped in Canary
    - Removed incorrect extra spaces from some race strings
    - Fixed bug that caused an error if the receipt year was set to the death year when creating coded response messages
    - Improved text that describes expected values for the Death Location Jurisdiction field
    - Fixed bug that caused incorrect data to be shown in Canary for Death Location Jurisdiction
* Updated connectathon records to align with the new IG and connectathon test record changes

### v3.1.0-preview1 - 2021-09-02

* Updated the VRDR-dotnet dependency for the project to use V3.2.0-preview3, which updates support to the latest published version of the VRDR IG
* Updated connectathon records to align with the new IG and connectathon test record changes
* Updated synthetic record generation to align with the new IG

### v3.0.1 - 2021-06-11

* Allow Certificate number to be provided for Connectathon Tests
* Add multiple previously omitted fields to the Synthetic Death Record Generator
* Rename all Validators to Syntax Checkers to better reflect what functionality the tool provides
* Add tool to validate FHIR Death Records against user-provided IJE files

### v3.0.0-RC5 - 2020-09-17

* Fixed Connectathon record OIDs to refer to code systems rather than value sets
* Fixed off-by-one counting errors related to duplicate counts for nested records
* Updated the VRDR-dotnet dependency for the project to use V3.1.0-RC5, which fixes bugs

### v3.0.0-RC4 - 2020-09-10

* Updated the VRDR-dotnet dependency for the project to use V3.1.0-RC4, which fixes bugs

### v3.0.0-RC3 - 2020-09-10

* Added better Death Record comparison support to messages
* Added state dropdown for Connectathon records
* Improved display of exceptions by providing more detail
* Improved information on unnecessary fields provided during message validation
* Updated the built in Connectathon records to address issues
* Updated the VRDR-dotnet dependency for the project to use V3.1.0-RC3, which fixes bugs
* Updated dependencies to more recent versions

### v3.0.0-RC2 - 2020-09-08

* Improved error handling
* Updated the built in Connectathon records to match test plan
* Updated the VRDR-dotnet dependency for the project to use V3.1.0-RC2, which fixes several bugs

### v3.0.0-RC1 - 2020-08-25

* Added the ability to validate and produce FHIR VRDR Messages
* Updated the VRDR-dotnet dependency for the project to use V3.1.0-RC1, which adds support for the latest published version of the VRDR IG, using FHIR R4

### v2.11.0 - 2020-01-16
 * 2.11.0 Release
 * Upgrading to vrdr-dotnet v2.13.0
 * JavaScript dependency updates
 * Added "Generate Downloadable Report" feature to tests

### v2.10.1 - 2020-01-15
 * 2.10.1 Release
 * Adjusted test comparison logic when inspecting Property.Types.String properties

### v2.10.0 - 2020-01-15
 * 2.10.0 Release
 * Tweaks to Connectathon test cases

### v2.9.0 - 2020-01-14
 * 2.9.0 Release
 * Upgrading to vrdr-dotnet v2.12.1

### v2.8.0 - 2020-01-14
 * 2.8.0 Release
 * Upgrading to vrdr-dotnet v2.10.1
 * Connectathon specific test case feature added

### v2.7.0 - 2019-12-17
 * 2.7.0 Release
 * Upgrading to vrdr-dotnet v2.9.0
 * Re-integrating faker code into Canary

### v2.6.0 - 2019-11-20
 * 2.6.0 Release
 * Upgrading to vrdr-dotnet (formerly csharp-fhir-death-record) v2.7.0

### v2.5.0 - 2019-09-15
 * 2.5.0 Release
 * Upgrading to csharp-fhir-death-record v2.6.3

### v2.4.0 - 2019-09-10
 * 2.4.0 Release
 * Upgrading to csharp-fhir-death-record v2.6.0

### v2.3.0 - 2019-09-10
 * 2.3.0 Release
 * Upgrading to csharp-fhir-death-record v2.5.0

### v2.2.0 - 2019-09-03
 * 2.2.0 Release
 * Upgrading to csharp-fhir-death-record v2.4.5

### v2.1.1 - 2019-08-21
 * 2.1.1 Release
 * Adjusted roundtrip tests to not include FHIR data that cannot be represented by IJE mortality

### v2.1.0 - 2019-08-20
 * 2.1.0 Release
 * Upgrading to csharp-fhir-death-record v2.4.0
 * Simplified backend to utilize more csharp-fhir-death-record functionality
 * Removed unnecessary imports
 * Improved error display style in frontend
 * Major frontend dependency updates
 * Updated Dockerfile

### v2.0.0 - 2019-06-14
 * 2.0.0 Release
 * Full support for the new balloted Vital Records Death Reporting FHIR Implementation Guide
 * Links to IG profiles are now included for each record property
 * Updated target C# FHIR library

### v1.0.0 - 2019-05-04
 * 1.0.0 Release

### v0.10.0 - 2019-02-14
 * Enabled ability to "POST" records into Canary
 * FHIR Validator now shows all issues found in a record, instead of just the first
 * Improvements made to record importing speed
 * Tweaked navigation dropdowns to be more responsive
 * Various dependency updates

### v0.9.0 - 2019-02-07
 * Updated csharp-fhir-death-record library to v0.22.0

### v0.8.0 - 2019-02-05
 * Updated csharp-fhir-death-record library to v0.21.0

### v0.7.0 - 2019-02-04
 * Revamped Canary into a testing tool for all types of FHIR mortality data systems

### v0.6.0 - 2018-12-10
 * Various dependency updates

### v0.5.0 - 2018-10-01
 * Upgraded Boostrap version

### v0.4.0 - 2018-09-07

* Refactored user interface
* Removed unecessary project files
* Changed target Ruby version to 2.4.4
* Updated README.md
* Various dependency updates

### v0.3.0 - 2018-05-10

* Updated FHIR implementations to use latest Death on FHIR profiles
* Improved UI for test results

### v0.2.0 - 2018-02-01

* FHIR Import testing implemented
* FHIR Export testing implemented

### v0.1.0 - 2017-11-01

* Initial UI design
* Basic support for testing IJE

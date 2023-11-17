[![Build Status](https://github.com/nightingaleproject/canary/actions/workflows/run-tests.yml/badge.svg?branch=master)](https://github.com/nightingaleproject/canary/actions/workflows/run-tests.yml)

## Canary EDRS Testing Framework

Canary is an open source testing framework that supports development of systems that perform standards based exchange of mortality data. Canary provides tests and tools to aid developers in implementing the Vital Records Death Reporting (VRDR) FHIR death record format STU2 (http://hl7.org/fhir/us/vrdr/2021Sep/) FHIR Revision 4.

## Versions

    Releases v2.x.x of the Canary project support FHIR STU3, in line with the May 2019 ballot version of VRDR (v0.1.0)
    Releases v3.x.x of the Canary project support FHIR R4, in line with the STU2 Ballot version of VRDR
    Releases v4.x.x of the Canary project support FHIR R4, in line with the STU2 v1.3 version of VRDR (v1.3)

If you are upgrading from 2.x.x to 3.x.x, please note that there are differences between FHIR STU3 and R4 that impact the structure of the VRDR Death Record. [This commit illustrates the differences between FHIR STU3 and FHIR R4 VRDR Death Records](https://github.com/nightingaleproject/vrdr-dotnet/commit/2b4c2026fdab80e7233f3a7d7ed6e17d5d63f38e). Death Record data may need similar updates from STU3 to R4 when updating to Canary Version 3.

Canary can test a data providers ability to:
- Produce FHIR VRDR Records
- Consume FHIR VRDR Records
- Roundtrip: Convert between FHIR (Consuming) and the IJE Mortality format (Producing)
- Roundtrip: Convert between the IJE Mortality format (Consuming) and FHIR (Producing)
- Produce predefined FHIR VRDR Records as tested at Connectathons
- Produce FHIR VRDR Messages
- Produce predefined FHIR VRDR Messages as tested at Connectathons

Canary also includes the following utilities:
- Generate Synthetic VRDR Records
- FHIR VRDR Record Syntax Checker
- VRDR Record Format Converter
- FHIR VRDR Record Inspector
- FHIR VRDR Record Creator
- IJE Mortality Record Inspector
- FHIR VRDR Message Syntax Checker
- Create FHIR VRDR Messages

### Background

Mortality data is collected, analyzed, and shared by jurisdictions across the United States to provide insight into important trends in health, including the impact of chronic conditions, progress on reducing deaths due to motor vehicle accidents, and the evolving challenge of substance abuse. Numerous systems are used to collect and share information on decedents, including demographic data and medical information relevant to determining the cause of death. By connecting these systems using modern standards like FHIR we can implement processes that reduce the burden on certifiers, improve data quality, and improve timeliness of data collection and reporting. Canary supports the development of interoperable systems by providing tools for testing implementations and working with mortality data formats.

### Running

A [Dockerized](https://www.docker.com/get-started) version of Canary has been published to Docker Hub. To make sure you have the most recent version of Canary:

```
docker pull mitre/canary:latest
```

Running Canary is as easy as:

```
docker run --rm -p 8080:80 mitre/canary:latest
```

These commands will pull the latest version of Canary from Docker Hub, and run it. You can access it from a web browser at [http://localhost:8080](http://localhost:8080). To run a specific version, simply append the version to the `docker run` command above. You can see all versions of Canary that are available to run from DockerHub [here](https://hub.docker.com/r/mitre/canary/tags). For example:

```
docker run --rm -p 8080:80 mitre/canary:v4.0.3
```

If you want to build a Dockerized Canary from scratch (from source), you can do so by running (inside the project root directory):

```
docker build -t canary .
docker run -d -p 8080:80 --name mycanary canary
```

If you prefer not to use Docker, you can run Canary from the root project directory using the version of [.NET Core](https://dotnet.microsoft.com/download) listed in [global.json](global.json) and an installation of [Node.js](https://nodejs.org/):

```
dotnet --version # Verify this only returns a version and no error
node --version # Verify Node is successfully installed
dotnet tool install --global dotnet-ef --version 6.0.*
dotnet ef database update --project canary
dotnet run --project canary
```

After you execute the `dotnet run` command, the application will be accessible from a web browser at [http://localhost:5000](http://localhost:5000).

Note that, if you're using windows, you may need to manually run `npm install` from the `canary/ClientApp/` directory.

### Publishing a Version

The primary method of deploying Canary is to DockerHub. Whenever a commit is merged into master, DockerHub will automatically build that commit and tag it as `latest` on DockerHub so any user that runs `docker pull mitre/canary:latest` will receive a copy of the image.

Whenever a tag is created on Canary of the form `vX.Y.Z`, Dockerhub will automatically build that git tag and and tag it as `vX.Y.Z` on DockerHub. This means any user who runs `docker pull mitre/canary:X.Y.Z` will receive a copy of the application at version `X.Y.Z`.

At this time the version number and date displayed in the application need to be manually updated by editing window.VERSION and window.VERSION_DATE in the file canary/ClientApp/src/index.js. The version should also be updated by changing the "version" setting in canary/ClientApp/package.json.

To create a release:

1. Update canary/canary.csproj to point to the desired version of the VRDR.Messaging library (and therefore VRDR .NET library)
1. Update the version number and date in canary/ClientApp/src/index.js
1. Update the CHANGELOG.md file with the latest change history
1. Update the README to use the latest version in the section on running canary with docker
1. Merge the above changes onto master
1. On the [Releases](https://github.com/nightingaleproject/canary/releases) page click "Draft a new Release"
1. Create a tag and release title for the new release following the existing naming convention
1. Add the release notes from the CHANGELOG as the description
1. Attach the most recent binary from "Build Self-Contained Windows Executable" GitHub task
    1. Follow [these instructions](https://docs.github.com/en/actions/managing-workflow-runs/downloading-workflow-artifacts) to find the build artifact
    1. Download the artifact and rename it to `canary-<version>-windows-x64.zip`
    1. Upload the artifact to the release page

### License

Copyright 2017, 2018, 2019, 2020 The MITRE Corporation

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

```
http://www.apache.org/licenses/LICENSE-2.0
```

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

### Contact Information

For questions or comments about Canary, please send email to

    nvssmodernization@cdc.gov

[![Build Status](https://travis-ci.org/nightingaleproject/canary.svg?branch=master)](https://travis-ci.org/nightingaleproject/canary)

## Canary EDRS Testing Framework

Canary is an open source testing framework that supports development of systems that perform standards based exchange of mortality data. Canary provides tests and tools to aid developers in implementing the Vital Records Death Reporting (VRDR) FHIR death record format (http://hl7.org/fhir/us/vrdr).

Canary can test a data providers ability to:
- Produce FHIR Death Records
- Consume FHIR Death Records
- Roundtrip: Convert between FHIR (Consuming) and the IJE Mortaility format (Producing)
- Roundtrip: Convert between the IJE Mortaility format (Consuming) and FHIR (Producing)

Canary also includes the following utilities:
- Generate Synthetic Death Records
- Validate FHIR Death Records
- Death Record Format Converter
- FHIR Death Record Inspector
- FHIR Death Record Creator
- IJE Mortality Record Inspector

### Background

Mortality data is collected, analyzed, and shared by jurisdictions across the United States to provide insight into important trends in health, including the impact of chronic conditions, progress on reducing deaths due to motor vehicle accidents, and the evolving challenge of substance abuse. Numerous systems are used to collect and share information on decedents, including demographic data and medical information relevant to determining the cause of death. By connecting these systems using modern standards like FHIR we can implement processes that reduce the burden on certifiers, improve data quality, and improve timeliness of data collection and reporting. Canary supports the development of interoperable systems by providing tools for testing implementations and working with mortality data formats.

### Running

A [Dockerized](https://www.docker.com/get-started) version of Canary has been published to Docker Hub, so running Canary is as easy as:

```
docker run --rm -p 8080:80 adammitre/canary
```

This command will pull the latest version of Canary from Docker Hub, and run it. You can access it from a web browser at [http://localhost:8080](http://localhost:8080).

If you want to build a Dockerized Canary from scratch (from source), you can do so by running (inside the project root directory):

```
docker build -t canary .
docker run -d -p 8080:80 --name mycanary canary
```

If you prefer not to use Docker, you can run Canary from the root project directory using [.NET Core](https://dotnet.microsoft.com/download):

```
dotnet ef database update
dotnet run
```

### License

Copyright 2017, 2018, 2019 The MITRE Corporation

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

```
http://www.apache.org/licenses/LICENSE-2.0
```

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.

### Contact Information

For questions or comments about Canary or the Nightingale EDRS, please send email to

    cdc-nvss-feedback-list@lists.mitre.org

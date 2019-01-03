[![Build Status](https://travis-ci.org/nightingaleproject/canary.svg?branch=master)](https://travis-ci.org/nightingaleproject/canary)

## Canary EDRS Testing Framework

Canary is a testing framework for electronic death registration systems (EDRS). Canary can be used to test support for various mortality related data formats.

### Installation and Setup for Development or Testing

Canary is a Ruby on Rails application that uses the PostgreSQL database for data storage.

For information on how to deploy Nightingale as a Docker container, see [CONTAINERDEPLOY.md](CONTAINERDEPLOY.md).

#### Prerequisites

To work with the application, you will need to install some prerequisites:

* [Ruby](https://www.ruby-lang.org/)
* [Bundler](http://bundler.io/)
* [Postgres](http://www.postgresql.org/)

Once the prerequisites are available, Canary can be installed and demonstration data can be loaded.

#### Setup

* Retrieve the application source code

    `git clone https://github.com/nightingaleproject/canary.git`

* Change into the new directory

    `cd canary`

* Install Ruby gem dependencies

    `bundle install`

* Set up the database tables

    `bundle exec rake db:drop db:create db:migrate db:setup RAILS_ENV=development`

* Set up system with demonstration data

    `bundle exec rake canary:demo:setup`

  * This will initialize Canary with a demonstration account, `user@example.com` (with a password of `123456`).

* Run the application server

    `bundle exec rails server`

    The server will be running at http://localhost:3000/

### Version History

This project adheres to [Semantic Versioning](http://semver.org/).

Releases are documented in the [CHANGELOG](https://github.com/nightingaleproject/canary/blob/master/CHANGELOG.md).

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

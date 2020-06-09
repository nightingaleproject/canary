import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { Layout } from './components/Layout';
import { Dashboard } from './components/dashboard/Dashboard';
import { RecentTests } from './components/tests/RecentTests';
import { FHIRConsuming } from './components/tests/FHIRConsuming';
import { FHIRProducing } from './components/tests/FHIRProducing';
import { FHIRMessageProducing } from './components/tests/FHIRMessageProducing';
import { EDRSRoundtripConsuming } from './components/tests/EDRSRoundtripConsuming';
import { EDRSRoundtripProducing } from './components/tests/EDRSRoundtripProducing';
import { FHIRCreator } from './components/tools/FHIRCreator';
import { FHIRInspector } from './components/tools/FHIRInspector';
import { FHIRValidator } from './components/tools/FHIRValidator';
import { FHIRMessageValidator } from './components/tools/FHIRMessageValidator';
import { IJEInspector } from './components/tools/IJEInspector';
import { RecordConverter } from './components/tools/RecordConverter';
import { RecordGenerator } from './components/tools/RecordGenerator';
import { ConnectathonDashboard } from './components/dashboard/ConnectathonDashboard';
import { Connectathon } from './components/tests/Connectathon';

export default class App extends Component {
  displayName = App.name;

  render() {
    return (
      <Layout>
        <Switch>
          <Route exact path="/" component={Dashboard} />
          <Route path="/recent-tests" component={RecentTests} />
          <Route path="/test-fhir-consuming/:id?" component={FHIRConsuming} />
          <Route path="/test-fhir-producing/:id?" component={FHIRProducing} />
          <Route path="/test-fhir-message-producing/:id?" component={FHIRMessageProducing} />
          <Route path="/test-edrs-roundtrip-consuming/:id?" component={EDRSRoundtripConsuming} />
          <Route path="/test-edrs-roundtrip-producing/:id?" component={EDRSRoundtripProducing} />
          <Route path="/test-connectathon-dash" component={ConnectathonDashboard} />
          <Route path="/test-connectathon/:id" component={Connectathon} />
          <Route path="/tool-fhir-inspector" component={FHIRInspector} />
          <Route path="/tool-fhir-creator" component={FHIRCreator} />
          <Route path="/tool-fhir-validator" component={FHIRValidator} />
          <Route path="/tool-fhir-message-validator" component={FHIRMessageValidator} />
          <Route path="/tool-ije-inspector" component={IJEInspector} />
          <Route path="/tool-record-converter" component={RecordConverter} />
          <Route path="/tool-record-generator" component={RecordGenerator} />
        </Switch>
      </Layout>
    );
  }
}

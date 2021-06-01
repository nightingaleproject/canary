import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { ConnectathonDashboard } from './components/dashboard/ConnectathonDashboard';
import { Dashboard } from './components/dashboard/Dashboard';
import { Layout } from './components/Layout';
import { Connectathon } from './components/tests/Connectathon';
import { EDRSRoundtripConsuming } from './components/tests/EDRSRoundtripConsuming';
import { EDRSRoundtripProducing } from './components/tests/EDRSRoundtripProducing';
import { FHIRConsuming } from './components/tests/FHIRConsuming';
import { FHIRMessageProducing } from './components/tests/FHIRMessageProducing';
import { FHIRProducing } from './components/tests/FHIRProducing';
import { RecentTests } from './components/tests/RecentTests';
import { FHIRCreator } from './components/tools/FHIRCreator';
import { FHIRInspector } from './components/tools/FHIRInspector';
import { FHIRMessageCreator } from './components/tools/FHIRMessageCreator';
import { FHIRMessageSyntaxChecker } from './components/tools/FHIRMessageSyntaxChecker';
import { FHIRSyntaxChecker } from './components/tools/FHIRSyntaxChecker';
import { IJEInspector } from './components/tools/IJEInspector';
import { MessageConnectathonProducing } from './components/tests/MessageConnectathonProducing';
import { RecordConverter } from './components/tools/RecordConverter';
import { RecordGenerator } from './components/tools/RecordGenerator';

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
          <Route path="/test-fhir-message-creation/:id?" component={FHIRMessageCreator} />
          <Route path="/test-edrs-roundtrip-consuming/:id?" component={EDRSRoundtripConsuming} />
          <Route path="/test-edrs-roundtrip-producing/:id?" component={EDRSRoundtripProducing} />
          <Route path="/test-connectathon-dash/:type" component={ConnectathonDashboard} />
          <Route path="/test-connectathon/:id" component={Connectathon} />
          <Route path="/test-connectathon-messaging/:id" component={MessageConnectathonProducing} />
          <Route path="/tool-fhir-inspector" component={FHIRInspector} />
          <Route path="/tool-fhir-creator" component={FHIRCreator} />
          <Route path="/tool-fhir-syntax-checker" component={FHIRSyntaxChecker} />
          <Route path="/tool-fhir-message-syntax-checker" component={FHIRMessageSyntaxChecker} />
          <Route path="/tool-ije-inspector" component={IJEInspector} />
          <Route path="/tool-record-converter" component={RecordConverter} />
          <Route path="/tool-record-generator" component={RecordGenerator} />
        </Switch>
      </Layout>
    );
  }
}

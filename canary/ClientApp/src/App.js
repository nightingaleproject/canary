import React, { Component } from 'react';
import { Routes, Route } from 'react-router-dom';
import { ConnectathonDashboard } from './components/dashboard/ConnectathonDashboard';
import { Dashboard } from './components/dashboard/Dashboard';
import { Layout } from './components/Layout';
import { Connectathon } from './components/tests/Connectathon';
import { EDRSRoundtripConsuming } from './components/tests/EDRSRoundtripConsuming';
import { EDRSRoundtripProducing } from './components/tests/EDRSRoundtripProducing';
import { FHIRIJEValidatorProducing } from './components/tests/FHIRIJEValidatorProducing';
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
        <Routes>
          <Route exact path="/" element={<Dashboard/>} />
          <Route path="recent-tests" element={<RecentTests/>} />
          <Route path="test-fhir-consuming/:id?" element={<FHIRConsuming/>} />
          <Route path="test-fhir-producing/:id?" element={<FHIRProducing/>} />
          <Route path="test-fhir-message-producing/:id?" element={<FHIRMessageProducing/>} />
          <Route path="test-fhir-message-creation/:id?" element={<FHIRMessageCreator/>} />
          <Route path="test-edrs-roundtrip-consuming/:id?" element={<EDRSRoundtripConsuming/>} />
          <Route path="test-edrs-roundtrip-producing/:id?" element={<EDRSRoundtripProducing/>} />
          <Route path="test-fhir-ije-validator-producing" element={<FHIRIJEValidatorProducing/>} />
          <Route path="test-connectathon-dash/:type" element={<ConnectathonDashboard/>} />
          <Route path="test-connectathon/:id" element={<Connectathon/>} />
          <Route path="test-connectathon-messaging/:id" element={<MessageConnectathonProducing/>} />
          <Route path="tool-fhir-inspector" element={<FHIRInspector/>} />
          <Route path="tool-fhir-creator" element={<FHIRCreator/>} />
          <Route path="tool-fhir-syntax-checker" element={<FHIRSyntaxChecker/>} />
          <Route path="tool-fhir-message-syntax-checker" element={<FHIRMessageSyntaxChecker/>} />
          <Route path="tool-ije-inspector" element={<IJEInspector/>} />
          <Route path="tool-record-converter" element={<RecordConverter/>} />
          <Route path="tool-record-generator" element={<RecordGenerator/>} />
        </Routes>
      </Layout>
    );
  }
}

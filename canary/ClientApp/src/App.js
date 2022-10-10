import React, { Component } from 'react';
import { Routes, Route, useParams } from 'react-router-dom';
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
    const FHIRProducingParams = addParams(FHIRProducing)
    const FHIRConsumingParams = addParams(FHIRConsuming)
    const FHIRMessageProducingParams = addParams(FHIRMessageProducing)
    const FHIRMessageCreatorParams = addParams(FHIRMessageCreator)
    const EDRSRoundtripConsumingParams = addParams(EDRSRoundtripConsuming)
    const EDRSRoundtripProducingParams = addParams(EDRSRoundtripProducing)
    const ConnectathonDashboardParams = addParams(ConnectathonDashboard)
    const ConnectathonParams = addParams(Connectathon)
    const MessageConnectathonProducingParams = addParams(MessageConnectathonProducing)

    return (
      <Layout>
        <Routes>
          <Route exact path="/" element={<Dashboard/>} />
          <Route path="recent-tests" element={<RecentTests/>}/>
          <Route path="test-fhir-consuming">
            <Route index element={<FHIRConsumingParams />} />
            <Route path=":id" element={<FHIRConsumingParams />} />
          </Route>
          <Route path="test-fhir-producing">
            <Route index element={<FHIRProducingParams />} />
            <Route path=":id" element={<FHIRProducingParams />} />
          </Route>
          <Route path="test-fhir-message-producing">
            <Route index element={<FHIRMessageProducingParams />} />
            <Route path=":id" element={<FHIRMessageProducingParams />} />
          </Route>
          <Route path="test-fhir-message-creation">
            <Route index element={<FHIRMessageCreatorParams />} />
            <Route path=":id" element={<FHIRMessageCreatorParams />} />
          </Route>
          <Route path="test-edrs-roundtrip-consuming">
            <Route index element={<EDRSRoundtripConsumingParams />} />
            <Route path=":id" element={<EDRSRoundtripConsumingParams />} />
          </Route>
          <Route path="test-edrs-roundtrip-producing">
            <Route index element={<EDRSRoundtripProducingParams />} />
            <Route path=":id" element={<EDRSRoundtripProducingParams />} />
          </Route>
          <Route path="test-fhir-ije-validator-producing" element={<FHIRIJEValidatorProducing/>} />
          <Route path="test-connectathon-dash/:type" element={<ConnectathonDashboardParams/>} />
          <Route path="test-connectathon/:id" element={<ConnectathonParams/>} />
          <Route path="test-connectathon-messaging/:id" element={<MessageConnectathonProducingParams/>} />
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

/**
 * Adds parameters to the given component class type.
 * @param {*} WrappedComponent The component type to add prarams to.
 * @returns A new version of the component type with parameters.
 */
const addParams = WrappedComponent => () => {
  const params = useParams();
  return (
    <WrappedComponent
      params = {params}
    />
  );
};
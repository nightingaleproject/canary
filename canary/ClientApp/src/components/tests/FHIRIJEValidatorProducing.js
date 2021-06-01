import axios from 'axios';
import _ from 'lodash';
import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Breadcrumb, Button, Container, Divider, Grid, Header, Icon, Table, Form, Statistic } from 'semantic-ui-react';
import { connectionErrorToast } from '../../error';
import { toast } from 'react-semantic-toasts';
import { Getter } from '../misc/Getter';
import { FHIRInfo } from '../misc/info/FHIRInfo';
import { Record } from '../misc/Record';
import report from '../report';

export class FHIRIJEValidatorProducing extends Component {
  displayName = FHIRIJEValidatorProducing.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, test: null, fhirrecord: null, ijerecord: null, fhirInfo: null, fhirIssues:null, results: null, ijeIssues: [], column: 'number', direction: 'ascending' };
    this.runTest = this.runTest.bind(this);
    this.updateRecord = this.updateRecord.bind(this);
    this.updateFHIRRecord = this.updateFHIRRecord.bind(this);
  }

  createTest() {
    console.log("Fetch, set test");
    var self = this;
    this.setState({ loading: true }, () => {
      axios
        .post(window.API_URL + '/tests/validator', this.setEmptyToNull(this.state.ijerecord.fhirInfo))
        .then(function(response) {
          var test = response.data;
          test.results = JSON.parse(test.results);
          console.log("The test id: " + test.testId);
          self.setState({ test: test, fhirInfo: JSON.parse(response.data.referenceRecord.fhirInfo), loading: false }, () => {
            toast({
              type: 'success',
              icon: 'check',
              title: 'Success',
              description: 'The IJE file was successfully uploaded',
              time: 5000,
            });
          });
        })
        .catch(function(error) {
          self.setState({ loading: false }, () => {
            connectionErrorToast(error);
          });
        });
    });
  }

  setEmptyToNull(obj) {
    const o = JSON.parse(JSON.stringify(obj));
    Object.keys(o).forEach(key => {
      if (o[key] && typeof o[key] === 'object') o[key] = this.setEmptyToNull(o[key]);
      else if (o[key] === undefined || o[key] === null || (!!!o[key] && o['Type'] !== 'Bool')) o[key] = null;
      // eslint-disable-next-line
      else o[key] = o[key];
    });
    return o;
  }

  updateRecord(record, issues) {
    if (record && record.ijeInfo) {
      this.setState({ ijerecord: record, ijeIssues: [], results: record.ijeInfo }, () => this.createTest());
    } else if (issues && issues.length > 0) {
      this.setState({ ijerecord: null, ijeIssues: issues, results: null });
    }
  }

  updateFHIRRecord(record, issues) {
    if (record && record.fhirInfo) {
      // store fhir record
      this.setState({ fhirrecord: record, fhirIssues: [] });
    } else if (issues && issues.length > 0) {
      this.setState({ fhirrecord: null, fhirIssues: issues });
    }
  }

  runTest() {
    var self = this;

    this.setState({ running: true }, () => {
      axios
        .post(window.API_URL + '/tests/Produce/run/' + this.state.test.testId, this.setEmptyToNull(this.state.fhirrecord.fhirInfo))
        .then(function(response) {
          var test = response.data;
          test.results = JSON.parse(test.results);
          console.log(test.results);
          self.setState({ test: test, running: false }, () => {
            document.getElementById('scroll-to').scrollIntoView({ behavior: 'smooth', block: 'start' });
          });
        })
        .catch(function(error) {
          self.setState({ loading: false, running: false }, () => {
            connectionErrorToast(error);
          });
        });
    });
  }

  render() {
    return (
      <React.Fragment>
        <Grid>
          <Grid.Row id="scroll-to">
            <Breadcrumb>
              <Breadcrumb.Section as={Link} to="/">
                Dashboard
              </Breadcrumb.Section>
              <Breadcrumb.Divider icon="right chevron" />
              <Breadcrumb.Section>Validate FHIR Death Records (Producing)</Breadcrumb.Section>
            </Breadcrumb>
          </Grid.Row>
          {!!this.state.test && this.state.test.completedBool && (
            <Grid.Row className="loader-height">
              <Container>
                <div className="p-b-10" />
                <Statistic.Group widths="three">
                  <Statistic size="large">
                    <Statistic.Value>{this.state.test.total}</Statistic.Value>
                    <Statistic.Label>Properties Checked</Statistic.Label>
                  </Statistic>
                  <Statistic size="large" color="green">
                    <Statistic.Value>
                      <Icon name="check circle" />
                      {this.state.test.correct}
                    </Statistic.Value>
                    <Statistic.Label>Correct</Statistic.Label>
                  </Statistic>
                  <Statistic size="large" color="red">
                    <Statistic.Value>
                      <Icon name="times circle" />
                      {this.state.test.incorrect}
                    </Statistic.Value>
                    <Statistic.Label>Incorrect</Statistic.Label>
                  </Statistic>
                </Statistic.Group>
                <Grid centered columns={1} className="p-t-30 p-b-15">
                  <Button icon labelPosition='left' primary onClick={() => this.downloadAsFile(report(this.state.test, this.connectathonRecordName(this.props.match.params.id)))}><Icon name='download' />Generate Downloadable Report</Button>
                </Grid>
                <div className="p-b-20" />
                <Form size="large">
                  <FHIRInfo fhirInfo={this.state.test.results} editable={false} testMode={true} updateFhirInfo={null} />
                </Form>
              </Container>
            </Grid.Row>
          )}
          {!(!!this.state.test && this.state.test.completedBool) && (
            <React.Fragment>
              <Grid.Row>
                <Container fluid>
                  <Divider horizontal />
                  <Header as="h2" dividing id="step-1">
                    <Icon name="upload" />
                    <Header.Content>
                      Step 1: Upload IJE Record
                      <Header.Subheader>
                        Upload the IJE record you will use to validate the FHIR record. The below prompt allows you to paste the record, upload it as a file, or POST to Canary.
                      </Header.Subheader>
                    </Header.Content>
                  </Header>
                  <div className="p-b-15" />
                  <Getter updateRecord={this.updateRecord} ijeOnly />
                </Container>
              </Grid.Row>
              <div className="p-b-15" />
              {!!this.state.ijeIssues && this.state.ijeIssues.length > 0 && (
                <Grid.Row>
                  <Record record={null} issues={this.state.ijeIssues} showIssues />
                </Grid.Row>
              )}
              <Grid.Row>
                <Container fluid>
                  <Divider horizontal />
                  <Header as="h2" dividing id="step-2">
                    <Icon name="upload" />
                    <Header.Content>
                      Step 2: Upload FHIR Record
                      <Header.Subheader>Upload the FHIR record to validate. The below prompt allows you to paste the record, upload it as a file, or POST to Canary.</Header.Subheader>
                    </Header.Content>
                  </Header>
                  <div className="p-b-15" />
                  {!!this.state.fhirIssues && this.state.fhirIssues.length > 0 && (
                    <Grid.Row id="scroll-to-fhir">
                      <Record issues={this.state.fhirIssues} showIssues />
                    </Grid.Row>
                  )}
                  <Getter updateRecord={this.updateFHIRRecord} allowIje={false} />
                </Container>
              </Grid.Row>
              <Grid.Row>
                <Container fluid>
                  <Divider horizontal />
                  <Header as="h2" dividing className="p-b-5" id="step-3">
                    <Icon name="check circle" />
                    <Header.Content>
                      Step 3: Calculate Results
                      <Header.Subheader>
                        When you have imported both records, click the button below and Canary will calculate the results of the test.
                      </Header.Subheader>
                    </Header.Content>
                  </Header>
                  <div className="p-b-10" />
                  <Button
                    fluid
                    size="huge"
                    primary
                    onClick={this.runTest}
                    loading={this.state.running}
                    disabled={!!!(this.state.fhirrecord && this.state.fhirrecord.xml)}
                  >
                    Calculate
                  </Button>
                </Container>
              </Grid.Row>
            </React.Fragment>
          )}
        </Grid>
      </React.Fragment>
    );
  }
}

import axios from 'axios';
import _ from 'lodash';
import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Breadcrumb, Button, Container, Dimmer, Divider, Form, Grid, Header, Icon, Loader, Statistic } from 'semantic-ui-react';
import { connectionErrorToast } from '../../error';
import { FHIRInfo } from '../misc/info/FHIRInfo';
import { Record } from '../misc/Record';
import report from '../report';

export class FHIRConsuming extends Component {
  displayName = FHIRConsuming.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, test: null, loading: true, fhirInfo: null, running: false };
    this.updateTest = this.updateTest.bind(this);
    this.setEmptyToNull = this.setEmptyToNull.bind(this);
    this.updateFhirInfo = this.updateFhirInfo.bind(this);
    this.runTest = this.runTest.bind(this);
  }

  componentDidMount() {
    var self = this;
    if (!!this.props.params.id) {
      axios
        .get(window.API_URL + '/tests/' + this.props.params.id)
        .then(function(response) {
          var test = response.data;
          test.results = JSON.parse(test.results);
          self.setState({ test: test, loading: false });
        })
        .catch(function(error) {
          self.setState({ loading: false }, () => {
            connectionErrorToast(error);
          });
        });
    } else {
      axios
        .get(window.API_URL + '/records/description')
        .then(function(response) {
          self.setState({ fhirInfo: response.data }, () => {
            axios
              .get(window.API_URL + '/tests/new')
              .then(function(response) {
                self.setState({ test: response.data, loading: false });
              })
              .catch(function(error) {
                self.setState({ loading: false }, () => {
                  connectionErrorToast(error);
                });
              });
          });
        })
        .catch(function(error) {
          self.setState({ loading: false }, () => {
            connectionErrorToast(error);
          });
        });
    }
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

  updateFhirInfo(path, value) {
    var fhirInfo = { ...this.state.fhirInfo };
    _.set(fhirInfo, path, value);
    this.setState({ fhirInfo: fhirInfo });
  }

  updateTest(test) {
    this.setState({ test: test });
  }

  runTest() {
    var self = this;
    this.setState({ running: true }, () => {
      axios
        .post(window.API_URL + '/tests/Consume/run/' + this.state.test.testId, this.setEmptyToNull(this.state.fhirInfo))
        .then(function(response) {
          var test = response.data;
          test.results = JSON.parse(test.results);
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

  downloadAsFile(contents) {
    var element = document.createElement('a');
    element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(contents));
    element.setAttribute('download', `canary-report-${new Date().getTime()}.html`);
    element.click();
  }

  render() {
    return (
      <React.Fragment>
        <Grid id="scroll-to">
          <Grid.Row>
            <Breadcrumb>
              <Breadcrumb.Section as={Link} to="/">
                Dashboard
              </Breadcrumb.Section>
              <Breadcrumb.Divider icon="right chevron" />
              <Breadcrumb.Section>Consuming FHIR VRDR Records</Breadcrumb.Section>
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
                  <Button icon labelPosition='left' primary onClick={() => this.downloadAsFile(report(this.state.test, null))}><Icon name='download' />Generate Downloadable Report</Button>
                </Grid>
                <div className="p-b-20" />
                <Form size="large">
                  <FHIRInfo fhirInfo={this.state.test.results} editable={false} testMode={true} />
                </Form>
              </Container>
            </Grid.Row>
          )}
          {!(this.state.test && this.state.test.completedBool) && !!this.state.loading && (
            <Grid.Row className="loader-height">
              <Container>
                <Dimmer active inverted>
                  <Loader size="massive">Initializing a New Test...</Loader>
                </Dimmer>
              </Container>
            </Grid.Row>
          )}
          {!(this.state.test && this.state.test.completedBool) && !!this.state.test && (
            <React.Fragment>
              <Grid.Row>
                <Container>
                  <Divider horizontal />
                  <Header as="h2" dividing id="step-1">
                    <Icon name="download" />
                    <Header.Content>
                      Step 1: Import Record
                      <Header.Subheader>
                        Import the generated record into your system. The below prompt allows you to copy the record, download it as a file, or POST it to an
                        endpoint.
                      </Header.Subheader>
                    </Header.Content>
                  </Header>
                  <div className="p-b-15" />
                  <Record record={this.state.test.referenceRecord} showSave lines={20} hideIje />
                </Container>
              </Grid.Row>
              <Grid.Row>
                <Container fluid>
                  <Divider horizontal />
                  <Header as="h2" dividing id="step-1">
                    <Icon name="keyboard" />
                    <Header.Content>
                      Step 2: Enter Details
                      <Header.Subheader>
                        Once you have imported the record into your system, enter the details that show up in your system below. When you are finished, move on
                        to step 3.
                      </Header.Subheader>
                    </Header.Content>
                  </Header>
                  <div className="p-b-10" />
                  <Form size="large">
                    <FHIRInfo fhirInfo={this.state.fhirInfo} updateFhirInfo={this.updateFhirInfo} editable={true} hideSnippets />
                  </Form>
                </Container>
              </Grid.Row>
              <Grid.Row>
                <Container fluid>
                  <Divider horizontal />
                  <Header as="h2" dividing className="p-b-5" id="step-1">
                    <Icon name="check circle" />
                    <Header.Content>
                      Step 3: Calculate Results
                      <Header.Subheader>
                        When you are finished entering the details from your system, click the button below and Canary will calculate the results of the test.
                      </Header.Subheader>
                    </Header.Content>
                  </Header>
                  <div className="p-b-10" />
                  <Button fluid size="huge" primary onClick={this.runTest} loading={this.state.running}>
                    Calculate
                  </Button>
                </Container>
              </Grid.Row>
            </React.Fragment>
          )}
        </Grid>
        <div className="p-b-100" />
      </React.Fragment>
    );
  }
}

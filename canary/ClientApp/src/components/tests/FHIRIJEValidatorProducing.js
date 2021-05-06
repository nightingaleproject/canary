import axios from 'axios';
import _ from 'lodash';
import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Breadcrumb, Button, Container, Divider, Grid, Header, Icon, Table } from 'semantic-ui-react';
import { connectionErrorToast } from '../../error';
import { Getter } from '../misc/Getter';
import { Record } from '../misc/Record';

export class FHIRIJEValidatorProducing extends Component {
  displayName = FHIRIJEValidatorProducing.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, fhirrecord: null, ijerecord: null, fhirInfo: null, results: null, issues: [], column: 'number', direction: 'ascending' };
    this.updateTest = this.updateTest.bind(this);
    this.runTest = this.runTest.bind(this);
    this.updateRecord = this.updateRecord.bind(this);
  }

  componentDidMount() {
    var self = this;
    if (!!this.props.match.params.id) {
      axios
        .get(window.API_URL + '/tests/' + this.props.match.params.id)
        .then(function(response) {
          var test = response.data;
          test.results = JSON.parse(test.results);
          self.setState({ test: test, fhirInfo: JSON.parse(response.data.referenceRecord.fhirInfo), loading: false });
        })
        .catch(function(error) {
          self.setState({ loading: false }, () => {
            connectionErrorToast(error);
          });
        });
    } else {
      axios
        .get(window.API_URL + '/tests/new')
        .then(function(response) {
          self.setState({ test: response.data, fhirInfo: JSON.parse(response.data.referenceRecord.fhirInfo), loading: false });
        })
        .catch(function(error) {
          self.setState({ loading: false }, () => {
            connectionErrorToast(error);
          });
        });
    }
  }
  
  // fetchTest() {
  //   var self = this;
  //   if (!!this.props.match.params.id && !!this.ijerecord) {
  //     this.setState({ loading: true }, () => {
  //       self.setState({ fhirInfo: JSON.parse(this.ijerecord.fhirInfo), loading: false });
  //     }).catch(function(error) {
  //       self.setState({ loading: false }, () => {
  //         connectionErrorToast(error);
  //       });
  //     });
  //   }
  // }

  // updateFHIRInfo(_, data) {
  //   if (!!data && !!data.value) {
  //     console.log("updated fhir")
  //     this.setState({ certificateNumber: data.value }, () => this.fetchTest());
  //   }
  // }

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
      // store ije record
      console.log(record.fhirInfo);
      this.setState({ ijerecord: record, issues: [], results: record.ijeInfo }, () => {
        document.getElementById('scroll-to').scrollIntoView({ behavior: 'smooth', block: 'start' });
      });
    } else if (record) {
      // store fhir record
      this.setState({ fhirrecord: record, issues: [] });
    } else if (issues && issues.length > 0) {
      this.setState({ record: null, issues: issues, results: null });
    }
  }


  handleSort = clickedColumn => () => {
    const column = this.state.column;
    const results = this.state.results;
    const direction = this.state.direction;
    if (column !== clickedColumn) {
      this.setState({
        column: clickedColumn,
        results: _.sortBy(results, [clickedColumn]),
        direction: 'ascending',
      });
      return;
    }
    this.setState({
      results: results.reverse(),
      direction: direction === 'ascending' ? 'descending' : 'ascending',
    });
  };

  updateTest(test) {
    this.setState({ test: test });
  }

  runTest() {
    var self = this;
    this.setState({ running: true }, () => {
      axios
        .post(window.API_URL + '/tests/Produce/run/' + this.state.test.testId, this.setEmptyToNull(this.state.record.fhirInfo))
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

  render() {
    return (
      <React.Fragment>
        <Grid>
          <Grid.Row>
            <Breadcrumb>
              <Breadcrumb.Section as={Link} to="/">
                Dashboard
              </Breadcrumb.Section>
              <Breadcrumb.Divider icon="right chevron" />
              <Breadcrumb.Section>Validate FHIR Death Records (Producing)</Breadcrumb.Section>
            </Breadcrumb>
          </Grid.Row>
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
              <Getter updateRecord={this.updateRecord} ijeOnly noFormat />
            </Container>
          </Grid.Row>
          <div className="p-b-15" />
          {!!this.state.issues && this.state.issues.length > 0 && (
            <Grid.Row>
              <Record record={null} issues={this.state.issues} showIssues />
            </Grid.Row>
          )}
          {this.state.issues.length === 0 && !!this.state.ijerecord && !!this.state.results && (
            <Grid.Row>
              <Container>
                <Divider horizontal>
                  <Header as="h2">
                    <Icon name="search" />
                    IJE Mortality Record Details
                  </Header>
                </Divider>
              </Container>
              <Table celled striped sortable id="scroll-to">
                <Table.Header>
                  <Table.Row>
                    <Table.HeaderCell sorted={this.state.column === 'name' ? this.state.direction : null} onClick={this.handleSort('name')}>
                      Name
                    </Table.HeaderCell>
                    <Table.HeaderCell sorted={this.state.column === 'number' ? this.state.direction : null} onClick={this.handleSort('number')}>
                      Number
                    </Table.HeaderCell>
                    <Table.HeaderCell sorted={this.state.column === 'location' ? this.state.direction : null} onClick={this.handleSort('location')}>
                      Location
                    </Table.HeaderCell>
                    <Table.HeaderCell sorted={this.state.column === 'length' ? this.state.direction : null} onClick={this.handleSort('length')}>
                      Length
                    </Table.HeaderCell>
                    <Table.HeaderCell sorted={this.state.column === 'contents' ? this.state.direction : null} onClick={this.handleSort('contents')}>
                      Contents
                    </Table.HeaderCell>
                    <Table.HeaderCell sorted={this.state.column === 'value' ? this.state.direction : null} onClick={this.handleSort('value')}>
                      Value
                    </Table.HeaderCell>
                  </Table.Row>
                </Table.Header>
                <Table.Body>
                  {!!this.state.results &&
                    this.state.results.map(field => (
                      <Table.Row key={'row' + field.name}>
                        <Table.Cell>{field.name}</Table.Cell>
                        <Table.Cell>{field.number}</Table.Cell>
                        <Table.Cell>{field.location}</Table.Cell>
                        <Table.Cell>{field.length}</Table.Cell>
                        <Table.Cell>{field.contents}</Table.Cell>
                        <Table.Cell>{field.value}</Table.Cell>
                      </Table.Row>
                    ))}
                </Table.Body>
              </Table>
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
              {!!this.state.issues && this.state.issues.length > 0 && (
                <Grid.Row id="scroll-to-issues">
                  <Record issues={this.state.issues} showIssues />
                </Grid.Row>
              )}
              <Getter updateRecord={this.updateRecord} allowIje={false} />
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
        </Grid>
      </React.Fragment>
    );
  }
}

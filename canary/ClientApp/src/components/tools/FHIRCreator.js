import axios from 'axios';
import _ from 'lodash';
import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Breadcrumb, Button, Container, Form, Grid, Icon } from 'semantic-ui-react';
import { connectionErrorToast } from '../../error';
import { FHIRInfo } from '../misc/info/FHIRInfo';
import { Record } from '../misc/Record';

export class FHIRCreator extends Component {
  displayName = FHIRCreator.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, record: null, fhirInfo: null, issues: null };
    this.updateRecord = this.updateRecord.bind(this);
    this.setEmptyToNull = this.setEmptyToNull.bind(this);
    this.updateFhirInfo = this.updateFhirInfo.bind(this);
  }

  componentDidMount() {
    var self = this;
    axios
      .get(window.API_URL + '/records/description')
      .then(function(response) {
        self.setState({ fhirInfo: response.data });
      })
      .catch(function(error) {
        self.setState({ loading: false }, () => {
          connectionErrorToast(error);
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

  updateRecord() {
    var self = this;
    self.setState({ loading: true }, () => {
      axios
        .post(window.API_URL + '/records/description/new', this.setEmptyToNull(this.state.fhirInfo))
        .then(function(response) {
          self.setState({ loading: false, record: response.data }, () => {
            document.getElementById('scroll-to').scrollIntoView({ behavior: 'smooth', block: 'start' });
          });
        })
        .catch(function(error) {
          self.setState({ loading: false }, () => {
            connectionErrorToast(error);
          });
        });
    });
  }

  updateFhirInfo(path, value) {
    var fhirInfo = { ...this.state.fhirInfo };
    _.set(fhirInfo, path, value);
    this.setState({ fhirInfo: fhirInfo });
  }

  render() {
    return (
      <React.Fragment>
        <Grid.Row>
          <Breadcrumb>
            <Breadcrumb.Section as={Link} to="/">
              Dashboard
            </Breadcrumb.Section>
            <Breadcrumb.Divider icon="right chevron" />
            <Breadcrumb.Section>FHIR VRDR Record Creator</Breadcrumb.Section>
          </Breadcrumb>
        </Grid.Row>
        {!!this.state.fhirInfo && (
          <Grid.Row>
            <Container fluid>
              <Form size="large">
                <FHIRInfo fhirInfo={this.state.fhirInfo} updateFhirInfo={this.updateFhirInfo} editable={true} hideSnippets />
                <div className="p-b-10" />
                <Button primary floated="right" fluid size="huge" onClick={this.updateRecord} loading={this.state.loading}>
                  <Icon name="magic" />
                  Generate Record
                </Button>
              </Form>
            </Container>
            <div className="p-b-15" />
          </Grid.Row>
        )}
        {!!this.state.record && (
          <Grid.Row id="scroll-to">
            <Record record={this.state.record} showSave />
          </Grid.Row>
        )}
      </React.Fragment>
    );
  }
}

import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Breadcrumb, Button, Container, Dimmer, Divider, Dropdown, Input, Form, Grid, Header, Icon, Loader, Menu, Message, Statistic, Transition } from 'semantic-ui-react';
import { Getter } from '../misc/Getter';
import { FHIRInfo } from '../misc/info/FHIRInfo';
import { Record } from '../misc/Record';

export class MessageInspector extends Component {
  displayName = MessageInspector.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, record: null, fhirInfo: null, issues: null };
    this.updateRecord = this.updateRecord.bind(this);
  }

  updateRecord(record, issues) {
    if (record && record.fhirInfo) {
      this.setState({ record: null, fhirInfo: null, issues: null }, () => {
        this.setState({ record: record, fhirInfo: record.fhirInfo, issues: [] }, () => {
          document.getElementById('scroll-to').scrollIntoView({ behavior: 'smooth', block: 'start' });
        });
      })
    } else if (issues && issues.length > 0) {
      this.setState({ issues: issues, fhirInfo: null });
    }
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
              <Breadcrumb.Section>FHIR Message Inspector</Breadcrumb.Section>
            </Breadcrumb>
          </Grid.Row>
          <Grid.Row>
           <Getter updateRecord={this.updateRecord} strict allowIje={false} source={"MessageInspector"} />
          </Grid.Row>
                  {!!this.state.fhirInfo && (
                      <Grid.Row>
                          <Container fluid>
                              <Divider horizontal />
                              <Header as="h2" dividing id="step-2">
                                  <Icon name="download" />
                                  <Header.Content>
                                      Whole message content.  Select required format.
                                      <Header.Subheader>
                                          Enter or load the appropriate Connectathon test case data into your EDRS. If your EDRS allows data to be loaded in FHIR or IJE format, you can load the data from the below prompt.
                                      </Header.Subheader>
                                  </Header.Content>
                              </Header>
                              <div className="p-b-15" />
                              <Record record={this.state.record} showSave lines={20} showIje />
                          </Container>
                      </Grid.Row>
                  )}
          <div className="p-b-15" />
            {!!this.state.issues && this.state.issues.length > 0 && (
                <Grid.Row>
                    <Record record={null} issues={this.state.issues} showIssues />
                </Grid.Row>
          )}
          {!!this.state.fhirInfo && (
            <Grid.Row>
              <Container fluid>
                <Form size="large" id="scroll-to">
                  <FHIRInfo fhirInfo={this.state.fhirInfo} editable={false} />
                </Form>
              </Container>
              <div className="p-b-50" />
            </Grid.Row>
          )}
            </Grid>
      </React.Fragment>
      );
  }
}

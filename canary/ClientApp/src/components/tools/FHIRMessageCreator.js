import axios from 'axios';
import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Breadcrumb, Button, Container, Divider, Grid, Header, Icon, Menu, Message, Transition } from 'semantic-ui-react';
import { connectionErrorToast } from '../../error';
import { Getter } from '../misc/Getter';
import { Record } from '../misc/Record';
import { messageTypeIcons } from '../../data';

export class FHIRMessageCreator extends Component {
  displayName = FHIRMessageCreator.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, record: null, issues: null, messageType: null, running: false, message: null, messageIssues: null };
    this.updateRecord = this.updateRecord.bind(this);
    this.runTest = this.runTest.bind(this);
    this.setActiveMessageType = this.setActiveMessageType.bind(this);
  }

  componentDidMount() {
    document.getElementById('scroll-to').scrollIntoView({
      behavior: 'smooth',
      block: 'start'
    });
  }

  runTest() {
    var self = this;
    this.setState({ running: true }, () => {
      axios
        .post(window.API_URL + '/messages/create?type=' + this.state.messageType, this.state.record.json, { headers: { 'Content-Type': 'application/json' } })
        .then(function (response) {
            self.setState({
              message: response.data.item1,
              messageIssues: response.data.item2,
              running: false
            });
          })
          .catch(function (error) {
            self.setState({
              running: false
            }, () => {
              connectionErrorToast(error);
            });
          });
    });
  }

  updateRecord(record, issues) {
    this.setState({ record: record, issues: issues });
  }

  setActiveMessageType(_, { name }) {
    this.setState({ messageType: name });
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
              <Breadcrumb.Section>Creating FHIR VRDR Messages</Breadcrumb.Section>
            </Breadcrumb>
          </Grid.Row>
          <Grid.Row>
            <Container fluid>
              <Divider horizontal />
              <Header as="h2" dividing id="step-1">
                <Icon name="mail" />
                <Header.Content>
                  Step 1: Choose the Message Type
                  <Header.Subheader>Select the type of message you would like Canary to generate.</Header.Subheader>
                </Header.Content>
              </Header>
              <Menu items={messageTypeIcons} widths={messageTypeIcons.length} onItemClick={this.setActiveMessageType} />
            </Container>
          </Grid.Row>
          <Grid.Row>
            {!!this.state.messageType && (
              <div className="inherit-width">
                <Transition transitionOnMount animation="fade" duration={1000}>
                  <div className="inherit-width">
                    <Message icon size="large" info>
                      <Icon name="info circle" />
                      <Message.Content>Canary will generate a {this.state.messageType} Message</Message.Content>
                    </Message>
                  </div>
                </Transition>
              </div>
            )}
          </Grid.Row>
          <Grid.Row>
            <Container fluid>
              <Divider horizontal />
              <Header as="h2" dividing id="step-2">
                <Icon name="upload" />
                <Header.Content>
                  Step 2: Upload a Record
                  <Header.Subheader>
                    Provide a valid record to Canary. The below prompt allows you to copy the record, download it as a file, or POST it to an endpoint.
                  </Header.Subheader>
                </Header.Content>
              </Header>
              <Getter updateRecord={this.updateRecord} strict allowIje={false} />
            </Container>
          </Grid.Row>
          <div className="p-b-15" />
          {!!this.state.issues && (
            <Grid.Row>
              <Record record={null} issues={this.state.issues} showIssues showSuccess />
            </Grid.Row>
          )}
          <Grid.Row>
            <Container fluid>
              <Divider horizontal />
              <Header as="h2" dividing id="step-3">
                <Icon name="check circle" />
                <Header.Content>
                  Step 3: Generate Results
                  <Header.Subheader>
                    When you have imported the FHIR record into Canary, click the button below and Canary will generate a {this.state.messageType} message below.
                  </Header.Subheader>
                </Header.Content>
              </Header>
              <div className="p-b-10" />
              <Button fluid size="huge" primary onClick={this.runTest} loading={this.state.running} disabled={!!!this.state.record || !!!this.state.messageType || !!!this.state.issues}>
                Generate
              </Button>
            </Container>
          </Grid.Row>
          <Grid.Row>
            <Record record={this.state.message} issues={this.state.messageIssues} showSave hideIje showIssues />
          </Grid.Row>
        </Grid>
      </React.Fragment>
    );
  }
}

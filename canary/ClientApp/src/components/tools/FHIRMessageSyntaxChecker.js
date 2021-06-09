import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Breadcrumb, Grid } from 'semantic-ui-react';
import { messageTypes } from '../../data';
import { Getter } from '../misc/Getter';
import { Record } from '../misc/Record';

export class FHIRMessageSyntaxChecker extends Component {
  displayName = FHIRMessageSyntaxChecker.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, messageType: null, issues: null };
    this.updateMessage = this.updateMessage.bind(this);
  }

  updateMessage(message, issues) {
    let messageType = "Unknown";
    if (message && message.messageType in messageTypes) {
      messageType = messageTypes[message.messageType];
    }

    this.setState({ message: message, messageType: messageType, issues: issues });
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
              <Breadcrumb.Section>FHIR VRDR Message Syntax Checker</Breadcrumb.Section>
            </Breadcrumb>
          </Grid.Row>
          <Grid.Row>
            <Getter updateRecord={this.updateMessage} strict messageValidation={true} allowIje={false} />
          </Grid.Row>
          <div className="p-b-15" />
          {!!this.state.issues && (
            <Grid.Row>
              <Record record={null} issues={this.state.issues} messageType={this.state.messageType} messageValidation={true} showIssues showSuccess />
            </Grid.Row>
          )}
        </Grid>
      </React.Fragment>
    );
  }
}

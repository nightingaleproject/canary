import React, { Component } from 'react';
import { Record } from '../misc/Record';
import { Getter } from '../misc/Getter';
import { messageTypes } from '../../data';
import { Grid, Breadcrumb } from 'semantic-ui-react';
import { Link } from 'react-router-dom';

export class FHIRMessageValidator extends Component {
  displayName = FHIRMessageValidator.name;

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
              <Breadcrumb.Section>Validate FHIR VRDR Messages</Breadcrumb.Section>
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

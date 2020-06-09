import React, { Component } from 'react';
import { Record } from '../misc/Record';
import { Getter } from '../misc/Getter';
import { Grid, Breadcrumb } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import _ from 'lodash';

export class FHIRMessageValidator extends Component {
  displayName = FHIRMessageValidator.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, messageType: null, issues: null };
    this.updateRecord = this.updateRecord.bind(this);
  }

  updateRecord(message, issues) {
    let messageType;
    switch (message && message.messageType) {
      case "http://nchs.cdc.gov/vrdr_submission":
        messageType = "Death Record Submission"
        break;
      case "http://nchs.cdc.gov/vrdr_submission_update":
        messageType = "Death Record Update"
        break;
      case "http://nchs.cdc.gov/vrdr_acknowledgement":
        messageType = "Death Record Acknowledgement"
        break;
      case "http://nchs.cdc.gov/vrdr_submission_void":
        messageType = "Death Record Void"
        break;
      case "http://nchs.cdc.gov/vrdr_coding":
        messageType = "Coding"
        break;
      case "http://nchs.cdc.gov/vrdr_coding_update":
        messageType = "Coding Update"
        break;
      case "http://nchs.cdc.gov/vrdr_extraction_error":
        messageType = "Extraction Error"
        break;
      default:
        messageType = "Unknown"
        break;
    }

    this.setState({ record: message, messageType: messageType, issues: issues });
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
            <Getter updateRecord={this.updateRecord} strict messageValidation={true} allowIje={false} />
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

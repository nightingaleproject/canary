import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Breadcrumb, Grid } from 'semantic-ui-react';
import { Getter } from '../misc/Getter';
import { Record } from '../misc/Record';

export class FHIRSyntaxChecker extends Component {
  displayName = FHIRSyntaxChecker.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, issues: null };
    this.updateRecord = this.updateRecord.bind(this);
  }

  updateRecord(record, issues) {
    this.setState({ issues: issues });
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
              <Breadcrumb.Section>FHIR VRDR Record Syntax Checker</Breadcrumb.Section>
            </Breadcrumb>
          </Grid.Row>
          <Grid.Row>
            <Getter updateRecord={this.updateRecord} strict allowIje={false} />
          </Grid.Row>
          <div className="p-b-15" />
          {!!this.state.issues && (
            <Grid.Row>
              <Record record={null} issues={this.state.issues} showIssues showSuccess />
            </Grid.Row>
          )}
        </Grid>
      </React.Fragment>
    );
  }
}

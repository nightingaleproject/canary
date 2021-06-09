import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Breadcrumb, Grid } from 'semantic-ui-react';
import { Getter } from '../misc/Getter';
import { Record } from '../misc/Record';

export class RecordConverter extends Component {
  displayName = RecordConverter.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, record: null, issues: [] };
    this.updateRecord = this.updateRecord.bind(this);
  }

  updateRecord(record, issues) {
    this.setState({ record: record, issues: issues }, () => {
      document.getElementById('scroll-to').scrollIntoView({ behavior: 'smooth', block: 'start' });
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
              <Breadcrumb.Section>VRDR Record Format Converter</Breadcrumb.Section>
            </Breadcrumb>
          </Grid.Row>
          <Grid.Row>
            <Getter updateRecord={this.updateRecord} allowIje />
          </Grid.Row>
          <div className="p-b-15" />
          {(!!this.state.record || (!!this.state.issues && this.state.issues.length > 0)) && (
            <Grid.Row id="scroll-to">
              <Record record={this.state.record} issues={this.state.issues} showIssues showSave />
            </Grid.Row>
          )}
        </Grid>
      </React.Fragment>
    );
  }
}

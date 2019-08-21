import React, { Component } from 'react';
import { FHIRInfo } from '../misc/info/FHIRInfo';
import { Getter } from '../misc/Getter';
import { Record } from '../misc/Record';
import { Grid, Breadcrumb, Container, Form } from 'semantic-ui-react';
import { Link } from 'react-router-dom';

export class FHIRInspector extends Component {
  displayName = FHIRInspector.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, record: null, fhirInfo: null, issues: null };
    this.updateRecord = this.updateRecord.bind(this);
  }

  updateRecord(record, issues) {
    if (record && record.fhirInfo) {
      this.setState({ record: null, fhirInfo: null, issues: null }, () => {
        this.setState({ record: record, fhirInfo: record.fhirInfo, issues: issues }, () => {
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
              <Breadcrumb.Section>FHIR Death Record Inspector</Breadcrumb.Section>
            </Breadcrumb>
          </Grid.Row>
          <Grid.Row>
            <Getter updateRecord={this.updateRecord} strict allowIje={false} />
          </Grid.Row>
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

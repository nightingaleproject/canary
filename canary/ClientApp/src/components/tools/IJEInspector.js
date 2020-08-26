import _ from 'lodash';
import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Breadcrumb, Container, Divider, Grid, Header, Icon, Table } from 'semantic-ui-react';
import { Getter } from '../misc/Getter';
import { Record } from '../misc/Record';

export class IJEInspector extends Component {
  displayName = IJEInspector.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, record: null, results: null, issues: [], column: 'number', direction: 'ascending' };
    this.updateRecord = this.updateRecord.bind(this);
  }

  updateRecord(record, issues) {
    if (record && record.ijeInfo) {
      this.setState({ record: record, issues: [], results: record.ijeInfo }, () => {
        document.getElementById('scroll-to').scrollIntoView({ behavior: 'smooth', block: 'start' });
      });
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
              <Breadcrumb.Section>IJE Mortality Record Inspector</Breadcrumb.Section>
            </Breadcrumb>
          </Grid.Row>
          <Grid.Row>
            <Getter updateRecord={this.updateRecord} ijeOnly noFormat />
          </Grid.Row>
          <div className="p-b-15" />
          {!!this.state.issues && this.state.issues.length > 0 && (
            <Grid.Row>
              <Record record={null} issues={this.state.issues} showIssues />
            </Grid.Row>
          )}
          {this.state.issues.length === 0 && !!this.state.record && !!this.state.results && (
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
        </Grid>
      </React.Fragment>
    );
  }
}

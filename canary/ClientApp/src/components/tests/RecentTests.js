import axios from 'axios';
import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Dimmer, Loader, Table } from 'semantic-ui-react';
import { connectionErrorToast } from '../../error';

export class RecentTests extends Component {
  displayName = RecentTests.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, tests: [], loading: true };
    this.typeToLink = this.typeToLink.bind(this);
  }

  componentDidMount() {
    var self = this;
    axios
      .get(window.API_URL + '/tests')
      .then(function(response) {
        self.setState({ tests: response.data, loading: false });
      })
      .catch(function(error) {
        self.setState({ loading: false }, () => {
          connectionErrorToast(error);
        });
      });
  }

  typeToLink(type) {
    if (type === 'RoundtripProducing') {
      return 'test-edrs-roundtrip-producing';
    } else if (type === 'RoundtripConsuming') {
      return 'test-edrs-roundtrip-consuming';
    } else if (type === 'Produce') {
      return 'test-fhir-producing';
    } else if (type === 'Consume') {
      return 'test-fhir-consuming';
    }
  }

  render() {
    return (
      <React.Fragment>
        <Table selectable>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>Id</Table.HeaderCell>
              <Table.HeaderCell>Started</Table.HeaderCell>
              <Table.HeaderCell>Type</Table.HeaderCell>
              <Table.HeaderCell>Completed</Table.HeaderCell>
              <Table.HeaderCell>Score</Table.HeaderCell>
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {this.state.tests.map(test => {
              return (
                <Table.Row key={test.testId + 'row'}>
                  <Table.Cell>
                    <Link to={!!test.type ? this.typeToLink(test.type) + '/' + test.testId : ''}>{test.testId}</Link>
                  </Table.Cell>
                  <Table.Cell>{test.created}</Table.Cell>
                  <Table.Cell>{!!test.type ? test.type : ''}</Table.Cell>
                  <Table.Cell>{!!test.completedDateTime ? test.completedDateTime : ''}</Table.Cell>
                  <Table.Cell>{!!test.correct ? test.correct + '/' + test.total : ''}</Table.Cell>
                </Table.Row>
              );
            })}
            {this.state.tests.length === 0 && (
              <Dimmer active inverted>
                <Loader size="massive">Loading...</Loader>
              </Dimmer>
            )}
          </Table.Body>
        </Table>
      </React.Fragment>
    );
  }
}

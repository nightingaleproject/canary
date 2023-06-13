import React, { Component } from 'react';
import { Container, Divider, Grid, Header, Icon, Item } from 'semantic-ui-react';
import { DashboardItem } from './DashboardItem';
import axios from 'axios';
import { connectionErrorToast } from '../../error';


export class ConnectathonDashboard extends Component {
  displayName = ConnectathonDashboard.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, records: null, loading: false };
    this.fetchRecords()
  }

  fetchRecords() {
    var self = this;
    axios
      .get(window.API_URL + '/records/connectathon')
      .then(function (response) {
        var records = response.data;
        self.setState({ records: records, loading: false });
      })
      .catch(function (error) {
        self.setState({ loading: false }, () => {
          connectionErrorToast(error);
        });
      });
  };

  getUrl(id) {
    if (this.props.params.type === "message") {
      return `/test-connectathon-messaging/${id}`
    }
    else {
      return `/test-connectathon/${id}`
    }
  }

  render() {
    return (
      <React.Fragment>
        <Grid centered columns={1}>
          <Grid.Column width={15}>
            <Container className="p-b-50">
              <Divider horizontal>
                <Header as="h2">
                  <Icon name="clipboard list" />
                  Connectathon {this.props.params.type} testing
                </Header>
              </Divider>
              <Item.Group className="m-h-30">
                {
                  !!this.state.records && this.state.records.map((x, i) =>
                    <DashboardItem
                      icon={x['sexAtDeath']['code'] || 'male'}
                      title={`#${i + 1}: ${[x['familyName'], ...x['givenNames']].join(' ')}`}
                      description={`${x['coD1A']}`}
                      route={this.getUrl(i + 1)}
                    />
                  )
                }
              </Item.Group>
            </Container>
          </Grid.Column>
        </Grid>
      </React.Fragment>
    );
  }
}

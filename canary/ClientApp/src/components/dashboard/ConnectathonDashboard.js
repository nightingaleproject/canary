import React, { Component } from 'react';
import { Container, Divider, Grid, Header, Icon, Item } from 'semantic-ui-react';
import { DashboardItem } from './DashboardItem';

export class ConnectathonDashboard extends Component {
  displayName = ConnectathonDashboard.name;

  getUrl(id) {
    if(this.props.match.params.type === "message")
    {
      return `/test-connectathon-messaging/${id}`
    }
    else
    {
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
                  IHE Connectathon 2020 {this.props.match.params.type} testing
                </Header>
              </Divider>
              <Item.Group className="m-h-30">
                <DashboardItem
                  icon="male"
                  title="Cancer"
                  description="#111111; Janet Page; Congestive Heart Failure"
                  route={ this.getUrl(1) }
                />
                <DashboardItem
                  icon="male"
                  title="Opioid Death at Home"
                  description="#222222; Madelyn Patel; Cocaine toxicity"
                  route={ this.getUrl(2) }
                />
                <DashboardItem
                  icon="male"
                  title="Pregnant"
                  description="#333333; Vivienne Lee Wright; Cardiopulmonary arrest"
                  route={ this.getUrl(3) }
                />
                <DashboardItem
                  icon="male"
                  title="Car accident at work: Full"
                  description="#444444; Javier Luis Perez; Blunt head trama"
                  route={ this.getUrl(4) }
                />
                <DashboardItem
                  icon="male"
                  title="Car accident at work: Partial"
                  description="#444444; Javier Luis Perez; Blunt head trama"
                  route={ this.getUrl(5) }
                />
              </Item.Group>
            </Container>
          </Grid.Column>
        </Grid>
      </React.Fragment>
    );
  }
}

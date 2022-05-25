import React, { Component } from 'react';
import { Container, Divider, Grid, Header, Icon, Item } from 'semantic-ui-react';
import { DashboardItem } from './DashboardItem';
import { connectathonRecordNames, connectathonRecordCertificateNumbers } from '../../data';

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
                  Connectathon {this.props.match.params.type} testing
                </Header>
              </Divider>
              <Item.Group className="m-h-30">
                <DashboardItem
                  icon="male"
                  title={connectathonRecordNames[1]}
                  description={`#${connectathonRecordCertificateNumbers[1]}; Twila Hilty; Cardiopulmonary arrest`}
                  route={ this.getUrl(1) }
                />
                <DashboardItem
                  icon="male"
                  title={connectathonRecordNames[2]}
                  description={`#${connectathonRecordCertificateNumbers[2]}; Fidelia Alsup; Blunt head trauma`}
                  route={ this.getUrl(2) }
                />
                <DashboardItem
                  icon="male"
                  title={connectathonRecordNames[3]}
                  description={`#${connectathonRecordCertificateNumbers[3]}; Davis Lineberry; Pending`}
                  route={ this.getUrl(3) }
                />
              </Item.Group>
            </Container>
          </Grid.Column>
        </Grid>
      </React.Fragment>
    );
  }
}

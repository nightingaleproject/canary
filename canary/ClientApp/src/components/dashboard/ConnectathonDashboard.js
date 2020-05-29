import React, { Component } from 'react';
import { Header, Divider, Icon, Item, Container, Grid } from 'semantic-ui-react';
import { DashboardItem } from './DashboardItem';
import { DownloadItem } from './DownloadItem';

export class ConnectathonDashboard extends Component {
  displayName = ConnectathonDashboard.name;

  render() {
    return (
      <React.Fragment>
        <Grid centered columns={1}>
          <Grid.Column width={15}>
            <Container className="p-b-50">
              <Divider horizontal>
                <Header as="h2">
                  <Icon name="clipboard list" />
                  IHE Connectathon 2020
                </Header>
              </Divider>
              <Item.Group className="m-h-30">
                <Divider horizontal>
                  <Header as='h3'>
                    <Icon name='file archive outline' />
                    Download Test Files
                  </Header>
                </Divider>
                <DashboardItem
                  icon="download"
                  title="Download FHIR R4 Test Files"
                  description="Download the FHIR R4 version of all the test files below.  These files should be used to test current and future VRDR systems."
                  route="#" // stay on current page
                  downloadFile="FHIR_R4_test_files.zip"
                />
                <DashboardItem
                  icon="download"
                  title="Download FHIR STU3 Test Files"
                  description="Download the FHIR STU3 test files that were used for integration testing during the January 2020 Connectathon.  These are included here only for regression testing purposes.  You should use the R4 versions above for current work."
                  route="#" // stay on current page
                  downloadFile="FHIR_STU3_test_files.zip"
                />
                <Divider horizontal>
                  <Header as='h3'>
                    <Icon name='clipboard outline' />
                    Work with each test page
                  </Header>
                </Divider>
                <DashboardItem
                  icon="male"
                  title="Cancer"
                  description="#111111; Janet Page; Congestive Heart Failure"
                  route="test-connectathon/1"
                />
                <DashboardItem
                  icon="male"
                  title="Opioid Death at Home"
                  description="#222222; Madelyn Patel; Cocaine toxicity"
                  route="test-connectathon/2"
                />
                <DashboardItem
                  icon="male"
                  title="Pregnant"
                  description="#333333; Vivienne Lee Wright; Cardiopulmonary arrest"
                  route="test-connectathon/3"
                />
                <DashboardItem
                  icon="male"
                  title="Car accident at work: Full"
                  description="#444444; Javier Luis Perez; Blunt head trama"
                  route="test-connectathon/4"
                />
                <DashboardItem
                  icon="male"
                  title="Car accident at work: Partial"
                  description="#444444; Javier Luis Perez; Blunt head trama"
                  route="test-connectathon/5"
                />
              </Item.Group>
            </Container>
          </Grid.Column>
        </Grid>
      </React.Fragment>
    );
  }
}

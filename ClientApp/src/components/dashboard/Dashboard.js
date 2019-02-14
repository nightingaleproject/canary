import React, { Component } from 'react';
import { Header, Divider, Icon, Item, Container, Grid, Segment } from 'semantic-ui-react';
import { DashboardItem } from './DashboardItem';

export class Dashboard extends Component {
  displayName = Dashboard.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, activeItem: 'Testing' };
    this.handleItemClick = this.handleItemClick.bind(this);
  }

  handleItemClick = (e, { name }) => this.setState({ activeItem: name });

  render() {
    return (
      <React.Fragment>
        <Grid centered columns={1}>
          <Grid.Column width={15}>
            <Container className="p-b-50">
              <Divider horizontal>
                <Header as="h2">
                  <Icon name="fire" />
                  Open Source FHIR Mortality Testing
                </Header>
              </Divider>
              <Segment size="huge" basic>
                <Container>
                  <p>
                    Canary is a testing framework that supports development of systems that perform standards based exchange of mortality data, providing tests
                    and tools to aid developers in implementing the FHIR death record format.
                  </p>

                  <p>
                    Canary is developed by&nbsp;
                    <a className="underline" href="http://www.mitre.org">
                      The MITRE Corporation
                    </a>
                    &nbsp;/&nbsp;
                    <a className="underline" href="https://www.mitre.org/centers/cms-alliances-to-modernize-healthcare/who-we-are">
                      the Health FFRDC
                    </a>
                    .
                  </p>
                </Container>
              </Segment>
              <Divider horizontal className="p-t-30">
                <Header as="h2">
                  <Icon name="clipboard list" />
                  Testing
                </Header>
              </Divider>
              <Item.Group className="m-h-30">
                <DashboardItem
                  icon="upload"
                  title="Producing FHIR Death Records"
                  description="Test a data provider system's ability to produce a valid FHIR Death Record document."
                  route="test-fhir-producing"
                />
                <DashboardItem
                  icon="download"
                  title="Consuming FHIR Death Records"
                  description="Test a data provider system's ability to consume a valid FHIR Death Record document."
                  route="test-fhir-consuming"
                />
                <DashboardItem
                  icon="sync"
                  title="Death Record Roundtrip (Consuming)"
                  description="Test a data provider system's ability to handle converting between internal data structures and external data formats. This test involves consuming FHIR, and then producing equivalent IJE."
                  route="test-edrs-roundtrip-consuming"
                />
                <DashboardItem
                  icon="sync"
                  title="Death Record Roundtrip (Producing)"
                  description="Test a data provider system's ability to handle converting between internal data structures and external data formats. This test involves consuming IJE, and then producing equivalent FHIR."
                  route="test-edrs-roundtrip-producing"
                />
              </Item.Group>
              <Divider horizontal className="p-t-20">
                <Header as="h2">
                  <Icon name="wrench" />
                  Tools
                </Header>
              </Divider>
              <Item.Group className="m-h-30">
                <DashboardItem
                  icon="clipboard list"
                  title="Generate Synthetic Death Records"
                  description="Generate synthetic death records in FHIR (XML or JSON) and IJE Mortality format. These generated records can be downloaded locally, copied to the clipboard, or POSTed to an endpoint."
                  route="tool-record-generator"
                />
                <DashboardItem
                  icon="clipboard check"
                  title="Validate FHIR Records"
                  description="Check a given FHIR Record for syntax issues."
                  route="tool-fhir-validator"
                />
                <DashboardItem
                  icon="random"
                  title="Death Record Format Converter"
                  description="Convert between various Death Record formats, including FHIR and IJE Mortality."
                  route="tool-record-converter"
                />
                <DashboardItem
                  icon="find"
                  title="FHIR Death Record Inspector"
                  description="Inspect a FHIR Death Record and show details about the record and what it contains."
                  route="tool-fhir-inspector"
                />
                <DashboardItem
                  icon="magic"
                  title="FHIR Death Record Creator"
                  description="Create a new record from scratch by filling out a web form, and generate FHIR from it."
                  route="tool-fhir-creator"
                />
                <DashboardItem
                  icon="search"
                  title="IJE Mortality Record Inspector"
                  description="Inspect an IJE Mortality file and show details about the record and what it contains."
                  route="tool-ije-inspector"
                />
              </Item.Group>
            </Container>
          </Grid.Column>
        </Grid>
      </React.Fragment>
    );
  }
}

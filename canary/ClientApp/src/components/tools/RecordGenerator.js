import axios from 'axios';
import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Breadcrumb, Button, Form, Grid, Icon, Select } from 'semantic-ui-react';
import { stateOptions } from '../../data';
import { connectionErrorToast } from '../../error';
import { Record } from '../misc/Record';

import 'react-semantic-toasts/styles/react-semantic-alert.css';

export class RecordGenerator extends Component {
  displayName = RecordGenerator.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, record: null, loading: false, activeState: 'MA', activeType: 'Natural', activeSex: 'Male' };
    this.generateRecord = this.generateRecord.bind(this);
    this.changeState = this.changeState.bind(this);
    this.changeSex = this.changeSex.bind(this);
    this.changeType = this.changeType.bind(this);
  }

  changeState(event, data) {
    if (data.value !== this.state.activeState) {
      this.setState({ activeState: data.value }, () => {
        this.generateRecord(); // Re-generate record since the state changed
      });
    }
  }

  changeSex(event, data) {
    if (data.children !== this.state.activeSex) {
      this.setState({ activeSex: data.children }, () => {
        this.generateRecord(); // Re-generate record since the sex changed
      });
    }
  }

  changeType(event, data) {
    const type = data.children.replace(/ .*/, ''); // Strip out fancy button descriptions (use only "Natural" or "Injury")
    if (type !== this.state.activeType) {
      this.setState({ activeType: type }, () => {
        this.generateRecord(); // Re-generate record since the type changed
      });
    }
  }

  componentDidMount() {
    this.generateRecord();
  }

  generateRecord() {
    var self = this;
    this.setState({ loading: true }, () => {
      axios
        .get(window.API_URL + '/records/new?state=' + self.state.activeState + '&type=' + self.state.activeType + '&sex=' + self.state.activeSex)
        .then(function(response) {
          if (response.data) {
            self.setState({
              record: response.data,
              loading: false,
            });
          }
        })
        .catch(function(error) {
          connectionErrorToast(error);
          self.setState({
            loading: false,
          });
        });
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
              <Breadcrumb.Section>Generate Synthetic VRDR Records</Breadcrumb.Section>
            </Breadcrumb>
          </Grid.Row>
          <Grid.Row className="no-padding-b">
            <Form>
              <Form.Group inline>
                <Form.Field>
                  <Button icon primary labelPosition="left" floated="left" onClick={this.generateRecord} loading={this.state.loading}>
                    <Icon name="refresh" />
                    New Record
                  </Button>
                </Form.Field>
                <Form.Field>
                  <Button.Group>
                    <Button primary={this.state.activeSex === 'Male'} active={this.state.activeSex === 'Male'} onClick={this.changeSex}>
                      Male
                    </Button>
                    <Button.Or />
                    <Button primary={this.state.activeSex === 'Female'} active={this.state.activeSex === 'Female'} onClick={this.changeSex}>
                      Female
                    </Button>
                  </Button.Group>
                </Form.Field>
                <Form.Field>
                  <Button.Group>
                    <Button primary={this.state.activeType === 'Natural'} active={this.state.activeType === 'Natural'} onClick={this.changeType}>
                      Natural Death
                    </Button>
                    <Button.Or />
                    <Button primary={this.state.activeType === 'Injury'} active={this.state.activeType === 'Injury'} onClick={this.changeType}>
                      Injury or Poisoning
                    </Button>
                  </Button.Group>
                </Form.Field>
                <Form.Field
                  control={Select}
                  options={stateOptions}
                  search
                  searchInput={{ id: 'form-select-control-state' }}
                  floated="left"
                  value={this.state.activeState}
                  onChange={this.changeState}
                />
              </Form.Group>
            </Form>
          </Grid.Row>
          <Grid.Row>
            <Record record={this.state.record} showSave />
          </Grid.Row>
        </Grid>
      </React.Fragment>
    );
  }
}

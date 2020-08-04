import React, { Component } from 'react';
import { Button, Form, Header } from 'semantic-ui-react';

export class BoolType extends Component {
  displayName = BoolType.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props };
    this.updateValue = this.updateValue.bind(this);
  }

  updateValue(event, data) {
    if (!!this.props.editable) {
      const value = data.children === 'True';
      this.setState({ value: value }, () => {
        if (value) {
          this.props.updateProperty('Value', this.state.value);
        }
      });
    }
  }

  render() {
    return (
      <React.Fragment>
        <Form.Field>
          {!!!this.props.igurl && <Header as='h5'><a target="_blank" rel="noopener noreferrer" href={this.props.igurl}>{this.props.igurl}</a></Header>}
          <label>{this.props.description}</label>
          <div className="p-t-10" />
          <Button.Group>
            <Button primary={!!this.state.value} active={!!this.state.value} onClick={this.updateValue}>
              True
            </Button>
            <Button.Or />
            <Button primary={!!!this.state.value} active={!!!this.state.value} onClick={this.updateValue}>
              False
            </Button>
          </Button.Group>
        </Form.Field>
      </React.Fragment>
    );
  }
}

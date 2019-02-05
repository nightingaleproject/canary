import React, { Component } from 'react';
import { Form, Button } from 'semantic-ui-react';

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

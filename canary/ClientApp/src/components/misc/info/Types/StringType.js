import React, { Component } from 'react';
import { Form, Header, Input } from 'semantic-ui-react';

export class StringType extends Component {
  displayName = StringType.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props };
    this.updateValue = this.updateValue.bind(this);
  }

  updateValue(event, data) {
    if (!!this.props.editable) {
      if (event.type === 'change') {
        const value = data.value;
        this.setState({ value: value });
      } else if (event.type === 'blur') {
        this.props.updateProperty('Value', this.state.value);
      }
    }
  }

  render() {
    return (
      <React.Fragment>
        <Form.Field>
          {!!!this.props.igurl && <Header as='h5'><a target="_blank" rel="noopener noreferrer" href={this.props.igurl}>{this.props.igurl}</a></Header>}
          <Form.Field
            label={this.props.description}
            control={Input}
            error={this.props.error}
            value={this.state.value || ''}
            onChange={this.updateValue}
            onBlur={this.updateValue}
          />
        </Form.Field>
      </React.Fragment>
    );
  }
}

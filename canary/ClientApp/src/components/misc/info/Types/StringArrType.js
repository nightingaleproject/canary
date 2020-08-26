import React, { Component } from 'react';
import { Button, Form, Header, Input } from 'semantic-ui-react';

export class StringArrType extends Component {
  displayName = StringArrType.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props };
    this.updateValue = this.updateValue.bind(this);
    this.addRow = this.addRow.bind(this);
    this.deleteRow = this.deleteRow.bind(this);
  }

  componentDidMount() {
    if (!!!this.state.testMode && this.state.value && this.state.value.length === 0) {
      this.setState({ value: [''] });
    }
  }

  addRow() {
    this.setState({ value: [...this.state.value, ''] });
  }

  deleteRow() {
    var minusVal = [...this.state.value];
    minusVal.pop();
    this.setState({ value: minusVal });
  }

  updateValue(event, data, index) {
    if (!!this.props.editable) {
      if (event.type === 'change') {
        const setValue = data.value;
        var value = [...this.state.value];
        value[index] = setValue;
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
          <label>{this.props.description}</label>
          <Form.Group widths="equal">
            {this.state.value &&
              this.state.value.length > 0 &&
              this.state.value.map((value, index) => {
                return (
                  <Form.Field
                    key={index + 'str'}
                    control={Input}
                    value={value || ''}
                    error={this.props.error}
                    onChange={(event, data) => {
                      this.updateValue(event, data, index);
                    }}
                    onBlur={(event, data) => {
                      this.updateValue(event, data, index);
                    }}
                  />
                );
              })}
          </Form.Group>
        </Form.Field>
        {!!this.props.editable && (
          <Button.Group floated="right">
            <Button icon="minus" onClick={this.deleteRow} disabled={this.state.value && this.state.value.length < 2} />
            <Button primary icon="plus" onClick={this.addRow} />
          </Button.Group>
        )}
      </React.Fragment>
    );
  }
}

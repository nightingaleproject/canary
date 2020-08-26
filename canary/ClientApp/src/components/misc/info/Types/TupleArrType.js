import React, { Component } from 'react';
import { Button, Form, Header, Input } from 'semantic-ui-react';

export class TupleArrType extends Component {
  displayName = TupleArrType.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props };
    this.updateValue = this.updateValue.bind(this);
    this.addRow = this.addRow.bind(this);
    this.deleteRow = this.deleteRow.bind(this);
  }

  componentDidMount() {
    if (!!!this.state.testMode && this.state.value && this.state.value.length === 0) {
      var newRow = {};
      newRow['Item1'] = '';
      newRow['Item2'] = '';
      this.setState({ value: [newRow] });
    }
  }

  addRow() {
    var newRow = {};
    newRow['Item1'] = '';
    newRow['Item2'] = '';
    this.setState({ value: [...this.state.value, newRow] });
  }

  deleteRow() {
    var minusVal = [...this.state.value];
    minusVal.pop();
    this.setState({ value: minusVal });
  }

  updateValue(event, data, index, num) {
    if (!!this.props.editable) {
      if (event.type === 'change') {
        const setValue = data.value;
        var value = [...this.state.value];
        if (num === 1) {
          value[index].Item1 = setValue;
        } else if (num === 2) {
          value[index].Item2 = setValue;
        }
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
          {this.state.value &&
            this.state.value.length > 0 &&
            this.state.value.map((value, index) => {
              return (
                <Form.Group key={index + 'cgrp'}>
                  <Form.Field
                    label="Display"
                    width={8}
                    key={index + 'strc'}
                    control={Input}
                    value={value.Item1 || ''}
                    onChange={(event, data) => {
                      this.updateValue(event, data, index, 1);
                    }}
                    onBlur={(event, data) => {
                      this.updateValue(event, data, index, 1);
                    }}
                    error={this.props.error}
                  />
                  <Form.Field
                    label="Code"
                    width={8}
                    key={index + 'stri'}
                    control={Input}
                    value={value.Item2 || ''}
                    onChange={(event, data) => {
                      this.updateValue(event, data, index, 2);
                    }}
                    onBlur={(event, data) => {
                      this.updateValue(event, data, index, 2);
                    }}
                    error={this.props.error}
                  />
                </Form.Group>
              );
            })}
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

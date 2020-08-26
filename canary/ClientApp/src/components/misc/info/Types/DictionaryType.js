import _ from 'lodash';
import React, { Component } from 'react';
import { Form, Header, Input } from 'semantic-ui-react';

export class DictionaryType extends Component {
  displayName = DictionaryType.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props };
    this.updateValue = this.updateValue.bind(this);
  }

  updateValue(key, event, data) {
    if (!!this.props.editable) {
      if (event.type === 'change') {
        const setValue = data.value;
        var value = { ...this.state.value };
        _.set(value, key + '.Value', setValue);
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
          <h4>{this.props.description}</h4>
          {!!this.props.value &&
            Object.keys(this.props.value).map(name => {
              return (
                <Form.Field
                  key={name}
                  label={this.state.value[name].Description.charAt(0).toUpperCase() + this.state.value[name].Description.substr(1)}
                  control={Input}
                  value={this.state.value[name].Value || ''}
                  onChange={(event, data) => {
                    this.updateValue(name, event, data);
                  }}
                  onBlur={(event, data) => {
                    this.updateValue(name, event, data);
                  }}
                  error={this.state.value[name] && this.state.value[name]['Match'] && this.state.value[name].Match === 'false'}
                />
              );
            })}
        </Form.Field>
      </React.Fragment>
    );
  }
}

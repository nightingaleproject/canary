import moment from 'moment';
import React, { Component } from 'react';
import { DateTimeInput } from 'semantic-ui-calendar-react';
import { Form, Header, Input } from 'semantic-ui-react';

moment.locale('en');

export class StringDateTimeType extends Component {
  displayName = StringDateTimeType.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props };
    this.updateValue = this.updateValue.bind(this);
  }

  componentDidMount() {
    if (!!!this.state.value) {
      this.setState({ value: moment.now() });
    }
  }

  updateValue(event, data) {
    if (!!this.props.editable) {
      const value = data.value;
      this.setState({ value: value }, () => {
        if (value) {
          this.props.updateProperty('Value', moment(this.state.value).format('YYYY-MM-DDTHH:mm:ss.SSSSSSSZ'));
        }
      });
    }
  }

  render() {
    return (
      <React.Fragment>
        <Form.Field>
          {!!!this.props.igurl && <Header as='h5'><a target="_blank" rel="noopener noreferrer" href={this.props.igurl}>{this.props.igurl}</a></Header>}
          {!!this.props.editable && (<DateTimeInput
            name="dateTime"
            placeholder="Date and Time"
            value={moment(this.state.value).format('LLL')}
            iconPosition="left"
            onChange={this.updateValue}
            dateTimeFormat="LLL"
            label={this.props.description}
            error={this.props.error}
          />)}
          {!!!this.props.editable && (<Form.Field
            label={this.props.description}
            control={Input}
            error={this.props.error}
            value={this.state.value || ''}
            onChange={this.updateValue}
            onBlur={this.updateValue}
          />)}
        </Form.Field>
      </React.Fragment>
    );
  }
}

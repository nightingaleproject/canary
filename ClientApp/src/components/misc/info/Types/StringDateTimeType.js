import React, { Component } from 'react';
import { Form, Header } from 'semantic-ui-react';
import moment from 'moment';
import { DateTimeInput } from 'semantic-ui-calendar-react';

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
          <Header as='h5'><a target="_blank" rel="noopener noreferrer" href={this.props.igurl}>{this.props.igurl}</a></Header>
          <DateTimeInput
            name="dateTime"
            placeholder="Date and Time"
            value={moment(this.state.value).format('LLL')}
            iconPosition="left"
            onChange={this.updateValue}
            dateTimeFormat="LLL"
            label={this.props.description}
            error={this.props.error}
          />
        </Form.Field>
      </React.Fragment>
    );
  }
}

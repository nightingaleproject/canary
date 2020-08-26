import React, { Component } from 'react';
import { Accordion } from 'semantic-ui-react';
import { Snippet } from '../Snippet';

export class DeathRecordType extends Component {
  displayName = DeathRecordType.name;

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
        <Accordion styled fluid exclusive={false}>
          <Snippet
            name="XML"
            snippet={this.props.snippetXML}
            snippetTest={this.props.snippetXMLTest}
            lines={this.props.lines}
            testMode={this.props.testMode}
          />
          <Snippet
            name="JSON"
            snippet={this.props.snippetJSON}
            snippetTest={this.props.snippetJSONTest}
            lines={this.props.lines}
            testMode={this.props.testMode}
          />
        </Accordion>
      </React.Fragment>
    );
  }
}

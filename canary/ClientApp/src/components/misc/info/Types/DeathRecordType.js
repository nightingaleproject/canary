import React, { Component } from 'react';
import { Accordion, Icon } from 'semantic-ui-react';
import { FHIRInfo } from '../../info/FHIRInfo';

export class DeathRecordType extends Component {
  displayName = DeathRecordType.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, visible: false };
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
        <Accordion styled fluid>
        <Accordion.Title
          active={this.state.visible}
          onClick={() => {
            this.setState({ visible: !this.state.visible });
          }}
        >
          <Icon name="dropdown" />
          Death Record Inspector
        </Accordion.Title>
        <Accordion.Content active={this.state.visible}>
          <FHIRInfo fhirInfo={JSON.parse(this.state.snippetJSON)} hideSnippets={false} editable={false} testMode={true} />
        </Accordion.Content>
        </Accordion>
      </React.Fragment>
    );
  }
}

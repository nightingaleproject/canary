import React, { Component } from 'react';
import { Category } from './Category';

export class FHIRInfo extends Component {
  displayName = FHIRInfo.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props };
  }

  render() {
    return (
      <React.Fragment>
        {!!this.props.fhirInfo &&
          Object.keys(this.props.fhirInfo).map(name => {
            return (
              <Category
                key={name}
                name={name}
                category={this.props.fhirInfo[name]}
                updateFhirInfo={this.props.updateFhirInfo}
                editable={this.props.editable}
                hideSnippets={this.props.hideSnippets}
                testMode={this.props.testMode}
              />
            );
          })}
      </React.Fragment>
    );
  }
}

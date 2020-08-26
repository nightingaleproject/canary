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
        {!!this.props.fhirInfo && <React.Fragment>
          {Object.keys(this.props.fhirInfo).map((category) => (
            <Category
              key={category.replace(/ /g, '')}
              name={category}
              category={this.props.fhirInfo[category]}
              updateFhirInfo={this.props.updateFhirInfo}
              editable={this.props.editable}
              hideSnippets={this.props.hideSnippets}
              testMode={this.props.testMode}
              hideBlanks={this.props.hideBlanks}
            />
          ))}
          </React.Fragment>
        }
      </React.Fragment>
    );
  }
}

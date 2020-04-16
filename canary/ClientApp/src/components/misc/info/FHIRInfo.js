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
          (<React.Fragment><Category
            key={"DecedentDemographics"}
            name={"Decedent Demographics"}
            category={this.props.fhirInfo["Decedent Demographics"]}
            updateFhirInfo={this.props.updateFhirInfo}
            editable={this.props.editable}
            hideSnippets={this.props.hideSnippets}
            testMode={this.props.testMode}
            hideBlanks={this.props.hideBlanks}
          />
          <Category
            key={"DecedentDisposition"}
            name={"Decedent Disposition"}
            category={this.props.fhirInfo["Decedent Disposition"]}
            updateFhirInfo={this.props.updateFhirInfo}
            editable={this.props.editable}
            hideSnippets={this.props.hideSnippets}
            testMode={this.props.testMode}
            hideBlanks={this.props.hideBlanks}
          />
          <Category
            key={"DeathCertification"}
            name={"Death Certification"}
            category={this.props.fhirInfo["Death Certification"]}
            updateFhirInfo={this.props.updateFhirInfo}
            editable={this.props.editable}
            hideSnippets={this.props.hideSnippets}
            testMode={this.props.testMode}
            hideBlanks={this.props.hideBlanks}
          />
          <Category
            key={"DeathInvestigation"}
            name={"Death Investigation"}
            category={this.props.fhirInfo["Death Investigation"]}
            updateFhirInfo={this.props.updateFhirInfo}
            editable={this.props.editable}
            hideSnippets={this.props.hideSnippets}
            testMode={this.props.testMode}
            hideBlanks={this.props.hideBlanks}
          /></React.Fragment>)
        }
      </React.Fragment>
    );
  }
}

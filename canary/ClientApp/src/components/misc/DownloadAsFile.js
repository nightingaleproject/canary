import React, { Component } from 'react';
import { Button, Icon } from 'semantic-ui-react';
import report from '../report';

export class DownloadAsFile extends Component {
  displayName = DownloadAsFile.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props };
  }

  downloadAsFile(contents, type = 'html') {
    var ext = 'html';
    var encoding = 'text/html;charset=utf-8';

    if (type === 'json') {
      ext = 'json';
      encoding = 'application/json;charset=utf-8';
    }

    var element = document.createElement('a');
    element.setAttribute('href', `data:${encoding},` + encodeURIComponent(contents));
    element.setAttribute('download', `canary-report-${this.connectathonRecordName(this.props.params.id).toLowerCase()}-${new Date().getTime()}.${ext}`);
    element.click();
  }

  render() {
    return (
      <React.Fragment>
        <Button icon labelPosition='left' primary onClick={() => this.downloadAsFile(report(this.state.test, this.connectathonRecordName(this.props.params.id)))}><Icon name='download' />Generate Downloadable Report</Button>
      </React.Fragment>
    );
  }
}

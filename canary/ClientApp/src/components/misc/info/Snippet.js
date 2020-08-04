import React, { Component } from 'react';
import AceEditor from 'react-ace';
import { Accordion, Icon } from 'semantic-ui-react';

import 'ace-builds/src-noconflict/mode-json';
import 'ace-builds/src-noconflict/mode-xml';
import 'ace-builds/src-noconflict/theme-chrome';

export class Snippet extends Component {
  displayName = Snippet.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, visible: false };
  }

  render() {
    return (
      <React.Fragment>
        <Accordion.Title
          active={this.state.visible}
          onClick={() => {
            this.setState({ visible: !this.state.visible });
          }}
        >
          <Icon name="dropdown" />
          {this.props.name}
        </Accordion.Title>
        <Accordion.Content active={this.state.visible}>
          {!!this.props.testMode && <h4>Expected:</h4>}
          <AceEditor
            mode = {this.state.name.toLowerCase()}
            theme="chrome"
            name = {"record-" + this.props.name + "-b"}
            fontSize={12}
            showGutter={true}
            highlightActiveLine={true}
            showPrintMargin={false}
            value={this.props.snippet}
            width="100%"
            readOnly={true}
            maxLines={this.props.lines || Infinity}
            setOptions={{ useWorker: false }}
          />
          {!!this.props.testMode && (
            <React.Fragment>
              <h4>How Canary interpreted your input:</h4>
              <AceEditor
                mode = {this.state.name.toLowerCase()}
                theme="chrome"
                name = {"record-" + this.props.name + "-b"}
                fontSize={12}
                showGutter={true}
                highlightActiveLine={true}
                showPrintMargin={false}
                value={this.props.snippetTest}
                width="100%"
                readOnly={true}
                maxLines={this.props.lines || Infinity}
                setOptions={{ useWorker: false }}
              />
            </React.Fragment>
          )}
        </Accordion.Content>
      </React.Fragment>
    );
  }
}

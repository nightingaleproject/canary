import axios from 'axios';
import React, { Component } from 'react';
import AceEditor from 'react-ace';
import { toast } from 'react-semantic-toasts';
import 'react-semantic-toasts/styles/react-semantic-alert.css';
import { Button, Form, Icon, Menu, Message, Modal, Popup, Segment, Transition } from 'semantic-ui-react';
import { Issues } from '../misc/Issues';

import 'ace-builds/src-noconflict/mode-json';
import 'ace-builds/src-noconflict/mode-xml';
import 'ace-builds/src-noconflict/theme-chrome';
// import 'ace-builds/webpack-resolver';

export class Record extends Component {
  displayName = Record.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, activeItem: 'JSON', modalOpen: false, endpoint: 'http://localhost:3000/fhir/v1/death_records.json', sending: false };
    this.downloadAsFile = this.downloadAsFile.bind(this);
    this.copyToClipboard = this.copyToClipboard.bind(this);
    this.handleEndpointChange = this.handleEndpointChange.bind(this);
    this.postRecord = this.postRecord.bind(this);
  }

  componentDidMount() {
    if (!!this.props.ijeOnly) {
      this.setState({ activeItem: 'IJE' });
    }
  }

  handleItemClick = (e, { name }) => this.setState({ activeItem: name });

  formatXml(xml, spaces) {
    const PADDING = ' '.repeat(spaces);
    const reg = /(>)(<)(\/*)/g;
    let pad = 0;
    xml = xml.replace(reg, '$1\r\n$2$3');
    return xml
      .split('\r\n')
      .map((node, index) => {
        let indent = 0;
        if (node.match(/.+<\/\w[^>]*>$/)) {
          indent = 0;
        } else if (node.match(/^<\/\w/) && pad > 0) {
          pad -= 1;
        } else if (node.match(/^<\w[^>]*[^/]>.*$/)) {
          indent = 1;
        } else {
          indent = 0;
        }
        pad += indent;
        return PADDING.repeat(pad - indent) + node;
      })
      .join('\r\n');
  }

  formatJson(json, spaces) {
    return JSON.stringify(JSON.parse(json), null, spaces);
  }

  formatIje(ije) {
    return ije.match(/.{1,140}/g).join('\n');
  }

  downloadAsFile() {
    var element = document.createElement('a');
    var file;
    if (this.state.activeItem === 'JSON') {
      file = new Blob([this.formatJson(this.props.record.json, 2)], { type: 'application/json' });
      element.download = `record-${Date.now().toString()}.json`;
    }
    if (this.state.activeItem === 'XML') {
      file = new Blob([this.formatXml(this.props.record.xml, 2)], { type: 'application/xml' });
      element.download = `record-${Date.now().toString()}.xml`;
    }
    if (this.state.activeItem === 'IJE') {
      file = new Blob([this.props.record.ije.replace(/(\r\n|\n|\r)/gm, '').substr(0, 5000)], { type: 'text/plain' });
      element.download = `record-${Date.now().toString()}.MOR`;
    }
    element.href = URL.createObjectURL(file);
    element.click();
  }

  copyToClipboard() {
    var element = document.createElement('textarea');
    if (this.state.activeItem === 'JSON') {
      element.value = this.formatJson(this.props.record.json, 2);
    }
    if (this.state.activeItem === 'XML') {
      element.value = this.formatXml(this.props.record.xml, 2);
    }
    if (this.state.activeItem === 'IJE') {
      element.value = this.props.record.ije.replace(/(\r\n|\n|\r)/gm, '').substr(0, 5000);
    }
    document.body.appendChild(element);
    element.select();
    document.execCommand('copy');
    document.body.removeChild(element);
  }

  postRecord() {
    var type;
    var content;
    if (this.state.activeItem === 'JSON') {
      type = 'application/json';
      content = this.props.record.json;
    }
    if (this.state.activeItem === 'XML') {
      type = 'application/xml';
      content = this.props.record.xml;
    }
    if (this.state.activeItem === 'IJE') {
      type = 'plain/text';
      content = this.props.record.ije.replace(/(\r\n|\n|\r)/gm, '').substr(0, 5000);
    }
    var self = this;
    this.setState({ sending: true }, () => {
      axios
        .post(this.state.endpoint, content, { headers: { 'Content-Type': type } })
        .then(function(response) {
          self.setState(
            {
              modalOpen: false,
              sending: false,
            },
            () => {
              toast({
                type: 'success',
                icon: 'check circle',
                title: 'Success!',
                description:
                  'The record was successfully POSTed to: "' +
                  self.state.endpoint,
                time: 5000,
              });
            }
          );
        })
        .catch(function(error) {
          self.setState(
            {
              modalOpen: false,
              sending: false,
            },
            () => {
              toast({
                type: 'error',
                icon: 'exclamation circle',
                title: 'Error!',
                description: 'There was an error POSTing the record. The error was: "' + error + '"',
                time: 5000,
              });
            }
          );
        });
    });
  }

  handleEndpointChange(event, data) {
    this.setState({ endpoint: event.target.value });
  }

  render() {
    return (
      <React.Fragment>
        <Modal dimmer="blurring" open={this.state.modalOpen} onClose={this.closeModal}>
          <Modal.Header>POST this Death Record to an Endpoint</Modal.Header>
          <Modal.Content image>
            <Modal.Description>
              <Form>
                <Form.Field>
                  <label>Endpoint URL</label>
                  <input value={this.state.endpoint} onChange={this.handleEndpointChange} />
                </Form.Field>
              </Form>
            </Modal.Description>
          </Modal.Content>
          <Modal.Actions>
            <Button
              negative
              icon="cancel"
              labelPosition="left"
              content="Cancel"
              onClick={() => {
                this.setState({ modalOpen: false });
              }}
            />
            <Button positive icon="send" labelPosition="left" content="Submit" onClick={this.postRecord} loading={this.state.sending} />
          </Modal.Actions>
        </Modal>
        {!!this.props.showIssues && (
          <Issues issues={this.props.issues} />
        )}
        {!!this.props.issues && this.props.issues.length === 0 && !!this.props.showSuccess && (
          <div className="inherit-width">
            <Transition transitionOnMount animation="fade" duration={1000}>
              <div className="inherit-width">
                <Message icon size="large" positive>
                  <Icon name="check circle" />
                  {!!this.props.messageValidation && (
                    <Message.Content>Valid {this.props.messageType} message found!</Message.Content>
                  )}
                  {!!!this.props.messageValidation && (<Message.Content>No issues were found!</Message.Content>)}
                </Message>
              </div>
            </Transition>
          </div>
        )}
        {!!this.props.record && !!this.props.record.xml && !!this.props.record.json && (!!this.props.hideIje || !!this.props.record.ije) && (
          <React.Fragment>
            <Menu attached="top" pointing size="large">
              {!!!this.props.ijeOnly && (
                <React.Fragment>
                  <Menu.Item name="JSON" active={this.state.activeItem === 'JSON'} onClick={this.handleItemClick} />
                  <Menu.Item name="XML" active={this.state.activeItem === 'XML'} onClick={this.handleItemClick} />
                </React.Fragment>
              )}
              {!!!this.props.hideIje && <Menu.Item name="IJE" active={this.state.activeItem === 'IJE'} onClick={this.handleItemClick} />}
              {!!this.props.showSave && (
                <Menu.Menu position="right">
                  <Popup trigger={<Menu.Item icon="download" onClick={this.downloadAsFile} />} content="Download as File" />
                  <Popup trigger={<Menu.Item icon="clipboard" onClick={this.copyToClipboard} />} content="Copy to Clipboard" />
                  <Popup
                    trigger={
                      <Menu.Item
                        icon="send"
                        onClick={() => {
                          this.setState({ modalOpen: true });
                        }}
                      />
                    }
                    content="POST to Endpoint"
                  />
                </Menu.Menu>
              )}
            </Menu>
            <Segment className="no-padding inherit-width full-height">
              {this.state.activeItem === 'XML' && (
                <AceEditor
                  mode="xml"
                  theme="chrome"
                  name="record-xml"
                  fontSize={12}
                  showGutter={true}
                  highlightActiveLine={true}
                  showPrintMargin={false}
                  value={this.props.record ? this.formatXml(this.props.record.xml, 2) : ''}
                  width="100%"
                  readOnly={true}
                  maxLines={this.props.lines || Infinity}
                />
              )}
              {this.state.activeItem === 'JSON' && (
                <AceEditor
                  mode="json"
                  theme="chrome"
                  name="record-json"
                  fontSize={12}
                  showGutter={true}
                  highlightActiveLine={true}
                  showPrintMargin={false}
                  value={this.props.record ? this.formatJson(this.props.record.json, 2) : ''}
                  width="100%"
                  readOnly={true}
                  maxLines={this.props.lines || Infinity}
                />
              )}
              {this.state.activeItem === 'IJE' && (
                <AceEditor
                  theme="chrome"
                  name="record-ije"
                  fontSize={12}
                  showGutter={true}
                  highlightActiveLine={true}
                  showPrintMargin={false}
                  value={this.props.record ? this.formatIje(this.props.record.ije) : ''}
                  width="100%"
                  readOnly={true}
                  maxLines={this.props.lines || Infinity}
                  tabSize={0}
                />
              )}
            </Segment>
          </React.Fragment>
        )}
      </React.Fragment>
    );
  }
}

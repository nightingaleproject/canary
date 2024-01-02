import axios from 'axios';
import _ from 'lodash';
import React, { Component } from 'react';
import AceEditor from 'react-ace';
import { toast } from 'react-semantic-toasts';
import { Button, Container, Dimmer, Form, Header, Icon, Label, Loader, Segment } from 'semantic-ui-react';
import { connectionErrorToast } from '../../error';

import 'ace-builds/src-noconflict/mode-text';
import 'ace-builds/src-noconflict/theme-chrome';

export class Getter extends Component {
  displayName = Getter.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, activeItem: 'paste', pasteText: '', loading: false, endpoint: null, endpointId: null, checking: false, waiting: false };
    this.pasteChange = this.pasteChange.bind(this);
    this.submitPaste = this.submitPaste.bind(this);
    this.tryFormat = this.tryFormat.bind(this);
    this.onChangeFile = this.onChangeFile.bind(this);
    this.newEndpoint = this.newEndpoint.bind(this);
  }

  tryFormat() {
    try {
      if (this.state.pasteText.trim().startsWith('{')) {
        this.setState({ pasteText: this.formatJson(this.state.pasteText, 2) });
      } else if (this.state.pasteText.trim().startsWith('<')) {
        this.setState({ pasteText: this.formatXml(this.state.pasteText, 2) });
      } else if (this.state.pasteText.length === 5000) {
        this.setState({ pasteText: this.formatIje(this.state.pasteText) });
      }
    } catch (err) {}
  }

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

  pasteChange(data) {
    this.setState({ pasteText: data });
  }

  submitPaste() {
    var self = this;
    self.props.updateRecord(this.state.pasteText, null);
      this.setState({ loading: true }, () => {
        // When POSTing, be clever about if this component was mounted with a "ijeOnly", and detect any leading "<" and "{"
        // that would cause the back end to think this was JSON or XML. This allows us to use a more general back end route
        // to handle all record types. TODO: Should probably migrate away from an unnecessary back end call here if we know
        // there is bad data.
        var data = self.state.pasteText;
        if (!!this.props.ijeOnly) {
            data.replace(/(\r\n|\n|\r)/gm, ''); // Strip any line breaks if IJE!
        }
        if (!!this.props.ijeOnly && (data[0] === '<' || data[0] === '{')) {
            data = 'bogus'; // The IJE catch in the back end will not like this, and will thus throw an error.
        }
        var endpoint = '';
        if (this.props.returnType) {
            endpoint = '/records/return/new';
        } else if (this.props.messageValidation) {
            endpoint = '/messages/new'
        } else if (this.props.source == 'MessageInspector') {
            endpoint = '/messages/inspect';
        } else {
            endpoint = '/records/new';
        } 

       axios
        .post(window.API_URL + endpoint + (!!this.props.strict ? '?strict=yes' : '?strict=no'), data)
        .then(function(response) {
          self.setState({ loading: false }, () => {
            var record = response.data.item1;
            if (record && record.fhirInfo) {
              record.fhirInfo = JSON.parse(record.fhirInfo);
            }
            self.props.updateRecord(record, response.data.item2);
          });
        })
        .catch(function(error) {
          self.setState({ loading: false }, () => {
            connectionErrorToast(error);
          });
        });
    });
  }

  newEndpoint() {
    var self = this;
    axios
      .get(window.API_URL + '/endpoints/new')
      .then(function(response) {
        self.setState(
          {
            endpoint: window.location.protocol + '//' + window.location.host + '/endpoints/record/' + response.data,
            endpointId: response.data,
            checking: true,
          },
          () => {
            var timerID = setInterval(() => {
              if (!self.state.checking) {
                clearInterval(timerID);
              } else if (!self.state.waiting) {
                self.setState({ waiting: true }, () => {
                  axios
                    .get(window.API_URL + '/endpoints/' + self.state.endpointId)
                    .then(function(response) {
                      if (response.data && response.data.finished) {
                        self.setState({ checking: false, waiting: false }, () => {
                          var record = null;
                          if (response.data && response.data.record && _.isString(response.data.record)) {
                            record = JSON.parse(record);
                          } else if (response.data && response.data.record && _.isObject(response.data.record)) {
                            record = response.data.record;
                          }
                          if (record && record.fhirInfo) {
                            record.fhirInfo = JSON.parse(record.fhirInfo);
                          }
                          self.props.updateRecord(record, response.data.issues);
                        });
                      } else {
                        self.setState({ waiting: false });
                      }
                    })
                    .catch(function(error) {
                      self.setState({ checking: false, waiting: false }, () => {
                        console.error('Error checking endpoint: ' + JSON.stringify(error));
                      });
                    });
                });
              }
            }, 3000);
          }
        );
      })
      .catch(function(error) {
        self.setState({ loading: false }, () => {
          connectionErrorToast(error);
        });
      });
  }

  onChangeFile(event) {
    var self = this;
    var reader = new FileReader();
    reader.onload = function(event) {
      self.setState({ pasteText: event.target.result }, () => {
        self.submitPaste();
      });
    };
    reader.onerror = function(event) {
      self.setState({ loading: false }, () => {
        toast({
          type: 'error',
          icon: 'exclamation circle',
          title: 'Error!',
          description: 'There was an error reading the chosen file.',
          time: 5000,
        });
      });
    };
    reader.readAsText(event.target.files[0]);
  }

  render() {
    let containerTip;
    if (!!!this.props.ijeOnly) {
      if (!!this.props.allowIje) {
        containerTip = 'The contents can be formatted as FHIR XML, FHIR JSON, or IJE Mortality.'
      } else {
        containerTip = 'The contents can be formatted as FHIR XML or FHIR JSON.'
      }
    } else {
      containerTip = 'The contents must be formatted as an IJE Mortality record.'
    }
    return (
      <React.Fragment>
        <Form className="p-t-10">
          <Form.Field>
            <Button.Group>
              <Button
                primary={this.state.activeItem === 'paste'}
                onClick={() => {
                  this.setState({ activeItem: 'paste', checking: false });
                }}
              >
                <Icon name="clipboard" />
                Copy &amp; Paste
              </Button>
              <Button.Or />
              <Button
                primary={this.state.activeItem === 'upload'}
                onClick={() => {
                  this.setState({ activeItem: 'upload', checking: false });
                }}
              >
                <Icon name="upload" />
                Upload
              </Button>
              <Button.Or />
              <Button
                primary={this.state.activeItem === 'post'}
                onClick={() => {
                  this.setState({ activeItem: 'post', checking: false }, () => {
                    this.newEndpoint();
                  });
                }}
              >
                <Icon name="send" />
                POST to Canary
              </Button>
            </Button.Group>
            <Label pointing="left" basic className="m-l-15">
              How would you like to import the record into Canary?
            </Label>
          </Form.Field>
        </Form>
        {this.state.activeItem === 'paste' && (
          <Segment>
            <Dimmer inverted active={this.state.loading}>
              <Loader size="big">Processing</Loader>
            </Dimmer>
            <Container>
              <Form>
                <h4>Paste the {!!this.props.messageValidation ? 'message' : 'record'} contents below. {containerTip}</h4>
                <AceEditor
                  mode="text"
                  theme="chrome"
                  name="paste-input"
                  fontSize={12}
                  showGutter={true}
                  highlightActiveLine={true}
                  showPrintMargin={false}
                  value={this.state.pasteText}
                  onChange={this.pasteChange}
                  width="100%"
                  height="400px"
                />
                {!!!this.state.noFormat && (
                  <Button primary floated="left" className="m-t-10" onClick={this.tryFormat}>
                    <Icon name="code" />
                    Format Input
                  </Button>
                )}
                <Button primary floated="right" className="m-t-10" onClick={this.submitPaste} disabled={!!!this.state.pasteText}>
                  <Icon name="send" />
                  Submit
                </Button>
              </Form>
            </Container>
          </Segment>
        )}
        {this.state.activeItem === 'upload' && (
          <Segment>
            <Container>
              <Button as="label" fluid size="big" htmlFor={`upload-btn-${this.props.id ? this.props.id : 'default'}`} icon labelPosition="left" loading={this.state.loading} primary>
                <Icon name="file" />
                Select the {this.props.messageValidation ? 'message' : 'record'} file you wish to upload. {containerTip}
              </Button>
              <input hidden id={`upload-btn-${this.props.id ? this.props.id : 'default'}`} type="file" onChange={this.onChangeFile} />
            </Container>
          </Segment>
        )}
        {this.state.activeItem === 'post' && (
          <Segment>
            <Container textAlign="center" className="p-t-20 p-b-10">
              <Header as="h1" icon>
                <Icon name="sync" loading={this.state.checking} circular />
                <span>{this.state.endpoint}</span>
                <Header.Subheader className="p-t-10">
                  POST your {this.props.messageValidation ? 'message' : 'record'} to the endpoint shown above, with the message body being the string representation of your record.
                  Canary will update this page when it detects it has received the record.
                </Header.Subheader>
              </Header>
            </Container>
          </Segment>
        )}
      </React.Fragment>
    );
  }
}

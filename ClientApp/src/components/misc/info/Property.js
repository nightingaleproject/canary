import React, { Component } from 'react';
import { StringType } from './Types/StringType';
import { StringDateTimeType } from './Types/StringDateTimeType';
import { BoolType } from './Types/BoolType';
import { DictionaryType } from './Types/DictionaryType';
import { StringArrType } from './Types/StringArrType';
import { TupleCODType } from './Types/TupleCODType';
import { TupleArrType } from './Types/TupleArrType';
import { Accordion, Icon } from 'semantic-ui-react';
import _ from 'lodash';
import AceEditor from 'react-ace';

import 'brace/mode/json';
import 'brace/mode/xml';
import 'brace/theme/chrome';

export class Property extends Component {
  displayName = Property.name;

  constructor(props) {
    super(props);
    this.state = { ...this.props, jsonVisible: false, xmlVisible: false };
    this.updateProperty = this.updateProperty.bind(this);
    this.renderType = this.renderType.bind(this);
  }

  updateProperty(path, value) {
    var property = { ...this.state.property };
    _.set(property, path, value);
    this.setState({ property: property }, () => {
      this.props.updateCategory(this.props.lookup, this.state.property);
    });
  }

  renderType(type, value, description, error) {
    if (type === 'String') {
      return (
        <StringType
          name={this.props.name}
          value={value}
          description={description}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
        />
      );
    } else if (type === 'StringDateTime') {
      return (
        <StringDateTimeType
          name={this.props.name}
          value={value}
          description={description}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
        />
      );
    } else if (type === 'Bool') {
      return (
        <BoolType
          name={this.props.name}
          value={value}
          description={description}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
        />
      );
    } else if (type === 'Dictionary') {
      return (
        <DictionaryType
          name={this.props.name}
          value={value}
          description={description}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
        />
      );
    } else if (type === 'StringArr') {
      return (
        <StringArrType
          name={this.props.name}
          value={value}
          description={description}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
        />
      );
    } else if (type === 'TupleCOD') {
      return (
        <TupleCODType
          name={this.props.name}
          value={value}
          description={description}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
        />
      );
    } else if (type === 'TupleArr') {
      return (
        <TupleArrType
          name={this.props.name}
          value={value}
          description={description}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
        />
      );
    }
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

  tryFormat(content) {
    try {
      if (content.startsWith('{')) {
        if (this.props.property.Type === 'TupleCOD') {
          return this.formatJson(content, 2);
        }
        return this.formatJson(content, 2);
      } else if (content.startsWith('<')) {
        return this.formatXml(content, 2);
      }
    } catch (err) {}
    return content;
  }

  render() {
    return (
      <React.Fragment>
        <fieldset>
          <legend>
            <h3>
              {this.props.name}
              {!!this.props.testMode &&
                (this.props.property.Match === 'false' ? (
                  <Icon name="times" className="red-icon p-l-5" />
                ) : (
                  <Icon name="checkmark" className="green-icon p-l-5" />
                ))}
            </h3>
          </legend>
          <div className="p-l-4">
            {this.renderType(
              this.props.property.Type,
              this.props.property.Value,
              this.props.property.Description,
              !!this.props.testMode && this.props.property.Match === 'false'
            )}
            {!!this.props.testMode &&
              this.props.property.Match === 'false' &&
              this.renderType(this.props.property.Type, this.props.property.FoundValue, this.props.property.Type === 'Dictionary' ? '' : 'Found value:', true)}
            {!!!this.props.hideSnippets && (
              <Accordion styled fluid exclusive={false}>
                <Accordion.Title
                  active={this.state.xmlVisible}
                  onClick={() => {
                    this.setState({ xmlVisible: !this.state.xmlVisible });
                  }}
                >
                  <Icon name="dropdown" />
                  XML
                </Accordion.Title>
                <Accordion.Content active={this.state.xmlVisible}>
                  {!!this.props.testMode && this.props.property.Match === 'false' && <h4>Expected:</h4>}
                  <AceEditor
                    mode="xml"
                    theme="chrome"
                    name="record-xml-b"
                    fontSize={12}
                    showGutter={true}
                    highlightActiveLine={true}
                    showPrintMargin={false}
                    value={this.tryFormat(this.state.property.SnippetXML)}
                    width="100%"
                    readOnly={true}
                    maxLines={this.props.lines || Infinity}
                    setOptions={{ useWorker: false }}
                    editorProps={{
                      $blockScrolling: Infinity,
                    }}
                  />
                  {!!this.props.testMode && this.props.property.Match === 'false' && (
                    <React.Fragment>
                      <h4>Found:</h4>
                      <AceEditor
                        mode="xml"
                        theme="chrome"
                        name="record-xml-b"
                        fontSize={12}
                        showGutter={true}
                        highlightActiveLine={true}
                        showPrintMargin={false}
                        value={this.tryFormat(this.state.property.SnippetXMLTest)}
                        width="100%"
                        readOnly={true}
                        maxLines={this.props.lines || Infinity}
                        setOptions={{ useWorker: false }}
                        editorProps={{
                          $blockScrolling: Infinity,
                        }}
                      />
                    </React.Fragment>
                  )}
                </Accordion.Content>
                <Accordion.Title
                  active={this.state.jsonVisible}
                  onClick={() => {
                    this.setState({ jsonVisible: !this.state.jsonVisible });
                  }}
                >
                  <Icon name="dropdown" />
                  JSON
                </Accordion.Title>
                <Accordion.Content active={this.state.jsonVisible}>
                  {!!this.props.testMode && this.props.property.Match === 'false' && <h4>Expected:</h4>}
                  <AceEditor
                    mode="json"
                    theme="chrome"
                    name="record-json-b"
                    fontSize={12}
                    showGutter={true}
                    highlightActiveLine={true}
                    showPrintMargin={false}
                    value={this.tryFormat(this.state.property.SnippetJSON)}
                    width="100%"
                    readOnly={true}
                    maxLines={this.props.lines || Infinity}
                    setOptions={{ useWorker: false }}
                    editorProps={{
                      $blockScrolling: Infinity,
                    }}
                  />
                  {!!this.props.testMode && this.props.property.Match === 'false' && (
                    <React.Fragment>
                      <h4>Found:</h4>
                      <AceEditor
                        mode="json"
                        theme="chrome"
                        name="record-json-b"
                        fontSize={12}
                        showGutter={true}
                        highlightActiveLine={true}
                        showPrintMargin={false}
                        value={this.tryFormat(this.state.property.SnippetJSONTest)}
                        width="100%"
                        readOnly={true}
                        maxLines={this.props.lines || Infinity}
                        setOptions={{ useWorker: false }}
                        editorProps={{
                          $blockScrolling: Infinity,
                        }}
                      />
                    </React.Fragment>
                  )}
                </Accordion.Content>
              </Accordion>
            )}
          </div>
        </fieldset>
      </React.Fragment>
    );
  }
}

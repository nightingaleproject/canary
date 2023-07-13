import _ from 'lodash';
import React, { Component } from 'react';
import { Accordion, Icon } from 'semantic-ui-react';
import { Snippet } from './Snippet';
import { BoolType } from './Types/BoolType';
import { DeathRecordType } from './Types/DeathRecordType';
import { DictionaryType } from './Types/DictionaryType';
import { StringArrType } from './Types/StringArrType';
import { StringDateTimeType } from './Types/StringDateTimeType';
import { StringType } from './Types/StringType';
import { TupleArrType } from './Types/TupleArrType';
import { TupleCODType } from './Types/TupleCODType';

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

  renderType(type, value, description, igurl, error) {
    if (type === 'String' || type === 'UInt32') {
      return (
        <StringType
          key={`${this.props.name}${value}`} // Key ensures re-render if value changes
          name={this.props.name}
          value={value}
          description={description}
          igurl={igurl}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
        />
      );
    } else if (type === 'StringDateTime' || type === 'DateTimeOffset') {
      return (
        <StringDateTimeType
          key={`${this.props.name}${value}`} // Key ensures re-render if value changes
          name={this.props.name}
          value={value}
          description={description}
          igurl={igurl}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
        />
      );
    } else if (type === 'Bool') {
      return (
        <BoolType
          key={`${this.props.name}${value}`} // Key ensures re-render if value changes
          name={this.props.name}
          value={value}
          description={description}
          igurl={igurl}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
        />
      );
    } else if (type === 'Dictionary') {
      return (
        <DictionaryType
          key={`${this.props.name}${JSON.stringify(value)}`} // Key ensures re-render if value changes
          name={this.props.name}
          value={value}
          description={description}
          igurl={igurl}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
        />
      );
    } else if (type === 'StringArr' || type === 'List`1') {
      return (
        <StringArrType
          key={`${this.props.name}${JSON.stringify(value)}`} // Key ensures re-render if value changes
          name={this.props.name}
          value={value}
          description={description}
          igurl={igurl}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
        />
      );
    } else if (type === 'TupleCOD') {
      return (
        <TupleCODType
          key={`${this.props.name}${JSON.stringify(value)}`} // Key ensures re-render if value changes
          name={this.props.name}
          value={value}
          description={description}
          igurl={igurl}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
        />
      );
    } else if (type === 'TupleArr') {
      return (
        <TupleArrType
          key={`${this.props.name}${JSON.stringify(value)}`} // Key ensures re-render if value changes
          name={this.props.name}
          value={value}
          description={description}
          igurl={igurl}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
        />
      );
    } else if (type === 'DeathRecord') {
      return (
        <DeathRecordType
          name={this.props.name}
          value={value}
          description={description}
          igurl={igurl}
          updateProperty={this.updateProperty}
          editable={this.props.editable}
          testMode={this.props.testMode}
          error={error}
          snippetJSON={this.props.property.SnippetJSON}
          lines={10}
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
      if (content.trim().startsWith('{') || content.trim().startsWith('[')) {
        return this.formatJson(content, 2);
      } else if (content.trim().startsWith('<')) {
        return this.formatXml(content, 2);
      }
    } catch (err) {}
    return content;
  }

  render() {
    if (!!!this.props.editable) {
      if (this.props.property.Type !== "Bool" && this.props.property.Type !== "DeathRecord") {
        if ((!!!this.props.property.Value && this.props.property.Match !== "false") ||
            (Array.isArray(this.props.property.Value) && this.props.property.Value !== null && this.props.property.Value.length === 0) ||
            (typeof this.props.property.Value === 'object' && !Array.isArray(this.props.property.Value) && this.props.property.Value !== null && _.compact(_.values(_.mapValues(this.props.property.Value, 'Value'))).length === 0 ))
        {
          return (<React.Fragment></React.Fragment>);
        }
      }
    }
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
              this.props.property.IGUrl,
              !!this.props.testMode && this.props.property.Match === 'false'
            )}
            {!!this.props.testMode &&
              this.props.property.Match === 'false' && this.props.property.Type !== 'DeathRecord' &&
              this.renderType(this.props.property.Type, this.props.property.FoundValue, this.props.property.Type === 'Dictionary' ? '' : 'Found value:', true)}
            {!!!this.props.hideSnippets && (
              <Accordion styled fluid exclusive={false}>
                <Snippet
                  name="XML"
                  snippet={this.tryFormat(this.state.property.SnippetXML)}
                  snippetTest={this.tryFormat(this.state.property.SnippetXMLTest)}
                  lines={this.props.lines}
                  testMode={this.props.testMode}
                />
                <Snippet
                  name="JSON"
                  snippet={this.tryFormat(this.state.property.SnippetJSON)}
                  snippetTest={this.tryFormat(this.state.property.SnippetJSONTest)}
                  lines={this.props.lines}
                  testMode={this.props.testMode}
                />
              </Accordion>
            )}
          </div>
        </fieldset>
      </React.Fragment>
    );
  }
}

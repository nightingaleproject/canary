import { faFeatherAlt, faMailBulk } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Dropdown, Menu } from 'semantic-ui-react';

export class Navigation extends Component {
  displayName = Navigation.name;

  render() {
    return (
      <React.Fragment>
        <Menu inverted attached size="huge">
          <Menu.Item header>
            <FontAwesomeIcon icon={faFeatherAlt} size="lg" fixedWidth />
            <span className="p-l-5">
              Canary Testing Framework
            </span>
            <span className="p-l-5">
              <small>
                {window.VERSION}; VRDR {window.VRDR_VERSION}
              </small>
            </span>
          </Menu.Item>
          <Menu.Menu position="right">
            <Menu.Item name="dashboard" as={Link} to="/" icon="dashboard" />
            <Dropdown item text="Record Testing" direction="left">
              <Dropdown.Menu>
                <Dropdown.Item icon="upload" text="Producing FHIR VRDR Records" as={Link} to="/test-fhir-producing" />
                <Dropdown.Item icon="download" text="Consuming FHIR VRDR Records" as={Link} to="/test-fhir-consuming" />
                <Dropdown.Item icon="sync" text="VRDR Record Roundtrip (Consuming)" as={Link} to="/test-edrs-roundtrip-consuming" />
                <Dropdown.Item icon="sync" text="VRDR Record Roundtrip (Producing)" as={Link} to="/test-edrs-roundtrip-producing" />
                <Dropdown.Item icon="tasks" text="Connectathon FHIR VRDR Records (Producing)" as={Link} to="/test-connectathon-dash/records" />
                <Dropdown.Item icon="tasks" text="Validate FHIR VRDR Records with IJE (Producing)" as={Link} to="/test-fhir-ije-validator-producing" />
              </Dropdown.Menu>
            </Dropdown>
            <Dropdown item text="Message Testing" direction="left">
              <Dropdown.Menu>
                <Dropdown.Item icon="cloud upload" text="Producing FHIR VRDR Messages" as={Link} to="/test-fhir-message-producing" />
                <Dropdown.Item icon={<i className="icon"><FontAwesomeIcon icon={faMailBulk} fixedWidth /></i>} text="Connectathon FHIR VRDR Messages (Producing)" as={Link} to="/test-connectathon-dash/message" />
              </Dropdown.Menu>
            </Dropdown>
            <Dropdown item text="Record Tools" direction="left">
              <Dropdown.Menu>
                <Dropdown.Item icon="clipboard list" text="Generate Synthetic VRDR Records" as={Link} to="/tool-record-generator" />
                <Dropdown.Item icon="clipboard check" text="FHIR VRDR Record Syntax Checker" as={Link} to="/tool-fhir-syntax-checker" />
                <Dropdown.Item icon="random" text="VRDR Record Format Converter" as={Link} to="/tool-record-converter" />
                <Dropdown.Item icon="find" text="FHIR VRDR Record Inspector" as={Link} to="/tool-fhir-inspector" />
                <Dropdown.Item icon="magic" text="FHIR VRDR Record Creator" as={Link} to="/tool-fhir-creator" />
                <Dropdown.Item icon="search" text="IJE Mortality Record Inspector" as={Link} to="/tool-ije-inspector" />
              </Dropdown.Menu>
            </Dropdown>
            <Dropdown item text="Message Tools" direction="left">
              <Dropdown.Menu>
                <Dropdown.Item icon="envelope" text="FHIR VRDR Message Syntax Checker" as={Link} to="/tool-fhir-message-syntax-checker" />
                <Dropdown.Item icon="find" text="FHIR Message Inspector" as={Link} to="/tool-message-inspector" />
                <Dropdown.Item icon="cloud download" text="Creating FHIR VRDR Messages" as={Link} to="/test-fhir-message-creation" />
              </Dropdown.Menu>
            </Dropdown>
          </Menu.Menu>
        </Menu>
        <div className="p-b-30" />
      </React.Fragment>
    );
  }
}

import React, { Component } from 'react';
import { Menu, Dropdown } from 'semantic-ui-react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faFeatherAlt } from '@fortawesome/free-solid-svg-icons';
import { Link } from 'react-router-dom';

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
              <small>
                {window.VERSION} ({window.VERSION_DATE})
              </small>
            </span>
          </Menu.Item>
          <Menu.Menu position="right">
            <Menu.Item name="dashboard" as={Link} to="/" icon="dashboard" />
            <Dropdown item text="Record Testing" direction="left">
              <Dropdown.Menu>
                <Dropdown.Item icon="upload" text="Producing FHIR Death Records" as={Link} to="test-fhir-producing" />
                <Dropdown.Item icon="download" text="Consuming FHIR Death Records" as={Link} to="test-fhir-consuming" />
                <Dropdown.Item icon="sync" text="Death Record Roundtrip (Consuming)" as={Link} to="test-edrs-roundtrip-consuming" />
                <Dropdown.Item icon="sync" text="Death Record Roundtrip (Producing)" as={Link} to="test-edrs-roundtrip-producing" />
                <Dropdown.Item icon="tasks" text="Connectathon FHIR Death Records (Producing)" as={Link} to="test-connectathon-dash" />
              </Dropdown.Menu>
            </Dropdown>
            <Dropdown item text="Message Testing" direction="left">
              <Dropdown.Menu>
                <Dropdown.Item icon="cloud upload" text="Producing FHIR Messages" as={Link} to="test-fhir-message-producing" />
              </Dropdown.Menu>
            </Dropdown>
            <Dropdown item text="Record Tools" direction="left">
              <Dropdown.Menu>
                <Dropdown.Item icon="clipboard list" text="Generate Synthetic Death Records" as={Link} to="tool-record-generator" />
                <Dropdown.Item icon="clipboard check" text="Validate FHIR Records" as={Link} to="tool-fhir-validator" />
                <Dropdown.Item icon="random" text="Death Record Format Converter" as={Link} to="tool-record-converter" />
                <Dropdown.Item icon="find" text="FHIR Death Record Inspector" as={Link} to="tool-fhir-inspector" />
                <Dropdown.Item icon="magic" text="FHIR Death Record Creator" as={Link} to="tool-fhir-creator" />
                <Dropdown.Item icon="search" text="IJE Mortality Record Inspector" as={Link} to="tool-ije-inspector" />
              </Dropdown.Menu>
            </Dropdown>
            <Dropdown item text="Message Tools" direction="left">
              <Dropdown.Menu>
                <Dropdown.Item icon="envelope" text="Validate FHIR Messages" as={Link} to="tool-fhir-message-validator" />
                <Dropdown.Item icon="cloud download" text="Creating FHIR Messages" as={Link} to="test-fhir-message-creation" />
              </Dropdown.Menu>
            </Dropdown>
          </Menu.Menu>
        </Menu>
        <div className="p-b-30" />
      </React.Fragment>
    );
  }
}

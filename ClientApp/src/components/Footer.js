import React, { Component } from 'react';
import { Menu } from 'semantic-ui-react';

export class Footer extends Component {
  displayName = Footer.name;

  render() {
    return (
      <React.Fragment>
        <div className="p-t-30" />
        <Menu inverted attached borderless>
          <Menu.Item>
            Canary is developed by&nbsp;
            <a className="underline" href="http://www.mitre.org">
              The MITRE Corporation
            </a>
            &nbsp;/&nbsp;
            <a className="underline" href="https://www.mitre.org/centers/cms-alliances-to-modernize-healthcare/who-we-are">
              the Health FFRDC
            </a>
            .
          </Menu.Item>
        </Menu>
      </React.Fragment>
    );
  }
}

import React, { Component } from 'react';
import { Navigation } from './Navigation';
import { Container, Grid } from 'semantic-ui-react';
import { SemanticToastContainer } from 'react-semantic-toasts';

export class Layout extends Component {
  displayName = Layout.name;

  render() {
    return (
      <React.Fragment>
        <SemanticToastContainer />
        <Navigation />
        <Container>
          <Grid>{this.props.children}</Grid>
        </Container>
      </React.Fragment>
    );
  }
}

import React, { Component } from 'react';
import { Item, Icon } from 'semantic-ui-react';
import { Link } from 'react-router-dom';

export class DashboardItem extends Component {
  displayName = DashboardItem.name;

  render() {
    return (
      <React.Fragment>
        <Item as={Link} to={this.props.route} className="p-t-10">
          <Item.Image className="align-center">
            <Icon name={this.props.icon} size="huge" circular color="black" className="float-center" />
          </Item.Image>
          <Item.Content verticalAlign="middle">
            <Item.Header>{this.props.title}</Item.Header>
            <Item.Description>{this.props.description}</Item.Description>
          </Item.Content>
        </Item>
      </React.Fragment>
    );
  }
}

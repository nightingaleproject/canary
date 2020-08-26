import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Icon, Item } from 'semantic-ui-react';

export class DashboardItem extends Component {
  displayName = DashboardItem.name;

  render() {
    return (
      <React.Fragment>
        <Item as={Link} to={this.props.route} className="p-t-10">
          <Item.Image className="align-center">
            {!!this.props.icon && <Icon name={this.props.icon} size="huge" circular color="black" className="float-center" />}
            {!!this.props.faIcon && <Icon circular size="huge">
              <FontAwesomeIcon icon={this.props.faIcon} color="black" className="float-center" />
            </Icon>}
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

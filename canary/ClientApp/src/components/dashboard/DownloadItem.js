import React, { Component } from 'react';
import { Item, Icon } from 'semantic-ui-react';
import { Link } from 'react-router-dom';

export class DownloadItem extends Component {
  displayName = DownloadItem.name;

  downloadAsFile(e, path) {
    e.stopPropagation();
    var element = document.createElement('a');
    element.setAttribute('href', path);
    element.setAttribute('download', path);
    element.click();
  }

  render() {
    return (
      <React.Fragment>
        <Item as={Link} to={this.props.route} className="p-t-10" onClick={(e) => this.downloadAsFile(e, this.props.downloadFile)}>
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

import React, { Component } from 'react';
import { Icon, Message, Transition } from 'semantic-ui-react';

export class Issues extends Component {
  displayName = Issues.name;

  render() {
    return !!this.props.issues && this.props.issues.length > 0 && (
      <React.Fragment>
          <div className="inherit-width p-b-50">
            {this.props.issues.map((issue, index) => {
              return (
                  <Transition key={`issue-t-${index}`} transitionOnMount animation="fade" duration={1000}>
                  <div className="inherit-width p-b-10">
                        <Message icon size="large" error={issue.severity === 'error'} warning={issue.severity === 'warning'}>
                          <Icon name={issue.severity === 'error' ? 'exclamation triangle' : 'info circle'} />
                          <Message.Content>{`${issue.message}`}</Message.Content>
                        </Message>
                  </div>
                </Transition>
              );
            })}
          </div>
      </React.Fragment>
    );
  }
}

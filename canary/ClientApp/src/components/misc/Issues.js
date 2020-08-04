import React, { Component } from 'react';
import { Icon, Message, Transition } from 'semantic-ui-react';

export class Issues extends Component {
  displayName = Issues.name;

  render() {
    return (
      <React.Fragment>
        {!!this.props.issues && this.props.issues.length > 0 && (
          <div className="inherit-width p-b-50">
            {this.props.issues.map(function(issue, index) {
              return (
                <Transition key={`issue-t-${index}`} transitionOnMount animation="fade" duration={1000}>
                  <div className="inherit-width p-b-10">
                    <Message icon size="large" negative={issue.severity.toLowerCase() === 'error'} warning={issue.severity.toLowerCase() === 'warning'}>
                      <Icon name="exclamation triangle" />
                      <Message.Content>{`${issue.message}`}</Message.Content>
                    </Message>
                  </div>
                </Transition>
              );
            })}
          </div>
        )}
      </React.Fragment>
    );
  }
}

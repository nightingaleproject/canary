import React, { Component } from 'react';
import { Icon, Message, Transition } from 'semantic-ui-react';

export class Issues extends Component {
  displayName = Issues.name;

  render() {
    const { issues, severity } = this.props;
    const header = severity === 'error' ? 'Errors' : 'Warnings';
    const messageIcon = severity === 'error' ? 'exclamation triangle' : 'info circle';

    return !!issues && issues.length > 0 && ['error','warning'].includes(severity) &&  (
      <React.Fragment>
            <h3>{header}:</h3>
          <div className="inherit-width p-b-50">
            {issues.filter(x => x.severity == this.props.severity).map(function(issue, index) {
              return (
                  <Transition key={`issue-t-${index}`} transitionOnMount animation="fade" duration={1000}>
                  <div className="inherit-width p-b-10">
                        <Message icon size="large" error={issue.severity === 'error'} warning={issue.severity === 'warning'}>
                          <Icon name={messageIcon} />
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

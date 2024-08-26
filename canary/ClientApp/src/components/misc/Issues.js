import React, { Component } from 'react';
import { Icon, Message, Transition } from 'semantic-ui-react';

export class Issues extends Component {
  displayName = Issues.name;

  render() {
    return (
      <React.Fragment>
        {!!this.props.issues && this.props.issues.length > 0 && this.props.severity === 'errors' && (
            <h3>Errors:</h3>
        )}
        {!!this.props.issues && this.props.issues.length > 0 && this.props.severity === 'errors' && (
          <div className="inherit-width p-b-50">
            {this.props.issues.map(function(issue, index) {
              return (
                  <Transition key={`issue-t-${index}`} transitionOnMount animation="fade" duration={1000}>
                  <div className="inherit-width p-b-10">
                    {issue.severity.toLowerCase() === 'error' && (
                        <Message icon size="large" negative={issue.severity.toLowerCase() === 'error'}>
                          <Icon name="exclamation triangle" />
                          <Message.Content>{`${issue.message}`}</Message.Content>
                        </Message>
                    )}
                  </div>
                </Transition>
              );
            })}
          </div>
        )}

        {!!this.props.issues && this.props.issues.length > 0 && this.props.severity === 'warnings' && (
            <h3>Warnings:</h3>
        )}
        {!!this.props.issues && this.props.issues.length > 0 && this.props.severity === 'warnings' && (
          <div className="inherit-width p-b-50">
            {this.props.issues.map(function(issue, index) {
              return (
                  <Transition  key={`issue-t-${index}`} transitionOnMount animation="fade" duration={1000}>
                  <div className="inherit-width p-b-10">
                    {issue.severity.toLowerCase() === 'warning' && (
                        <Message icon size="large" warning={issue.severity.toLowerCase() === 'warning'}>
                          <Icon name="info circle" />
                          <Message.Content>{`${issue.message}`}</Message.Content>
                        </Message>
                    )}
                  </div>
                </Transition>
              );
            })}
          </div>
        )}

        {!!!this.props.severity && !!this.props.issues && this.props.issues.length > 0 && (
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

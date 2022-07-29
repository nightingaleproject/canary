import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import { unregister } from './registerServiceWorker';
import App from './App';
import 'semantic-ui-css/semantic.min.css';
import './index.css';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

//window.API_URL = 'http://localhost:5000';
window.API_URL = '';
window.VERSION = 'v4.0.0-preview3';
window.VERSION_DATE = 'July 29, 2022';

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <App />
  </BrowserRouter>,
  rootElement
);

unregister();

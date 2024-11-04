import React from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import { unregister } from './registerServiceWorker';
import App from './App';
import 'semantic-ui-css/semantic.min.css';
import './index.css';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');

//window.API_URL = 'http://localhost:5000';
window.API_URL = '';
window.VERSION = '4.4.1';
window.VERSION_DATE = 'November 4, 2024';
window.VRDR_VERSION = '4.4.1';
window.VRDR_VERSION_DATE = 'October 28, 2024';

const container = document.getElementById('root');
const root = createRoot(container);
root.render(
  <BrowserRouter basename={baseUrl}>
    <App />
  </BrowserRouter>
);

unregister();

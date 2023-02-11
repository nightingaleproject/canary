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
window.VERSION = 'v4.0.0-preview9';
window.VERSION_DATE = 'February 10, 2023';

const container = document.getElementById('root');
const root = createRoot(container);
root.render(
  <BrowserRouter basename={baseUrl}>
    <App />
  </BrowserRouter>
);

unregister();

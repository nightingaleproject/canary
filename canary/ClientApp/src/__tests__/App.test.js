import React from 'react';
import App from '../App';
import {
  MemoryRouter
} from 'react-router-dom';
import {
  createRoot
} from 'react-dom/client';
import { act, create} from "react-test-renderer"

it('renders without crashing', () => {
  const container = document.createElement('root');
  const root = createRoot(container);
  root.render(<MemoryRouter><App /></MemoryRouter>);
});

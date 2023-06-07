import renderer from 'react-test-renderer';
import {Record} from '../Record';

it('renders correctly', () => {
  const tree = renderer.create(<Record />).toJSON();
  expect(tree).toMatchSnapshot();
});
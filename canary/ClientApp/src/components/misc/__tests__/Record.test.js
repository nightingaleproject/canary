import renderer from 'react-test-renderer';
import {Record} from '../Record';

it('renders correctly', () => {
  const record = {
    json: '{"a":2}',
    xml: '<a>b</a>',
    hideIje: true
  }

  const tree = renderer.create(<Record record={record} />).toJSON();
  expect(tree).toMatchSnapshot();
});
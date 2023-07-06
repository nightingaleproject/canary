import renderer from 'react-test-renderer';
import {
    MemoryRouter} from 'react-router-dom';
import { MessageConnectathonProducing } from '../MessageConnectathonProducing'

it('renders', () => {
    const tree = renderer.create(
        <MemoryRouter>
            <MessageConnectathonProducing params={{ id: "1" }} />
        </MemoryRouter>
    ).toJSON();
    expect(tree).toMatchSnapshot();
});
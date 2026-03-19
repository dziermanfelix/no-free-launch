import './Home.css';
import Launches from '../components/Launches';
import { Link } from 'react-router-dom';

const Home = () => {
  return (
    <>
      <section className='home-container'>
        <h3>Launches</h3>
        <div className='launches-container'>
          <Launches />
        </div>
        <Link to='/launches' className='view-all-launches-button'>
          View All Launches
        </Link>
      </section>
    </>
  );
};

export default Home;

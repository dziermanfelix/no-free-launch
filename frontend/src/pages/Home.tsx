import Launches from '../components/Launches';

const Home = () => {
  return (
    <>
      <section id='center'>
        <div className='hero'>
          <h1>No Free Launch Home</h1>
        </div>
        <div>
          <div>
            <Launches />
          </div>
        </div>
      </section>
    </>
  );
};

export default Home;

import './App.css';
import Launches from './components/Launches';
import Launch from './components/Launch';

function App() {
  return (
    <>
      <section id='center'>
        <div className='hero'>
          <h1>No Free Launch</h1>
        </div>
        <div>
          <div>
            <Launch id={null} flightNumber={1} />
          </div>
          <div>
            <Launches />
          </div>
        </div>
      </section>
    </>
  );
}

export default App;

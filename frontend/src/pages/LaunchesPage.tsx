import './LaunchesPage.css';
import Launches from '../components/Launches';

export default function LaunchesPage() {
  return (
    <div className='launches-page'>
      <h1>Launches</h1>
      <div className='launches-page-scroll'>
        <Launches />
      </div>
    </div>
  );
}

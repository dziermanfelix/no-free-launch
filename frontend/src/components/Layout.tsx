import { Outlet } from 'react-router-dom';
import './Layout.css';

export function Layout() {
  return (
    <div className='layout'>
      <header className='layout-header'>header</header>
      <main className='layout-main'>
        <Outlet />
      </main>
      <footer className='layout-footer'>footer</footer>
    </div>
  );
}

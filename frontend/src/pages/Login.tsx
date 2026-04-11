import { useState, type SubmitEvent } from 'react';
import './Login.css';
import { Link, useNavigate } from 'react-router-dom';
import { useMutation } from '@apollo/client/react';
import { LOGIN } from '../graphql/mutations';
import type { LoginMutationData } from '../types/graphql';
import { useAuth } from '../contexts/useAuth';

export default function Login() {
  const navigate = useNavigate();
  const [error, setError] = useState<string>('');
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const auth = useAuth();
  const [login] = useMutation<LoginMutationData>(LOGIN);
  const [username, setUsername] = useState<string>('');
  const [password, setPassword] = useState<string>('');

  const submitForm = async (e: SubmitEvent) => {
    e.preventDefault();
    setError('');
    setIsSubmitting(true);
    try {
      const res = await login({ variables: { username, password } });
      const payload = res.data?.login;
      if (!payload?.user || !payload?.token) {
        throw new Error('Login failed: missing user/token');
      }
      auth.login(payload.user, payload.token);
      navigate('/');
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : 'Login failed');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div>
      <h1>Login</h1>

      <form onSubmit={submitForm}>
        <div className='row'>
          <label htmlFor='username'>Username</label>
          <input id='username' type='text' value={username} onChange={(e) => setUsername(e.target.value)} required />
        </div>
        <div className='row'>
          <label htmlFor='password'>Password</label>
          <input
            id='password'
            type='password'
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>

        <button type='submit' disabled={isSubmitting}>
          Login
        </button>
      </form>

      {error && <div className='error'>{error}</div>}

      <div>
        <p>
          Don't have an account? <Link to='/register'>Register</Link>
        </p>
      </div>
    </div>
  );
}

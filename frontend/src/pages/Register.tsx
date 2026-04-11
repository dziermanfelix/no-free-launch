import { useState, type SubmitEvent } from 'react';
import './Login.css';
import { Link, useNavigate } from 'react-router-dom';
import { useMutation } from '@apollo/client/react';
import { REGISTER } from '../graphql/mutations';
import type { RegisterMutationData } from '../types/graphql';
import { useAuth } from '../contexts/useAuth';

export default function Register() {
  const navigate = useNavigate();
  const [register] = useMutation<RegisterMutationData>(REGISTER);
  const auth = useAuth();
  const [username, setUsername] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [password2, setPassword2] = useState<string>('');
  const [error, setError] = useState<string>('');
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

  const submitForm = async (e: SubmitEvent) => {
    e.preventDefault();
    setError('');
    setIsSubmitting(true);
    try {
      const res = await register({ variables: { username, password } });
      const payload = res.data?.register;
      if (!payload?.user || !payload?.token) {
        throw new Error('Register failed: missing user/token');
      }
      auth.login(payload.user, payload.token);
      navigate('/');
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : 'Register failed');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div>
      <h1>Register</h1>

      <form onSubmit={submitForm}>
        <div>
          <label htmlFor='username'>Username</label>
          <input id='username' type='text' value={username} onChange={(e) => setUsername(e.target.value)} required />
        </div>
        <div>
          <label htmlFor='password'>Password</label>
          <input
            id='password'
            type='password'
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <div>
          <label htmlFor='password2'>Confirm Password</label>
          <input
            id='password2'
            type='password'
            value={password2}
            onChange={(e) => setPassword2(e.target.value)}
            required
          />
        </div>
        <button type='submit' disabled={isSubmitting}>
          Register
        </button>
      </form>

      {error && <div className='error'>{error}</div>}

      <div>
        <p>
          Already have an account? <Link to='/login'>Login</Link>
        </p>
      </div>
    </div>
  );
}

import { useState, type SubmitEvent } from 'react';
import './Login.css';
import { Link } from 'react-router-dom';
import { useMutation } from '@apollo/client/react';
import { REGISTER } from '../graphql/mutations';

export default function Register() {
  const [register] = useMutation(REGISTER);
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
      await register({ variables: { username, password } });
    } catch (error: any) {
      setError(error.message);
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

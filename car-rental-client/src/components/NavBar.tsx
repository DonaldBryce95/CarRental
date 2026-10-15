import { Link, useNavigate } from 'react-router-dom'
import './NavBar.css'
import { getRoles, isLoggedIn, logout } from '../utils/auth'
function NavBar() {
  const loggedIn = isLoggedIn()
  const roles = getRoles()
  const navigate = useNavigate()

  const handleLogout = () => {
  logout()
  navigate('/login')
}

  return (
    <nav>
      <div className="navbar">
        <div className='brand'>
          <i className="bi bi-lightning-fill"></i>
          <span>Zap Rental</span>
      </div>
        {!loggedIn && (
          <div className='nav-links'>
            <Link to="/">HOME</Link>
            <Link to="/car-groups">CAR GROUPS</Link>

            <div className='auth-links'>
            <Link to="/login">LOGIN</Link>
            <Link to="/register">REGISTER</Link>
            </div>
          </div>
        )}
        {roles.includes("Customer") && (
          <div className='nav-links'>
            <Link to="/">HOME</Link>
            <Link to="/car-groups">CAR GROUPS</Link>
            <Link to="/my-reservations">MY RESERVATIONS</Link>
          </div>
        )}
        {loggedIn && (
          <button className='logout-button' onClick={handleLogout}>
            LOGOUT
          </button>
        )}
      </div>
    </nav>
  )
}

export default NavBar
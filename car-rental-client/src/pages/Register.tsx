import { useState } from "react"
import { Link, useNavigate } from "react-router-dom"
import './Register.css'

function Register() {
  const [email, setEmail] = useState("")
  const [emailError, setEmailError] = useState("")
  const [password, setPassword] = useState("")
  const [passwordError, setPasswordError] = useState("")
  const [firstName, setFirstName] = useState("")
  const [firstNameError, setFirstNameError] = useState("")
  const [lastName, setLastName] = useState("")
  const [lastNameError, setLastNameError] = useState("")
  const [phoneNumber, setPhoneNumber] = useState("")
  const [phoneNumberError, setPhoneNumberError] = useState("")
  const navigate = useNavigate()
  const [error, setError] = useState("")

  const handleRegister = async (event: React.SubmitEvent) => {
    event.preventDefault()

    setError("")

    if(!firstName)
    {
      setFirstNameError("Please enter your first name.")
      return
    }

    if(!lastName)
    {
      setLastNameError("Please enter your last name.")
      return
    }

    if(!phoneNumber)
    {
      setPhoneNumberError("Please enter your phone number.")
      return
    }

    if(!email)
    {
      setEmailError("Please enter your email.")
      return
    }

    if(!password)
    {
      setPasswordError("Please enter your password.")
      return
    }

    if(password.length < 6 || !/[A-Z]/.test(password) || !/[a-z]/.test(password) || !/[0-9]/.test(password) || !/[^a-zA-Z0-9]/.test(password))
    {
      setPasswordError("Password must contain at least 6 characters, 1 uppercase, 1 lowercase, 1 number, and a special character.")
      return
    }

    try
    {
      const response = await fetch(`/api/Auth/register`, {

        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          firstName,
          lastName,
          email,
          phoneNumber,
          password
        })
      })

      if(!response.ok)
      {
        setError("Unable to register account.")
        return
      }

      alert("Registration successful!")

      navigate("/login")
    }
    catch
    {
      setError("Unable to connect to server.")
    }


  }
  return (
    <main className="register-page">
        <div className="register-card">
          <h1>Register</h1>
            {error && (
              <p className="validation-error">
                {error}
              </p>
            )}

      <form  className="register-form" onSubmit={handleRegister}>
        <div className="form-group">
          {firstNameError && (
          <p className="validation-error">{firstNameError}</p>
          )}
          <label>First Name</label>
          <input
          type="text"
          value={firstName}
          onChange={event =>  {
            setFirstName(event.target.value)
            setFirstNameError("")
            setError("")
          }}
          />
        </div>

        <div className="form-group">
          {lastNameError && (
          <p className="validation-error">{lastNameError}</p>
          )}
          <label>Last Name</label>
          <input
          type="text"
          value={lastName}
          onChange={event => {
            setLastName(event.target.value)
            setLastNameError("")
            setError("")
          }}
          />
        </div>

        <div className="form-group">
          {phoneNumberError && (
          <p className="validation-error">{phoneNumberError}</p>
          )}
          <label>Phone Number</label>
          <input
          type="tel"
          value={phoneNumber}
          onChange={event => {
            setPhoneNumber(event.target.value)
            setPhoneNumberError("")
            setError("")
          }}
          />
        </div>

        <div className="form-group">
          {emailError && (
          <p className="validation-error">{emailError}</p>
          )}
          <label>Email</label>
          <input
          type="email"
          value={email}
          onChange={event => {
            setEmail(event.target.value)
            setEmailError("")
            setError("")
          }}
          />
        </div>

        <div className="form-group">
          {passwordError && (
          <p className="validation-error">{passwordError}</p>
          )}
          <label>Password</label>
          <input
          type="password"
          value={password}
          onChange={event => {
            setPassword(event.target.value)
            setPasswordError("")
            setError("")
          }}
          />
        </div>

        <button className="register-button" type="submit">
          Register
        </button>
      </form>
      <p className="auth-switch">
        Already have an account? <Link to="/login">Login</Link>
      </p>
    </div>
    </main>
  )
}

export default Register
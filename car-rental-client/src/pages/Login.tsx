import React, { useState } from "react"
import { Link, useNavigate } from "react-router-dom"
import { getRoles } from "../utils/auth"
import './Login.css'

function Login() {
  const [email, setEmail] = useState("")
  const [emailError, setEmailError] = useState("")
  const [password, setPassword] = useState("")
  const [passwordError, setPasswordError] = useState("")
  const [error, setError] = useState("")

  const navigate = useNavigate()

  const handleLogin = async (event: React.SubmitEvent) => {
    event.preventDefault()

    setError("")
    setEmailError("")
    setPasswordError("")

    if(!email)
    {
      setEmailError("Please enter your email")
      return
    }

    if(!password)
    {
      setPasswordError("Please enter your password")
      return
    }
    try
    {
      const response = await fetch(`/api/Auth/login`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          email,
          password
        })
      })

      if(!response.ok)
      {
        setError("Invalid email or password.")
        return
      }

      const data = await response.json()

      localStorage.setItem("token", data.token)

      const roles = getRoles()

      if (roles.includes("Admin")) {
        navigate("/admin-dashboard")
      } else if (roles.includes("MaintenanceWorker")) {
        navigate("/maintenance-dashboard")
      } else {
        navigate("/")
      }
    }
    catch
    {
      setError("Unable to connect to server.")
    }
  }
  return (
    <main className="login-page">
      <div className="login-card">
        <h1>Login</h1>
          {error && (
            <p className="validation-error">
              {error}
            </p>
          )}

        <form className="login-form" onSubmit={handleLogin}>
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

        <button className="login-button" type="submit">
          Login
        </button>
      </form>
      <p className="auth-switch">
        Don't have an account? <Link to="/register">Register</Link>
      </p>
      </div>
    </main>
  )
}

export default Login
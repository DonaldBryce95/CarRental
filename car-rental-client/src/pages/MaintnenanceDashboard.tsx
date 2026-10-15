import { useEffect, useState } from "react"
import { getToken } from "../utils/auth"
import { Link } from "react-router-dom"
import './MaintenanceDashboard.css'

function MaintenanceDashboard () {
  const [message, setMessage] = useState("")
  const [taskCount, setTaskCount] = useState<number>(0)

  useEffect (() => {
    const getTaskCount = async () => {
      try
      {
        const response = await fetch(`/api/Maintenance/my-tasks/dashboard`, {
          headers: {
          Authorization: `Bearer ${getToken()}`
        }
        })

        if(!response.ok)
        {
          setMessage("Unable to get tasks.")
          return
        }

        const data = await response.json()

        setTaskCount(data)
      }
      catch
      {
        setMessage("Unable to connect to server.")
      }
    }
    getTaskCount()
  }, [])
  return (
    <div className="maintenance-dashboard-container">
      <h1>Maintenance Tasks</h1>

      {message && (
      <p className="error-message">
        {message}
      </p>
      )}

      <div className="maintenance-dashboard-cards">
        <Link to="/maintenance-tasks"
          className="maintenance-dashboard-card">
            <div className="card-title">
              Active Tasks
            </div>

            <div className="maintenance-card-count">
              {taskCount}
            </div>

            <div className="maintenance-card-link">
              View My Tasks →
            </div>
        </Link>
      </div>
    </div>
  )
}

export default MaintenanceDashboard
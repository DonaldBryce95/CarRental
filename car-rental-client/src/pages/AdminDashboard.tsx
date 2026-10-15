import { useEffect, useState } from "react"
import { Link, useNavigate } from "react-router-dom"
import { getToken } from "../utils/auth"
import './AdminDashboard.css'

function AdminDashboard () {
  const [reservationCount, setReservationCount] = useState<number>(0)
  const [maintenanceTaskCount, setMaintenanceTaskCount] = useState<number>(0)
  const [carCount, setCarCount] = useState<number>(0)
  const [maintenanceCarCount, setMaintenanceCarCount] = useState<number>(0)
  const [maintenanceWorkerCount, setMaintenanceWorkerCount] = useState<number>(0)
  const [reservationError, setReservationError] = useState("")
  const [carError, setCarError] = useState("")
  const [maintenanceCarError, setMaintenanceCarError] = useState("")
  const [taskError, setTaskError] = useState("")
  const [workerError, setWorkerError] = useState("")
  const navigate = useNavigate()

  useEffect(() => {
    const getReservationCount = async () => {
      try
      {
        setReservationError("")
        const response = await fetch(`/api/Admin/reservations/dashboard`, {
          headers: {
            Authorization: `Bearer ${getToken()}`
          }
        })

        if(!response.ok)
        {
          setReservationError("Unable to load reservations.")
          return
        }

        const data = await response.json()

        setReservationCount(data)
      }
      catch
      {
        setReservationError("Unable to connect to server.")
      }
    }

    getReservationCount()
  }, [])
    useEffect(() => {
    const getCarCount = async () => {
      try
      {
        setCarError("")
        const response = await fetch(`/api/Admin/cars/dashboard`, {
          headers: {
            Authorization: `Bearer ${getToken()}`
          }
        })

        if(!response.ok)
        {
          setCarError("Unable to load cars.")
          return
        }

        const data = await response.json()

        setCarCount(data)
      }
      catch
      {
        setCarError("Unable to connect to server.")
      }
    }

    getCarCount()
  }, [])
    useEffect(() => {
    const getMaintenanceCarCount = async () => {
      try
      {
        setMaintenanceCarError("")
        const response = await fetch(`/api/Admin/maintenance/repairs/dashboard`, {
          headers: {
            Authorization: `Bearer ${getToken()}`
          }
        })

        if(!response.ok)
        {
          setMaintenanceCarError("Unable to load cars under maintenance.")
          return
        }

        const data = await response.json()

        setMaintenanceCarCount(data)
      }
      catch
      {
        setMaintenanceCarError("Unable to connect to server.")
      }
    }

    getMaintenanceCarCount()
  }, [])
  useEffect(() => {
    const getTaskCount = async () => {
      try
      {
        setTaskError("")
        const response = await fetch(`/api/Admin/maintenance-tasks-dashboard`, {
          headers: {
            Authorization: `Bearer ${getToken()}`
          }
        })

        if(!response.ok)
        {
          setTaskError("Unable to load maintenance tasks.")
          return
        }

        const data = await response.json()

        setMaintenanceTaskCount(data)
      }
      catch
      {
        setTaskError("Unable to connect to server.")
      }
    }

    getTaskCount()
  }, [])
    useEffect(() => {
    const getWorkerCount = async () => {
      try
      {
        setWorkerError("")
        const response = await fetch(`/api/Admin/maintenance/workers/dash`, {
          headers: {
            Authorization: `Bearer ${getToken()}`
          }
        })

        if(!response.ok)
        {
          setWorkerError("Unable to load maintenance workers.")
          return
        }

        const data = await response.json()

        setMaintenanceWorkerCount(data)
      }
      catch
      {
        setWorkerError("Unable to connect to server.")
      }
    }

    getWorkerCount()
  }, [])
  return (
    <div className="dashboard-container">
      <h1>Dashboard</h1>
        <button className="assign-button" onClick={() => navigate("/admin/assign/role")}>Assign Role</button>
        <div className="dashboard-cards">
          <Link to="/admin/customer/reservations"
          className="dashboard-card">
            <div className="card-title">
              Active Reservations
            </div>
            {reservationError ? (
              <p className="error-message">
                {reservationError}
              </p>
            )
            :
            (
            <div className="card-count">
              {reservationCount}
            </div>
          )}

          <div className="card-link">
            View reservations →
          </div>
        </Link>

        <Link to="/admin/cars/all"
        className="dashboard-card">
          <div className="card-title">
            Current Cars
          </div>

          {carError ? (
            <p className="error-message">
              {carError}
            </p>
          )
          :
          (
            <div className="card-count">
              {carCount}
            </div>
          )}

          <div className="card-link">
            Manage cars →
          </div>
        </Link>

        <Link to="/admin/maintenance/worker/tasks"
        className="dashboard-card">
          <div className="card-title">
            Maintenance Tasks
          </div>

          {taskError ? (
            <p className="error-message">
              {taskError}
            </p>
          )
          :
          (
            <div className="card-count">
              {maintenanceTaskCount}
            </div>
          )}

          <div className="card-link">
            Manage Tasks →
          </div>
        </Link>

        <Link to ="/admin/cars/undermaintenance/profile"
        className="dashboard-card">
          <div className="card-title">
            Cars Under Maintenance
          </div>

          {maintenanceCarError ? (
            <p className="error-message">
              {maintenanceCarError}
            </p>
          )
          :
          (
            <div className="card-count">
              {maintenanceCarCount}
            </div>
          )}

          <div className="card-link">
            Manage Cars →
          </div>
        </Link>

        <Link to="/admin/maintenance/workers/profile"
        className="dashboard-card">
          <div className="card-title">
            Maintenance Workers
          </div>

          {workerError ? (
            <p className="error-message">
              {workerError}
            </p>
          )
          :
          (
            <div className="card-count">
              {maintenanceWorkerCount}
            </div>
          )}

          <div className="card-link">
            Manage Workers →
          </div>
        </Link>
        </div>
      </div>
    )
  }

export default AdminDashboard
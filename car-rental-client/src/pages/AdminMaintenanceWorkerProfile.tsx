import { useEffect, useState } from "react"
import { getToken } from "../utils/auth"
import { useNavigate } from "react-router-dom"
import './AdminMaintenanceWorkerProfile.css'

type MaintenanceWorker = {
  maintenanceWorkerId: number
  firstName: string
  lastName: string
  maintenanceWorkerStatus: number
}

enum MaintenanceWorkerStatus {
  Available,
  Unavailable
}

function AdminMaintenanceWorkerProfile() {
  const[maintenanceWorkers, setMaintenanceWorkers] = useState<MaintenanceWorker[]>([])
  const [page, setPage] = useState(1)
  const [totalCount, setTotalCount] = useState(0)
  const [error, setError] = useState("")

  const navigate = useNavigate()

  const pageSize = 6

  useEffect(() => {
  const getWorkers = async () => {
    try
    {
      setError("")
      const response = await fetch(`/api/Admin/maintenance-workers?page=${page}&pageSize=${pageSize}`, {
        headers: {
          Authorization: `Bearer ${getToken()}`
        }
      })

      if(!response.ok)
      {
        setError("Unable to load maintenance worker profiles.")
        return
      }

      const data = await response.json()

      setMaintenanceWorkers(data.items)

      setTotalCount(data.totalCount)

      setError("")
    }
    catch
    {
      setError("Unable to connect to server.")
    }
  }

  getWorkers()
}, [page])

const totalPages = Math.ceil(totalCount / pageSize)
  return (
    <div className="maintenance-worker-profile-page">
    <h1>Maintenance Workers</h1>

    {error && (
      <p className="error-message">
        {error}
      </p>
    )}

    <button className="profile-task-back-to-dash" onClick={() => navigate("/admin-dashboard")} type="button">Back To Dashboard</button>
    <div className="worker-profile-cards">
    {maintenanceWorkers.map(worker => (
      <div className="worker-profile-card" key={worker.maintenanceWorkerId}>
        <p>Worker ID: {worker.maintenanceWorkerId}</p>
        <p>First Name: {worker.firstName}</p>
        <p>Last Name: {worker.lastName}</p>
        <p>Status: {MaintenanceWorkerStatus[worker.maintenanceWorkerStatus]}</p>
      </div>
    ))}
    </div>
    {totalCount > 0 && (
      <div className="pagination">
        <button
          onClick={() => setPage(page - 1)}
          disabled={page === 1}
        >
          Previous
        </button>

        <span>
          Page {page} of {totalPages}
        </span>

        <button
          onClick={() => setPage(page + 1)}
          disabled={page >= totalPages}
        >
          Next
        </button>
      </div>
    )}
  </div>
  )
}

export default AdminMaintenanceWorkerProfile
import { useParams, useNavigate } from "react-router-dom"
import { useEffect, useState } from "react"
import { getToken } from "../utils/auth"
import './AdminMaintenanceWorkerTasks.css'

type Task = {
  maintenanceTaskId: number
  carId: number
  make: string
  model: string
  maintenanceWorkerId: number
  maintenanceTaskStatus: number
  inspectionResult: number
  maintenanceReportStatus: number
}

enum InspectionResult {
  Pending,
  Pass,
  Warning,
  Fail
}

enum MaintenanceTaskStatus {
  AwaitingAssignment,
  Pending,
  InProgress,
  Completed
}

enum MaintenanceReportStatus {
  Pending,
  InProgress,
  Completed
}

function AdminMaintenanceWorkerTasks() {
  const {maintenanceWorkerId} = useParams()
  const [tasks, setTasks] = useState<Task[]>([])
  const [page, setPage] = useState(1)
  const [totalCount, setTotalCount] = useState(0)
  const [taskError, setTaskError] = useState("")
  const navigate = useNavigate()

  const pageSize = 6

  useEffect (() => {
    const getWorkerTasks = async () => {
      if(!maintenanceWorkerId)
      {
        setTasks([])
        setTotalCount(0)
        setTaskError("Maintenance worker ID missing.")
        return
      }

      const id = Number(maintenanceWorkerId)

      if(Number.isNaN(id))
      {
        setTasks([])
        setTotalCount(0)
        setTaskError("Invalid maintenance worker ID.")
        return
      }
      try
      {
        setTaskError("")

        const response = await fetch(`/api/Admin/maintenance-worker/${id}/tasks?page=${page}&pageSize=${pageSize}`,
          {
            headers: {
              Authorization: `Bearer ${getToken()}`
            }
          }
        )

        if(!response.ok)
        {
          setTaskError("Unable to load maintenance tasks.")
          return
        }

        const data = await response.json()

        setTasks(data.items)

        setTotalCount(data.totalCount)
      }
      catch
      {
        setTaskError("Unable to connect to server.")
      }
    }
    getWorkerTasks()
  }, [page, maintenanceWorkerId])

  const totalPages = Math.ceil(totalCount / pageSize)
  return (
    <div className="maintenance-worker-task-page">
    <h1>Tasks</h1>
    {taskError && (
      <p className="error-message">
        {taskError}
      </p>
    )}

    <button className="task-back-to-dash" onClick={() => navigate("/admin-dashboard")} type="button">Back To Dashboard</button>
    <div className="worker-task-cards">
    {tasks.map(task => (
      <div className="worker-task-card" key={task.maintenanceTaskId}>
        <p>Worker ID: {task.maintenanceWorkerId}</p>
        <p>Task ID: {task.maintenanceTaskId}</p>
        <p>Car: {task.make} {task.model}</p>
        <p>Task Status: {MaintenanceTaskStatus[task.maintenanceTaskStatus]}</p>
        <p>Report Status: {MaintenanceReportStatus[task.maintenanceReportStatus]}</p>
        <p>Inspection Result: {InspectionResult[task.inspectionResult]}</p>
        <button className="view-report" onClick={() => navigate(`/admin-maintenance-report/${task.maintenanceTaskId}`)} type="button">View Report</button>
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

export default AdminMaintenanceWorkerTasks
import { useEffect, useState } from "react"
import { getToken } from "../utils/auth"
import { useNavigate } from "react-router-dom"
import './MaintenanceTasks.css'

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

enum MaintenanceTaskStatus
{
  AwaitingAssignment,
  Pending,
  InProgress,
  Completed
}

enum InspectionResult
{
  Pending,
  Pass,
  Warning,
  Fail
}

enum MaintenanceReportStatus
{
  Pending,
  InProgress,
  Completed
}

function MaintenanceTasks() {
  const [tasks, setTasks] = useState<Task[]>([])
  const [message, setMessage] = useState("")
  const navigate = useNavigate()
  const [totalCount, setTotalCount] = useState(0)
  const [page, setPage] = useState(1)

  const pageSize = 10

  useEffect(() => {
    const getTasks = async () => {
      try
      {
        const response = await fetch(`/api/Maintenance/my-tasks?page=${page}&pageSize=${pageSize}`, {
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

        setTasks(data.items)

        setTotalCount(data.totalCount)
      }
      catch
      {
        setMessage("Unable to connect to server.")
      }
    }
    getTasks()
  }, [page])

  const totalPages = Math.ceil(totalCount / pageSize)

  const startTask = async (taskId: number) => {
    try
    {
      const response = await fetch(`/api/Maintenance/tasks/${taskId}/start`,
        {
          method: "POST",
          headers: {
            Authorization: `Bearer ${getToken()}`
          }
        }
      )

      if(!response.ok)
      {
        setMessage("Unable to start task")
        return
      }

      navigate(`/maintenance-report/${taskId}`)
    }
    catch
    {
      setMessage("Unable to connect to server.")
    }
  }
  return (
    <div className="mytasks-container">
      <h1>My Tasks</h1>

      {message && (
      <p className="error-message">
        {message}
      </p>
      )}

    <button className="task-back-to-dash" onClick={() => navigate("/maintenance-dashboard")} type="button">Back To Dashboard</button>

    <div className="task-cards">
      {tasks.map(task => (
        <div className="task-card" key={task.maintenanceTaskId}>
          <p>Task ID: {task.maintenanceTaskId}</p>
          <p>Car: {task.make} {task.model}</p>
          <p>Task Status: {MaintenanceTaskStatus[task.maintenanceTaskStatus]}</p>
          <p>Inspection Result: {InspectionResult[task.inspectionResult]}</p>
          <p>Report Status: {MaintenanceReportStatus[task.maintenanceReportStatus]}</p>
          {task.maintenanceTaskStatus === MaintenanceTaskStatus.Pending && (
          <button
            className="start-task-button"
            onClick={() => startTask(task.maintenanceTaskId)}
          >
            Start Task
          </button>
        )}

        {task.maintenanceTaskStatus === MaintenanceTaskStatus.InProgress && (
          <button
            className="start-task-button"
            onClick={() => navigate(`/maintenance-report/${task.maintenanceTaskId}`)}
          >
            Continue Task
          </button>
        )}
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

export default MaintenanceTasks
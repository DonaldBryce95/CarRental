import { useEffect, useState } from "react"
import { getToken } from "../utils/auth"
import { useNavigate } from "react-router-dom"
import './AdminUnderMaintenanceCars.css'

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

function AdminUnderMaintenanceCars() {

  const [page, setPage] = useState(1)
  const [tasks, setTasks] = useState<Task[]>([])
  const [totalCount, setTotalCount] = useState(0)
  const navigate = useNavigate()
  const [error, setError] = useState("")

  const pageSize = 6

  useEffect(() => {
      const getMaintenanceTasks = async () => {
        try
        {
          setError("")
          const response = await fetch(`/api/Admin/maintenance/repairs/tasks?page=${page}&pageSize=${pageSize}`,
          {
            headers: {
              Authorization: `Bearer ${getToken()}`
            }
          }
        )

        if(!response.ok)
        {
          setError("Unable to load available cars under maintenance.")
          return
        }

        const data = await response.json()

        setTasks(data.items)

        setTotalCount(data.totalCount)

        setError("")
      }
      catch
      {
        setError("Unable to connect to server.")
      }
    }
    getMaintenanceTasks()
  }, [page])

  const handleRepair = async (carId: number) => {
    try
    {
      setError("")
      const response = await fetch(`/api/Admin/maintenance/complete/repair/${carId}`,
        {
          method: "PUT",
          headers: {
            Authorization: `Bearer ${getToken()}`
          }
        }
      )

      if(!response.ok)
      {
        setError("Unable to repair car.")
        return
      }

      alert("Car repair completed!")

      navigate("/admin-dashboard")
    }

    catch
    {
      setError("Unable to connect to server.")
      return
    }
  }

  const totalPages = Math.ceil(totalCount / pageSize)
  return (
    <div className="maintenance-car-repairs">
    <h1>Cars Under Maintenance</h1>
    {error && (
      <p className="error-message">{error}</p>
    )}
    <button className="repair-back-to-dash" onClick={() => navigate("/admin-dashboard")} type="button">Back To Dashboard</button>
    <div className="repair-task-cards">
    {tasks.map(task => (
    <div className="repair-task-card" key={task.maintenanceTaskId}>
        <p>Worker ID: {task.maintenanceWorkerId}</p>
        <p>Task ID: {task.maintenanceTaskId}</p>
        <p>Car: {task.make} {task.model}</p>
        <p>Task Status: {MaintenanceTaskStatus[task.maintenanceTaskStatus]}</p>
        <p>Report Status: {MaintenanceReportStatus[task.maintenanceReportStatus]}</p>
        <p>Inspection Result: {InspectionResult[task.inspectionResult]}</p>
        <button className="submit-repair" type="button" onClick={() => handleRepair(task.carId)}>Repair Completed
        </button>
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
)}

export default AdminUnderMaintenanceCars
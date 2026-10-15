import { useEffect, useState } from "react"
import { useParams, useNavigate } from "react-router-dom"
import { getToken } from "../utils/auth"
import './AdminMaintenanceReport.css'

type Report = {
  maintenanceWorkerId: number
  firstName: string
  lastName: string
  make: string
  model: string
  overallResult: number
  tyres: number
  lights: number
  brakes: number
  engine: number
  battery: number
  airConditioning: number
  warningLights: number
  bodyWork: number
  glass: number
  interiorCondition: number
  fuelLevel: number
}

enum InspectionResult {
  Pending,
  Pass,
  Warning,
  Fail
}

function AdminMaintenanceReport () {
  const {maintenanceTaskId} = useParams()
  const [report, setReport] = useState<Report | null>(null)
  const navigate = useNavigate()
  const [error, setError] = useState("")

  useEffect(() => {
    const getReport = async () => {
      try
      {
        if (!maintenanceTaskId) {
          setError("Maintenance task ID missing.")
          return
        }

        const id = Number(maintenanceTaskId)

        if (Number.isNaN(id)) {
          setError("Invalid maintenance task ID.")
          return
        }

        const response = await fetch(`/api/Admin/maintenance/report/${id}`,
          {
            headers: {
              Authorization: `Bearer ${getToken()}`
            }
          }
        )

        if(!response.ok)
        {
          setError("Unable to load maintenance reports.")
          return
        }

        const data = await response.json()

        setReport(data)

        setError("")
      }
      catch
      {
        setError("Unable to connect to server.")
      }
    }

    getReport()
  }, [maintenanceTaskId])
  return (
    <div className="maintenance-report-container">
    <h1>Reports</h1>

    {error && (
      <p className="error-message">
        {error}
      </p>
    )}

      {report && (
        <>
        <div className="report-form">
          <div className="worker-details">
            <p>First Name: {report.firstName}</p>
            <p>Last Name: {report.lastName}</p>
        </div>

        <div className="car-details">
          <p>Model: {report.model}</p>
          <p>Make: {report.make}</p>
          <p>
            Inspection Result:{" "}
            {InspectionResult[report.overallResult]}
          </p>
        </div>
        </div>

        <div className="inspection-form">

          <h2>VEHICLE INSPECTION</h2>

          <div className="inspection-header">
            <span>Inspection Item</span>
            <span className="pass">Pass</span>
            <span className="warning">Warning</span>
            <span className="fail">Fail</span>
          </div>

          <div className="inspection-row">
            <span>Tyres:</span>
            <span>{report.tyres === InspectionResult.Pass && "✓"}</span>
            <span>{report.tyres === InspectionResult.Warning && "✓"}</span>
            <span>{report.tyres === InspectionResult.Fail && "✓"}</span>
          </div>

          <div className="inspection-row">
            <span>Brakes:</span>
            <span>{report.brakes === InspectionResult.Pass && "✓"}</span>
            <span>{report.brakes === InspectionResult.Warning && "✓"}</span>
            <span>{report.brakes === InspectionResult.Fail && "✓"}</span>
          </div>

          <div className="inspection-row">
            <span>Engine:</span>
            <span>{report.engine === InspectionResult.Pass && "✓"}</span>
            <span>{report.engine === InspectionResult.Warning && "✓"}</span>
            <span>{report.engine === InspectionResult.Fail && "✓"}</span>
          </div>

          <div className="inspection-row">
            <span>Lights:</span>
            <span>{report.lights === InspectionResult.Pass && "✓"}</span>
            <span>{report.lights === InspectionResult.Warning && "✓"}</span>
            <span>{report.lights === InspectionResult.Fail && "✓"}</span>
          </div>

          <div className="inspection-row">
            <span>Battery:</span>
            <span>{report.battery === InspectionResult.Pass && "✓"}</span>
            <span>{report.battery === InspectionResult.Warning && "✓"}</span>
            <span>{report.battery === InspectionResult.Fail && "✓"}</span>
          </div>

          <div className="inspection-row">
            <span>Air Conditioning:</span>
            <span>{report.airConditioning === InspectionResult.Pass && "✓"}</span>
            <span>{report.airConditioning === InspectionResult.Warning && "✓"}</span>
            <span>{report.airConditioning === InspectionResult.Fail && "✓"}</span>
          </div>

          <div className="inspection-row">
            <span>Warning Lights:</span>
            <span>{report.warningLights === InspectionResult.Pass && "✓"}</span>
            <span>{report.warningLights === InspectionResult.Warning && "✓"}</span>
            <span>{report.warningLights === InspectionResult.Fail && "✓"}</span>
          </div>

          <div className="inspection-row">
            <span>Interior Condition:</span>
            <span>{report.interiorCondition === InspectionResult.Pass && "✓"}</span>
            <span>{report.interiorCondition === InspectionResult.Warning && "✓"}</span>
            <span>{report.interiorCondition === InspectionResult.Fail && "✓"}</span>
          </div>
          <div className="inspection-row">
            <span>Fuel Level:</span>
            <span>{report.fuelLevel === InspectionResult.Pass && "✓"}</span>
            <span>{report.fuelLevel === InspectionResult.Warning && "✓"}</span>
            <span>{report.fuelLevel === InspectionResult.Fail && "✓"}</span>
          </div>

          <div className="inspection-row">
            <span>Body Work:</span>
            <span>{report.bodyWork === InspectionResult.Pass && "✓"}</span>
            <span>{report.bodyWork === InspectionResult.Warning && "✓"}</span>
            <span>{report.bodyWork === InspectionResult.Fail && "✓"}</span>
          </div>

          <div className="inspection-row">
            <span>Glass:</span>
            <span>{report.glass === InspectionResult.Pass && "✓"}</span>
            <span>{report.glass === InspectionResult.Warning && "✓"}</span>
            <span>{report.glass === InspectionResult.Fail && "✓"}</span>
          </div>
        </div>
        <div className="back-button-container">
          <button className="back-to-tasks" onClick={() => navigate(`/admin/maintenance/worker/tasks/${report.maintenanceWorkerId}`)}>Back To Tasks</button>
        </div>
      </>
    )}
  </div>
  )
}

export default AdminMaintenanceReport
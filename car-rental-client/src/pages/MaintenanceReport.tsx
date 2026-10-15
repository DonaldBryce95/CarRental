import { useParams, useNavigate } from "react-router-dom"
import { useEffect, useState } from "react"
import { getToken } from "../utils/auth"
import './MaintenanceReport.css'

type Report = {
  firstName: string
  lastName: string
  make: string
  model: string
  overallResult: number
}

enum InspectionResult {
  Pending,
  Pass,
  Warning,
  Fail
}

type InspectionConditions = {
  tyres: InspectionResult | null
  lights: InspectionResult | null
  brakes: InspectionResult | null
  engine: InspectionResult | null
  battery: InspectionResult | null
  airConditioning: InspectionResult | null
  warningLights: InspectionResult | null
  bodyWork: InspectionResult | null
  glass: InspectionResult | null
  interiorCondition: InspectionResult | null
  fuelLevel: InspectionResult | null
}

function MaintenanceReport() {

  const navigate = useNavigate()
  const {taskId} = useParams()
  const [message, setMessage] = useState("")
  const [report, setReport] = useState<Report | null>(null)
  const [inspectionCondition, setInspectionCondition] = useState<InspectionConditions>({ tyres: null, lights: null, brakes: null, engine: null, battery: null, airConditioning: null, warningLights: null, bodyWork: null, glass: null, interiorCondition: null, fuelLevel: null})

  useEffect(() => {
    const getReport = async () => {
      try
      {
        const response = await fetch(`/api/Maintenance/reports/${taskId}`,
          {
            headers: {
              Authorization: `Bearer ${getToken()}`
            }
          }
        )

        if(!response.ok)
        {
          setMessage("Unable to get report")
          return
        }

        const data = await response.json()

        setReport(data)
      }
      catch
      {
        setMessage("Unable to connect to server.")
      }
    }

    getReport()
  }, [taskId])

  const handleSubmit = async () => {
    const tyres = inspectionCondition.tyres
    const lights = inspectionCondition.lights
    const brakes = inspectionCondition.brakes
    const engine = inspectionCondition.engine
    const battery = inspectionCondition.battery
    const airConditioning = inspectionCondition.airConditioning
    const warningLights = inspectionCondition.warningLights
    const bodyWork = inspectionCondition.bodyWork
    const glass = inspectionCondition.glass
    const interiorCondition = inspectionCondition.interiorCondition
    const fuelLevel = inspectionCondition.fuelLevel

    const conditions = [tyres, lights, brakes, engine, battery, airConditioning, warningLights, bodyWork, glass, interiorCondition, fuelLevel ]
    let overallResult = InspectionResult.Pass

    if(conditions.some(x => x == null))
    {
      setMessage("All fields need to be selected")
      return
    }

    if(conditions.some(x => x === InspectionResult.Fail))
    {
      overallResult = InspectionResult.Fail
    }

    const inspectionData = {
      tyres,
      lights,
      brakes,
      engine,
      battery,
      airConditioning,
      warningLights,
      bodyWork,
      glass,
      interiorCondition,
      fuelLevel,
      overallResult
    }

    try
    {
      const response = await fetch(`/api/Maintenance/maintenance/report/${taskId}/submit`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${getToken()}`
          },
          body: JSON.stringify(inspectionData)
        }
      )

      if(!response.ok)
      {
        setMessage("Unable to submit report")
        return
      }

      alert('Report Submitted!')
      navigate("/maintenance-tasks")
    }
    catch
    {
      setMessage("Unable to connect to server.")
    }
  }
  return (
  <div className="maintenance-report-container">
    <h1>Maintenance Report</h1>

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
            <span>Tyres</span>

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  tyres: InspectionResult.Pass
                })
              }
              type="radio"
              name="tyres"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  tyres: InspectionResult.Warning
                })
              }
              type="radio"
              name="tyres"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  tyres: InspectionResult.Fail
                })
              }
              type="radio"
              name="tyres"
            />
          </div>

          <div className="inspection-row">
            <span>Lights</span>

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  lights: InspectionResult.Pass
                })
              }
              type="radio"
              name="lights"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  lights: InspectionResult.Warning
                })
              }
              type="radio"
              name="lights"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  lights: InspectionResult.Fail
                })
              }
              type="radio"
              name="lights"
            />
          </div>

          <div className="inspection-row">
            <span>Brakes</span>

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  brakes: InspectionResult.Pass
                })
              }
              type="radio"
              name="brakes"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  brakes: InspectionResult.Warning
                })
              }
              type="radio"
              name="brakes"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  brakes: InspectionResult.Fail
                })
              }
              type="radio"
              name="brakes"
            />
          </div>

          <div className="inspection-row">
            <span>Engine</span>

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  engine: InspectionResult.Pass
                })
              }
              type="radio"
              name="engine"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  engine: InspectionResult.Warning
                })
              }
              type="radio"
              name="engine"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  engine: InspectionResult.Fail
                })
              }
              type="radio"
              name="engine"
            />
          </div>
          <div className="inspection-row">
            <span>Battery</span>

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  battery: InspectionResult.Pass
                })
              }
              type="radio"
              name="battery"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  battery: InspectionResult.Warning
                })
              }
              type="radio"
              name="battery"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  battery: InspectionResult.Fail
                })
              }
              type="radio"
              name="battery"
            />
          </div>

          <div className="inspection-row">
            <span>Air Conditioning</span>

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  airConditioning: InspectionResult.Pass
                })
              }
              type="radio"
              name="airConditioning"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  airConditioning: InspectionResult.Warning
                })
              }
              type="radio"
              name="airConditioning"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  airConditioning: InspectionResult.Fail
                })
              }
              type="radio"
              name="airConditioning"
            />
          </div>
          <div className="inspection-row">
            <span>Warning Lights</span>

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  warningLights: InspectionResult.Pass
                })
              }
              type="radio"
              name="warningLights"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  warningLights: InspectionResult.Warning
                })
              }
              type="radio"
              name="warningLights"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  warningLights: InspectionResult.Fail
                })
              }
              type="radio"
              name="warningLights"
            />
          </div>
          <div className="inspection-row">
            <span>Body Work</span>

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  bodyWork: InspectionResult.Pass
                })
              }
              type="radio"
              name="bodyWork"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  bodyWork: InspectionResult.Warning
                })
              }
              type="radio"
              name="bodyWork"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  bodyWork: InspectionResult.Fail
                })
              }
              type="radio"
              name="bodyWork"
            />
          </div>
          <div className="inspection-row">
            <span>Glass</span>

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  glass: InspectionResult.Pass
                })
              }
              type="radio"
              name="glass"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  glass: InspectionResult.Warning
                })
              }
              type="radio"
              name="glass"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  glass: InspectionResult.Fail
                })
              }
              type="radio"
              name="glass"
            />
          </div>
          <div className="inspection-row">
            <span>Interior Condition</span>

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  interiorCondition: InspectionResult.Pass
                })
              }
              type="radio"
              name="interiorCondition"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  interiorCondition: InspectionResult.Warning
                })
              }
              type="radio"
              name="interiorCondition"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  interiorCondition: InspectionResult.Fail
                })
              }
              type="radio"
              name="interiorCondition"
            />
          </div>
          <div className="inspection-row">
            <span>Fuel Level</span>

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  fuelLevel: InspectionResult.Pass
                })
              }
              type="radio"
              name="fuelLevel"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  fuelLevel: InspectionResult.Warning
                })
              }
              type="radio"
              name="fuelLevel"
            />

            <input
              onChange={() =>
                setInspectionCondition({
                  ...inspectionCondition,
                  fuelLevel: InspectionResult.Fail
                })
              }
              type="radio"
              name="fuelLevel"
            />
          </div>
          {message && <p>{message}</p>}
          <button className="submit-button" onClick={handleSubmit}>Submit</button>
        </div>
      </>
    )}
  </div>
)
}

export default MaintenanceReport
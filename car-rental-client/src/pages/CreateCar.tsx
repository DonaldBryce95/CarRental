import { useEffect, useState } from "react"
import { getToken } from "../utils/auth"
import { useNavigate } from "react-router-dom"
import "./CreateCar.css"

type Branch = {
  branchId: number
  branchName: string
  address: string
  phoneNumber: string
  openingHours: string
}

type VehicleType = {
  vehicleTypeId: number
  vehicleCategories: number
  pricePerDay: number
}

enum VehicleCategories {
  Economy,
  Compact,
  SUV,
  Premium
}

function CreateCar () {
  const [message, setMessage] = useState("")
  const [branches, setBranches] = useState<Branch[]>([])
  const [vehicleTypes, setVehicleTypes] = useState<VehicleType[]>([])
  const [make, setMake] = useState("")
  const [model, setModel] = useState("")
  const [engineCapacity, setEngineCapacity] = useState(0)
  const [numberOfDoors, setNumberOfDoors] = useState(0)
  const [transmissionType, setTransmissionType] = useState(0)
  const [carStatus, setCarStatus] = useState(0)
  const [hasAC, setHasAC] = useState(false)
  const [branchId, setBranchId] = useState(0)
  const [vehicleTypeId, setVehicleTypeId] = useState(0)

  const navigate = useNavigate()

  useEffect (() => {
    const getBranches = async () => {
      try
      {
        const response = await fetch(`/api/Admin/branches`,
          {
            headers: {
              Authorization: `Bearer ${getToken()}`
            }
          }
        )

        if(!response.ok)
        {
          setMessage("Unable to get branches")
          return
        }

        const data = await response.json()

        setBranches(data)
      }
      catch
      {
        setMessage("Unable to connect to server.")
      }
    }
    getBranches()
  }, [])

  useEffect(() => {
    const getVehicleTypes = async () => {
      try
      {
        const response = await fetch(`/api/Admin/vehicle-types`,
          {
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${getToken()}`
            }
          }
        )

        if(!response.ok)
        {
          setMessage("Unable to get vehicle types")
          return
        }

        const data = await response.json()

        setVehicleTypes(data)
      }
      catch
      {
        setMessage("Unable to connect to server.")
      }
    }
    getVehicleTypes()
  }, [])

  const handleCreate = async (event: React.SubmitEvent) => {
    event.preventDefault()

    setMessage("")

    if(vehicleTypeId === 0)
    {
      setMessage("Please select a vehicle type")
      return
    }

    if(branchId === 0)
    {
      setMessage("Please select a branch")
      return
    }

    try
    {
      const response = await fetch(`/api/Admin/cars`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${getToken()}`
        },
        body: JSON.stringify({
          branchId,
          vehicleTypeId,
          make,
          model,
          numberOfDoors,
          engineCapacity,
          transmissionType,
          carStatus,
          hasAC
        })
      })

      if(!response.ok)
      {
        setMessage("Unable to create car")
        console.log("Create car failed:", response.status)
        console.log(await response.text())
        return
      }

      setMessage("Car created successfully")
    }
    catch
    {
      setMessage("Unable to connect to server.")
    }
  }
  return (
    <div className="create-car-container">
      <h1>Create Car</h1>

      {message && (
      <p className="error-message">
        {message}
      </p>
      )}
      <form onSubmit={handleCreate}>
        <label>Branch</label>
        <select onChange={(e) => setBranchId(Number(e.target.value))}>
        <option value="0">Select a branch</option>

        {branches.map(branch => {
        return (
          <option key={branch.branchId} value={branch.branchId}>
          {branch.branchName}
          </option>
          )
        })}
        </select>
        <label>Make</label>
        <input onChange={(e) => {setMake(e.target.value)}}>
        </input>
        <label>Model</label>
        <input onChange={(e) => {setModel(e.target.value)}}>
        </input>
        <label>VehicleType</label>
        <select onChange={(e) => setVehicleTypeId(Number(e.target.value))}>
          <option value="0">Select a vehicle type</option>
          {vehicleTypes.map(vehicleType => {
            return (
            <option key={vehicleType.vehicleTypeId} value={vehicleType.vehicleTypeId}>{VehicleCategories[vehicleType.vehicleCategories]}</option>
            )
          })}
        </select>
        <label>Number Of Doors</label>
        <select onChange={(e) => {setNumberOfDoors(Number(e.target.value))}}>
          <option value="2">2</option>
          <option value="3">3</option>
          <option value="4">4</option>
          <option value="5">5</option>
        </select>
        <label>Transmission Type</label>
        <select onChange={(e) => setTransmissionType(Number(e.target.value))}>
          <option value="0">Manual</option>
          <option value="1">Automatic</option>
        </select>
        <label>Engine Capacity</label>
          <input
            type="number"
            step="0.1"
            onChange={(e) => setEngineCapacity(Number(e.target.value))}
          />
          <label>Car Status</label>
          <select onChange={(e) => setCarStatus(Number(e.target.value))}>
            <option value="0">Available</option>
            <option value="1">Unavailable</option>
          </select>
        <label>
          <input
            type="checkbox"
            onChange={(e) => setHasAC(e.target.checked)}
          />
          Has AC
        </label>
        <button type="submit" className="create-button">
        Create Car
        </button>
        <button onClick={() => navigate("/admin/cars/all")} type="button" className="back-button">
        Back
        </button>
      </form>
    </div>
  )
}

export default CreateCar
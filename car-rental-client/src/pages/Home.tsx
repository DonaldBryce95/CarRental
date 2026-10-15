import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import './Home.css'

type Branch = {
  branchId: number
  branchName: string
  address: string
  phoneNumber: string
  openingHours: string
}

type Fleet = {
  make: string
  model: string
  engineCapacity: number
  vehicleCategories: number
}

enum VehicleCategories {
  Economy,
  Compact,
  SUV,
  Premium
}

const carImages: Record<string, string[]> = {
  "Hyundai i20": [
    "/cars/hyundai-i20-prix-neuf-neuf.jpg",
    "/cars/hyundai-i20-2021-03.jpg"
  ],
  "Ford Focus": [
    "/cars/Frozen-White.png",
    "/cars/2019-ford-focus-trend-trim.jpg"
  ],
  "Kia Sportage": [
    "/cars/ks-white.jpg",
    "/cars/OIP.webp"
  ],
  "Mercedes-Benz C-Class": [
    "/cars/Mercfront.webp"
  ],
  "Volkswagen Golf" : [
    "/cars/vw-golf-front.webp",
    "/cars/vw-golf-back.jpg"
  ],
  "Toyota RAV4" : [
    "/cars/toyota-rav-4-front.webp",
    "/cars/toyota-rav4-back.jpg"
  ],
  "Toyota Yaris": [
    "/cars/yaris-front.webp",
    "/cars/yaris-back.webp"
  ],
  "BMW 3 Series": [
    "/cars/bmw-3-front.avif",
    "/cars/bmw3-back.webp"
  ],
  "Dodge Ram": [
    "/cars/ram-front.webp",
    "/cars/ram-back.webp"
  ],
    "Audi A6": [
    "/cars/2022-Audi-A6-white-full_color-driver_side_front_quarter.avif",
    "/cars/A6 back.webp"
  ],
    "BMW 5 Series": [
    "/cars/bmw 5 front.webp",
    "/cars/bmw 5 rear.jpg"
  ],
    "Chevrolet Onix": [
    "/cars/onix front.webp",
    "/cars/onix-hatch-rear.jpg"
  ],
    "Chevrolet Tracker": [
    "/cars/tracker front.png",
    "/cars/tracker-back.webp"
  ],
    "Fiat Argo": [
    "/cars/argo-front.webp",
    "/cars/argo-rear.webp"
  ],
    "Honda Civic": [
    "/cars/civic-front.webp",
    "/cars/civic-rear.webp"
  ],
    "Hyundai Creta": [
    "/cars/creta front.webp",
    "/cars/creta rear.webp"
  ],
    "Hyundai HB20": [
    "/cars/hb20 front.jpg",
    "/cars/hb20 rear.jpg"
  ],
    "Jeep Compass": [
    "/cars/jeepcompass-front.webp",
    "/cars/jeepcompass-rear.webp"
  ],
    "Mercedes-Benz E-Class": [
    "/cars/eclass front.webp",
    "/cars/eclass rear.jpg"
  ],
    "Peugeot 208": [
    "/cars/208 front.webp",
    "/cars/208 rear.webp"
  ],
    "Renault Kwid": [
    "/cars/kwid front.webp",
    "/cars/kwid rear.webp"
  ],
    "Toyota Corolla": [
    "/cars/corolla-front.webp",
    "/cars/corolla-rear.webp"
  ],
    "Volkswagen Polo": [
    "/cars/polo-front.webp",
    "/cars/polo-rear.webp"
  ],
    "Volkswagen T-Cross": [
    "/cars/t-cross-front.webp",
    "/cars/t-cross-rear.webp"
  ],
    "Volvo S90": [
    "/cars/s90-front.webp",
    "/cars/s90-rear.webp"
  ]
}

function Home() {
  const [branches, setBranches] = useState<Branch[]>([])
  const [fleet, setFleet] = useState<Fleet[]>([])
  const [fleetPage, setFleetPage] = useState(0)

  const navigate = useNavigate()

  const [showBranches, setShowBranches] = useState(false)
  const [selectedBranchId, setSelectedBranchId] = useState<number | null>(null)
  const [selectedBranchName, setSelectedBranchName] = useState<string | null>(null)
  const [returnBranchName, setReturnBranchName] = useState("")
  const [userInput, setUserInput] = useState("")
  const [hoveredBranchId, setHoveredBranchId] = useState<number | null>(null)

  const filteredBranches = branches.filter(branch =>
    branch.branchName.toLowerCase().includes(userInput.toLowerCase())
  )

  const [pickupDate, setPickupDate] = useState("")
  const [pickupTime, setPickupTime] = useState("")

  const [returnBranchId, setReturnBranchId] = useState<number | null>(null)
  const [returnUserInput, setReturnUserInput] = useState("")
  const [showReturnBranches, setShowReturnBranches] = useState(false)
  const [hoveredReturnBranchId, setHoveredReturnBranchId] = useState<number | null>(null)

  const filteredReturnBranches = branches.filter(branch =>
    branch.branchName.toLowerCase().includes(returnUserInput.toLowerCase())
  )

  const [returnDate, setReturnDate] = useState("")
  const [returnTime, setReturnTime] = useState("")
  const [pickupDateError, setPickupDateError] = useState("")
  const [returnDateError, setReturnDateError] = useState("")
  const [returnTimeError, setReturnTimeError] = useState("")
  const [pickupTimeError, setPickupTimeError] = useState("")
  const selectedPickupDate = new Date(pickupDate)
  const selectedReturnDate = new Date(returnDate)
  const today = new Date()
  today.setHours(0, 0, 0, 0);
  const selectedPickupTime = new Date(`1970-01-01T${pickupTime}`)
  const selectedReturnTime = new Date(`1970-01-01T${returnTime}`)
  const [pickupBranchError, setPickupBranchError] = useState("")
  const [returnBranchError, setReturnBranchError] = useState("")
  const [branchError, setBranchError] = useState("")
  const [fleetError, setFleetError] = useState("")

  const handleSearch = () => {

    if (selectedBranchId === null) {
      setPickupBranchError("please enter a pick up branch.")
      return
    }

    if(!pickupDate) {
      setPickupDateError("Please select a pick up date.")
      return
    }

    if(selectedPickupDate < today)
    {
      setPickupDateError("Pick up date cannot be before today.")
      return
    }

    if(!pickupTime) {
      setPickupTimeError("Please select a pick up time.")
      return
    }

    if (returnBranchId === null) {
      setReturnBranchError("Please enter a return branch.")
      return
    }

    if(!returnDate) {
      setReturnDateError("Please select a return date.")
      return
    }

    if(selectedReturnDate < selectedPickupDate)
    {
      setReturnDateError("Return date cannot be before the pick up date.")
      return
    }

    if(!returnTime) {
      setReturnTimeError("Please select a return time.")
      return
    }

    if (selectedPickupDate.getTime() === selectedReturnDate.getTime() &&
    selectedPickupTime >= selectedReturnTime)
    {
      setReturnTimeError("Return time must be after pick up time.")
      return
    }

    navigate("/available-cars", {
      state: {
      selectedBranchId,
      selectedBranchName,
      returnBranchId,
      returnBranchName,
      pickupDate,
      pickupTime,
      returnDate,
      returnTime
    }
    })
  }

  useEffect(() => {
    const getBranches = async () => {
      try
      {
        const response = await fetch(
        `/api/Customer/branches`
        )

        if (!response.ok) {
          setBranchError("Unable to load branches.")
          return
        }

        const data = await response.json()

        setBranches(data)
        setBranchError("")
      }
      catch
      {
        setBranchError("Unable to connect to server.")
      }
    }

    getBranches()
  }, [])

  useEffect(() => {
    const getFleet = async () => {
      try
      {
        const response = await fetch(`/api/Customer/cars`
        )

        if(!response.ok)
        {
          setFleetError("Unable to load fleet.")
          return
        }

        const data = await response.json()

        setFleet(data)
        setFleetError("")
      }
      catch
      {
        setFleetError("Unable to connect to server.")
      }
    }

    getFleet()
  }, [])

  return (
    <div className='home-page'>

      <div className="searchbar-container">

        <div className="pickup-container">

          <div className="search-input">
            {pickupBranchError && (
            <p className="validation-error">{pickupBranchError}</p>
            )}
            <input
              value={userInput}
              type="text"
              placeholder="Where would you like to pick up your car?"
              onChange={(e) => {
                setUserInput(e.target.value)
                setShowBranches(e.target.value.length > 0)
                setPickupBranchError("")
                setSelectedBranchId(null)
                setSelectedBranchName(null)
              }}
            />
          </div>

          {showBranches && (
            <div className="branch-selection">

              <div className="branch-dropdown">

                {filteredBranches.map(branch => (
                  <div
                    className="branch-option"
                    key={branch.branchId}
                    onMouseEnter={() =>
                      setHoveredBranchId(branch.branchId)
                    }
                    onMouseLeave={() =>
                      setHoveredBranchId(null)
                    }
                    onClick={() => {
                      setSelectedBranchId(branch.branchId)
                      setShowBranches(false)
                      setSelectedBranchName(branch.branchName)
                      setUserInput(branch.branchName)
                    }}
                  >
                    <div className="branch-name">
                      <span>{branch.branchName}</span>
                      <i className="bi bi-chevron-right"></i>
                    </div>
                  </div>
                ))}

              </div>

              {hoveredBranchId && (
                <div className="branch-info">

                  {branches
                    .filter(branch =>
                      branch.branchId === hoveredBranchId
                    )
                    .map(branch => (
                      <div key={branch.branchId}>
                        <h3>{branch.branchName}</h3>
                        <p>📍 {branch.address}</p>
                        <p>☎ {branch.phoneNumber}</p>
                        <p>🕐 {branch.openingHours}</p>
                      </div>
                    ))}

                </div>
              )}

            </div>
          )}

        </div>

        <div className="search-input">
          {pickupDateError && (
          <p className="validation-error">{pickupDateError}</p>
          )}
          <input
            type="date"
            value={pickupDate}
            onChange={(e) => {
              setPickupDate(e.target.value)
              setPickupDateError("")
            }}
          />
        </div>

        <div className="search-input">
          {pickupTimeError && (
          <p className="validation-error">{pickupTimeError}</p>
          )}
          <input
            type="time"
            value={pickupTime}
            onChange={(e) => {
              setPickupTime(e.target.value)
              setPickupTimeError("")
            }}
          />
        </div>

        <button onClick={handleSearch} className="search-button">
          Search
        </button>

      </div>

      {selectedBranchId && pickupDate && pickupTime && (

        <div className="return-container">
          {branchError && (
            <p className="error-message">
              {branchError}
            </p>
          )}

          <div className="pickup-container">

            <div className="search-input">
              {returnBranchError && (
              <p className="validation-error">{returnBranchError}</p>
              )}
              <input
                value={returnUserInput}
                type="text"
                placeholder="Where would you like to return your car?"
                onChange={(e) => {
                  setReturnBranchId(null)
                  setReturnBranchName("")
                  setReturnUserInput(e.target.value)
                  setShowReturnBranches(
                    e.target.value.length > 0
                  )
                  setReturnBranchError("")
                }}
              />
            </div>

            {showReturnBranches && (
              <div className="branch-selection">
                <div className="branch-dropdown">

                  {filteredReturnBranches.map(branch => (
                    <div
                      className="branch-option"
                      key={branch.branchId}
                      onMouseEnter={() =>
                        setHoveredReturnBranchId(
                          branch.branchId
                        )
                      }
                      onMouseLeave={() =>
                        setHoveredReturnBranchId(null)
                      }
                      onClick={() => {
                        setReturnBranchId(branch.branchId)
                        setReturnBranchName(branch.branchName)
                        setReturnUserInput(branch.branchName)
                        setShowReturnBranches(false)
                      }}
                    >
                      <div className="branch-name">
                        <span>{branch.branchName}</span>
                        <i className="bi bi-chevron-right"></i>
                      </div>
                    </div>
                  ))}

                </div>

                {hoveredReturnBranchId && (
                  <div className="branch-info">

                    {branches
                      .filter(branch =>
                        branch.branchId ===
                        hoveredReturnBranchId
                      )
                      .map(branch => (
                        <div key={branch.branchId}>
                          <h3>{branch.branchName}</h3>
                          <p>📍 {branch.address}</p>
                          <p>☎ {branch.phoneNumber}</p>
                          <p>🕐 {branch.openingHours}</p>
                        </div>
                      ))}

                  </div>
                )}

              </div>
            )}

          </div>

          <div className="search-input">
            {returnDateError && (
            <p className="validation-error">{returnDateError}</p>
            )}
            <input
              type="date"
              value={returnDate}
              onChange={(e) => {
                setReturnDate(e.target.value)
                setReturnDateError("")
              }}
            />
          </div>

          <div className="search-input">
            {returnTimeError && (
            <p className="validation-error">{returnTimeError}</p>
            )}
            <input
              type="time"
              value={returnTime}
              onChange={(e) => {
                setReturnTime(e.target.value)
                setReturnTimeError("")
              }}
            />
          </div>

        </div>

      )}

    <div className="fleet-section">

      <h2>Our Fleet</h2>

      {fleetError && (
        <p className="error-message">
          {fleetError}
        </p>
      )}

      <div className="fleet-carousel">

        <button
          className="fleet-arrow"
          onClick={() => setFleetPage(fleetPage - 1)}
          disabled={fleetPage === 0}
        >
          ←
        </button>
        <div className="fleet-container">

          {fleet
            .slice(fleetPage * 3, fleetPage * 3 + 3)
            .map(car => (
              <div
                className="fleet-card"
                key={`${car.make}-${car.model}`}
              >
                <h3>{car.make} {car.model}</h3>
                <p>Engine: {car.engineCapacity}L Category: {VehicleCategories[car.vehicleCategories]}</p>
                <img
                  src={carImages[`${car.make} ${car.model}`]?.[0]}
                  alt={`${car.make} ${car.model}`}
                />
              </div>
            ))}

        </div>
        <button
          className="fleet-arrow"
          onClick={() => setFleetPage(fleetPage + 1)}
          disabled={(fleetPage + 1) * 3 >= fleet.length}
        >
          →
        </button>

      </div>

    </div>
    </div>
  )
}

export default Home
import { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import './CarGroups.css'

type GroupCars = {
  carId: number
  make: string
  model: string
  vehicleCategories: number
  numberOfDoors: number
  transmissionType: number
  engineCapacity: number
  hasAC: boolean
}

type Branch = {
  branchId: number
  branchName: string
  address: string
  phoneNumber: string
  openingHours: string
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
  "Volkswagen Golf": [
    "/cars/vw-golf-front.webp",
    "/cars/vw-golf-back.jpg"
  ],
  "Toyota RAV4": [
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

function CarGroups() {
  const [groupedCars, setGroupedCars] = useState<GroupCars[]>([])
  const [branches, setBranches] = useState<Branch[]>([])
  const [selectedImages, setSelectedImages] = useState<Record<number, number>>({})
  const [selectedCarId, setSelectedCarId] = useState<number | null>(null)

  const [showBranches, setShowBranches] = useState(false)
  const [selectedBranchId, setSelectedBranchId] = useState<number | null>(null)
  const [selectedBranchName, setSelectedBranchName] = useState<string | null>(null)
  const [returnBranchName, setReturnBranchName] = useState("")
  const [userInput, setUserInput] = useState("")
  const [hoveredBranchId, setHoveredBranchId] = useState<number | null>(null)
  const [page, setPage] = useState(1)
  const [totalCount, setTotalCount] = useState(0)
  const [error, setError] = useState("")

  const pageSize = 6

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

  const navigate = useNavigate()

  const handleSearch = async () => {
    try {
      if (
        !pickupDate ||
        !returnDate ||
        !pickupTime ||
        !returnTime ||
        selectedBranchId === null ||
        returnBranchId === null
      ) {
        setError("Please complete your pickup and return details.")
        return
      }

      if (selectedCarId === null) {
        setError("Please select a car.")
        return
      }

      setError("")

      const response = await fetch(
        `/api/Customer/available-cars/${selectedCarId}/check?branchId=${selectedBranchId}&pickUpDate=${pickupDate}&returnDate=${returnDate}&pickUpTime=${pickupTime}&returnTime=${returnTime}`
      )

      if (!response.ok) {
        setError("Unable to check car availability.")
        return
      }

      const data = await response.json()

      navigate("/view-bookings", {
        state: {
          car: data,
          booking: {
            selectedBranchId,
            selectedBranchName,
            returnBranchId,
            returnBranchName,
            pickupDate,
            pickupTime,
            returnDate,
            returnTime
          }
        }
      })
    } catch {
      setError("Unable to connect to server.")
    }
  }

  useEffect(() => {
    const getGroupedCars = async () => {
      try {
        const response = await fetch(
          `/api/Customer/grouped-cars?page=${page}&pageSize=${pageSize}`
        )

        if (!response.ok) {
          return
        }

        const data = await response.json()

        setGroupedCars(data.items)
        setTotalCount(data.totalCount)
      } catch {
        setError("Unable to load cars.")
      }
    }

    getGroupedCars()
  }, [page])

  const totalPages = Math.ceil(totalCount / pageSize)

  useEffect(() => {
    const getBranches = async () => {
      try {
        const response = await fetch(
          `/api/Customer/branches`
        )

        if (!response.ok) {
          return
        }

        const data = await response.json()

        setBranches(data)
      } catch {
        setError("Unable to connect to server.")
      }
    }

    getBranches()
  }, [])

  return (
    <>
      {error && (
        <p className="error-message">
          {error}
        </p>
      )}

      <div className="searchbar-container">
        <div className="pickup-container">
          <div className="search-input">
            <input
              value={userInput}
              type="text"
              placeholder="Where would you like to pick up your car?"
              onChange={(e) => {
                setUserInput(e.target.value)
                setShowBranches(e.target.value.length > 0)
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
          <input
            type="date"
            value={pickupDate}
            onChange={(e) => setPickupDate(e.target.value)}
          />
        </div>

        <div className="search-input">
          <input
            type="time"
            value={pickupTime}
            onChange={(e) => setPickupTime(e.target.value)}
          />
        </div>

        <button
          onClick={handleSearch}
          className="search-button"
        >
          Search
        </button>
      </div>

      {selectedBranchId && pickupDate && pickupTime && (
        <div className="return-container">
          <div className="pickup-container">
            <div className="search-input">
              <input
                value={returnUserInput}
                type="text"
                placeholder="Where would you like to return your car?"
                onChange={(e) => {
                  setReturnUserInput(e.target.value)
                  setShowReturnBranches(e.target.value.length > 0)
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
                        setHoveredReturnBranchId(branch.branchId)
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
                        branch.branchId === hoveredReturnBranchId
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
            <input
              type="date"
              value={returnDate}
              onChange={(e) => setReturnDate(e.target.value)}
            />
          </div>

          <div className="search-input">
            <input
              type="time"
              value={returnTime}
              onChange={(e) => setReturnTime(e.target.value)}
            />
          </div>
        </div>
      )}

      <div className="car-groups-container">
        {groupedCars.map(car => {
          const images = carImages[`${car.make} ${car.model}`] ?? []
          const selectedImageIndex = selectedImages[car.carId] ?? 0

          return (
            <div
              className="car-groups-card"
              key={car.carId}
            >
              <h2>
                {car.make} {car.model}
              </h2>

              <h3>{VehicleCategories[car.vehicleCategories]}</h3>

              {images.length > 0 && (
                <img
                  src={images[selectedImageIndex] ?? images[0]}
                  alt={`${car.make} ${car.model}`}
                />
              )}

              {images.length > 1 && (
                <div className="image-dots">
                  <button
                    type="button"
                    className={`dot ${selectedImageIndex === 0 ? 'active' : ''}`}
                    aria-label={`Show first image of ${car.make} ${car.model}`}
                    aria-pressed={selectedImageIndex === 0}
                    onClick={() =>
                      setSelectedImages(prev => ({
                        ...prev,
                        [car.carId]: 0
                      }))
                    }
                  />

                  <button
                    type="button"
                    className={`dot ${selectedImageIndex === 1 ? 'active' : ''}`}
                    aria-label={`Show second image of ${car.make} ${car.model}`}
                    aria-pressed={selectedImageIndex === 1}
                    onClick={() =>
                      setSelectedImages(prev => ({
                        ...prev,
                        [car.carId]: 1
                      }))
                    }
                  />
                </div>
              )}

              <button
                onClick={() => setSelectedCarId(car.carId)}
                className="select-car-button"
              >
                Select Car
              </button>
            </div>
          )
        })}
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
    </>
  )
}

export default CarGroups
import { useLocation } from 'react-router-dom'
import { useNavigate } from 'react-router-dom'
import { useEffect, useState } from 'react'
import './AvailableCars.css'
import BookingProgress from '../components/BookingProgress'

type AvailableCar = {
  carId: number
  make: string
  model: string
  vehicleCategories: number
  numberOfDoors: number
  transmissionType: number
  engineCapacity: number
  hasAC: boolean
  pricePerDay: number
}

enum VehicleCategories {
  Economy,
  Compact,
  SUV,
  Premium
}

enum TransmissionType {
  Automatic,
  Manual
}

const carImages: Record<string, string[]> = {
  "Hyundai i20": [ "/cars/hyundai-i20-prix-neuf-neuf.jpg",
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

function AvailableCars() {
  const [availableCars, setAvailableCars] = useState<AvailableCar[]>([])
  const [showDetails, setShowDetails] = useState<number | null>(null)
  const [selectedImages, setSelectedImages] = useState<Record<number, number>>({})
  const [totalCount, setTotalCount] = useState(0)
  const [page, setPage] = useState(1)
  const [error, setError] = useState("")

  const pageSize = 6

  const navigate = useNavigate();

  const location = useLocation()

  const handleSearch = (car: AvailableCar) => {

    navigate("/view-bookings", {
      state: {
        car,
        booking: location.state
      }
    })
  }

  async function getAvailableCars() {
    if (!location.state?.selectedBranchId ||
    !location.state?.pickupDate ||
    !location.state?.returnDate ||
    !location.state?.pickupTime ||
    !location.state?.returnTime) {
    setError("Please search for a car before viewing available cars.")
    setAvailableCars([])
    setTotalCount(0)
    return
    }

    try
    {
      const response = await fetch(
      `/api/Customer/available-cars/${location.state.selectedBranchId}?pickUpDate=${location.state.pickupDate}&returnDate=${location.state.returnDate}&pickUpTime=${location.state.pickupTime}&returnTime=${location.state.returnTime}&page=${page}&pageSize=${pageSize}`
      )

      if (!response.ok) {
        setError("Unable to load available cars.")
        return
      }

      const data = await response.json()

      setAvailableCars(data.items)

      setTotalCount(data.totalCount)

      setError("")
    }
    catch
    {
      setError("Unable to connect to server.")
    }
  }

  useEffect(() => {
    getAvailableCars()
  }, [page])

  const totalPages = Math.ceil(totalCount / pageSize)

  return (
    <div>
      <BookingProgress />

      {error && (
        <p className="error-message">
          {error}
        </p>
      )}
      <div className='cars-container'>

        {availableCars.map(car => (

          <div className='car-card' key={car.carId}>

            <div
              className={
                showDetails === car.carId
                  ? 'card-inner flipped'
                  : 'card-inner'
              }
            >

              <div className='card-front'>

                <h2>{car.make} {car.model}</h2>

                <img
                  src={carImages[`${car.make} ${car.model}`]?.[selectedImages[car.carId] ?? 0]}
                  alt={`${car.make} ${car.model}`}
                />

                <div className="image-dots">
                  {carImages[`${car.make} ${car.model}`]?.map((_, index: number) => (
                  <button
                    key={index}
                    type="button"
                    className={`dot ${
                      (selectedImages[car.carId] ?? 0) === index ? 'active' : ''
                    }`}
                    aria-label={`Show image ${index + 1} of ${car.make} ${car.model}`}
                    aria-pressed={(selectedImages[car.carId] ?? 0) === index}
                    onClick={() =>
                      setSelectedImages(prev => ({
                        ...prev,
                        [car.carId]: index
                      }))
                    }
                  />
                ))}
                </div>

                <div className='price-container'>
                  <p>Price Per Day</p>
                    <strong>£{car.pricePerDay}.00</strong>
                </div>

                <button onClick={() => handleSearch(car)} className="select-button">
                  Select Car
                </button>

                <button
                  onClick={() => setShowDetails(car.carId)}
                  className='details-button'
                >
                  Show Details
                </button>
              </div>


              <div className='card-back'>

                <h2>{car.make} {car.model}</h2>

                <h3>
                  Category: {VehicleCategories[car.vehicleCategories]}
                </h3>

                <div className='properties-container'>

                  <div className='car-property'>
                    <span>
                      <i className="bi bi-car-front"></i>
                    </span>
                    <strong>{car.numberOfDoors}</strong>
                  </div>

                  <div className='car-property'>
                    <span>
                      <i className="bi bi-gear-fill"></i>
                    </span>
                    <strong>{TransmissionType[car.transmissionType]}</strong>
                  </div>

                  <div className='car-property'>
                    <span>
                      <i className="bi bi-speedometer2"></i>
                    </span>
                    <strong>{car.engineCapacity}L</strong>
                  </div>

                  <div className='car-property'>
                    <span>
                      <i className="bi bi-snow"></i>
                    </span>
                    <strong>{car.hasAC ? "Yes" : "No"}</strong>
                  </div>
                </div>

                <button
                  onClick={() => setShowDetails(null)}
                >
                  Hide Details
                </button>
              </div>
            </div>
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

export default AvailableCars
import { useEffect, useState } from "react"
import { getToken } from "../utils/auth"
import { useNavigate } from "react-router-dom"
import './AdminCars.css'


type Car = {
  carId: number
  branchId: number
  vehicleTypeId: number
  make: string
  model: string
  vehicleCategories: number
  numberOfDoors: number
  transmissionType: number
  carStatus: number
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

enum CarStatus {
  Available,
  OutOnHire,
  UnderMaintenance
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
  ]
}
function AdminCars() {
  const [cars, setCars] = useState<Car[]>([])
  const [selectedImages, setSelectedImages] = useState<Record<number, number>>({})
  const [showDetails, setShowDetails] = useState<number | null>(null)
  const [carError, setCarError] = useState("")
  const [totalCount, setTotalCount] = useState(0)
  const [page, setPage] = useState(1)

  const pageSize = 6

  const navigate = useNavigate()

  const getCarImage = (car: Car) => {
  const images = carImages[`${car.make} ${car.model}`]
  return images?.[selectedImages[car.carId] ?? 0] ?? "/cars/placeholder.webp"
}

  useEffect (() => {
    const getCars = async () => {
      try
      {
        setCarError("")

        const response = await fetch(`/api/Admin/cars?page=${page}&pageSize=${pageSize}`, {
          headers: {
            Authorization: `Bearer ${getToken()}`
          }
        })

        if(!response.ok)
        {
          setCarError("Unable to get cars.")
          return
        }

        const data = await response.json()

        setCars(data.items)

        setTotalCount(data.totalCount)
      }
      catch
      {
        setCarError("Unable to connect to server.")
      }
    }

    getCars()
  }, [page])

  const totalPages = Math.ceil(totalCount / pageSize)
  return (
    <div className="admin-cars-container">

      <h1>Admin Cars</h1>
      {carError && (
        <p className="error-message">
          {carError}
        </p>
      )}

      <button className="add-car" onClick={() => navigate("/admin/createcar")}>
        Add Car
      </button>

      <button className="cars-back-to-dash" onClick={() => navigate("/admin-dashboard")} type="button">Back To Dashboard</button>

      <div className="cars-container">

        {cars.map(car => (

          <div className="car-card" key={car.carId}>

            <div
              className={
                showDetails === car.carId
                  ? 'card-inner flipped'
                  : 'card-inner'
              }
            >

              <div className='card-front'>

                <h2>
                  {car.make} {car.model}
                </h2>

                <img
                  src={getCarImage(car)}
                  alt={`${car.make} ${car.model}`}
                />

                <div className="image-dots">
                  {carImages[`${car.make} ${car.model}`]?.map((_, index: number) => (
                  <span
                    key={index}
                    className={`dot ${
                      (selectedImages[car.carId] ?? 0) === index ? 'active' : ''
                    }`}
                    onClick={() =>
                      setSelectedImages(prev => ({
                        ...prev,
                        [car.carId]: index
                      }))
                    }
                  ></span>
                  ))}
                </div>

                <div className="price-container">

                  <p>Price Per Day</p>

                  <strong>
                    £{car.pricePerDay}.00
                  </strong>

                </div>

                <button
                  onClick={() => setShowDetails(car.carId)}
                  className='details-button'
                >
                  Show Details
                </button>
                <div className="car-status">
                  <p>{CarStatus[car.carStatus]}</p>
                </div>

              </div>


              <div className='card-back'>

                <h2>
                  {car.make} {car.model}
                </h2>

                <h3>
                  Category: {VehicleCategories[car.vehicleCategories]}
                </h3>

                <div className='properties-container'>

                  <div className='car-property'>

                    <span>
                      <i className="bi bi-car-front"></i>
                    </span>

                    <strong>
                      {car.numberOfDoors}
                    </strong>

                  </div>


                  <div className='car-property'>

                    <span>
                      <i className="bi bi-gear-fill"></i>
                    </span>

                    <strong>
                      {TransmissionType[car.transmissionType]}
                    </strong>

                  </div>


                  <div className='car-property'>

                    <span>
                      <i className="bi bi-speedometer2"></i>
                    </span>

                    <strong>
                      {car.engineCapacity}L
                    </strong>

                  </div>


                  <div className='car-property'>

                    <span>
                      <i className="bi bi-snow"></i>
                    </span>

                    <strong>
                      {car.hasAC ? "Yes" : "No"}
                    </strong>

                  </div>


                  <div className='car-property'>

                    <span>
                      Status
                    </span>

                    <strong>
                      {CarStatus[car.carStatus]}
                    </strong>

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
      {totalCount> 0 && (
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


export default AdminCars
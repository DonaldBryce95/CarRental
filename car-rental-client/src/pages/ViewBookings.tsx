import { useLocation, useNavigate } from 'react-router-dom'
import { useEffect, useState } from 'react'
import { getToken, isLoggedIn } from "../utils/auth"
import './ViewBookings.css'
import BookingProgress from '../components/BookingProgress'

type BookingSummary = {
  carId: number
  numberOfDays: number
  pickUpDate: string
  pickUpTime: string
  returnDate: string
  returnTime: string
  totalPrice: number
  pricePerDay: number
}

type Customer = {
  firstName: string
  lastName: string
  email: string
  phoneNumber: string
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

function ViewBookings() {
  const [summary, setSummary] = useState<BookingSummary | null>(null)
  const [customer, setCustomer] = useState<Customer | null>(null)
  const [error, setError] = useState("")
  const location = useLocation()
  const navigate = useNavigate()
  const car = location.state?.car
  const booking = location.state?.booking

  useEffect(() => {
    if (!car || !booking) {
      navigate("/")
    }
  }, [car, booking, navigate])

  useEffect(() => {
    if (!car || !booking) return

    const calculateTotal = async () => {
      try {
        setError("")

        const response = await fetch(
          "/api/Customer/calculate-booking",
          {
            method: "POST",
            headers: {
              "Content-Type": "application/json"
            },
            body: JSON.stringify({
              carId: car.carId,
              pickUpDate: booking.pickupDate,
              returnDate: booking.returnDate,
              pickUpTime: booking.pickupTime,
              returnTime: booking.returnTime
            })
          }
        )

        if (!response.ok) {
          setError("Unable to calculate booking.")
          return
        }

        const data = await response.json()
        setSummary(data)
      } catch {
        setError("Unable to connect to server.")
      }
    }

    calculateTotal()
  }, [car, booking])

  useEffect(() => {
    if (!isLoggedIn()) return

    const getCustomerProfile = async () => {
      try {
        setError("")

        const response = await fetch(
          "/api/Customer/customer-profile",
          {
            headers: {
              Authorization: `Bearer ${getToken()}`
            }
          }
        )

        if (!response.ok) {
          setError("Unable to load customer profile.")
          return
        }

        const data = await response.json()
        setCustomer(data)
      } catch {
        setError("Unable to connect to server.")
      }
    }

    getCustomerProfile()
  }, [])

  if (!car || !booking) return null

  return (
    <div className="booking-page">
      {error && <p className="error-message">{error}</p>}

      <BookingProgress />

      {customer && (
        <div className="welcome-message">
          <p>Welcome Back, {customer.firstName}</p>
          <p>Please click continue to go ahead with your booking</p>
        </div>
      )}

      <div className="booking-summary-container">
        <div className="test-title">
          <h1>Booking Summary</h1>
        </div>

        {summary && (
          <div>
            <div className="booking-pickup">
              <h1>Pick Up</h1>
              <div className="pickup-data">
                <p>{summary.pickUpDate} {summary.pickUpTime}</p>
                <p className="pickup-branch">
                  {booking.selectedBranchName}
                </p>
              </div>
            </div>

            <div className="booking-return">
              <h1>Return</h1>
              <div className="return-data">
                <p>{summary.returnDate} {summary.returnTime}</p>
                <p className="return-branch">
                  {booking.returnBranchName}
                </p>
              </div>
            </div>

            <div className="car-info-heading">
              <h1>Group</h1>
              <div className="type-info">
                <p>
                  {VehicleCategories[car.vehicleCategories]}{" "}
                  {TransmissionType[car.transmissionType]}
                </p>
                <p className="car-info">
                  {car.make} {car.model} {car.engineCapacity}L
                </p>
              </div>
            </div>

            <div className="pricing">
              <h1>Standard Pricing</h1>
              <p>Daily Rates</p>
              <p>
                {summary.numberOfDays} Day(s) x £{summary.pricePerDay}.00
              </p>

              <div className="pricing-total">
                <h1>Total</h1>
                <p>£{summary.totalPrice}.00</p>
              </div>
            </div>
          </div>
        )}
      </div>

      <button
        className="continue-button"
        disabled={!summary}
        onClick={() => {
          if (!summary) return

          navigate("/details", {
            state: {
              car,
              booking,
              summary,
              customer
            }
          })
        }}
      >
        Continue
      </button>
    </div>
  )
}

export default ViewBookings
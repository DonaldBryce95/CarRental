import { useLocation, useNavigate } from 'react-router-dom'
import { useEffect, useState } from 'react'
import { getToken, isLoggedIn } from '../utils/auth'
import BookingProgress from '../components/BookingProgress'
import './Details.css'

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

function Details() {
  const location = useLocation()
  const navigate = useNavigate()

  const car = location.state?.car
  const customer = location.state?.customer
  const booking = location.state?.booking
  const summary = location.state?.summary

  useEffect(() => {
    if (!car || !booking || !summary) {
      navigate('/')
    }
  }, [car, booking, summary, navigate])

  const [firstName, setFirstName] = useState(customer?.firstName || '')
  const [lastName, setLastName] = useState(customer?.lastName || '')
  const [message, setMessage] = useState('')
  const [phoneNumber, setPhoneNumber] = useState(customer?.phoneNumber || '')
  const [email, setEmail] = useState(customer?.email || '')
  const [error, setError] = useState('')

  if (!car || !booking || !summary) {
    return null
  }

  const reservation = {
    firstName,
    lastName,
    phoneNumber,
    email,
    carId: car.carId,
    pickUpDate: booking.pickupDate,
    pickUpTime: booking.pickupTime,
    returnDate: booking.returnDate,
    returnTime: booking.returnTime,
    pickUpBranchId: booking.selectedBranchId,
    returnBranchId: booking.returnBranchId
  }

  const handleBook = async () => {
    setMessage('')
    setError('')

    if (!firstName || !lastName || !phoneNumber || !email) {
      setMessage('All fields are required')
      return
    }

    if (isLoggedIn()) {
      try {
        const response = await fetch(
          '/api/Customer/create-customer-reservation',
          {
            method: 'POST',
            headers: {
              Authorization: `Bearer ${getToken()}`,
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(reservation)
          }
        )

        if (!response.ok) {
          setError('Unable to create reservation.')
          return
        }

        const data = await response.json()
        navigate(`/confirmation/${data.reservationId}`)
      } catch {
        setError('Unable to connect to server.')
      }
    } else {
      try {
        const response = await fetch(
          '/api/Customer/create-reservation',
          {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(reservation)
          }
        )

        if (!response.ok) {
          setError('Unable to create reservation.')
          return
        }

        const data = await response.json()

        navigate(`/confirmation/${data.reservationId}`, {
          state: {
            reservation: data
          }
        })
      } catch {
        setError('Unable to connect to server.')
      }
    }
  }

  return (
    <div className="booking-page">
      {error && (
        <p className="error-message">
          {error}
        </p>
      )}

      <BookingProgress />

      <div className="details-content">
        <div className="customer-details">
          <h1>Customer Details</h1>

          <form
            onSubmit={(e) => {
              e.preventDefault()
              handleBook()
            }}
          >
            <label>First Name</label>
            <input
              type="text"
              required
              value={firstName}
              onChange={(e) => {
                setFirstName(e.target.value)
                setMessage('')
                setError('')
              }}
            />

            <label>Last Name</label>
            <input
              type="text"
              required
              value={lastName}
              onChange={(e) => {
                setLastName(e.target.value)
                setMessage('')
                setError('')
              }}
            />

            <label>Email</label>
            <input
              type="email"
              required
              value={email}
              onChange={(e) => {
                setEmail(e.target.value)
                setMessage('')
                setError('')
              }}
            />

            <label>Phone Number</label>
            <input
              type="tel"
              required
              value={phoneNumber}
              onChange={(e) => {
                setPhoneNumber(e.target.value)
                setMessage('')
                setError('')
              }}
            />

            <button type="submit" className="book-button">
              Book
            </button>
          </form>

          {message && <p>{message}</p>}
        </div>

        <div className="booking-summary-container">
          <div className="test-title">
            <h1>Booking Summary</h1>
          </div>

          <div>
            <div className="booking-pickup">
              <h1>Pick Up</h1>
              <div className="pickup-data">
                <p>{summary.pickUpDate} {summary.pickUpTime}</p>
                <p className="pickup-branch">{booking.selectedBranchName}</p>
              </div>
            </div>

            <div className="booking-return">
              <h1>Return</h1>
              <div className="return-data">
                <p>{summary.returnDate} {summary.returnTime}</p>
                <p className="return-branch">{booking.returnBranchName}</p>
              </div>
            </div>

            <div className="car-info-heading">
              <h1>Group</h1>
              <div className="type-info">
                <p>
                  {VehicleCategories[car.vehicleCategories]}{' '}
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
        </div>
      </div>
    </div>
  )
}

export default Details
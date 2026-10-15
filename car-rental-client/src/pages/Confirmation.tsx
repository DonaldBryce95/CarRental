import { useEffect, useState } from "react"
import { useParams, useNavigate, useLocation } from "react-router-dom"
import { getToken } from "../utils/auth"
import './Confirmation.css'

type Reservation = {
  reservationId: number
  pickUpBranchId: number
  returnBranchId: number
  customerId: number
  firstName: string
  lastName: string
  email: string
  phoneNumber: string
  carId: number
  pickUpDate: string
  returnDate: string
  pickUpTime: string
  returnTime: string
  totalPrice: number
  reservationStatus: ReservationStatus
}

enum ReservationStatus {
  Confirmed,
  Cancelled
}


function Confirmation() {
  const { reservationId } = useParams()
  const navigate = useNavigate()
  const location = useLocation()
  const guestReservation = location.state?.reservation
  const [reservation, setReservation] = useState<Reservation | null>(null)
  const [error, setError] = useState("")
  useEffect(() => {
    if(!reservationId && !guestReservation)
    {
      navigate("/")
      return
    }
      const getReservation = async () => {
        setError("")
        if (guestReservation) {
        setReservation(guestReservation)
        return
        }
        try
        {
          const response = await fetch(`/api/Customer/reservations/${reservationId}`,
            {
              method: "GET",
              headers: {
                Authorization: `Bearer ${getToken()}`
              }
            }
          )

          if(!response.ok)
          {
            setReservation(null)
            setError("Unable to load reservation.")
            return
          }

          const data = await response.json()

          setReservation(data)
          setError("")
        }
        catch
        {
          setReservation(null)
          setError("Unable to connect to server.")
        }
      }
      getReservation()
    }, [reservationId, guestReservation, navigate])
  return (
    <div>
      {error && (
      <p className="error-message">
        {error}
      </p>
      )}

      {reservation && (
        <div className="reservation-container">
          <h1>Booking Confirmation</h1>
          <p className="reservation-detail">
            Reservation ID: {reservation.reservationId}
          </p>
          <p className="reservation-detail">
            First Name: {reservation.firstName}
          </p>
          <p className="reservation-detail">
            Last Name: {reservation.lastName}
          </p>
          <p className="reservation-detail">
            Email: {reservation.email}
          </p>
          <p className="reservation-detail">
            Phone Number: {reservation.phoneNumber}
          </p>
          <p className="reservation-detail">
            Pick Up Date: {reservation.pickUpDate}
          </p>
          <p className="reservation-detail">
            Return Date: {reservation.returnDate}
          </p>
          <p className="reservation-detail">
            Pick Up Time: {reservation.pickUpTime}
          </p>
          <p className="reservation-detail">
            Return Time: {reservation.returnTime}
          </p>
          <p className="reservation-detail">
            Total Price: £{reservation.totalPrice}
          </p>
          <p className="reservation-detail">
            Reservation Status: {ReservationStatus[reservation.reservationStatus]}
          </p>
          <button onClick={() => navigate("/")} className="home-button"> Back To Home</button>
        </div>
      )}
    </div>
  )
}

export default Confirmation
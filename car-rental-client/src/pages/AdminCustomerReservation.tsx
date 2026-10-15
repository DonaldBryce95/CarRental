import { useEffect, useState } from "react"
import { useParams, useNavigate } from "react-router-dom"
import { getToken } from "../utils/auth"
import './AdminCustomerReservation.css'

type Reservation = {
  reservationId: number
  customerId: number
  firstName: string
  lastName: string
  email: string
  phoneNumber: string
  carId: number
  carStatus: number
  make: string
  model: string
  pickUpDate: string
  returnDate: string
  pickUpTime: string
  returnTime: string
  pickUpBranchId: number
  pickUpBranchName: string
  returnBranchName: string
  returnBranchId: number
  reservationStatus: number
  totalPrice: number
}

type OutForHire = {
  reservationId: number
  carId: number
  carStatus: number;
}

enum CarStatus {
  Available,
  OutOnHire,
  UnderMaintenance
}

enum ReservationStatus {
  Confirmed,
  Cancelled
}

function AdminCustomerReservation() {
  const [reservation, setReservation] = useState<Reservation>()
  const [carStatus, setCarStatus] = useState<CarStatus>()
  const {reservationId} = useParams()
  const [reservationError, setReservationError] = useState("")
  const [actionError, setActionError] = useState("")
  const navigate = useNavigate()
  useEffect (() => {
    const getReservation = async () => {
      if(!reservationId)
      {
        setReservationError("Reservation ID missing.")
        return
      }

      const id = Number(reservationId)

      if(Number.isNaN(id))
      {
        setReservationError("Invalid reservation ID.")
        return
      }
      try
      {
        setReservationError("")

        const response = await fetch(`/api/Admin/reservations/${id}`,
          {
            headers: {
              Authorization: `Bearer ${getToken()}`
            }
          }
        )

        if(!response.ok)
        {
          setReservationError("Unable to load reservation.")
          return
        }

        const data = await response.json()

        setReservation(data)
        setCarStatus(data.carStatus)
      }
      catch
      {
        setReservationError("Unable to connect to server.")
      }
    }

    getReservation()
  }, [reservationId])

  const handleCancel = async () => {
    try
    {
      if(!reservation)
      {
        setActionError("Reservation not loaded.")
        return
      }

      setActionError("")

      const response = await fetch(`/api/Admin/reservations/${reservation.reservationId}/cancel`,
        {
          method: "PUT",
          headers: {
            Authorization: `Bearer ${getToken()}`
          }
        }
      )

      if(!response.ok)
      {
        setActionError("Unable to cancel reservation.")
        return
      }

      setReservation(prev =>
      prev ? { ...prev, reservationStatus: ReservationStatus.Cancelled } : prev
      )
    }
    catch
    {
      setActionError("Unable to connect to server.")
    }
  }

  const handleHire = async () => {
    try
    {
      if(!reservation)
      {
        setActionError("Reservation ID missing.")
        return
      }

      setActionError("")

      const response = await fetch(`/api/Admin/reservations/${reservation.reservationId}/hire`,
        {
          method: "PUT",
          headers: {
          Authorization: `Bearer ${getToken()}`
          }
        }
      )

      if(!response.ok)
      {
        setActionError("Unable to update car status.")
        return
      }

      const data : OutForHire = await response.json()

      setCarStatus(data.carStatus)
    }
    catch
    {
      setActionError("Unable to connect to server.")
    }
  }
    const handleCarReturned = async () => {
      try
      {
        if(!reservation)
        {
          setActionError("Reservation not loaded.")
          return
        }

        setActionError("")

        const response = await fetch(`/api/Admin/maintenance-tasks`,
          {
            method: "POST",
            headers: {
            Authorization: `Bearer ${getToken()}`,
            "Content-Type": "application/json"
            },
            body: JSON.stringify({
              carId: reservation.carId
            })
          }
        )

        if(!response.ok)
        {
          setActionError("Unable to create maintenance task.")
          return
        }

        setCarStatus(CarStatus.UnderMaintenance)
      }
      catch
      {
        setActionError("Unable to connect to server.")
      }
    }
  return (
    <div className="admin-reservation-page">
      <h1>Customer Reservation</h1>
      {reservationError && (
        <p className="error-message">
          {reservationError}
        </p>
      )}

      {actionError && (
        <p className="error-message">
          {actionError}
        </p>
      )}

      <button className="back-to-reservations" onClick={() => navigate("/admin/customer/reservations")} type="button">Back To Reservations</button>

      {reservation && (
      <>
      {reservation.reservationStatus !== ReservationStatus.Cancelled && (
      <button
        className= "cancel-reservation"
        onClick={handleCancel}
        type="button"
      >
        Cancel Reservation
      </button>
      )}

      <button onClick={handleHire}
        className="hire-car"
        type="button"
      >
        Car Picked Up
      </button>

      <button onClick={handleCarReturned}
        className="return-car"
        type="button"
      >
        Car Returned
      </button>

      <div className="reservation-container">
        <div className="customer-info">
          <p>Reservation #{reservation.reservationId}</p>
          <p>{reservation.firstName} {reservation.lastName}</p>
          <p>{reservation.email}</p>
        </div>

        <p>Make: {reservation.make}</p>
        <p>Model: {reservation.model}</p>
        <p>Pick Up Date: {reservation.pickUpDate}</p>
        <p>Pick Up Branch: {reservation.pickUpBranchName}</p>
        <p>Return Date: {reservation.returnDate}</p>
        <p>Return Branch: {reservation.returnBranchName}</p>
        <p>Pick Up Time: {reservation.pickUpTime}</p>
        <p>Return Time: {reservation.returnTime}</p>
        <p>
          Reservation Status:{" "}
          {ReservationStatus[reservation.reservationStatus]}
        </p>
        <p>Total Price: £{reservation.totalPrice}</p>

        {carStatus === CarStatus.OutOnHire && (
          <div className="out-for-hire">
            Car On Hire
          </div>
        )}
      </div>
      </>
      )}
    </div>
  )
}

export default AdminCustomerReservation
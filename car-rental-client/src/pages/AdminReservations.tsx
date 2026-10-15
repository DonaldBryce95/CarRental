import { useEffect, useState } from "react"
import { Link, useNavigate } from "react-router-dom"
import { getToken } from "../utils/auth"
import './AdminReservations.css'

type Reservation = {
  reservationId: number
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
  pickUpBranchId: number
  returnBranchId: number
  reservationStatus: number
  totalPrice: number
}

enum ReservationStatus {
  Confirmed,
  Cancelled
}

function AdminReservations () {
    const [reservations, setReservations] = useState<Reservation[]>([])
    const navigate = useNavigate()
    const [totalCount, setTotalCount] = useState(0)
    const [page, setPage] = useState(1)
    const [error, setError] = useState("")

    const pageSize = 10

    useEffect(() => {
      const getReservations = async () => {
        try
        {
          setError("")

          const response = await fetch(`/api/Admin/reservations?page=${page}&pageSize=${pageSize}`, {
            headers: {
              Authorization: `Bearer ${getToken()}`
            }
          })

          if(!response.ok)
          {
            setError("Unable to get reservations.")
            return
          }

          const data = await response.json()

          setReservations(data.items)

          setTotalCount(data.totalCount)
        }
        catch
        {
          setError("Unable to connect to server.")
        }
      }
      getReservations()
    }, [page])

    const totalPages = Math.ceil(totalCount / pageSize)
  return (
    <div className="reservations-container">
      <h1>Reservations</h1>

      {error && (
        <p className="error-message">
          {error}
        </p>
      )}
      <button className="back-to-dash" onClick={() => navigate("/admin-dashboard")} type="button">Back To Dashboard</button>
        <div className="reservation-page">
          <table>
          <thead>
            <tr>
              <th>Reservation ID</th>
              <th>Customer</th>
              <th>Pick Up Branch</th>
              <th>Return Branch</th>
              <th>Pick Up Date</th>
              <th>Return Date</th>
              <th>Total</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            {reservations.map(reservation => (
              <tr key={reservation.reservationId}>
                <td>
                  <Link to={`/admin-customer-reservation/${reservation.reservationId}`}>
                    {reservation.reservationId}
                  </Link>
                </td>
                <td>{reservation.firstName} {reservation.lastName}</td>
                <td>{reservation.pickUpBranchId}</td>
                <td>{reservation.returnBranchId}</td>
                <td>{reservation.pickUpDate}</td>
                <td>{reservation.returnDate}</td>
                <td>£{reservation.totalPrice}.00</td>
                <td>{ReservationStatus[reservation.reservationStatus]}</td>
              </tr>
            ))}
          </tbody>
          </table>
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
    </div>
  )
}

export default AdminReservations
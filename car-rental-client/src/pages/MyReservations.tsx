import { useState, useEffect} from 'react'
import { getToken } from "../utils/auth"
import './MyReservations.css'


type Reservation = {
  reservationId: number
  customerId: number
  make: string
  model: string
  pickUpDate: string
  returnDate: string
  pickUpBranchName: string
  returnBranchName: string
  pickUpTime: string
  returnTime: string
  reservationStatus: number
  totalPrice: number
}

enum ReservationStatus {
  Confirmed,
  Cancelled
}

function MyReservations() {
  const [reservations, setReservations] = useState<Reservation[]>([])
  const [page, setPage] = useState(1)
  const [totalCount, setTotalCount] = useState(0)
  const [message, setMessage] = useState("")

  const pageSize = 10

  useEffect (() => {
    const getMyReservation = async () => {
      try
      {
        setMessage("")

        const response = await fetch(`/api/Customer/my-reservations?page=${page}&pageSize=${pageSize}`,
          {
            headers: {
              Authorization: `Bearer ${getToken()}`
            }
          }
        )

        if(!response.ok)
        {
          setMessage("Unable to get reservations.")
          return
        }

        const data = await response.json()

        setReservations(data.items)

        setTotalCount(data.totalCount)
      }
      catch
      {
        setMessage("Unable to connect to the server.")
      }
    }

    getMyReservation()
  }, [page])

  const totalPages = Math.ceil(totalCount / pageSize)


  return (
    <div className='reservations-container'>
      <h1>My Reservations</h1>

      {message && (
      <p className="error-message">
        {message}
      </p>
      )}

    <div className="reservation-page">

      <table>

        <thead>
          <tr>
            <th>Reservation</th>
            <th>Car</th>
            <th>Pick Up</th>
            <th>Return</th>
            <th>Total</th>
            <th>Status</th>
          </tr>
        </thead>

        <tbody>

          {reservations.map(reservation => (

            <tr key={reservation.reservationId}>

              <td>
                {reservation.reservationId}
              </td>

              <td>
                {reservation.make} {reservation.model}
              </td>

              <td>
                {reservation.pickUpDate} {reservation.pickUpTime}
                <br/>
                {reservation.pickUpBranchName}
              </td>

              <td>
                {reservation.returnDate} {reservation.returnTime}
                <br/>
                {reservation.returnBranchName}
              </td>

              <td>
                £{reservation.totalPrice}
              </td>

              <td>
                {ReservationStatus[reservation.reservationStatus]}
              </td>

            </tr>
          ))}
        </tbody>
      </table>
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

export default MyReservations
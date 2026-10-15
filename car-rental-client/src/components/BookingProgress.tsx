function BookingProgress() {

  return (
    <div className="booking-progress">
      <div className="progress-step completed">
        <span className="progress-dot">1</span>
        <p>Search</p>
      </div>

      <div className="progress-line"></div>

      <div className="progress-step active">
        <span className="progress-dot">2</span>
        <p>Choose Car</p>
      </div>

      <div className="progress-line"></div>

      <div className="progress-step">
        <span className="progress-dot">3</span>
        <p>Extras</p>
      </div>

      <div className="progress-line"></div>

      <div className="progress-step">
        <span className="progress-dot">4</span>
        <p>Your Details</p>
      </div>

      <div className="progress-line"></div>

      <div className="progress-step">
        <span className="progress-dot">5</span>
        <p>Confirmation</p>
      </div>
    </div>
  )
}

export default BookingProgress
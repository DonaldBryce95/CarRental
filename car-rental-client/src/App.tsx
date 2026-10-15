import { Routes, Route, BrowserRouter} from 'react-router-dom'
import Home from './pages/Home'
import NavBar from './components/NavBar'
import AvailableCars from './pages/AvailableCars'
import ViewBookings from './pages/ViewBookings'
import Details from './pages/Details'
import Confirmation from './pages/Confirmation'
import Login from './pages/Login'
import Register from './pages/Register'
import CarGroups from './pages/CarGroups'
import AdminDashboard from './pages/AdminDashboard'
import AdminReservations from './pages/AdminReservations'
import AdminCars from './pages/AdminCars'
import MyReservations from './pages/MyReservations'
import CreateCar from './pages/CreateCar'
import AdminCustomerReservation from './pages/AdminCustomerReservation'
import MaintenanceDashboard from './pages/MaintnenanceDashboard'
import MaintenanceTasks from './pages/MaintenanceTasks'
import MaintenanceReport from './pages/MaintenanceReport'
import AdminMaintenanceWorkers from './pages/AdminMaintenanceWorkers'
import AdminMaintenanceWorkerTasks from './pages/AdminMaintenanceWorkerTasks'
import AdminMaintenanceReport from './pages/AdminMaintenanceReport'
import AdminUnderMaintenanceCars from './pages/AdminUnderMaintenanceCars'
import AdminMaintenanceWorkerProfile from './pages/AdminMaintenanceWorkerProfile'
import AdminAssignRole from './pages/AdminAssignRole'

function App() {
  return (
    <BrowserRouter>
    <NavBar/>
      <Routes>
        <Route path='/login' element={<Login />} />
        <Route path='/register' element={<Register />} />
        <Route path='/car-groups' element={<CarGroups />} />
        <Route path='/' element={<Home />} />
        <Route path='/available-cars' element={<AvailableCars />} />
        <Route path='/view-bookings' element={<ViewBookings /> } />
        <Route path='/details' element={<Details /> } />
        <Route path="/confirmation/:reservationId" element={<Confirmation />} />
        <Route path="/admin-dashboard" element={<AdminDashboard />} />
        <Route path="/admin/customer/reservations" element={<AdminReservations />} />
        <Route path="/admin/cars/all" element={<AdminCars />} />
        <Route path="my-reservations" element={<MyReservations />} />
        <Route path="/admin/createcar" element={<CreateCar />} />
        <Route path="/admin-customer-reservation/:reservationId" element={<AdminCustomerReservation />} />
        <Route path="/maintenance-dashboard" element={<MaintenanceDashboard />} />
        <Route path="/maintenance-tasks" element={<MaintenanceTasks />} />
        <Route path="/maintenance-report/:taskId" element={<MaintenanceReport />} />
        <Route path="/admin/maintenance/worker/tasks" element={<AdminMaintenanceWorkers />} />
        <Route path="/admin/maintenance/worker/tasks/:maintenanceWorkerId" element={<AdminMaintenanceWorkerTasks />} />
        <Route path="/admin-maintenance-report/:maintenanceTaskId" element={<AdminMaintenanceReport />} />
        <Route path="/admin/cars/undermaintenance/profile" element={<AdminUnderMaintenanceCars />} />
        <Route path="/admin/maintenance/workers/profile" element={<AdminMaintenanceWorkerProfile />} />
        <Route path="/admin/assign/role" element={<AdminAssignRole />} />
      </Routes>

    </BrowserRouter>
  )
}

export default App

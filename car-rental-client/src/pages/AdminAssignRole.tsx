import { useEffect, useState } from "react"
import { getToken } from "../utils/auth"
import type { SubmitEvent } from "react"
import { useNavigate } from "react-router-dom"
import './AdminAssignRole.css'


type Role = {
  name: string
}

type Customer = {
  userId: string
  firstName: string
  lastName: string
}

function AdminAssignRole() {

  const [roles, setRoles] = useState<Role[]>([])
  const [customers, setCustomers] = useState<Customer[]>([])
  const [selectedUserId, setSelectedUserId] = useState("")
  const [roleError, setRoleError] = useState("")
  const [selectedRole, setSelectedRole] = useState("")
  const [successMessage, setSuccessMessage] = useState("")

  const navigate = useNavigate()

  useEffect(() => {
    const getRoles = async () => {
      try
      {
        const response = await fetch(`/api/Admin/admin/roles`,
          {
            headers: {
              Authorization: `Bearer ${getToken()}`
            }
          }
        )

        if(!response.ok)
        {
          setRoleError("Unable to set role.")
          return
        }

        const data = await response.json()

        setRoles(data)
      }
      catch
      {
        setRoleError("Unable to connect to server.")
      }
    }
    getRoles()
  }, [])
    useEffect(() => {
    const getCustomers = async () => {
      try
      {
        const response = await fetch(`/api/Admin/admin/customers`,
          {
            headers: {
              Authorization: `Bearer ${getToken()}`
            }
          }
        )

        if(!response.ok)
        {
          setRoleError("Unable to set role.")
          return
        }

        const data = await response.json()

        setCustomers(data)
      }
      catch
      {
        setRoleError("Unable to connect to server.")
      }
    }
    getCustomers()
  }, [])

  const handleAssign = async (e: SubmitEvent<HTMLFormElement>) => {
    e.preventDefault()

    setRoleError("")
    setSuccessMessage("")

    const selectedCustomer = customers.find(customer => customer.userId === selectedUserId)

    if (!selectedCustomer || !selectedRole) {
      setRoleError("Please select a customer and a role.")
      return
    }
    try
    {
      const response = await fetch(`/api/Admin/users/${selectedUserId}/role`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${getToken()}`
          },
          body: JSON.stringify({
            Role: selectedRole,
            FirstName: selectedCustomer.firstName,
            LastName: selectedCustomer.lastName
          })
        }
      )

      if(!response.ok)
      {
        setRoleError("Unable to assign role.")
        return
      }

      setRoleError("")
      setSuccessMessage("Role assigned successfully.")
    }
    catch
    {
      setRoleError("Unable to connect to server.")
    }
  }
  return (
    <div className="assign-role-page">
      <button className="assign-back-to-dash" onClick={() => navigate("/admin-dashboard")}>Back to Dashboard</button>
      <div className="assign-card">
      <form onSubmit={handleAssign}>
        <h1>Assign Role</h1>

        <label>Customer</label>
        <select value={selectedUserId} onChange={(e) => setSelectedUserId(e.target.value)} required>

          <option value="">Select a customer</option>
          {customers.map(customer => (
            <option key={customer.userId} value={customer.userId}>
              {customer.firstName} {customer.lastName}
            </option>
          ))}
        </select>

        <label>Role</label>
        <select value={selectedRole} onChange={(e) => setSelectedRole(e.target.value)} required>

          <option value="">Select a role</option>
          {roles.map(role => (
            <option key={role.name} value={role.name}>
              {role.name}
            </option>
          ))}
        </select>

        <button type="submit">Assign Role</button>
        {roleError && <p role="alert">{roleError}</p>}
        {successMessage && <p role="status">{successMessage}</p>}
      </form>
      </div>
    </div>
  )
}

export default AdminAssignRole
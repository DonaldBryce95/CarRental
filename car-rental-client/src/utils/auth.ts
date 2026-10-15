export function getToken(): string | null {
  return localStorage.getItem("token")
}

export function getRoles(): string[] {
  const token = getToken()

  if (!token) {
    return []
  }

  try {
    const payload = JSON.parse(atob(token.split(".")[1]))

    const roleClaim =
      payload[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ]

    if (Array.isArray(roleClaim)) {
      return roleClaim
    }

    if (typeof roleClaim === "string") {
      return [roleClaim]
    }

    return []
  } catch {
    return []
  }
}

export function isLoggedIn(): boolean {
  return getToken() !== null
}

export function logout(): void {
  localStorage.removeItem("token")
}
'use server'


import { getApiErrorMessageFromResponse } from "@/lib/api-error";
import { getSession } from '@/lib/auth'

const API_URL = process.env.API_URL || 'http://localhost:5245/api'
export async function GetAllUsers() {
    // const session = await getSession()

    /* if (!session?.accessToken) {
        return { error: 'Unauthorized' }
    } */
    try {
        const res = await fetch(`${API_URL}/User`, {
            headers: {
                // 'Authorization': `Bearer ${session.accessToken}`,
                'Content-Type': 'application/json'
            },
            cache: 'no-store'
        })
        if (!res.ok) {
            return { error: await getApiErrorMessageFromResponse(res, 'Failed to fetch users') }
        }

        return await res.json()
    } catch (e) {
        return { error: 'Network error occurred' }
    }
}

export async function GetUserById(id: number) {
    // const session = await getSession()

    /* if (!session?.accessToken) {
        return { error: 'Unauthorized' }
    } */

    try {
        const res = await fetch(`${API_URL}/User/${id}`, {
            headers: {
                // 'Authorization': `Bearer ${session.accessToken}`,
                'Content-Type': 'application/json'
            },
            cache: 'no-store'
        })
        if (!res.ok) {
            return { error: await getApiErrorMessageFromResponse(res, 'Failed to fetch user') }
        }

        return await res.json()
    } catch (e) {
        return { error: 'Network error occurred' }
    }
}

export async function CreateUser(data: any) {
    // const session = await getSession()

    /* if (!session?.accessToken) {
        return { error: 'Unauthorized' }
    } */

    try {
        const res = await fetch(`${API_URL}/User`, {
            method: 'POST',
            headers: {
                // 'Authorization': `Bearer ${session.accessToken}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })

        if (!res.ok) {
            return { error: await getApiErrorMessageFromResponse(res, "Failed to create user") }
        }

        return await res.json()
    } catch (e) {
        return { error: 'Network error occurred' }
    }
}

export async function UpdateUser(id: number, data: any) {
    // const session = await getSession()

    /* if (!session?.accessToken) {
        return { error: 'Unauthorized' }
    } */

    try {
        const res = await fetch(`${API_URL}/User/${id}`, {
            method: 'PUT',
            headers: {
                // 'Authorization': `Bearer ${session.accessToken}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })

        if (!res.ok) {
            return { error: await getApiErrorMessageFromResponse(res, "Failed to update user") }
        }

        return await res.json()
    } catch (e) {
        return { error: 'Network error occurred' }
    }
}

export async function DeleteUser(id: number) {
    // const session = await getSession()

    /* if (!session?.accessToken) {
        return { error: 'Unauthorized' }
    } */

    try {
        const res = await fetch(`${API_URL}/User/${id}`, {
            method: 'DELETE',
            headers: {
                // 'Authorization': `Bearer ${session.accessToken}`,
                'Content-Type': 'application/json'
            }
        })

        if (!res.ok) {
            return { error: await getApiErrorMessageFromResponse(res, 'Failed to delete user') }
        }

        return await res.json()
    } catch (e) {
        return { error: 'Network error occurred' }
    }
}


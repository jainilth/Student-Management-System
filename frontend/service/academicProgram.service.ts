'use server'


import { getApiErrorMessageFromResponse } from "@/lib/api-error";
import { getSession } from '@/lib/auth'

const API_URL = process.env.API_URL || 'http://localhost:5245/api'

export async function GetAllAcademicPrograms() {
    // const session = await getSession()
    // // if (!session?.accessToken) return { error: 'Unauthorized' }

    try {
        const res = await fetch(`${API_URL}/AcademicProgram`, {
            headers: {
                // 'Authorization': `Bearer ${session.accessToken}`,
                'Content-Type': 'application/json'
            },
            cache: 'no-store'
        })
        if (!res.ok) return { error: await getApiErrorMessageFromResponse(res, 'Failed to fetch') }
        return await res.json()
    } catch (e) {
        return { error: 'Network error occurred' }
    }
}

export async function GetAcademicProgramById(id: number) {
    // const session = await getSession()
    // // if (!session?.accessToken) return { error: 'Unauthorized' }

    try {
        const res = await fetch(`${API_URL}/AcademicProgram/${id}`, {
            headers: {
                // 'Authorization': `Bearer ${session.accessToken}`,
                'Content-Type': 'application/json'
            },
            cache: 'no-store'
        })
        if (!res.ok) return { error: await getApiErrorMessageFromResponse(res, 'Failed to fetch') }
        return await res.json()
    } catch (e) {
        return { error: 'Network error occurred' }
    }
}

export async function CreateAcademicProgram(data: any) {
    // const session = await getSession()
    // // if (!session?.accessToken) return { error: 'Unauthorized' }

    try {
        const res = await fetch(`${API_URL}/AcademicProgram`, {
            method: 'POST',
            headers: {
                // 'Authorization': `Bearer ${session.accessToken}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
        if (!res.ok) {
            return { error: await getApiErrorMessageFromResponse(res, "Failed to create") }
        }
        return await res.json()
    } catch (e) {
        return { error: 'Network error occurred' }
    }
}

export async function UpdateAcademicProgram(id: number, data: any) {
    // const session = await getSession()
    // // if (!session?.accessToken) return { error: 'Unauthorized' }

    try {
        const res = await fetch(`${API_URL}/AcademicProgram/${id}`, {
            method: 'PUT',
            headers: {
                // 'Authorization': `Bearer ${session.accessToken}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
        if (!res.ok) {
            return { error: await getApiErrorMessageFromResponse(res, "Failed to update") }
        }
        return await res.json()
    } catch (e) {
        return { error: 'Network error occurred' }
    }
}

export async function DeleteAcademicProgram(id: number) {
    // const session = await getSession()
    // // if (!session?.accessToken) return { error: 'Unauthorized' }

    try {
        const res = await fetch(`${API_URL}/AcademicProgram/${id}`, {
            method: 'DELETE',
            headers: {
                // 'Authorization': `Bearer ${session.accessToken}`,
                'Content-Type': 'application/json'
            }
        })
        if (!res.ok) return { error: await getApiErrorMessageFromResponse(res, 'Failed to delete') }
        return await res.json()
    } catch (e) {
        return { error: 'Network error occurred' }
    }
}


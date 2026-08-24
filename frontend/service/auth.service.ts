'use server'


import { getApiErrorMessage } from "@/lib/api-error";
import { encrypt } from '@/lib/session'
import { cookies } from 'next/headers'
import { redirect } from 'next/navigation'

export async function login(formData: FormData) {
    const email = formData.get('email') as string
    const password = formData.get('password') as string

    // TODO: Replace with actual backend API call once Auth is implemented on C# backend
    // For now, we mock the authentication based on the email provided
    let role = 'Student'
    if (email.includes('admin')) role = 'Admin'
    else if (email.includes('faculty')) role = 'Faculty'

    const user = {
        id: "mock-id-123",
        email: email,
        name: email.split('@')[0],
        role: role,
        accessToken: "mock-jwt-token"
    }

    const expiresAt = new Date(Date.now() + 7 * 24 * 60 * 60 * 1000)
    const session = await encrypt({ ...user, expiresAt: expiresAt.getTime() })

    const cookieStore = await cookies()
    cookieStore.set('session', session, {
        httpOnly: true,
        secure: process.env.NODE_ENV === 'production',
        expires: expiresAt,
        sameSite: 'lax',
        path: '/'
    })

    if (role === 'Admin') redirect('/admin')
    else if (role === 'Faculty') redirect('/faculty')
    else redirect('/student')
}


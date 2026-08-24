import { NextRequest, NextResponse } from "next/server"
import { decrypt } from "./lib/session"

const protectedRoute = ['/', '/admin', '/faculty', '/student']
const publicRoute = ['/login']

const rolePaths: Record<string, string> = {
    'Admin': '/admin',
    'Faculty': '/faculty',
    'Student': '/student'
}

export default async function middleware(req: NextRequest) {
    const path = req.nextUrl.pathname
    const isProtectedRoute = protectedRoute.includes(path) || path.startsWith('/admin') || path.startsWith('/faculty') || path.startsWith('/student')
    const isPublicRoute = publicRoute.includes(path)

    const cookie = req.cookies.get("session")?.value
    const session = await decrypt(cookie)

    if (isProtectedRoute && !session?.email) {
        return NextResponse.redirect(new URL('/login', req.nextUrl))
    }

    if (path === '/' && session?.role) {
        const userRolePath = rolePaths[session.role]
        if (userRolePath) {
            return NextResponse.redirect(new URL(userRolePath, req.nextUrl))
        }
    }

    if (isPublicRoute && session?.email) {
        const userRolePath = rolePaths[session.role]
        if (userRolePath) {
            return NextResponse.redirect(new URL(userRolePath, req.nextUrl))
        }
    }

    if (session?.role) {
        const userRolePath = rolePaths[session.role]
        if (!userRolePath) {
            return NextResponse.redirect(new URL('/login', req.nextUrl))
        }
        if (path.startsWith('/admin') && session.role !== 'Admin') {
            return NextResponse.redirect(new URL(userRolePath, req.nextUrl))
        }
        if (path.startsWith('/faculty') && session.role !== 'Faculty') {
            return NextResponse.redirect(new URL(userRolePath, req.nextUrl))
        }
        if (path.startsWith('/student') && session.role !== 'Student') {
            return NextResponse.redirect(new URL(userRolePath, req.nextUrl))
        }
    }
}

export const config = {
    matcher: ['/((?!api|_next/static|_next/image|.*\\.png$).*)'],
}

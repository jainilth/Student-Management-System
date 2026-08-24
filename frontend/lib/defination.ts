import { JWTPayload } from "jose";

export interface SessionPlayload extends JWTPayload {
    id: string
    email: string
    role: string
    name: string
    accessToken: string
    expiresAt: number
}

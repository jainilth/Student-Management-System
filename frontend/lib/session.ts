import { jwtVerify, SignJWT } from "jose"
import { SessionPlayload } from "./defination"

const secretKey = process.env.SESSION_SECRET || 'default-secret-key-for-dev'
const encodeKey = new TextEncoder().encode(secretKey)

export async function encrypt(payload: SessionPlayload) {
    return new SignJWT(payload)
        .setProtectedHeader({ alg: "HS256" })
        .setIssuedAt()
        .setExpirationTime("7d")
        .sign(encodeKey)
}

export async function decrypt(session?: string) {
    if (!session) return null

    try {
        const { payload } = await jwtVerify(session, encodeKey, {
            algorithms: ["HS256"]
        })
        return payload as SessionPlayload
    }
    catch {
        return null
    }
}

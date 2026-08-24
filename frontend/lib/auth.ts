import { cookies } from "next/headers";
import { decrypt } from "./session";

export async function getSession() {
    // const cookie = (await cookies()).get("session")?.value
    // return await decrypt(cookie)
    return {
        accessToken: "mock_token_since_auth_is_disabled",
        role: "Admin",
        name: "Admin User"
    }
}

export async function logout() {
    (await cookies()).delete("session")
}

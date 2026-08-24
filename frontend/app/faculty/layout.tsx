import { getSession } from "@/lib/auth";
import { redirect } from "next/navigation";

export default async function FacultyLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    const session = await getSession();
    // if (!session || session.role !== 'Faculty') {
    //     redirect('/login')
    // }

    return (
        <div className="min-h-screen bg-gray-100 flex flex-col">
            <nav className="bg-emerald-600 shadow-md">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="flex justify-between h-16">
                        <div className="flex items-center">
                            <span className="text-white font-bold text-xl">Faculty Dashboard</span>
                        </div>
                        <div className="flex items-center space-x-4">
                            <span className="text-emerald-100">{session.name}</span>
                            <form action={async () => {
                                'use server'
                                const { logout } = await import('@/lib/auth')
                                await logout()
                                redirect('/login')
                            }}>
                                <button className="bg-emerald-500 hover:bg-emerald-400 text-white px-3 py-1 rounded">Logout</button>
                            </form>
                        </div>
                    </div>
                </div>
            </nav>
            <main className="flex-1 w-full max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
                {children}
            </main>
        </div>
    );
}

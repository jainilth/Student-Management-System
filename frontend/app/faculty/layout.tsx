import { getSession, logout } from "@/lib/auth";
import { redirect } from "next/navigation";
import FacultySidebar from "@/components/FacultySidebar";
import { Bell, Search } from "lucide-react";

export default async function FacultyLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    const session = await getSession();
    // if (!session || session.role !== 'Faculty') {
    //     redirect('/login')
    // }

    async function handleLogout() {
        "use server";
        await logout();
        redirect("/login");
    }

    return (
        <div className="admin-shell flex h-screen overflow-hidden bg-background">
            <FacultySidebar />

            <div className="min-w-0 flex-1 overflow-y-auto">
                {/* Top nav */}
                <nav className="sticky top-0 z-50 border-b bg-surface/95 shadow-sm backdrop-blur">
                    <div className="flex h-[72px] items-center gap-4 px-5 sm:px-8">
                        {/* Search */}
                        <form className="mx-auto flex max-w-[460px] flex-1 items-center gap-2">
                            <div className="relative flex-1">
                                <Search className="absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-muted" />
                                <input
                                    aria-label="Search"
                                    placeholder="Search students, subjects, sessions..."
                                    className="w-full rounded-full border-none bg-background py-2.5 pl-10 pr-5 text-sm shadow-inner"
                                />
                            </div>
                        </form>

                        <div className="flex items-center gap-3">
                            <button
                                aria-label="Notifications"
                                className="relative rounded-full border bg-surface p-2.5 hover:bg-background"
                            >
                                <Bell className="h-4 w-4 text-foreground" />
                                <span className="absolute right-1.5 top-1.5 h-2 w-2 rounded-full bg-brand border-2 border-surface" />
                            </button>

                            <div className="flex items-center gap-2.5">
                                <div className="flex h-9 w-9 items-center justify-center rounded-full bg-sidebar text-sm font-bold text-brand">
                                    {session.name?.[0]?.toUpperCase() ?? "F"}
                                </div>
                                <div className="hidden lg:block">
                                    <p className="text-sm font-semibold leading-tight">{session.name}</p>
                                    <p className="text-xs text-muted">Faculty ↓</p>
                                </div>
                            </div>

                            <form action={handleLogout}>
                                <button
                                    type="submit"
                                    className="rounded-full border border-sidebar px-4 py-2 text-sm font-semibold text-sidebar hover:bg-sidebar hover:text-white transition-colors"
                                >
                                    Logout
                                </button>
                            </form>
                        </div>
                    </div>
                </nav>

                <main className="mx-auto max-w-[1400px] p-5 sm:p-8">
                    {children}
                </main>
            </div>
        </div>
    );
}

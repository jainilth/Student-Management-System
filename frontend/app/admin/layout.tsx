import { getSession, logout } from "@/lib/auth";
import { redirect } from "next/navigation";
import Sidebar from "@/components/Sidebar";

export default async function AdminLayout({ children }: Readonly<{ children: React.ReactNode }>) {
    const session = await getSession();
    async function handleLogout() {
        "use server";
        await logout();
        redirect("/login");
    }

    return <div className="admin-shell flex h-screen overflow-hidden bg-background"><Sidebar /><div className="min-w-0 flex-1 overflow-y-auto"><nav className="sticky top-0 z-50 border-b bg-surface/95 shadow-sm backdrop-blur"><div className="flex h-[86px] items-center gap-4 px-5 sm:px-10"><button aria-label="Collapse menu" className="hidden h-10 w-10 items-center justify-center rounded-xl border bg-surface text-lg md:flex">|&lt;</button><button className="hidden rounded-full bg-sidebar px-5 py-3 text-sm font-bold text-white sm:block">+ &nbsp; Create</button><form className="mx-auto flex max-w-[500px] flex-1 items-center" action="/admin"><input aria-label="Search" placeholder="Search anything in Spark..." className="w-full rounded-full border-none bg-background px-5 py-3 text-sm shadow-inner" /></form><div className="hidden items-center gap-3 sm:flex"><button aria-label="Fullscreen" className="rounded-full border bg-surface px-4 py-2 text-lg hover:bg-background">⛶</button><button aria-label="Notifications" className="relative rounded-full border bg-surface px-4 py-2 text-lg hover:bg-background">♧<span className="absolute right-1 top-0 h-2 w-2 rounded-full bg-brand" /></button></div><div className="flex h-10 w-10 items-center justify-center rounded-full bg-sidebar text-sm font-bold text-brand">A</div><div className="hidden lg:block"><p className="text-sm font-semibold">{session.name}</p><p className="text-xs text-muted">Administrator⌄</p></div><form action={handleLogout}><button type="submit" className="rounded-full border border-sidebar px-4 py-2 text-sm font-semibold text-sidebar hover:bg-sidebar hover:text-white">Logout</button></form></div></nav><main className="mx-auto max-w-[1500px] p-5 sm:p-10">{children}</main></div></div>;
}

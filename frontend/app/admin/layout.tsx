import { getSession } from "@/lib/auth";
import { redirect } from "next/navigation";

import Sidebar from "@/components/Sidebar";

export default async function AdminLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const session = await getSession();
  // if (!session || session.role !== 'Admin') {
  //     redirect('/login')
  // }

  return (
    <div className="h-screen overflow-hidden bg-gray-50 flex flex-col">
      <nav className="shrink-0 bg-indigo-600 shadow-md sticky top-0 z-50">
        <div className="max-w-[1400px] mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between h-16">
            <div className="flex items-center">
              <span className="text-white font-extrabold text-2xl tracking-tight">
                SMS Admin
              </span>
            </div>
            <div className="flex items-center space-x-6">
              <span className="text-indigo-100 font-medium">
                {session.name}
              </span>
              <form
                action={async () => {
                  "use server";
                  const { logout } = await import("@/lib/auth");
                  await logout();
                  redirect("/login");
                }}
              >
                <button className="bg-white/10 hover:bg-white/20 text-white border border-white/20 px-4 py-1.5 rounded-full font-medium transition-colors text-sm">
                  Sign Out
                </button>
              </form>
            </div>
          </div>
        </div>
      </nav>
      <div className="min-h-0 flex flex-1 overflow-hidden">
        <Sidebar />
        <main className="min-h-0 flex-1 w-full max-w-[1400px] mx-auto overflow-y-auto p-8">
          {children}
        </main>
      </div>
    </div>
  );
}

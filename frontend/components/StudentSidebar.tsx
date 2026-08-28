"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import {
    Activity,
    BookOpen,
    Calendar,
    ChartBar,
    ClipboardList,
    FileText,
    GraduationCap,
    Home,
    Layers,
    LayoutDashboard,
} from "lucide-react";

const navItems = [
    { name: "Dashboard", path: "/student", icon: LayoutDashboard, exact: true },
    { name: "My Subjects", path: "/student/subjects", icon: BookOpen },
    { name: "My Grades", path: "/student/grades", icon: FileText },
    { name: "Attendance", path: "/student/attendance", icon: Activity },
    { name: "My Projects", path: "/student/projects", icon: Layers },
    { name: "Semester Results", path: "/student/results", icon: ChartBar },
    { name: "Study Materials", path: "/student/materials", icon: ClipboardList },
    { name: "Class Schedule", path: "/student/schedule", icon: Calendar },
];

export default function StudentSidebar() {
    const pathname = usePathname();

    return (
        <aside className="hidden h-full min-h-0 w-[260px] shrink-0 bg-sidebar text-white md:block">
            <div className="flex h-full flex-col overflow-y-auto px-5 py-7">
                {/* Logo */}
                <Link href="/student" className="mb-10 flex items-center gap-3">
                    <div className="flex h-9 w-9 items-center justify-center rounded-xl bg-brand/20">
                        <GraduationCap className="h-5 w-5 text-brand" />
                    </div>
                    <div>
                        <span className="block text-sm font-bold tracking-tight text-white">SPMS</span>
                        <span className="block text-xs text-slate-400">Student Portal</span>
                    </div>
                </Link>

                {/* Nav */}
                <nav>
                    <p className="mb-3 px-3 text-[10px] font-semibold uppercase tracking-widest text-slate-500">
                        Navigation
                    </p>
                    <ul className="space-y-1">
                        {navItems.map((item) => {
                            const Icon = item.icon;
                            const isActive = item.exact
                                ? pathname === item.path
                                : pathname.startsWith(item.path);
                            return (
                                <li key={item.path}>
                                    <Link
                                        href={item.path}
                                        className={`group flex items-center gap-3 rounded-xl px-3 py-2.5 text-sm transition-all ${
                                            isActive
                                                ? "bg-brand/15 font-semibold text-brand"
                                                : "text-slate-400 hover:bg-white/8 hover:text-white"
                                        }`}
                                    >
                                        <Icon
                                            className={`h-4 w-4 shrink-0 ${
                                                isActive ? "text-brand" : "text-slate-500 group-hover:text-slate-300"
                                            }`}
                                        />
                                        <span>{item.name}</span>
                                        {isActive && (
                                            <span className="ml-auto h-1.5 w-1.5 rounded-full bg-brand" />
                                        )}
                                    </Link>
                                </li>
                            );
                        })}
                    </ul>
                </nav>

                {/* Quick links section */}
                <div className="mt-8">
                    <p className="mb-3 px-3 text-[10px] font-semibold uppercase tracking-widest text-slate-500">
                        Quick Access
                    </p>
                    <ul className="space-y-1">
                        <li>
                            <Link
                                href="/student/profile"
                                className="group flex items-center gap-3 rounded-xl px-3 py-2.5 text-sm text-slate-400 transition-all hover:bg-white/8 hover:text-white"
                            >
                                <Home className="h-4 w-4 shrink-0 text-slate-500 group-hover:text-slate-300" />
                                <span>My Profile</span>
                            </Link>
                        </li>
                    </ul>
                </div>

                {/* Bottom decoration */}
                <div className="mt-auto pt-6">
                    <div className="rounded-2xl bg-brand/10 p-4">
                        <p className="text-xs font-semibold text-brand">Academic Year 2024</p>
                        <p className="mt-1 text-[11px] text-slate-400">Semester 5 · In Progress</p>
                        <div className="mt-3 h-1 rounded-full bg-white/10">
                            <div className="h-1 w-[65%] rounded-full bg-brand" />
                        </div>
                        <p className="mt-1 text-[10px] text-slate-500">65% completed</p>
                    </div>
                </div>
            </div>
        </aside>
    );
}

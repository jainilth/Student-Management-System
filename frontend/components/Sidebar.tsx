import Link from "next/link";
import {
    Activity,
    BookOpen,
    Calendar,
    Database,
    FileText,
    GraduationCap,
    Home,
    LayoutDashboard,
    Users,
} from "lucide-react";

const entities = [
    { name: "Users", path: "user", icon: Users },
    { name: "Roles", path: "role", icon: Users },
    { name: "Students", path: "student", icon: GraduationCap },
    { name: "Faculty", path: "faculty", icon: Users },
    { name: "Academic Programs", path: "academic-program", icon: BookOpen },
    { name: "Academic Years", path: "academic-year", icon: Calendar },
    { name: "Semesters", path: "semester", icon: Calendar },
    { name: "Subjects", path: "subject", icon: BookOpen },
    { name: "Departments", path: "department", icon: LayoutDashboard },
    { name: "Attendance", path: "attendance", icon: Activity },
    { name: "Attendance Records", path: "attendance-record", icon: Activity },
    { name: "Class Sessions", path: "class-session", icon: Calendar },
    { name: "Faculty Subjects", path: "faculty-subject", icon: BookOpen },
    { name: "Grades", path: "grade", icon: FileText },
    { name: "Materials", path: "material", icon: FileText },
    { name: "Projects", path: "project", icon: Database },
    { name: "Project Allocations", path: "project-allocation", icon: Database },
    { name: "Project Tasks", path: "project-task", icon: FileText },
    { name: "Semester Results", path: "semester-result", icon: FileText },
    { name: "Semester Subjects", path: "semester-subject", icon: BookOpen },
    { name: "Student Semesters", path: "student-semester", icon: GraduationCap },
    { name: "Subject Results", path: "subject-result", icon: FileText },
];

export default function Sidebar() {
    return (
        <aside className="hidden h-full min-h-0 w-[280px] shrink-0 bg-sidebar text-white md:block">
            <div className="flex h-full flex-col overflow-y-auto px-6 py-7">
                <Link href="/admin" className="mb-12 flex items-center gap-3">
                    <div className="text-3xl font-bold text-brand">✱</div>
                    <span className="text-xl font-bold tracking-tight">Spark Admin</span>
                </Link>
                <ul className="space-y-1 font-medium">
                    <li>
                        <Link href="/admin" className="group flex items-center gap-3 rounded-xl px-3 py-3 text-sm text-slate-300 transition-colors hover:bg-white/10 hover:text-white">
                            <Home className="h-5 w-5 text-slate-400 group-hover:text-brand" />
                            <span>Dashboard Home</span>
                        </Link>
                    </li>
                    <li className="pb-2 pt-4">
                        <span className="px-2 text-xs font-semibold uppercase tracking-wider text-gray-400">Management Entities</span>
                    </li>
                    {entities.map((entity) => {
                        const Icon = entity.icon;
                        return (
                            <li key={entity.path}>
                                <Link href={`/admin/${entity.path}`} className="group flex items-center gap-3 rounded-xl px-3 py-3 text-sm text-slate-300 transition-colors hover:bg-white/10 hover:text-white">
                                    <Icon className="h-5 w-5 text-slate-400 group-hover:text-brand" />
                                    <span className="whitespace-nowrap">{entity.name}</span>
                                </Link>
                            </li>
                        );
                    })}
                </ul>
            </div>
        </aside>
    );
}

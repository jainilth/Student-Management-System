import {
    Activity,
    ArrowRight,
    BookOpen,
    ChevronRight,
    FileText,
    GraduationCap,
    Layers,
    TrendingUp,
    Users,
    Zap,
    Building2,
    CalendarDays,
    ClipboardCheck,
    BarChart3,
    Database,
    CheckCircle,
    AlertTriangle,
} from "lucide-react";

// ─────────────────────────────────────────────
// Mock data
// ─────────────────────────────────────────────
const stats = [
    {
        label: "Total Students",
        value: "1,284",
        sub: "Enrolled this year",
        icon: GraduationCap,
        color: "bg-[#062b1d]",
        textColor: "text-[#062b1d]",
        linkLabel: "View Students",
        href: "/admin/student",
    },
    {
        label: "Total Faculty",
        value: "86",
        sub: "Active members",
        icon: Users,
        color: "bg-blue-500",
        textColor: "text-blue-600",
        linkLabel: "View Faculty",
        href: "/admin/faculty",
    },
    {
        label: "Departments",
        value: "12",
        sub: "Across campus",
        icon: Building2,
        color: "bg-purple-500",
        textColor: "text-purple-600",
        linkLabel: "View Depts",
        href: "/admin/department",
    },
    {
        label: "Active Programs",
        value: "18",
        sub: "Academic programs",
        icon: BookOpen,
        color: "bg-amber-500",
        textColor: "text-amber-600",
        linkLabel: "View Programs",
        href: "/admin/academic-program",
    },
    {
        label: "Current Semester",
        value: "Sem 5",
        sub: "Aug – Dec 2024",
        icon: CalendarDays,
        color: "bg-emerald-500",
        textColor: "text-emerald-600",
        linkLabel: "View Semesters",
        href: "/admin/semester",
    },
    {
        label: "Active Projects",
        value: "47",
        sub: "Ongoing allocations",
        icon: Layers,
        color: "bg-rose-500",
        textColor: "text-rose-600",
        linkLabel: "View Projects",
        href: "/admin/project",
    },
];

const departmentStats = [
    { name: "Computer Science", students: 320, faculty: 22, programs: 4 },
    { name: "Electronics", students: 215, faculty: 18, programs: 3 },
    { name: "Mechanical", students: 198, faculty: 16, programs: 3 },
    { name: "Civil", students: 176, faculty: 14, programs: 3 },
    { name: "Information Tech.", students: 162, faculty: 10, programs: 3 },
    { name: "Chemical", students: 213, faculty: 6, programs: 2 },
];
const maxStudents = Math.max(...departmentStats.map((d) => d.students));

const enrollmentTrend = [
    { month: "Jan", count: 980 },
    { month: "Feb", count: 1020 },
    { month: "Mar", count: 1050 },
    { month: "Apr", count: 1090 },
    { month: "May", count: 1120 },
    { month: "Jun", count: 1160 },
    { month: "Jul", count: 1210 },
    { month: "Aug", count: 1284 },
];
const maxEnroll = Math.max(...enrollmentTrend.map((e) => e.count));

const recentTransactions = [
    { action: "Student Enrolled", detail: "B.Tech CS — Jainil Patel", date: "28 Aug 2024", status: "New" },
    { action: "Faculty Updated", detail: "Dr. Sharma — Electronics", date: "27 Aug 2024", status: "Updated" },
    { action: "Grade Submitted", detail: "CS303 Mid-term", date: "26 Aug 2024", status: "Complete" },
    { action: "Project Allocated", detail: "Library Mgmt System", date: "25 Aug 2024", status: "Pending" },
    { action: "Semester Created", detail: "Semester 6 — 2025", date: "24 Aug 2024", status: "New" },
    { action: "Attendance Marked", detail: "CS301 — 32 students", date: "24 Aug 2024", status: "Complete" },
];

const statusStyles: Record<string, string> = {
    New: "bg-blue-100 text-blue-600",
    Updated: "bg-amber-100 text-amber-600",
    Complete: "bg-emerald-100 text-emerald-700",
    Pending: "bg-rose-100 text-rose-600",
};

const systemAlerts = [
    { msg: "8 students below 75% attendance in CS304.", severity: "warn" },
    { msg: "Semester 6 registration opens in 12 days.", severity: "info" },
    { msg: "3 project allocations missing faculty guide.", severity: "warn" },
    { msg: "All mid-term grades submitted successfully.", severity: "ok" },
];

const alertStyles: Record<string, { bg: string; icon: string }> = {
    warn: { bg: "bg-amber-50 border-amber-200 text-amber-800", icon: "⚠️" },
    info: { bg: "bg-blue-50 border-blue-200 text-blue-800", icon: "ℹ️" },
    ok: { bg: "bg-emerald-50 border-emerald-200 text-emerald-800", icon: "✅" },
};

const quickLinks = [
    { label: "Add Student", icon: GraduationCap, href: "/admin/student", color: "text-[#062b1d]", bg: "bg-green-50" },
    { label: "Add Faculty", icon: Users, href: "/admin/faculty", color: "text-blue-600", bg: "bg-blue-50" },
    { label: "Subjects", icon: BookOpen, href: "/admin/subject", color: "text-amber-600", bg: "bg-amber-50" },
    { label: "Attendance", icon: Activity, href: "/admin/attendance", color: "text-emerald-600", bg: "bg-emerald-50" },
    { label: "Grades", icon: ClipboardCheck, href: "/admin/grade", color: "text-purple-600", bg: "bg-purple-50" },
    { label: "Reports", icon: BarChart3, href: "/admin/semester-result", color: "text-rose-600", bg: "bg-rose-50" },
    { label: "Projects", icon: Database, href: "/admin/project", color: "text-cyan-600", bg: "bg-cyan-50" },
    { label: "Materials", icon: FileText, href: "/admin/material", color: "text-slate-600", bg: "bg-slate-100" },
];

// ─────────────────────────────────────────────
// Page
// ─────────────────────────────────────────────
export default function AdminPage() {
    const today = new Date().toLocaleDateString("en-IN", {
        weekday: "long",
        year: "numeric",
        month: "long",
        day: "numeric",
    });

    return (
        <section className="space-y-7">
            {/* ── Header ── */}
            <div className="flex flex-col justify-between gap-4 sm:flex-row sm:items-end">
                <div>
                    <p className="mb-2 text-xs font-semibold uppercase tracking-[0.18em] text-brand-dark">
                        Admin Dashboard
                    </p>
                    <h1 className="text-3xl font-bold tracking-tight text-foreground">
                        Welcome back, Administrator 👋
                    </h1>
                    <p className="mt-2 text-sm text-muted">{today} · System-wide academic overview.</p>
                </div>
                <div className="flex items-center gap-3">
                    <span className="inline-flex items-center gap-1.5 rounded-full bg-emerald-50 px-3 py-1.5 text-xs font-semibold text-emerald-700">
                        <span className="h-1.5 w-1.5 rounded-full bg-emerald-500 animate-pulse" />
                        All systems operational
                    </span>
                    <span className="rounded-full border bg-surface px-4 py-2 text-xs font-semibold text-muted shadow-sm">
                        Academic Year 2024–25
                    </span>
                </div>
            </div>

            {/* ── Stats row ── */}
            <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-6">
                {stats.map((s) => {
                    const Icon = s.icon;
                    return (
                        <div
                            key={s.label}
                            className="group relative overflow-hidden rounded-2xl bg-surface p-5 shadow-sm transition-all hover:shadow-md hover:-translate-y-0.5"
                        >
                            <div className="flex items-start justify-between">
                                <div className={`flex h-10 w-10 items-center justify-center rounded-xl ${s.color} shadow-sm`}>
                                    <Icon className="h-5 w-5 text-white" />
                                </div>
                                <ChevronRight className="h-4 w-4 text-muted opacity-0 transition-opacity group-hover:opacity-100" />
                            </div>
                            <p className="mt-4 text-3xl font-bold tracking-tight">{s.value}</p>
                            <p className="mt-0.5 text-xs text-muted">{s.label}</p>
                            <p className="mt-0.5 text-[11px] text-muted/70">{s.sub}</p>
                            <a
                                href={s.href}
                                className={`mt-3 inline-flex items-center gap-1 text-xs font-semibold ${s.textColor} hover:underline`}
                            >
                                {s.linkLabel} <ArrowRight className="h-3 w-3" />
                            </a>
                            <div className="pointer-events-none absolute -bottom-4 -right-4 h-16 w-16 rounded-full bg-black/[0.03]" />
                        </div>
                    );
                })}
            </div>

            {/* ── Row 2: Enrollment trend + Dept breakdown ── */}
            <div className="grid gap-5 xl:grid-cols-[1.3fr_1fr]">
                {/* Enrollment trend bar chart */}
                <div className="rounded-2xl bg-surface p-6 shadow-sm">
                    <div className="flex items-start justify-between">
                        <div>
                            <h2 className="text-base font-bold">Student Enrollment Trend</h2>
                            <p className="mt-0.5 text-xs text-muted">Monthly enrollment count — 2024</p>
                        </div>
                        <span className="inline-flex items-center gap-1 rounded-full bg-brand/15 px-3 py-1 text-xs font-bold text-brand-dark">
                            <TrendingUp className="h-3 w-3" /> +31% YTD
                        </span>
                    </div>

                    <div className="mt-6 flex h-48 items-end gap-2 border-b border-dashed border-line px-1">
                        {enrollmentTrend.map((e, i) => {
                            const isLatest = i === enrollmentTrend.length - 1;
                            return (
                                <div key={e.month} className="flex flex-1 flex-col items-center gap-1">
                                    <span className="text-[10px] font-bold text-foreground">{e.count}</span>
                                    <div
                                        className={`w-full rounded-t-lg transition-all duration-500 ${isLatest ? "bg-sidebar" : "bg-brand"}`}
                                        style={{ height: `${(e.count / maxEnroll) * 100}%` }}
                                    />
                                </div>
                            );
                        })}
                    </div>
                    <div className="mt-2 flex justify-around px-1 text-[11px] text-muted">
                        {enrollmentTrend.map((e) => (
                            <span key={e.month}>{e.month}</span>
                        ))}
                    </div>

                    <div className="mt-5 grid grid-cols-4 gap-3">
                        {[
                            { label: "Total Students", val: "1,284" },
                            { label: "New This Month", val: "+74" },
                            { label: "Total Faculty", val: "86" },
                            { label: "Programs", val: "18" },
                        ].map((m) => (
                            <div key={m.label} className="rounded-xl bg-background px-3 py-2.5 text-center">
                                <p className="text-base font-bold">{m.val}</p>
                                <p className="text-[10px] text-muted">{m.label}</p>
                            </div>
                        ))}
                    </div>
                </div>

                {/* Department breakdown */}
                <div className="rounded-2xl bg-surface p-6 shadow-sm">
                    <div className="flex items-center justify-between">
                        <div>
                            <h2 className="text-base font-bold">Department Overview</h2>
                            <p className="mt-0.5 text-xs text-muted">Students · Faculty · Programs</p>
                        </div>
                        <a href="/admin/department" className="text-xs font-semibold text-brand-dark hover:underline">
                            View all →
                        </a>
                    </div>
                    <div className="mt-5 space-y-4">
                        {departmentStats.map((dept) => (
                            <div key={dept.name}>
                                <div className="flex items-center justify-between text-sm">
                                    <span className="font-semibold">{dept.name}</span>
                                    <div className="flex items-center gap-3 text-[11px] text-muted">
                                        <span className="flex items-center gap-1">
                                            <GraduationCap className="h-3 w-3" /> {dept.students}
                                        </span>
                                        <span className="flex items-center gap-1">
                                            <Users className="h-3 w-3" /> {dept.faculty}
                                        </span>
                                    </div>
                                </div>
                                <div className="mt-1.5 h-1.5 w-full rounded-full bg-background">
                                    <div
                                        className="h-1.5 rounded-full bg-brand-dark transition-all duration-500"
                                        style={{ width: `${(dept.students / maxStudents) * 100}%` }}
                                    />
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>

            {/* ── Row 3: Recent Transactions + System Alerts + Donut ── */}
            <div className="grid gap-5 xl:grid-cols-[1.6fr_1fr]">
                {/* Recent transactions */}
                <div className="rounded-2xl bg-surface p-6 shadow-sm">
                    <div className="flex items-center justify-between">
                        <div>
                            <h2 className="text-base font-bold">Recent System Activity</h2>
                            <p className="mt-0.5 text-xs text-muted">Latest actions across all modules</p>
                        </div>
                        <button className="text-xs font-semibold text-brand-dark hover:underline">View all →</button>
                    </div>
                    <div className="mt-4 overflow-x-auto">
                        <table className="w-full text-sm">
                            <thead>
                                <tr className="border-b text-left text-xs text-muted">
                                    <th className="pb-3 font-semibold">Action</th>
                                    <th className="pb-3 font-semibold">Detail</th>
                                    <th className="pb-3 font-semibold">Date</th>
                                    <th className="pb-3 font-semibold">Status</th>
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-line">
                                {recentTransactions.map((t) => (
                                    <tr key={t.action + t.date} className="transition-colors hover:bg-brand/5">
                                        <td className="py-3 pr-4 font-semibold">{t.action}</td>
                                        <td className="py-3 pr-4 text-xs text-muted">{t.detail}</td>
                                        <td className="py-3 pr-4 text-xs text-muted whitespace-nowrap">{t.date}</td>
                                        <td className="py-3">
                                            <span className={`rounded-full px-2.5 py-0.5 text-[11px] font-bold ${statusStyles[t.status]}`}>
                                                {t.status}
                                            </span>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>

                {/* System Alerts + Summary Donut */}
                <div className="space-y-4">
                    {/* Alerts */}
                    <div className="rounded-2xl bg-surface p-6 shadow-sm">
                        <div className="flex items-center justify-between">
                            <h2 className="text-base font-bold">System Alerts</h2>
                            <span className="rounded-full bg-amber-100 px-2.5 py-1 text-xs font-bold text-amber-600">
                                {systemAlerts.filter((a) => a.severity === "warn").length} warnings
                            </span>
                        </div>
                        <div className="mt-4 space-y-2.5">
                            {systemAlerts.map((alert, i) => (
                                <div
                                    key={i}
                                    className={`flex items-start gap-2.5 rounded-xl border px-3 py-2.5 text-xs ${alertStyles[alert.severity].bg}`}
                                >
                                    <span>{alertStyles[alert.severity].icon}</span>
                                    <span>{alert.msg}</span>
                                </div>
                            ))}
                        </div>
                    </div>

                    {/* Summary donut */}
                    <div className="rounded-2xl bg-surface p-6 shadow-sm">
                        <h2 className="text-base font-bold">User Distribution</h2>
                        <div className="mt-4 flex items-center gap-5">
                            <div
                                className="h-24 w-24 shrink-0 rounded-full"
                                style={{
                                    background: `conic-gradient(#062b1d 0 73%, #b7f000 73% 93%, #ff7417 93% 100%)`,
                                }}
                            >
                                <div className="flex h-full w-full scale-[0.68] items-center justify-center rounded-full bg-surface">
                                    <span className="text-xs font-bold text-center leading-tight">
                                        1,376<br />
                                        <span className="text-[9px] font-normal text-muted">total</span>
                                    </span>
                                </div>
                            </div>
                            <div className="space-y-2 text-xs">
                                <div className="flex items-center gap-2">
                                    <span className="h-2.5 w-2.5 rounded-full bg-sidebar" />
                                    <span className="text-muted">Students</span>
                                    <span className="ml-auto font-bold">1,284</span>
                                </div>
                                <div className="flex items-center gap-2">
                                    <span className="h-2.5 w-2.5 rounded-full bg-brand" />
                                    <span className="text-muted">Faculty</span>
                                    <span className="ml-auto font-bold">86</span>
                                </div>
                                <div className="flex items-center gap-2">
                                    <span className="h-2.5 w-2.5 rounded-full bg-orange-400" />
                                    <span className="text-muted">Admins</span>
                                    <span className="ml-auto font-bold">6</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {/* ── Row 4: Quick Links ── */}
            <div className="rounded-2xl bg-surface p-6 shadow-sm">
                <div className="flex items-center justify-between">
                    <div>
                        <h2 className="text-base font-bold">Quick Access</h2>
                        <p className="mt-0.5 text-xs text-muted">Jump directly to any management module</p>
                    </div>
                    <div className="relative overflow-hidden rounded-xl bg-sidebar px-4 py-2.5 text-white">
                        <span className="text-xs font-semibold">
                            <Zap className="mr-1.5 inline h-3 w-3 text-brand" />
                            Admin Control Panel
                        </span>
                        <div className="absolute -right-2 -top-2 text-2xl font-bold text-brand/30">✱</div>
                    </div>
                </div>
                <div className="mt-5 grid grid-cols-2 gap-3 sm:grid-cols-4 lg:grid-cols-8">
                    {quickLinks.map((q) => {
                        const Icon = q.icon;
                        return (
                            <a
                                key={q.label}
                                href={q.href}
                                className="group flex flex-col items-center gap-2 rounded-xl bg-background p-4 text-center transition-all hover:shadow-md hover:-translate-y-0.5"
                            >
                                <div className={`flex h-10 w-10 items-center justify-center rounded-xl ${q.bg}`}>
                                    <Icon className={`h-5 w-5 ${q.color}`} />
                                </div>
                                <span className="text-[11px] font-semibold leading-tight">{q.label}</span>
                            </a>
                        );
                    })}
                </div>
            </div>
        </section>
    );
}

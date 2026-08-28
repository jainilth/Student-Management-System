import {
    Activity,
    ArrowRight,
    BookOpen,
    CheckCircle,
    ChevronRight,
    Clock,
    FileText,
    Layers,
    Star,
    TrendingUp,
    Users,
    Zap,
    CalendarDays,
    BarChart3,
    ClipboardCheck,
    AlertTriangle,
} from "lucide-react";

// ─────────────────────────────────────────────
// Mock data
// ─────────────────────────────────────────────
const stats = [
    {
        label: "Assigned Subjects",
        value: "6",
        sub: "This semester",
        icon: BookOpen,
        color: "bg-[#062b1d]",
        textColor: "text-[#062b1d]",
        linkLabel: "View Subjects",
        href: "/faculty/subjects",
    },
    {
        label: "Total Students",
        value: "148",
        sub: "Across all subjects",
        icon: Users,
        color: "bg-blue-500",
        textColor: "text-blue-600",
        linkLabel: "View Students",
        href: "/faculty/students",
    },
    {
        label: "Classes Today",
        value: "3",
        sub: "Scheduled",
        icon: CalendarDays,
        color: "bg-amber-500",
        textColor: "text-amber-600",
        linkLabel: "View Schedule",
        href: "/faculty/class-sessions",
    },
    {
        label: "Avg Attendance",
        value: "79%",
        sub: "Across subjects",
        icon: Activity,
        color: "bg-emerald-500",
        textColor: "text-emerald-600",
        linkLabel: "View Records",
        href: "/faculty/attendance",
    },
    {
        label: "Pending Grades",
        value: "12",
        sub: "Awaiting entry",
        icon: ClipboardCheck,
        color: "bg-rose-500",
        textColor: "text-rose-600",
        linkLabel: "Enter Grades",
        href: "/faculty/grades",
    },
    {
        label: "Active Projects",
        value: "3",
        sub: "Supervised",
        icon: Layers,
        color: "bg-purple-500",
        textColor: "text-purple-600",
        linkLabel: "View Projects",
        href: "/faculty/projects",
    },
];

const mySubjects = [
    { name: "Data Structures", code: "CS301", students: 32, attended: 78, sessions: 22, done: 18 },
    { name: "Operating Systems", code: "CS302", students: 30, attended: 72, sessions: 20, done: 14 },
    { name: "Database Systems", code: "CS303", students: 28, attended: 88, sessions: 22, done: 20 },
    { name: "Computer Networks", code: "CS304", students: 26, attended: 65, sessions: 20, done: 13 },
    { name: "Software Engineering", code: "CS305", students: 22, attended: 81, sessions: 22, done: 17 },
    { name: "Web Technology", code: "CS306", students: 10, attended: 91, sessions: 22, done: 21 },
];

const todaysSchedule = [
    { subject: "Data Structures", code: "CS301", time: "09:00 – 10:00", room: "Lab A", status: "Upcoming" },
    { subject: "Database Systems", code: "CS303", time: "11:00 – 12:00", room: "Room 201", status: "Upcoming" },
    { subject: "Web Technology", code: "CS306", time: "14:00 – 15:00", room: "Lab B", status: "Upcoming" },
];

const recentActivities = [
    { text: "Attendance marked for CS301 — 28/32 present.", time: "1 hour ago", icon: "✅" },
    { text: "Grade submitted for CS303 mid-term exam.", time: "3 hours ago", icon: "📝" },
    { text: "New study material uploaded for CS306.", time: "Yesterday", icon: "📄" },
    { text: "Project review completed for Library Mgmt System.", time: "2 days ago", icon: "🔍" },
    { text: "Semester 5 grading period started.", time: "3 days ago", icon: "🎓" },
];

const projects = [
    { title: "Library Management System", team: 4, progress: 68, status: "In Progress" },
    { title: "Hospital Records Portal", team: 3, progress: 42, status: "In Progress" },
    { title: "E-Commerce Platform", team: 5, progress: 91, status: "Near Complete" },
];

const pendingActions = [
    { task: "Enter mid-term grades for CS304", due: "30 Aug 2024", urgency: "High" },
    { task: "Review project report — Library Mgmt", due: "01 Sep 2024", urgency: "Medium" },
    { task: "Upload OS Lab materials", due: "03 Sep 2024", urgency: "Medium" },
    { task: "Submit semester attendance report", due: "10 Sep 2024", urgency: "Low" },
];

const weeklyAttendance = [
    { day: "Mon", pct: 82 },
    { day: "Tue", pct: 75 },
    { day: "Wed", pct: 90 },
    { day: "Thu", pct: 68 },
    { day: "Fri", pct: 85 },
];

const urgencyColor: Record<string, string> = {
    High: "bg-red-100 text-red-600",
    Medium: "bg-amber-100 text-amber-600",
    Low: "bg-emerald-100 text-emerald-700",
};

const statusColor: Record<string, string> = {
    "In Progress": "bg-blue-100 text-blue-600",
    "Near Complete": "bg-emerald-100 text-emerald-700",
    Completed: "bg-slate-100 text-slate-600",
};

// ─────────────────────────────────────────────
// Page
// ─────────────────────────────────────────────
export default function FacultyDashboard() {
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
                        Faculty Portal
                    </p>
                    <h1 className="text-3xl font-bold tracking-tight text-foreground">
                        Welcome back, Admin User 👋
                    </h1>
                    <p className="mt-2 text-sm text-muted">{today} · Here&apos;s your teaching overview.</p>
                </div>
                <div className="flex items-center gap-3">
                    <span className="inline-flex items-center gap-1.5 rounded-full bg-emerald-50 px-3 py-1.5 text-xs font-semibold text-emerald-700">
                        <span className="h-1.5 w-1.5 rounded-full bg-emerald-500 animate-pulse" />
                        Active · Semester 5
                    </span>
                    <span className="rounded-full border bg-surface px-4 py-2 text-xs font-semibold text-muted shadow-sm">
                        Dept: Computer Science
                    </span>
                </div>
            </div>

            {/* ── Stats row (2 rows of 3) ── */}
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

            {/* ── Row 2: My Subjects table + Weekly attendance chart ── */}
            <div className="grid gap-5 xl:grid-cols-[1.5fr_1fr]">
                {/* Subjects table */}
                <div className="rounded-2xl bg-surface p-6 shadow-sm">
                    <div className="flex items-center justify-between">
                        <div>
                            <h2 className="text-base font-bold">My Subjects</h2>
                            <p className="mt-0.5 text-xs text-muted">Current semester — attendance &amp; sessions</p>
                        </div>
                        <a href="/faculty/subjects" className="text-xs font-semibold text-brand-dark hover:underline">
                            View all →
                        </a>
                    </div>
                    <div className="mt-4 overflow-x-auto">
                        <table className="w-full text-sm">
                            <thead>
                                <tr className="border-b text-left text-xs text-muted">
                                    <th className="pb-3 font-semibold">Subject</th>
                                    <th className="pb-3 font-semibold text-center">Students</th>
                                    <th className="pb-3 font-semibold text-center">Attendance</th>
                                    <th className="pb-3 font-semibold text-center">Sessions</th>
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-line">
                                {mySubjects.map((sub) => (
                                    <tr key={sub.code} className="group transition-colors hover:bg-brand/5">
                                        <td className="py-3 pr-4">
                                            <p className="font-semibold">{sub.name}</p>
                                            <p className="text-[11px] text-muted">{sub.code}</p>
                                        </td>
                                        <td className="py-3 text-center font-medium">{sub.students}</td>
                                        <td className="py-3 text-center">
                                            <span
                                                className={`rounded-full px-2.5 py-0.5 text-xs font-bold ${
                                                    sub.attended >= 75
                                                        ? "bg-emerald-100 text-emerald-700"
                                                        : "bg-red-100 text-red-600"
                                                }`}
                                            >
                                                {sub.attended}%
                                            </span>
                                        </td>
                                        <td className="py-3 text-center">
                                            <div className="flex flex-col items-center gap-1">
                                                <span className="text-xs font-medium">
                                                    {sub.done}/{sub.sessions}
                                                </span>
                                                <div className="h-1 w-16 rounded-full bg-background">
                                                    <div
                                                        className="h-1 rounded-full bg-brand-dark"
                                                        style={{ width: `${(sub.done / sub.sessions) * 100}%` }}
                                                    />
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>

                {/* Weekly attendance bar chart */}
                <div className="rounded-2xl bg-surface p-6 shadow-sm">
                    <div className="flex items-center justify-between">
                        <div>
                            <h2 className="text-base font-bold">Weekly Attendance</h2>
                            <p className="mt-0.5 text-xs text-muted">Average across all subjects this week</p>
                        </div>
                        <span className="inline-flex items-center gap-1 rounded-full bg-brand/15 px-3 py-1 text-xs font-bold text-brand-dark">
                            <TrendingUp className="h-3 w-3" /> +4%
                        </span>
                    </div>
                    <div className="mt-6 flex h-40 items-end gap-4 border-b border-dashed border-line px-2">
                        {weeklyAttendance.map((d) => (
                            <div key={d.day} className="flex flex-1 flex-col items-center gap-1">
                                <span className="text-[11px] font-bold text-foreground">{d.pct}%</span>
                                <div
                                    className={`w-full rounded-t-lg transition-all duration-500 ${
                                        d.pct >= 75 ? "bg-sidebar" : "bg-rose-400"
                                    }`}
                                    style={{ height: `${d.pct}%` }}
                                />
                            </div>
                        ))}
                    </div>
                    <div className="mt-2 flex justify-around px-2 text-[11px] text-muted">
                        {weeklyAttendance.map((d) => (
                            <span key={d.day}>{d.day}</span>
                        ))}
                    </div>

                    {/* Summary */}
                    <div className="mt-5 grid grid-cols-3 gap-3">
                        {[
                            { label: "Avg This Week", val: "80%" },
                            { label: "Best Day", val: "Wed" },
                            { label: "At Risk", val: "9 students", warn: true },
                        ].map((m) => (
                            <div key={m.label} className="rounded-xl bg-background px-3 py-2.5 text-center">
                                <p className={`text-sm font-bold ${m.warn ? "text-rose-500" : ""}`}>{m.val}</p>
                                <p className="text-[10px] text-muted">{m.label}</p>
                            </div>
                        ))}
                    </div>
                </div>
            </div>

            {/* ── Row 3: Today's schedule + Projects + Pending Actions ── */}
            <div className="grid gap-5 xl:grid-cols-3">
                {/* Today's schedule */}
                <div className="rounded-2xl bg-surface p-6 shadow-sm">
                    <div className="flex items-center justify-between">
                        <h2 className="text-base font-bold">Today&apos;s Classes</h2>
                        <a href="/faculty/class-sessions" className="text-xs font-semibold text-brand-dark hover:underline">
                            Full schedule →
                        </a>
                    </div>
                    <div className="mt-4 space-y-3">
                        {todaysSchedule.map((cls) => (
                            <div
                                key={cls.subject}
                                className="flex items-center gap-3 rounded-xl border border-line bg-background p-4 transition-colors hover:bg-brand/5"
                            >
                                <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-sidebar text-brand">
                                    <BookOpen className="h-4 w-4" />
                                </div>
                                <div className="flex-1 min-w-0">
                                    <p className="truncate text-sm font-semibold">{cls.subject}</p>
                                    <p className="text-[11px] text-muted">{cls.code} · {cls.room}</p>
                                </div>
                                <div className="text-right shrink-0">
                                    <p className="text-xs font-semibold">{cls.time}</p>
                                    <span className="mt-1 inline-block rounded-full bg-brand/15 px-2 py-0.5 text-[10px] font-bold text-brand-dark">
                                        {cls.status}
                                    </span>
                                </div>
                            </div>
                        ))}
                        <div className="flex items-center justify-center rounded-xl border border-dashed border-line py-4 text-xs text-muted">
                            No more classes today
                        </div>
                    </div>
                </div>

                {/* Supervised Projects */}
                <div className="rounded-2xl bg-surface p-6 shadow-sm">
                    <div className="flex items-center justify-between">
                        <h2 className="text-base font-bold">Supervised Projects</h2>
                        <a href="/faculty/projects" className="text-xs font-semibold text-brand-dark hover:underline">
                            View all →
                        </a>
                    </div>
                    <div className="mt-4 space-y-4">
                        {projects.map((proj) => (
                            <div key={proj.title} className="rounded-xl bg-background p-4">
                                <div className="flex items-start justify-between gap-2">
                                    <p className="text-sm font-semibold leading-snug">{proj.title}</p>
                                    <span
                                        className={`shrink-0 rounded-full px-2 py-0.5 text-[10px] font-bold ${statusColor[proj.status]}`}
                                    >
                                        {proj.status}
                                    </span>
                                </div>
                                <p className="mt-1 flex items-center gap-1 text-[11px] text-muted">
                                    <Users className="h-3 w-3" /> {proj.team} students
                                </p>
                                <div className="mt-3">
                                    <div className="flex justify-between text-[11px]">
                                        <span className="text-muted">Progress</span>
                                        <span className="font-bold text-foreground">{proj.progress}%</span>
                                    </div>
                                    <div className="mt-1 h-1.5 w-full rounded-full bg-line">
                                        <div
                                            className={`h-1.5 rounded-full transition-all ${
                                                proj.progress >= 80 ? "bg-emerald-500" : "bg-sidebar"
                                            }`}
                                            style={{ width: `${proj.progress}%` }}
                                        />
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>

                {/* Pending Actions */}
                <div className="rounded-2xl bg-surface p-6 shadow-sm">
                    <div className="flex items-center justify-between">
                        <h2 className="text-base font-bold">Pending Actions</h2>
                        <span className="rounded-full bg-rose-100 px-2.5 py-1 text-xs font-bold text-rose-600">
                            {pendingActions.length} items
                        </span>
                    </div>
                    <div className="mt-4 space-y-3">
                        {pendingActions.map((action) => (
                            <div
                                key={action.task}
                                className="flex items-start gap-3 rounded-xl bg-background p-3 transition-colors hover:bg-brand/5"
                            >
                                <div className="mt-0.5 flex h-8 w-8 shrink-0 items-center justify-center rounded-lg bg-surface shadow-sm">
                                    <AlertTriangle className="h-3.5 w-3.5 text-amber-500" />
                                </div>
                                <div className="min-w-0 flex-1">
                                    <p className="text-sm font-semibold leading-snug">{action.task}</p>
                                    <div className="mt-1.5 flex items-center gap-2">
                                        <span className="text-[11px] text-muted">Due: {action.due}</span>
                                        <span
                                            className={`rounded-full px-2 py-0.5 text-[10px] font-bold ${urgencyColor[action.urgency]}`}
                                        >
                                            {action.urgency}
                                        </span>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>

            {/* ── Row 4: Recent Activity + Quick Links ── */}
            <div className="grid gap-5 xl:grid-cols-[1.6fr_1fr]">
                {/* Recent Activities */}
                <div className="rounded-2xl bg-surface p-6 shadow-sm">
                    <div className="flex items-center justify-between">
                        <div>
                            <h2 className="text-base font-bold">Recent Activity</h2>
                            <p className="mt-0.5 text-xs text-muted">Latest updates in your portal</p>
                        </div>
                        <button className="text-xs font-semibold text-brand-dark hover:underline">View all →</button>
                    </div>
                    <div className="mt-5 space-y-3">
                        {recentActivities.map((a, i) => (
                            <div key={i} className="flex items-start gap-3">
                                <span className="mt-0.5 text-base">{a.icon}</span>
                                <div className="flex-1">
                                    <p className="text-sm">{a.text}</p>
                                    <p className="mt-0.5 text-[11px] text-muted">{a.time}</p>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>

                {/* Quick Links */}
                <div className="space-y-4">
                    <div className="relative overflow-hidden rounded-2xl bg-sidebar p-6 text-white">
                        <span className="inline-flex rounded-full bg-white/10 px-2.5 py-1 text-[11px] font-semibold text-brand">
                            <Zap className="mr-1 h-3 w-3" /> Quick Actions
                        </span>
                        <p className="mt-3 text-sm font-bold leading-snug">
                            Manage your classes and students efficiently.
                        </p>
                        <div className="absolute -bottom-6 -right-4 text-7xl font-bold text-brand/20">✱</div>
                    </div>

                    <div className="grid grid-cols-2 gap-3">
                        {[
                            { label: "Mark Attendance", icon: CheckCircle, href: "/faculty/attendance", color: "text-emerald-600", bg: "bg-emerald-50" },
                            { label: "Enter Grades", icon: Star, href: "/faculty/grades", color: "text-amber-600", bg: "bg-amber-50" },
                            { label: "My Subjects", icon: BookOpen, href: "/faculty/subjects", color: "text-blue-600", bg: "bg-blue-50" },
                            { label: "Projects", icon: Layers, href: "/faculty/projects", color: "text-purple-600", bg: "bg-purple-50" },
                            { label: "Reports", icon: BarChart3, href: "/faculty/reports", color: "text-rose-600", bg: "bg-rose-50" },
                            { label: "Materials", icon: FileText, href: "/faculty/materials", color: "text-cyan-600", bg: "bg-cyan-50" },
                        ].map((q) => {
                            const Icon = q.icon;
                            return (
                                <a
                                    key={q.label}
                                    href={q.href}
                                    className="group flex items-center gap-3 rounded-xl bg-surface p-3.5 shadow-sm transition-all hover:shadow-md hover:-translate-y-0.5"
                                >
                                    <div className={`flex h-9 w-9 shrink-0 items-center justify-center rounded-xl ${q.bg}`}>
                                        <Icon className={`h-4 w-4 ${q.color}`} />
                                    </div>
                                    <span className="text-xs font-semibold">{q.label}</span>
                                    <ChevronRight className="ml-auto h-3 w-3 text-muted opacity-0 transition-opacity group-hover:opacity-100" />
                                </a>
                            );
                        })}
                    </div>
                </div>
            </div>
        </section>
    );
}

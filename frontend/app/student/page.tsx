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
} from "lucide-react";

// ─────────────────────────────────────────────
// Mock data (replace with real API calls later)
// ─────────────────────────────────────────────
const stats = [
    {
        label: "Current Semester",
        value: "5th",
        sub: "Ongoing",
        icon: BookOpen,
        color: "bg-[#062b1d]",
        textColor: "text-brand",
        linkLabel: "View Subjects",
        href: "/student/subjects",
    },
    {
        label: "Total Subjects",
        value: "6",
        sub: "Enrolled",
        icon: Layers,
        color: "bg-blue-500",
        textColor: "text-blue-500",
        linkLabel: "View All",
        href: "/student/subjects",
    },
    {
        label: "Attendance",
        value: "82%",
        sub: "This semester",
        icon: Activity,
        color: "bg-amber-500",
        textColor: "text-amber-500",
        linkLabel: "View Records",
        href: "/student/attendance",
    },
    {
        label: "Avg. Grade",
        value: "A−",
        sub: "Last semester",
        icon: Star,
        color: "bg-emerald-500",
        textColor: "text-emerald-500",
        linkLabel: "View Grades",
        href: "/student/grades",
    },
    {
        label: "Active Project",
        value: "1",
        sub: "Assigned",
        icon: FileText,
        color: "bg-purple-500",
        textColor: "text-purple-500",
        linkLabel: "View Project",
        href: "/student/projects",
    },
];

const subjectProgress = [
    { name: "Data Structures", code: "CS301", faculty: "Dr. A. Sharma", progress: 78, grade: "A" },
    { name: "Operating Systems", code: "CS302", faculty: "Prof. R. Mehta", progress: 65, grade: "B+" },
    { name: "Database Systems", code: "CS303", faculty: "Dr. P. Iyer", progress: 90, grade: "A+" },
    { name: "Computer Networks", code: "CS304", faculty: "Prof. S. Joshi", progress: 55, grade: "B" },
    { name: "Software Engineering", code: "CS305", faculty: "Dr. L. Patel", progress: 70, grade: "A−" },
    { name: "Web Technology", code: "CS306", faculty: "Prof. M. Shah", progress: 82, grade: "A" },
];

const attendanceBySub = [
    { name: "Data Structures", present: 18, total: 22, pct: 82 },
    { name: "Operating Systems", present: 14, total: 20, pct: 70 },
    { name: "Database Systems", present: 21, total: 22, pct: 95 },
    { name: "Computer Networks", present: 13, total: 20, pct: 65 },
    { name: "Software Engineering", present: 17, total: 22, pct: 77 },
    { name: "Web Technology", present: 19, total: 22, pct: 86 },
];

const projectInfo = {
    title: "Library Management System",
    faculty: "Dr. Emily Johnson",
    status: "In Progress",
    progress: 68,
    deadline: "20 May 2024",
    tasks: { total: 8, done: 5, pending: 3 },
};

const upcomingTasks = [
    { title: "Submit DB Assignment", subject: "Database Systems", due: "30 Aug 2024", priority: "High" },
    { title: "OS Lab Report", subject: "Operating Systems", due: "02 Sep 2024", priority: "Medium" },
    { title: "Network Topology Diagram", subject: "Computer Networks", due: "05 Sep 2024", priority: "Medium" },
    { title: "SE Project Phase 2", subject: "Software Engineering", due: "10 Sep 2024", priority: "Low" },
];

const recentActivities = [
    { text: "Grade posted for Database Systems mid-term.", time: "2 hours ago", icon: "✅" },
    { text: "Attendance marked for Data Structures lecture.", time: "5 hours ago", icon: "📋" },
    { text: "New material uploaded in Operating Systems.", time: "Yesterday", icon: "📄" },
    { text: "Project task \"ER Diagram\" marked complete.", time: "2 days ago", icon: "✔️" },
    { text: "Semester 5 result published.", time: "3 days ago", icon: "🎓" },
];

const semesterGrades = [
    { sem: "Sem 1", sgpa: 7.8 },
    { sem: "Sem 2", sgpa: 8.1 },
    { sem: "Sem 3", sgpa: 8.5 },
    { sem: "Sem 4", sgpa: 8.9 },
    { sem: "Sem 5", sgpa: 9.1 },
];

const priorityColor: Record<string, string> = {
    High: "bg-red-100 text-red-600",
    Medium: "bg-amber-100 text-amber-600",
    Low: "bg-emerald-100 text-emerald-700",
};

const maxSgpa = Math.max(...semesterGrades.map((s) => s.sgpa));

// ─────────────────────────────────────────────
// Page
// ─────────────────────────────────────────────
export default function StudentDashboard() {
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
                        Student Portal
                    </p>
                    <h1 className="text-3xl font-bold tracking-tight text-foreground">
                        Welcome back, Admin User 👋
                    </h1>
                    <p className="mt-2 text-sm text-muted">{today} · Here&apos;s your academic overview.</p>
                </div>
                <div className="flex items-center gap-3">
                    <span className="inline-flex items-center gap-1.5 rounded-full bg-emerald-50 px-3 py-1.5 text-xs font-semibold text-emerald-700">
                        <span className="h-1.5 w-1.5 rounded-full bg-emerald-500 animate-pulse" />
                        Active · Semester 5
                    </span>
                    <span className="rounded-full border bg-surface px-4 py-2 text-xs font-semibold text-muted shadow-sm">
                        Enroll No: STU2023001
                    </span>
                </div>
            </div>

            {/* ── Stats row ── */}
            <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-5">
                {stats.map((s) => {
                    const Icon = s.icon;
                    return (
                        <div
                            key={s.label}
                            className="group relative overflow-hidden rounded-2xl bg-surface p-5 shadow-sm transition-all hover:shadow-md hover:-translate-y-0.5"
                        >
                            <div className="flex items-start justify-between">
                                <div
                                    className={`flex h-10 w-10 items-center justify-center rounded-xl ${s.color} shadow-sm`}
                                >
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
                            {/* decorative corner */}
                            <div className="pointer-events-none absolute -bottom-4 -right-4 h-16 w-16 rounded-full bg-black/[0.03]" />
                        </div>
                    );
                })}
            </div>

            {/* ── Row 2: Subject progress + SGPA chart ── */}
            <div className="grid gap-5 xl:grid-cols-[1.4fr_1fr]">
                {/* Subject Progress */}
                <div className="rounded-2xl bg-surface p-6 shadow-sm">
                    <div className="flex items-center justify-between">
                        <div>
                            <h2 className="text-base font-bold">Subject Performance</h2>
                            <p className="mt-0.5 text-xs text-muted">Current semester grade overview</p>
                        </div>
                        <a href="/student/grades" className="text-xs font-semibold text-brand-dark hover:underline">
                            View all →
                        </a>
                    </div>
                    <div className="mt-5 space-y-4">
                        {subjectProgress.map((sub) => (
                            <div key={sub.code}>
                                <div className="flex items-center justify-between text-sm">
                                    <div>
                                        <span className="font-semibold">{sub.name}</span>
                                        <span className="ml-2 text-xs text-muted">{sub.code}</span>
                                    </div>
                                    <div className="flex items-center gap-3">
                                        <span className="text-[11px] text-muted">{sub.faculty}</span>
                                        <span
                                            className={`w-10 rounded-full px-2 py-0.5 text-center text-xs font-bold ${
                                                sub.grade.startsWith("A")
                                                    ? "bg-emerald-100 text-emerald-700"
                                                    : "bg-amber-100 text-amber-600"
                                            }`}
                                        >
                                            {sub.grade}
                                        </span>
                                    </div>
                                </div>
                                <div className="mt-2 h-1.5 w-full rounded-full bg-background">
                                    <div
                                        className="h-1.5 rounded-full bg-brand-dark transition-all duration-500"
                                        style={{ width: `${sub.progress}%` }}
                                    />
                                </div>
                                <p className="mt-0.5 text-right text-[10px] text-muted">{sub.progress}% complete</p>
                            </div>
                        ))}
                    </div>
                </div>

                {/* SGPA Bar Chart */}
                <div className="rounded-2xl bg-surface p-6 shadow-sm">
                    <div className="flex items-center justify-between">
                        <div>
                            <h2 className="text-base font-bold">SGPA Trend</h2>
                            <p className="mt-0.5 text-xs text-muted">Performance across semesters</p>
                        </div>
                        <span className="inline-flex items-center gap-1 rounded-full bg-brand/15 px-3 py-1 text-xs font-bold text-brand-dark">
                            <TrendingUp className="h-3 w-3" /> Improving
                        </span>
                    </div>

                    {/* Bars */}
                    <div className="mt-6 flex h-44 items-end gap-3 border-b border-dashed border-line px-2">
                        {semesterGrades.map((s) => {
                            const heightPct = (s.sgpa / 10) * 100;
                            const isLatest = s.sem === "Sem 5";
                            return (
                                <div key={s.sem} className="flex flex-1 flex-col items-center gap-1">
                                    <span className="text-[10px] font-bold text-foreground">{s.sgpa}</span>
                                    <div
                                        className={`w-full rounded-t-lg transition-all duration-500 ${
                                            isLatest ? "bg-sidebar" : "bg-brand"
                                        }`}
                                        style={{ height: `${heightPct}%` }}
                                    />
                                </div>
                            );
                        })}
                    </div>
                    <div className="mt-2 flex justify-around px-2 text-[11px] text-muted">
                        {semesterGrades.map((s) => (
                            <span key={s.sem}>{s.sem}</span>
                        ))}
                    </div>

                    {/* Legend */}
                    <div className="mt-4 flex gap-4 text-xs text-muted">
                        <span className="flex items-center gap-1.5">
                            <span className="inline-block h-2 w-3 rounded bg-brand" /> Past Semesters
                        </span>
                        <span className="flex items-center gap-1.5">
                            <span className="inline-block h-2 w-3 rounded bg-sidebar" /> Current
                        </span>
                    </div>

                    {/* Summary */}
                    <div className="mt-5 grid grid-cols-3 gap-3">
                        {[
                            { label: "Best SGPA", val: "9.1" },
                            { label: "Avg SGPA", val: "8.5" },
                            { label: "CGPA", val: "8.48" },
                        ].map((m) => (
                            <div key={m.label} className="rounded-xl bg-background px-3 py-2.5 text-center">
                                <p className="text-base font-bold">{m.val}</p>
                                <p className="text-[10px] text-muted">{m.label}</p>
                            </div>
                        ))}
                    </div>
                </div>
            </div>

            {/* ── Row 3: Attendance + Project + Tasks ── */}
            <div className="grid gap-5 xl:grid-cols-[1fr_1fr_1fr]">
                {/* Attendance by subject */}
                <div className="rounded-2xl bg-surface p-6 shadow-sm">
                    <div className="flex items-center justify-between">
                        <h2 className="text-base font-bold">Attendance</h2>
                        <a href="/student/attendance" className="text-xs font-semibold text-brand-dark hover:underline">
                            Details →
                        </a>
                    </div>
                    {/* Donut summary */}
                    <div className="mt-4 flex items-center gap-4">
                        <div
                            className="h-20 w-20 shrink-0 rounded-full"
                            style={{
                                background: `conic-gradient(#062b1d 0 82%, #e3eae6 82% 100%)`,
                            }}
                        >
                            <div className="flex h-full w-full scale-[0.72] items-center justify-center rounded-full bg-surface">
                                <span className="text-base font-bold">82%</span>
                            </div>
                        </div>
                        <div className="space-y-1 text-xs">
                            <p className="font-semibold">Overall Attendance</p>
                            <p className="text-muted">102 present / 128 total classes</p>
                            <p className="text-[11px] text-amber-600 font-medium">⚠ Keep above 75%</p>
                        </div>
                    </div>
                    <div className="mt-4 space-y-2.5">
                        {attendanceBySub.map((a) => (
                            <div key={a.name}>
                                <div className="flex justify-between text-xs">
                                    <span className="truncate font-medium">{a.name}</span>
                                    <span
                                        className={`font-bold ${
                                            a.pct >= 75 ? "text-emerald-600" : "text-red-500"
                                        }`}
                                    >
                                        {a.pct}%
                                    </span>
                                </div>
                                <div className="mt-1 h-1 w-full rounded-full bg-background">
                                    <div
                                        className={`h-1 rounded-full ${a.pct >= 75 ? "bg-emerald-500" : "bg-red-400"}`}
                                        style={{ width: `${a.pct}%` }}
                                    />
                                </div>
                            </div>
                        ))}
                    </div>
                </div>

                {/* Project Overview */}
                <div className="rounded-2xl bg-surface p-6 shadow-sm">
                    <div className="flex items-center justify-between">
                        <h2 className="text-base font-bold">My Project</h2>
                        <a href="/student/projects" className="text-xs font-semibold text-brand-dark hover:underline">
                            View →
                        </a>
                    </div>
                    <div className="mt-4 rounded-xl bg-sidebar p-4 text-white">
                        <span className="inline-flex rounded-full bg-white/10 px-2.5 py-1 text-[11px] font-semibold text-brand">
                            {projectInfo.status}
                        </span>
                        <p className="mt-3 text-sm font-bold leading-snug">{projectInfo.title}</p>
                        <p className="mt-1 text-[11px] text-slate-400">Guide: {projectInfo.faculty}</p>
                        <div className="mt-4 flex items-end justify-between">
                            <div>
                                <p className="text-2xl font-bold text-brand">{projectInfo.progress}%</p>
                                <p className="text-[11px] text-slate-400">Overall Progress</p>
                            </div>
                            <p className="text-[11px] text-slate-400">Due: {projectInfo.deadline}</p>
                        </div>
                        <div className="mt-3 h-1.5 w-full rounded-full bg-white/10">
                            <div
                                className="h-1.5 rounded-full bg-brand transition-all"
                                style={{ width: `${projectInfo.progress}%` }}
                            />
                        </div>
                    </div>

                    {/* Task breakdown */}
                    <div className="mt-4 grid grid-cols-3 gap-3">
                        {[
                            { label: "Total Tasks", val: projectInfo.tasks.total, color: "text-foreground" },
                            { label: "Completed", val: projectInfo.tasks.done, color: "text-emerald-600" },
                            { label: "Pending", val: projectInfo.tasks.pending, color: "text-amber-500" },
                        ].map((t) => (
                            <div key={t.label} className="rounded-xl bg-background p-3 text-center">
                                <p className={`text-xl font-bold ${t.color}`}>{t.val}</p>
                                <p className="text-[10px] text-muted">{t.label}</p>
                            </div>
                        ))}
                    </div>

                    {/* Task distribution donut */}
                    <div className="mt-4 flex items-center gap-3">
                        <div
                            className="h-14 w-14 shrink-0 rounded-full"
                            style={{
                                background: `conic-gradient(#062b1d 0 ${(5 / 8) * 100}%, #b7f000 ${(5 / 8) * 100}% ${((5 + 3) / 8) * 100}%, #e3eae6 ${((5 + 3) / 8) * 100}% 100%)`,
                            }}
                        >
                            <div className="flex h-full w-full scale-[0.65] items-center justify-center rounded-full bg-surface">
                                <span className="text-[10px] font-bold">8</span>
                            </div>
                        </div>
                        <div className="space-y-1 text-[11px]">
                            <span className="flex items-center gap-1.5 text-muted">
                                <span className="inline-block h-2 w-2 rounded-full bg-sidebar" /> 5 Done
                            </span>
                            <span className="flex items-center gap-1.5 text-muted">
                                <span className="inline-block h-2 w-2 rounded-full bg-brand" /> 3 Pending
                            </span>
                        </div>
                    </div>
                </div>

                {/* Upcoming Tasks */}
                <div className="rounded-2xl bg-surface p-6 shadow-sm">
                    <div className="flex items-center justify-between">
                        <h2 className="text-base font-bold">Upcoming Tasks</h2>
                        <a href="/student/projects" className="text-xs font-semibold text-brand-dark hover:underline">
                            View all →
                        </a>
                    </div>
                    <div className="mt-4 space-y-3">
                        {upcomingTasks.map((task) => (
                            <div
                                key={task.title}
                                className="flex items-start gap-3 rounded-xl bg-background p-3 transition-colors hover:bg-brand/5"
                            >
                                <div className="mt-0.5 flex h-8 w-8 shrink-0 items-center justify-center rounded-lg bg-surface shadow-sm">
                                    <Clock className="h-3.5 w-3.5 text-muted" />
                                </div>
                                <div className="min-w-0 flex-1">
                                    <p className="truncate text-sm font-semibold">{task.title}</p>
                                    <p className="text-[11px] text-muted">{task.subject}</p>
                                    <div className="mt-1.5 flex items-center gap-2">
                                        <span className="text-[11px] text-muted">Due: {task.due}</span>
                                        <span
                                            className={`rounded-full px-2 py-0.5 text-[10px] font-bold ${priorityColor[task.priority]}`}
                                        >
                                            {task.priority}
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
                            <p className="mt-0.5 text-xs text-muted">Latest updates across your courses</p>
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

                {/* Quick Links + Summary */}
                <div className="space-y-4">
                    {/* Hero promo card */}
                    <div className="relative overflow-hidden rounded-2xl bg-sidebar p-6 text-white">
                        <span className="inline-flex rounded-full bg-white/10 px-2.5 py-1 text-[11px] font-semibold text-brand">
                            <Zap className="mr-1 h-3 w-3" /> Quick Actions
                        </span>
                        <p className="mt-3 text-sm font-bold leading-snug">
                            Everything you need, one click away.
                        </p>
                        <div className="absolute -bottom-6 -right-4 text-7xl font-bold text-brand/20">✱</div>
                    </div>

                    {/* Quick links grid */}
                    <div className="grid grid-cols-2 gap-3">
                        {[
                            { label: "My Subjects", icon: BookOpen, href: "/student/subjects", color: "text-blue-600", bg: "bg-blue-50" },
                            { label: "Grades", icon: Star, href: "/student/grades", color: "text-amber-600", bg: "bg-amber-50" },
                            { label: "Attendance", icon: Activity, href: "/student/attendance", color: "text-emerald-600", bg: "bg-emerald-50" },
                            { label: "My Project", icon: Layers, href: "/student/projects", color: "text-purple-600", bg: "bg-purple-50" },
                            { label: "Results", icon: CheckCircle, href: "/student/results", color: "text-rose-600", bg: "bg-rose-50" },
                            { label: "Materials", icon: FileText, href: "/student/materials", color: "text-cyan-600", bg: "bg-cyan-50" },
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

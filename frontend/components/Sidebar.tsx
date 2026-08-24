import Link from "next/link";
import {
  Home,
  Users,
  BookOpen,
  Calendar,
  GraduationCap,
  LayoutDashboard,
  Database,
  Activity,
  FileText,
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
    <aside className="hidden h-full min-h-0 w-64 shrink-0 bg-gray-900 text-white md:block">
      <div className="h-full px-3 py-4 overflow-y-auto">
        <ul className="space-y-2 font-medium">
          <li>
            <Link
              href="/admin"
              className="flex items-center p-2 rounded-lg hover:bg-gray-800 group"
            >
              <Home className="w-5 h-5 text-gray-400 group-hover:text-white" />
              <span className="ms-3">Dashboard Home</span>
            </Link>
          </li>
          <li className="pt-4 pb-2">
            <span className="px-2 text-xs font-semibold text-gray-400 uppercase tracking-wider">
              Management Entities
            </span>
          </li>
          {entities.map((entity) => {
            const Icon = entity.icon;
            return (
              <li key={entity.path}>
                <Link
                  href={`/admin/${entity.path}`}
                  className="flex items-center p-2 rounded-lg hover:bg-gray-800 group"
                >
                  <Icon className="w-5 h-5 text-gray-400 group-hover:text-white" />
                  <span className="flex-1 ms-3 whitespace-nowrap">
                    {entity.name}
                  </span>
                </Link>
              </li>
            );
          })}
        </ul>
      </div>
    </aside>
  );
}

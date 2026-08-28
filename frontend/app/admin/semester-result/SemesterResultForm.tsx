import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { GetAllStudentSemesters } from "@/service/studentSemester.service";
import { GetAllSemesterSubjects } from "@/service/semesterSubject.service";
import SemesterResultFormClient from "./SemesterResultFormClient";

type SemesterResultFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
type SelectOption = { value: number; label: string };
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default async function SemesterResultForm({
  initialData = {},
  onSubmitAction,
  mode,
}: SemesterResultFormProps) {
  const editing = mode === "edit";
  const studentSemesters = await GetAllStudentSemesters();
  const semesterSubjects = await GetAllSemesterSubjects();
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/semester-result"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to semester results
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Semester Results" : "Add Semester Results"}
        </h1>
      </header>
      <SemesterResultFormClient
        initialData={initialData}
        onSubmitAction={onSubmitAction}
        mode={mode}
        studentSemesters={studentSemesters?.data ?? []}
        semesterSubjects={semesterSubjects?.data ?? []}
      />
    </section>
  );
}

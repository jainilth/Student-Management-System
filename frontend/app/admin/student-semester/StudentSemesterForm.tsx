import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { GetAllAcademicYears } from "@/service/academicYear.service";
import { GetAllSemesters } from "@/service/semester.service";
import { GetAllStudents } from "@/service/student.service";

type StudentSemesterFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
type SelectOption = { value: number; label: string };

const semesterStatusOptions = [
  "Active",
  "Completed",
  "Failed",
  "Dropped",
  "Withdrawn",
] as const;

const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default async function StudentSemesterForm({
  initialData = {},
  onSubmitAction,
  mode,
}: StudentSemesterFormProps) {
  const editing = mode === "edit";
  const students = await GetAllStudents();
  const semesters = await GetAllSemesters();
  const academicYears = await GetAllAcademicYears();
  const studentIdOptions: SelectOption[] = (students?.data ?? []).map((record: any) => ({
    value: Number(record.studentId),
    label: record.userName || record.enrollmentNumber || `Student ${record.studentId}`,
  }));
  const semesterIdOptions: SelectOption[] = (semesters?.data ?? []).map((record: any) => ({
    value: Number(record.semesterId),
    label: record.semesterName || `Record ${record.semesterId}`,
  }));
  const academicYearIdOptions: SelectOption[] = (academicYears?.data ?? []).map((record: any) => ({
    value: Number(record.academicYearId),
    label: record.year || `Record ${record.academicYearId}`,
  }));
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/student-semester"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to student semesters
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Student Semesters" : "Add Student Semesters"}
        </h1>
      </header>
      <AdminForm
        action={onSubmitAction}
        preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <label className="text-sm font-medium text-slate-700">
            Student
            <select
              className={inputClass}
              name="studentId"
              required
              defaultValue={initialData.studentId ?? ""}
            >
              <option value="" disabled>
                Select student
              </option>
              {studentIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Semester
            <select
              className={inputClass}
              name="semesterId"
              required
              defaultValue={initialData.semesterId ?? ""}
            >
              <option value="" disabled>
                Select semester
              </option>
              {semesterIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Academic year
            <select
              className={inputClass}
              name="academicYearId"
              required
              defaultValue={initialData.academicYearId ?? ""}
            >
              <option value="" disabled>
                Select academic year
              </option>
              {academicYearIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Enrollment date
            <input
              className={inputClass}
              name="enrollmentDate"
              type="date"
              step="any"
              defaultValue={initialData.enrollmentDate ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Status
            <select
              className={inputClass}
              name="status"
              required
              defaultValue={initialData.status ?? "Active"}
            >
              {semesterStatusOptions.map((status) => (
                <option key={status} value={status}>
                  {status}
                </option>
              ))}
            </select>
          </label>
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/student-semester"
            className="rounded-lg border border-slate-200 px-4 py-2.5 text-sm font-semibold text-slate-600"
          >
            Cancel
          </Link>
          <button
            className="rounded-lg bg-emerald-950 px-4 py-2.5 text-sm font-semibold text-white hover:bg-emerald-900"
            type="submit"
          >
            {editing ? "Save changes" : "Create record"}
          </button>
        </div>
      </AdminForm>
    </section>
  );
}

import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { GetAllClassSessions } from "@/service/classSession.service";
import { GetAllStudentSemesters } from "@/service/studentSemester.service";

type AttendanceRecordFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
type SelectOption = { value: number; label: string };
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default async function AttendanceRecordForm({
  initialData = {},
  onSubmitAction,
  mode,
}: AttendanceRecordFormProps) {
  const editing = mode === "edit";
  const sessions = await GetAllClassSessions();
  const studentSemesters = await GetAllStudentSemesters();
  const sessionIdOptions: SelectOption[] = (sessions?.data ?? []).map((record: any) => ({
    value: Number(record.sessionId),
    label: record.topic || `Record ${record.sessionId}`,
  }));
  const studentSemesterIdOptions: SelectOption[] = (studentSemesters?.data ?? []).map((record: any) => ({
    value: Number(record.studentSemesterId),
    label: [record.studentName, record.academicProgramName, record.semesterName]
      .filter(Boolean)
      .join(" - ") || `Record ${record.studentSemesterId}`,
  }));
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/attendance-record"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to attendance records
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Attendance Records" : "Add Attendance Records"}
        </h1>
      </header>
      <AdminForm
        action={onSubmitAction}
        preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <label className="text-sm font-medium text-slate-700">
            Class session
            <select
              className={inputClass}
              name="sessionId"
              required
              defaultValue={initialData.sessionId ?? ""}
            >
              <option value="" disabled>
                Select class session
              </option>
              {sessionIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Student semester
            <select
              className={inputClass}
              name="studentSemesterId"
              required
              defaultValue={initialData.studentSemesterId ?? ""}
            >
              <option value="" disabled>
                Select student semester
              </option>
              {studentSemesterIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Status
            <input
              className={inputClass}
              name="status"
              type="text"
              defaultValue={initialData.status ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Remarks
            <textarea
              className={inputClass}
              name="remarks"
              defaultValue={initialData.remarks || ""}
            />
          </label>
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/attendance-record"
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

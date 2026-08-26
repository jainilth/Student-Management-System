import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { getAdminOptions } from "@/lib/admin-options";

type AttendanceRecordFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-indigo-500 focus:ring-2 focus:ring-indigo-100";

export default async function AttendanceRecordForm({
  initialData = {},
  onSubmitAction,
  mode,
}: AttendanceRecordFormProps) {
  const editing = mode === "edit";
  const sessionIdOptions = await getAdminOptions("ClassSession");
  const studentSemesterIdOptions = await getAdminOptions("StudentSemester");
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/attendance-record"
          className="text-sm font-semibold text-indigo-600 hover:text-indigo-800"
        >
          &lt;- Back to attendance records
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-indigo-600">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Attendance Records" : "Add Attendance Records"}
        </h1>
      </header>
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
            className="rounded-lg bg-indigo-600 px-4 py-2.5 text-sm font-semibold text-white hover:bg-indigo-700"
            type="submit"
          >
            {editing ? "Save changes" : "Create record"}
          </button>
        </div>
      </AdminForm>
    </section>
  );
}

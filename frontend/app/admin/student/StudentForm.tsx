import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { getAdminOptions } from "@/lib/admin-options";

type StudentFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-indigo-500 focus:ring-2 focus:ring-indigo-100";

export default async function StudentForm({
  initialData = {},
  onSubmitAction,
  mode,
}: StudentFormProps) {
  const editing = mode === "edit";
  const userIdOptions = await getAdminOptions("Student");
  const programIdOptions = await getAdminOptions("AcademicProgram");
  const currentSemesterIdOptions = editing
    ? await getAdminOptions("Semester")
    : [];
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/student"
          className="text-sm font-semibold text-indigo-600 hover:text-indigo-800"
        >
          &lt;- Back to students
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-indigo-600">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Students" : "Add Students"}
        </h1>
      </header>
              preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <label className="text-sm font-medium text-slate-700">
            User
            <select
              className={inputClass}
              name="userId"
              required
              defaultValue={initialData.userId ?? ""}
            >
              <option value="" disabled>
                Select user
              </option>
              {userIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Enrollment number
            <input
              className={inputClass}
              name="enrollmentNumber"
              type="text"
              defaultValue={initialData.enrollmentNumber ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Admission year
            <input
              className={inputClass}
              name="admissionYear"
              type="number"
              step="any"
              defaultValue={initialData.admissionYear ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Academic program
            <select
              className={inputClass}
              name="programId"
              required
              defaultValue={initialData.programId ?? ""}
            >
              <option value="" disabled>
                Select academic program
              </option>
              {programIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          {editing && (
            <label className="text-sm font-medium text-slate-700">
              Current semester
              <select
                className={inputClass}
                name="currentSemesterId"
                required
                defaultValue={initialData.currentSemesterId ?? ""}
              >
                <option value="" disabled>
                  Select current semester
                </option>
                {currentSemesterIdOptions.map((option) => (
                  <option key={option.value} value={option.value}>
                    {option.label}
                  </option>
                ))}
              </select>
            </label>
          )}
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/student"
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

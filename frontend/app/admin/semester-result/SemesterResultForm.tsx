import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { getAdminOptions } from "@/lib/admin-options";

type SemesterResultFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-indigo-500 focus:ring-2 focus:ring-indigo-100";

export default async function SemesterResultForm({
  initialData = {},
  onSubmitAction,
  mode,
}: SemesterResultFormProps) {
  const editing = mode === "edit";
  const studentSemesterIdOptions = await getAdminOptions("StudentSemester");
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/semester-result"
          className="text-sm font-semibold text-indigo-600 hover:text-indigo-800"
        >
          &lt;- Back to semester results
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-indigo-600">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Semester Results" : "Add Semester Results"}
        </h1>
      </header>
              preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
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
            SGPA
            <input
              className={inputClass}
              name="sgpa"
              type="number"
              step="any"
              defaultValue={initialData.sgpa ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Total credits
            <input
              className={inputClass}
              name="totalCredits"
              type="number"
              step="any"
              defaultValue={initialData.totalCredits ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Earned credits
            <input
              className={inputClass}
              name="earnedCredits"
              type="number"
              step="any"
              defaultValue={initialData.earnedCredits ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Result status
            <input
              className={inputClass}
              name="resultStatus"
              type="text"
              defaultValue={initialData.resultStatus ?? ""}
            />
          </label>
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/semester-result"
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

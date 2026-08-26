import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { getAdminOptions } from "@/lib/admin-options";

type SubjectResultFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-indigo-500 focus:ring-2 focus:ring-indigo-100";

export default async function SubjectResultForm({
  initialData = {},
  onSubmitAction,
  mode,
}: SubjectResultFormProps) {
  const editing = mode === "edit";
  const semesterResultIdOptions = await getAdminOptions("SemesterResult");
  const semesterSubjectIdOptions = await getAdminOptions("SemesterSubject");
  const gradeIdOptions = await getAdminOptions("Grade");
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/subject-result"
          className="text-sm font-semibold text-indigo-600 hover:text-indigo-800"
        >
          &lt;- Back to subject results
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-indigo-600">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Subject Results" : "Add Subject Results"}
        </h1>
      </header>
              preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <label className="text-sm font-medium text-slate-700">
            Semester result
            <select
              className={inputClass}
              name="semesterResultId"
              required
              defaultValue={initialData.semesterResultId ?? ""}
            >
              <option value="" disabled>
                Select semester result
              </option>
              {semesterResultIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Semester subject
            <select
              className={inputClass}
              name="semesterSubjectId"
              required
              defaultValue={initialData.semesterSubjectId ?? ""}
            >
              <option value="" disabled>
                Select semester subject
              </option>
              {semesterSubjectIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Internal marks
            <input
              className={inputClass}
              name="internalMarks"
              type="number"
              step="any"
              defaultValue={initialData.internalMarks ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            External marks
            <input
              className={inputClass}
              name="externalMarks"
              type="number"
              step="any"
              defaultValue={initialData.externalMarks ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Practical marks
            <input
              className={inputClass}
              name="practicalMarks"
              type="number"
              step="any"
              defaultValue={initialData.practicalMarks ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Total marks
            <input
              className={inputClass}
              name="totalMarks"
              type="number"
              step="any"
              defaultValue={initialData.totalMarks ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Grade
            <select
              className={inputClass}
              name="gradeId"
              required
              defaultValue={initialData.gradeId ?? ""}
            >
              <option value="" disabled>
                Select grade
              </option>
              {gradeIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Credits earned
            <input
              className={inputClass}
              name="creditsEarned"
              type="number"
              step="any"
              defaultValue={initialData.creditsEarned ?? ""}
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
            href="/admin/subject-result"
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

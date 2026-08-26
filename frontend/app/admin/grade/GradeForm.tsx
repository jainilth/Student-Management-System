import AdminForm from "@/components/AdminForm";
import Link from "next/link";

type GradeFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-indigo-500 focus:ring-2 focus:ring-indigo-100";

export default function GradeForm({
  initialData = {},
  onSubmitAction,
  mode,
}: GradeFormProps) {
  const editing = mode === "edit";
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/grade"
          className="text-sm font-semibold text-indigo-600 hover:text-indigo-800"
        >
          &lt;- Back to grades
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-indigo-600">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Grades" : "Add Grades"}
        </h1>
      </header>
              preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <label className="text-sm font-medium text-slate-700">
            Grade code
            <input
              className={inputClass}
              name="gradeCode"
              type="text"
              defaultValue={initialData.gradeCode ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Grade name
            <input
              className={inputClass}
              name="gradeName"
              type="text"
              defaultValue={initialData.gradeName ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Grade point
            <input
              className={inputClass}
              name="gradePoint"
              type="number"
              step="any"
              defaultValue={initialData.gradePoint ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Minimum marks
            <input
              className={inputClass}
              name="minMarks"
              type="number"
              step="any"
              defaultValue={initialData.minMarks ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Maximum marks
            <input
              className={inputClass}
              name="maxMarks"
              type="number"
              step="any"
              defaultValue={initialData.maxMarks ?? ""}
            />
          </label>
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/grade"
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

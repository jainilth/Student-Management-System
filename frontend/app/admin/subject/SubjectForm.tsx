import AdminForm from "@/components/AdminForm";
import Link from "next/link";

type SubjectFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default function SubjectForm({
  initialData = {},
  onSubmitAction,
  mode,
}: SubjectFormProps) {
  const editing = mode === "edit";
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/subject"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to subjects
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Subjects" : "Add Subjects"}
        </h1>
      </header>
      <AdminForm
        action={onSubmitAction}
        preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <label className="text-sm font-medium text-slate-700">
            Subject code
            <input
              className={inputClass}
              name="subjectCode"
              type="text"
              defaultValue={initialData.subjectCode ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Subject name
            <input
              className={inputClass}
              name="subjectName"
              type="text"
              defaultValue={initialData.subjectName ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Subject type
            <select
              className={inputClass}
              name="subjectType"
              required
              defaultValue={initialData.subjectType ?? ""}
            >
              <option value="" disabled>
                Select a subject type
              </option>
              <option value="Core">Core</option>
              <option value="Optional">Optional</option>
              <option value="Elective">Elective</option>
            </select>
          </label>
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/subject"
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

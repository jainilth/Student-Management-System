import AdminForm from "@/components/AdminForm";
import Link from "next/link";

type SubjectFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-indigo-500 focus:ring-2 focus:ring-indigo-100";

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
          className="text-sm font-semibold text-indigo-600 hover:text-indigo-800"
        >
          &lt;- Back to subjects
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-indigo-600">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Subjects" : "Add Subjects"}
        </h1>
      </header>
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
            <input
              className={inputClass}
              name="subjectType"
              type="text"
              defaultValue={initialData.subjectType ?? ""}
            />
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

import AdminForm from "@/components/AdminForm";
import Link from "next/link";

type SemesterFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default function SemesterForm({
  initialData = {},
  onSubmitAction,
  mode,
}: SemesterFormProps) {
  const editing = mode === "edit";
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/semester"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to semesters
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Semesters" : "Add Semesters"}
        </h1>
      </header>
      <AdminForm
        action={onSubmitAction}
        preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <label className="text-sm font-medium text-slate-700">
            Semester number
            <input
              className={inputClass}
              name="semesterNumber"
              type="number"
              step="any"
              defaultValue={initialData.semesterNumber ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Semester name
            <input
              className={inputClass}
              name="semesterName"
              type="text"
              defaultValue={initialData.semesterName ?? ""}
            />
          </label>
          <label className="flex items-center gap-3 text-sm font-medium text-slate-700">
            <input
              className="h-4 w-4 accent-emerald-950"
              name="isActive"
              type="checkbox"
              defaultChecked={
                initialData.isActive === undefined
                  ? true
                  : Boolean(initialData.isActive)
              }
            />
            Active
          </label>
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/semester"
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

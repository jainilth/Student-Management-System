import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { getAdminOptions } from "@/lib/admin-options";

type ProjectFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-indigo-500 focus:ring-2 focus:ring-indigo-100";

export default async function ProjectForm({
  initialData = {},
  onSubmitAction,
  mode,
}: ProjectFormProps) {
  const editing = mode === "edit";
  const semesterIdOptions = await getAdminOptions("Semester");
  const programIdOptions = await getAdminOptions("AcademicProgram");
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/project"
          className="text-sm font-semibold text-indigo-600 hover:text-indigo-800"
        >
          &lt;- Back to projects
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-indigo-600">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Projects" : "Add Projects"}
        </h1>
      </header>
      <AdminForm
        action={onSubmitAction}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <label className="text-sm font-medium text-slate-700">
            Title
            <input
              className={inputClass}
              name="title"
              type="text"
              defaultValue={initialData.title ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Description
            <textarea
              className={inputClass}
              name="description"
              defaultValue={initialData.description || ""}
            />
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
          <label className="text-sm font-medium text-slate-700">
            Start date
            <input
              className={inputClass}
              name="startDate"
              type="date"
              step="any"
              defaultValue={initialData.startDate ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            End date
            <input
              className={inputClass}
              name="endDate"
              type="date"
              step="any"
              defaultValue={initialData.endDate ?? ""}
            />
          </label>
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/project"
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

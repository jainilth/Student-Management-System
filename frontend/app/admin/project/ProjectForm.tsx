import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { GetAllAcademicPrograms } from "@/service/academicProgram.service";
import { GetAllSemesters } from "@/service/semester.service";

type ProjectFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
type SelectOption = { value: number; label: string };
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default async function ProjectForm({
  initialData = {},
  onSubmitAction,
  mode,
}: ProjectFormProps) {
  const editing = mode === "edit";
  const semesters = await GetAllSemesters();
  const programs = await GetAllAcademicPrograms();
  const semesterIdOptions: SelectOption[] = (semesters?.data ?? []).map((record: any) => ({
    value: Number(record.semesterId),
    label: record.semesterName || `Record ${record.semesterId}`,
  }));
  const programIdOptions: SelectOption[] = (programs?.data ?? []).map((record: any) => ({
    value: Number(record.programId),
    label: record.programName || `Record ${record.programId}`,
  }));
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/project"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to projects
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Projects" : "Add Projects"}
        </h1>
      </header>
      <AdminForm
        action={onSubmitAction}
        preserveValuesOnError={!editing}
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

import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { GetAllDepartments } from "@/service/department.service";

type AcademicProgramFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
type SelectOption = { value: number; label: string };
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default async function AcademicProgramForm({
  initialData = {},
  onSubmitAction,
  mode,
}: AcademicProgramFormProps) {
  const editing = mode === "edit";
  const departments = await GetAllDepartments();
  const departmentIdOptions: SelectOption[] = (departments?.data ?? []).map((record: any) => ({
    value: Number(record.departmentId),
    label: record.departmentName || `Record ${record.departmentId}`,
  }));
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/academic-program"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to academic programs
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Academic Programs" : "Add Academic Programs"}
        </h1>
      </header>
      <AdminForm
        action={onSubmitAction}
        preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <label className="text-sm font-medium text-slate-700">
            Program name
            <input
              className={inputClass}
              name="programName"
              type="text"
              defaultValue={initialData.programName ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Program code
            <input
              className={inputClass}
              name="programCode"
              type="text"
              defaultValue={initialData.programCode ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Department
            <select
              className={inputClass}
              name="departmentId"
              required
              defaultValue={initialData.departmentId ?? ""}
            >
              <option value="" disabled>
                Select department
              </option>
              {departmentIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Duration years
            <input
              className={inputClass}
              name="durationYears"
              type="number"
              step="any"
              defaultValue={initialData.durationYears ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Total semesters
            <input
              className={inputClass}
              name="totalSemesters"
              type="number"
              step="any"
              defaultValue={initialData.totalSemesters ?? ""}
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
            href="/admin/academic-program"
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

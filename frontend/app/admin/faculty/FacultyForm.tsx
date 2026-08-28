import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { GetAllDepartments } from "@/service/department.service";
import { GetAllUsers } from "@/service/user.service";

type FacultyFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
type SelectOption = { value: number; label: string };
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default async function FacultyForm({
  initialData = {},
  onSubmitAction,
  mode,
}: FacultyFormProps) {
  const editing = mode === "edit";
  const users = await GetAllUsers();
  const departments = await GetAllDepartments();
  const userIdOptions: SelectOption[] = (users?.data ?? [])
    .filter((record: any) => String(record.roleName ?? "").toLowerCase() === "faculty")
    .map((record: any) => ({
      value: Number(record.userId),
      label: record.userName || record.email || `Record ${record.userId}`,
    }));
  const departmentIdOptions: SelectOption[] = (departments?.data ?? []).map((record: any) => ({
    value: Number(record.departmentId),
    label: record.departmentName || `Record ${record.departmentId}`,
  }));
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/faculty"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to faculty
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Faculty" : "Add Faculty"}
        </h1>
      </header>
      <AdminForm
        action={onSubmitAction}
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
            Employee number
            <input
              className={inputClass}
              name="employeeNumber"
              type="text"
              defaultValue={initialData.employeeNumber ?? ""}
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
            Designation
            <input
              className={inputClass}
              name="designation"
              type="text"
              defaultValue={initialData.designation ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Joining date
            <input
              className={inputClass}
              name="joiningDate"
              type="date"
              step="any"
              defaultValue={initialData.joiningDate ?? ""}
            />
          </label>
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/faculty"
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

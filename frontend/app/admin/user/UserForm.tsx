import AdminForm from "@/components/AdminForm";
import Link from "next/link";

type RoleOption = { label: string; value: number };
type UserFormProps = {
  initialData?: Record<string, any>;
  roles: RoleOption[];
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default function UserForm({
  initialData = {},
  roles,
  onSubmitAction,
  mode,
}: UserFormProps) {
  const dateValue = initialData.dob ? String(initialData.dob).slice(0, 10) : "";
  const editing = mode === "edit";
  return (
    <section className="mx-auto max-w-3xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/user"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to users
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Account settings" : "New account"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit user" : "Add user"}
        </h1>
      </header>
      <AdminForm
        action={onSubmitAction}
        preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <label className="text-sm font-medium text-slate-700">
            Username
            <input
              className={inputClass}
              name="userName"
              required
              defaultValue={initialData.userName || ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Email
            <input
              className={inputClass}
              name="email"
              type="email"
              required
              defaultValue={initialData.email || ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            {editing ? "Password (optional)" : "Password"}
            <input
              className={inputClass}
              name="password"
              type="password"
              required={!editing}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Mobile number
            <input
              className={inputClass}
              name="mobilenumber"
              defaultValue={initialData.mobilenumber || ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Date of birth
            <input
              className={inputClass}
              name="dob"
              type="date"
              defaultValue={dateValue}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Role
            <select
              className={inputClass}
              name="roleId"
              required
              defaultValue={initialData.roleId || ""}
            >
              <option value="" disabled>
                Select a role
              </option>
              {roles.map((role) => (
                <option key={role.value} value={role.value}>
                  {role.label}
                </option>
              ))}
            </select>
          </label>
        </div>
        <label className="block text-sm font-medium text-slate-700">
          Address
          <textarea
            className={`${inputClass} min-h-24 resize-y`}
            name="address"
            defaultValue={initialData.address || ""}
          />
        </label>
        <label className="block text-sm font-medium text-slate-700">
          Profile photo URL
          <input
            className={inputClass}
            name="profilePhoto"
            type="url"
            defaultValue={initialData.profilePhoto || ""}
          />
        </label>
        {editing && (
          <label className="flex items-center gap-3 text-sm font-medium text-slate-700">
            <input
              type="checkbox"
              name="isActivate"
              value="true"
              defaultChecked={initialData.isActivate !== false}
              className="h-4 w-4 rounded border-slate-300 text-emerald-950 focus:ring-emerald-500"
            />{" "}
            Account is active
          </label>
        )}
        <div className="flex justify-end gap-3 border-t border-slate-100 pt-5">
          <Link
            href="/admin/user"
            className="rounded-lg px-4 py-2.5 text-sm font-semibold text-slate-600 hover:bg-slate-50"
          >
            Cancel
          </Link>
          <button
            type="submit"
            className="rounded-lg bg-emerald-950 px-5 py-2.5 text-sm font-semibold text-white shadow-sm hover:bg-emerald-900"
          >
            {editing ? "Save changes" : "Create account"}
          </button>
        </div>
      </AdminForm>
    </section>
  );
}

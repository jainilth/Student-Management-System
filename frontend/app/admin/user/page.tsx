import Link from "next/link";
import { revalidatePath } from "next/cache";
import { DeleteUser, GetAllUsers } from "@/service/user.service";

type UserRecord = {
  userId: number;
  userName: string;
  email: string;
  roleName?: string;
  mobilenumber?: string;
  createdAt?: string;
  isActivate: boolean;
};

function displayDate(value: string | null | undefined) {
  if (!value) return "-";
  return new Intl.DateTimeFormat("en", { dateStyle: "medium" }).format(
    new Date(value),
  );
}

export default async function UserListPage() {
  const response = await GetAllUsers();
  const data = response?.data || response || [];

  if (response?.error) {
    return (
      <div className="p-8 bg-red-50 border border-red-200 rounded-lg text-red-700">
        <strong>API Error:</strong> {response.error}
        <br />
        <small className="text-red-500">
          Check that the backend is running on the correct port.
        </small>
      </div>
    );
  }

  const users = Array.isArray(data) ? data : [];

  async function removeUser(formData: FormData) {
    "use server";
    const id = Number(formData.get("userId"));
    const result = await DeleteUser(id);
    if (!result?.error) revalidatePath("/admin/user");
  }

  return (
    <section className="space-y-7">
      <header className="flex flex-col gap-4 border-b border-slate-200 pb-6 sm:flex-row sm:items-end sm:justify-between">
        <div>
          <p className="text-sm font-semibold uppercase tracking-[0.2em] text-indigo-600">
            Directory
          </p>
          <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
            Users
          </h1>
          <p className="mt-2 text-sm text-slate-500">
            Manage access, profiles, and account status.
          </p>
        </div>
        <Link
          href="/admin/user/create"
          className="inline-flex w-fit items-center gap-2 rounded-lg bg-indigo-600 px-4 py-2.5 text-sm font-semibold text-white shadow-sm transition hover:bg-indigo-700"
        >
          <span className="text-lg leading-none">+</span> Add user
        </Link>
      </header>
      <div className="overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">
        <div className="flex items-center justify-between border-b border-slate-100 px-5 py-4">
          <div>
            <h2 className="font-semibold text-slate-900">Account directory</h2>
            <p className="mt-1 text-xs text-slate-500">
              {users.length} {users.length === 1 ? "account" : "accounts"}
            </p>
          </div>
        </div>
        {users.length === 0 ? (
          <div className="px-5 py-16 text-center text-sm text-slate-500">
            No users found. Add the first account to get started.
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full min-w-[760px] text-left text-sm">
              <thead className="bg-slate-50 text-xs uppercase tracking-wide text-slate-500">
                <tr>
                  <th className="px-5 py-3 font-medium">User</th>
                  <th className="px-5 py-3 font-medium">Role</th>
                  <th className="px-5 py-3 font-medium">Mobile</th>
                  <th className="px-5 py-3 font-medium">Created</th>
                  <th className="px-5 py-3 font-medium">Status</th>
                  <th className="px-5 py-3 text-right font-medium">Actions</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-100">
                {users.map((user: UserRecord) => (
                  <tr
                    key={user.userId}
                    className="transition hover:bg-indigo-50/30"
                  >
                    <td className="px-5 py-4">
                      <div className="font-medium text-slate-900">
                        {user.userName}
                      </div>
                      <div className="mt-1 text-xs text-slate-500">
                        {user.email}
                      </div>
                    </td>
                    <td className="px-5 py-4 text-slate-600">
                      {user.roleName || "Unassigned"}
                    </td>
                    <td className="px-5 py-4 text-slate-600">
                      {user.mobilenumber || "-"}
                    </td>
                    <td className="px-5 py-4 text-slate-600">
                      {displayDate(user.createdAt)}
                    </td>
                    <td className="px-5 py-4">
                      <span
                        className={
                          user.isActivate
                            ? "rounded-full bg-emerald-50 px-2.5 py-1 text-xs font-semibold text-emerald-700"
                            : "rounded-full bg-slate-100 px-2.5 py-1 text-xs font-semibold text-slate-600"
                        }
                      >
                        {user.isActivate ? "Active" : "Inactive"}
                      </span>
                    </td>
                    <td className="px-5 py-4">
                      <div className="flex justify-end gap-3">
                        <Link
                          href={`/admin/user/edit/${user.userId}`}
                          className="font-semibold text-indigo-600 hover:text-indigo-800"
                        >
                          Edit
                        </Link>
                        <form action={removeUser}>
                          <input
                            type="hidden"
                            name="userId"
                            value={user.userId}
                          />
                          <button
                            type="submit"
                            className="font-semibold text-rose-600 hover:text-rose-800"
                          >
                            Delete
                          </button>
                        </form>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </section>
  );
}

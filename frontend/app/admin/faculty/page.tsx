import Link from "next/link";
import AdminDeleteForm from "@/components/AdminDeleteForm";
import { revalidatePath } from "next/cache";
import { GetAllFacultys, DeleteFaculty } from "@/service/faculty.service";

type RecordData = Record<string, any>;

export default async function FacultyListPage() {
  const response = await GetAllFacultys();
  const data = response?.data || response || [];
  if (response?.error)
    return (
      <div className="rounded-lg border border-red-200 bg-red-50 p-8 text-red-700">
        {response.error}
      </div>
    );
  const records = Array.isArray(data) ? data : [];
  async function removeRecord(formData: FormData) {
    "use server";
    const result = await DeleteFaculty(Number(formData.get("facultyId")));
    if (!result?.error) revalidatePath("/admin/faculty");
  }
  return (
    <section className="space-y-7">
      <header className="flex flex-col gap-4 border-b border-slate-200 pb-6 sm:flex-row sm:items-end sm:justify-between">
        <div>
          <p className="text-sm font-semibold uppercase tracking-[0.2em] text-indigo-600">
            Administration
          </p>
          <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
            Faculty
          </h1>
          <p className="mt-2 text-sm text-slate-500">Manage faculty records.</p>
        </div>
        <Link
          href="/admin/faculty/create"
          className="inline-flex w-fit rounded-lg bg-indigo-600 px-4 py-2.5 text-sm font-semibold text-white hover:bg-indigo-700"
        >
          + Add record
        </Link>
      </header>
      <div className="overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">
        <div className="border-b border-slate-100 px-5 py-4">
          <h2 className="font-semibold text-slate-900">Faculty directory</h2>
          <p className="mt-1 text-xs text-slate-500">
            {records.length} {records.length === 1 ? "record" : "records"}
          </p>
        </div>
        {records.length === 0 ? (
          <div className="px-5 py-16 text-center text-sm text-slate-500">
            No faculty found.
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full min-w-[720px] text-left text-sm">
              <thead className="bg-slate-50 text-xs uppercase tracking-wide text-slate-500">
                <tr>
                  <th className="px-5 py-3 font-medium">FacultyId</th>
                  <th className="px-5 py-3 font-medium">User</th>
                  <th className="px-5 py-3 font-medium">Employee number</th>
                  <th className="px-5 py-3 font-medium">Department</th>
                  <th className="px-5 py-3 font-medium">Designation</th>
                  <th className="px-5 py-3 font-medium">Joining date</th>
                  <th className="px-5 py-3 text-right font-medium">Actions</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-100">
                {records.map((record: RecordData) => (
                  <tr key={record.facultyId} className="hover:bg-indigo-50/30">
                    <td className="px-5 py-4 font-medium text-slate-900">
                      {record.facultyId}
                    </td>
                    <td className="px-5 py-4 text-slate-600">
                      {String(record.userName || record.userId || "-")}
                    </td>
                    <td className="px-5 py-4 text-slate-600">
                      {String(record.employeeNumber ?? "-")}
                    </td>
                    <td className="px-5 py-4 text-slate-600">
                      {String(record.departmentName || record.departmentId || "-")}
                    </td>
                    <td className="px-5 py-4 text-slate-600">
                      {String(record.designation ?? "-")}
                    </td>
                    <td className="px-5 py-4 text-slate-600">
                      {String(record.joiningDate ?? "-")}
                    </td>
                    <td className="px-5 py-4">
                      <div className="flex justify-end gap-3">
                        <Link
                          href={"/admin/faculty/edit/" + record.facultyId}
                          className="font-semibold text-indigo-600 hover:text-indigo-800"
                        >
                          Edit
                        </Link>
                        <AdminDeleteForm action={removeRecord}>
                          <input
                            type="hidden"
                            name="facultyId"
                            value={record.facultyId}
                          />
                          <button
                            type="submit"
                            className="font-semibold text-rose-600 hover:text-rose-800"
                          >
                            Delete
                          </button>
                        </AdminDeleteForm>
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

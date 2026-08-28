import Link from "next/link";
import AdminDeleteForm from "@/components/AdminDeleteForm";
import { revalidatePath } from "next/cache";
import { GetAllGrades, DeleteGrade } from "@/service/grade.service";
import TableToolbar from "@/components/TableToolbar";

type RecordData = Record<string, any>;

export default async function GradeListPage() {
  const response = await GetAllGrades();
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
    const result = await DeleteGrade(Number(formData.get("gradeId")));
    if (!result?.error) revalidatePath("/admin/grade");
  }
  return (
    <section className="space-y-7">
      <header className="flex flex-col gap-4 border-b border-slate-200 pb-6 sm:flex-row sm:items-end sm:justify-between">
        <div>
          <p className="text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
            Administration
          </p>
          <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
            Grades
          </h1>
          <p className="mt-2 text-sm text-slate-500">Manage grades records.</p>
        </div>
        <Link
          href="/admin/grade/create"
          className="inline-flex w-fit rounded-lg bg-emerald-950 px-4 py-2.5 text-sm font-semibold text-white hover:bg-emerald-900"
        >
          + Add record
        </Link>
      </header>
      <div className="overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">
        <TableToolbar />
        <div className="border-b border-slate-100 px-5 py-4">
          <h2 className="font-semibold text-slate-900">Grades directory</h2>
          <p className="mt-1 text-xs text-slate-500">
            {records.length} {records.length === 1 ? "record" : "records"}
          </p>
        </div>
        {records.length === 0 ? (
          <div className="px-5 py-16 text-center text-sm text-slate-500">
            No grades found.
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full min-w-[720px] text-left text-sm">
              <thead className="bg-slate-50 text-xs uppercase tracking-wide text-slate-500">
                <tr>
                  <th className="px-5 py-3 font-medium">GradeId</th>
                  <th className="px-5 py-3 font-medium">Grade code</th>
                  <th className="px-5 py-3 font-medium">Grade name</th>
                  <th className="px-5 py-3 font-medium">Grade point</th>
                  <th className="px-5 py-3 font-medium">Minimum marks</th>
                  <th className="px-5 py-3 font-medium">Maximum marks</th>
                  <th className="px-5 py-3 text-right font-medium">Actions</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-100">
                {records.map((record: RecordData) => (
                  <tr key={record.gradeId} className="hover:bg-emerald-50/30">
                    <td className="px-5 py-4 font-medium text-slate-900">
                      {record.gradeId}
                    </td>
                    <td className="px-5 py-4 text-slate-600">
                      {String(record.gradeCode ?? "-")}
                    </td>
                    <td className="px-5 py-4 text-slate-600">
                      {String(record.gradeName ?? "-")}
                    </td>
                    <td className="px-5 py-4 text-slate-600">
                      {String(record.gradePoint ?? "-")}
                    </td>
                    <td className="px-5 py-4 text-slate-600">
                      {String(record.minMarks ?? "-")}
                    </td>
                    <td className="px-5 py-4 text-slate-600">
                      {String(record.maxMarks ?? "-")}
                    </td>
                    <td className="px-5 py-4">
                      <div className="flex justify-end gap-3">
                        <Link
                          href={"/admin/grade/edit/" + record.gradeId}
                          className="font-semibold text-emerald-950 hover:text-emerald-900"
                        >
                          Edit
                        </Link>
                        <AdminDeleteForm action={removeRecord}>
                          <input
                            type="hidden"
                            name="gradeId"
                            value={record.gradeId}
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

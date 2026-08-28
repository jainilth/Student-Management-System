import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { GetAllSemesterSubjects } from "@/service/semesterSubject.service";
import { GetAllUsers } from "@/service/user.service";

type MaterialFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
type SelectOption = { value: number; label: string };
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default async function MaterialForm({
  initialData = {},
  onSubmitAction,
  mode,
}: MaterialFormProps) {
  const editing = mode === "edit";
  const semesterSubjects = await GetAllSemesterSubjects();
  const users = await GetAllUsers();
  const semesterSubjectIdOptions: SelectOption[] = (semesterSubjects?.data ?? []).map((record: any) => ({
    value: Number(record.semesterSubjectId),
    label: [record.subjectName, record.programName, record.semesterName]
      .filter(Boolean)
      .join(" - ") || `Record ${record.semesterSubjectId}`,
  }));
  const uploadedByOptions: SelectOption[] = (users?.data ?? []).map((record: any) => ({
    value: Number(record.userId),
    label: record.userName || record.email || `Record ${record.userId}`,
  }));
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/material"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to materials
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Materials" : "Add Materials"}
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
            Semester subject
            <select
              className={inputClass}
              name="semesterSubjectId"
              required
              defaultValue={initialData.semesterSubjectId ?? ""}
            >
              <option value="" disabled>
                Select semester subject
              </option>
              {semesterSubjectIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Uploaded by user
            <select
              className={inputClass}
              name="uploadedBy"
              required
              defaultValue={initialData.uploadedBy ?? ""}
            >
              <option value="" disabled>
                Select uploaded by user
              </option>
              {uploadedByOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Material type
            <input
              className={inputClass}
              name="materialType"
              type="text"
              defaultValue={initialData.materialType ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            File name
            <input
              className={inputClass}
              name="fileName"
              type="text"
              defaultValue={initialData.fileName ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            File URL
            <input
              className={inputClass}
              name="fileUrl"
              type="url"
              defaultValue={initialData.fileUrl ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            File size
            <input
              className={inputClass}
              name="fileSize"
              type="number"
              step="any"
              defaultValue={initialData.fileSize ?? ""}
            />
          </label>
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/material"
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

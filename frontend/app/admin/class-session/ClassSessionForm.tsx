import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { GetAllFacultys } from "@/service/faculty.service";
import { GetAllSemesterSubjects } from "@/service/semesterSubject.service";

type ClassSessionFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
type SelectOption = { value: number; label: string };
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default async function ClassSessionForm({
  initialData = {},
  onSubmitAction,
  mode,
}: ClassSessionFormProps) {
  const editing = mode === "edit";
  const semesterSubjects = await GetAllSemesterSubjects();
  const faculties = await GetAllFacultys();
  const semesterSubjectIdOptions: SelectOption[] = (semesterSubjects?.data ?? []).map((record: any) => ({
    value: Number(record.semesterSubjectId),
    label: [record.subjectName, record.programName, record.semesterName]
      .filter(Boolean)
      .join(" - ") || `Record ${record.semesterSubjectId}`,
  }));
  const facultyIdOptions: SelectOption[] = (faculties?.data ?? []).map((record: any) => ({
    value: Number(record.facultyId),
    label: record.userName || record.employeeNumber || `Record ${record.facultyId}`,
  }));
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/class-session"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to class sessions
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Class Sessions" : "Add Class Sessions"}
        </h1>
      </header>
      <AdminForm
        action={onSubmitAction}
        preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
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
            Faculty
            <select
              className={inputClass}
              name="facultyId"
              required
              defaultValue={initialData.facultyId ?? ""}
            >
              <option value="" disabled>
                Select faculty
              </option>
              {facultyIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Session date
            <input
              className={inputClass}
              name="sessionDate"
              type="date"
              step="any"
              defaultValue={initialData.sessionDate ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Start time
            <input
              className={inputClass}
              name="startTime"
              type="time"
              step="any"
              defaultValue={initialData.startTime ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            End time
            <input
              className={inputClass}
              name="endTime"
              type="time"
              step="any"
              defaultValue={initialData.endTime ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Topic
            <input
              className={inputClass}
              name="topic"
              type="text"
              defaultValue={initialData.topic ?? ""}
            />
          </label>
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/class-session"
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

import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { GetAllAcademicPrograms } from "@/service/academicProgram.service";
import { GetAllSemesters } from "@/service/semester.service";
import { GetAllSubjects } from "@/service/subject.service";

type SemesterSubjectFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
type SelectOption = { value: number; label: string };
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default async function SemesterSubjectForm({
  initialData = {},
  onSubmitAction,
  mode,
}: SemesterSubjectFormProps) {
  const editing = mode === "edit";
  const programs = await GetAllAcademicPrograms();
  const semesters = await GetAllSemesters();
  const subjects = await GetAllSubjects();
  const programIdOptions: SelectOption[] = (programs?.data ?? []).map((record: any) => ({
    value: Number(record.programId),
    label: record.programName || `Record ${record.programId}`,
  }));
  const semesterIdOptions: SelectOption[] = (semesters?.data ?? []).map((record: any) => ({
    value: Number(record.semesterId),
    label: record.semesterName || `Record ${record.semesterId}`,
  }));
  const subjectIdOptions: SelectOption[] = (subjects?.data ?? []).map((record: any) => ({
    value: Number(record.subjectId),
    label: [record.subjectName, record.subjectCode].filter(Boolean).join(" - ") || `Record ${record.subjectId}`,
  }));
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/semester-subject"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to semester subjects
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Semester Subjects" : "Add Semester Subjects"}
        </h1>
      </header>
      <AdminForm
        action={onSubmitAction}
        preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
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
            Subject
            <select
              className={inputClass}
              name="subjectId"
              required
              defaultValue={initialData.subjectId ?? ""}
            >
              <option value="" disabled>
                Select subject
              </option>
              {subjectIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Credits
            <input
              className={inputClass}
              name="credits"
              type="number"
              step="any"
              defaultValue={initialData.credits ?? ""}
            />
          </label>
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/semester-subject"
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

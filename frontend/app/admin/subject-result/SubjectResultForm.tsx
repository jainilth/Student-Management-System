import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { GetAllGrades } from "@/service/grade.service";
import { GetAllStudentSemesters } from "@/service/studentSemester.service";
import { GetAllSemesterSubjects } from "@/service/semesterSubject.service";

type SubjectResultFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
type SelectOption = { value: number; label: string };
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default async function SubjectResultForm({
  initialData = {},
  onSubmitAction,
  mode,
}: SubjectResultFormProps) {
  const editing = mode === "edit";
  const studentSemesters = await GetAllStudentSemesters();
  const semesterSubjects = await GetAllSemesterSubjects();
  const grades = await GetAllGrades();
  const studentSemesterIdOptions: SelectOption[] = (studentSemesters?.data ?? []).map((record: any) => ({
    value: Number(record.studentSemesterId),
    label:[record.studentName,record.academicProgramName, record.semesterName]
      .filter(Boolean)
      .join(" - ") || `Record ${record.studentSemesterId}`,
  }));
  const semesterSubjectIdOptions: SelectOption[] = (semesterSubjects?.data ?? []).map((record: any) => ({
    value: Number(record.semesterSubjectId),
    label: [record.subjectName, record.programName, record.semesterName]
      .filter(Boolean)
      .join(" - ") || `Record ${record.semesterSubjectId}`,
  }));
  const gradeIdOptions: SelectOption[] = (grades?.data ?? []).map((record: any) => ({
    value: Number(record.gradeId),
    label: record.gradeCode || record.gradeName || `Record ${record.gradeId}`,
  }));
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/subject-result"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to subject results
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Subject Results" : "Add Subject Results"}
        </h1>
      </header>
      <AdminForm
        action={onSubmitAction}
        preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <label className="text-sm font-medium text-slate-700">
            Student semester
            <select
              className={inputClass}
              name="studentSemesterId"
              required
              defaultValue={initialData.studentSemesterId ?? ""}
            >
              <option value="" disabled>
                Select student semester
              </option>
              {studentSemesterIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
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
            Internal marks
            <input
              className={inputClass}
              name="internalMarks"
              type="number"
              step="any"
              defaultValue={initialData.internalMarks ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            External marks
            <input
              className={inputClass}
              name="externalMarks"
              type="number"
              step="any"
              defaultValue={initialData.externalMarks ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Practical marks
            <input
              className={inputClass}
              name="practicalMarks"
              type="number"
              step="any"
              defaultValue={initialData.practicalMarks ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Total marks
            <input
              className={inputClass}
              name="totalMarks"
              type="number"
              step="any"
              defaultValue={initialData.totalMarks ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Grade
            <select
              className={inputClass}
              name="gradeId"
              required
              defaultValue={initialData.gradeId ?? ""}
            >
              <option value="" disabled>
                Select grade
              </option>
              {gradeIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Credits earned
            <input
              className={inputClass}
              name="creditsEarned"
              type="number"
              step="any"
              defaultValue={initialData.creditsEarned ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Result status
            <input
              className={inputClass}
              name="resultStatus"
              type="text"
              defaultValue={initialData.resultStatus ?? ""}
            />
          </label>
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/subject-result"
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

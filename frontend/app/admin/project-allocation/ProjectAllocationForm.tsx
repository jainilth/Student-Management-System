import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { GetAllFacultys } from "@/service/faculty.service";
import { GetAllProjects } from "@/service/project.service";
import { GetAllStudentSemesters } from "@/service/studentSemester.service";

type ProjectAllocationFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
  showEvaluationFields?: boolean;
};
type SelectOption = { value: number; label: string };
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default async function ProjectAllocationForm({
  initialData = {},
  onSubmitAction,
  mode,
  showEvaluationFields,
}: ProjectAllocationFormProps) {
  const editing = showEvaluationFields ?? (mode === "edit" || Boolean(initialData.allocationId));
  const projects = await GetAllProjects();
  const studentSemesters = await GetAllStudentSemesters();
  const faculties = await GetAllFacultys();
  const projectIdOptions: SelectOption[] = (projects?.data ?? []).map((record: any) => ({
    value: Number(record.projectId),
    label: record.title || `Record ${record.projectId}`,
  }));
  const studentIdOptions: SelectOption[] = (studentSemesters?.data ?? []).map((record: any) => ({
    value: Number(record.studentSemesterId),
    label: [record.studentName, record.academicProgramName, record.semesterName]
      .filter(Boolean)
      .join(" - ") || `Record ${record.studentSemesterId}`,
  }));
  const facultyIdOptions: SelectOption[] = (faculties?.data ?? []).map((record: any) => ({
    value: Number(record.facultyId),
    label: record.userName || record.employeeNumber || `Record ${record.facultyId}`,
  }));
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/project-allocation"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to project allocations
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Project Allocations" : "Add Project Allocations"}
        </h1>
      </header>
      <AdminForm
        action={onSubmitAction}
        preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <label className="text-sm font-medium text-slate-700">
            Project
            <select
              className={inputClass}
              name="projectId"
              required
              defaultValue={initialData.projectId ?? ""}
            >
              <option value="" disabled>
                Select project
              </option>
              {projectIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Student
            <select
              className={inputClass}
              name="studentId"
              required
              defaultValue={initialData.studentId ?? ""}
            >
              <option value="" disabled>
                Select student
              </option>
              {studentIdOptions.map((option) => (
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
            Final score
            <input
              className={inputClass}
              name="finalScore"
              type="number"
              step="any"
              defaultValue={initialData.finalScore ?? ""}
            />
          </label>
          {editing ? (
            <>
              <label className="text-sm font-medium text-slate-700">
                Grade
                <input
                  className={inputClass}
                  name="grade"
                  type="text"
                  defaultValue={initialData.grade ?? ""}
                />
              </label>
              <label className="text-sm font-medium text-slate-700">
                Status
                <select
                  className={inputClass}
                  name="status"
                  required
                  defaultValue={initialData.status ?? "Pending"}
                >
                  <option value="Pending">Pending</option>
                  <option value="Completed">Completed</option>
                </select>
              </label>
            </>
          ) : (
            <>
              <input type="hidden" name="grade" value="N/A" />
              <input type="hidden" name="status" value="Pending" />
            </>
          )}
          <label className="text-sm font-medium text-slate-700">
            Repository URL
            <input
              className={inputClass}
              name="repositoryUrl"
              type="url"
              defaultValue={initialData.repositoryUrl ?? ""}
            />
          </label>
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/project-allocation"
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

import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { GetAllProjectAllocations } from "@/service/projectAllocation.service";

type ProjectTaskFormProps = {
  initialData?: Record<string, any>;
  onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
  mode: "create" | "edit";
};
type SelectOption = { value: number; label: string };
const inputClass =
  "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default async function ProjectTaskForm({
  initialData = {},
  onSubmitAction,
  mode,
}: ProjectTaskFormProps) {
  const editing = mode === "edit";
  const projectAllocations = await GetAllProjectAllocations();
  const projectAllocationIdOptions: SelectOption[] = (projectAllocations?.data ?? []).map((record: any) => ({
    value: Number(record.allocationId),
    label: [record.projectTitle, record.studentName, record.status]
      .filter(Boolean)
      .join(" - ") || `Record ${record.allocationId}`,
  }));
  return (
    <section className="mx-auto max-w-4xl space-y-7">
      <header className="border-b border-slate-200 pb-6">
        <Link
          href="/admin/project-task"
          className="text-sm font-semibold text-emerald-950 hover:text-emerald-900"
        >
          &lt;- Back to project tasks
        </Link>
        <p className="mt-6 text-sm font-semibold uppercase tracking-[0.2em] text-emerald-950">
          {editing ? "Edit record" : "New record"}
        </p>
        <h1 className="mt-2 text-3xl font-semibold tracking-tight text-slate-950">
          {editing ? "Edit Project Tasks" : "Add Project Tasks"}
        </h1>
      </header>
      <AdminForm
        action={onSubmitAction}
        preserveValuesOnError={!editing}
        className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <label className="text-sm font-medium text-slate-700">
            Project allocation
            <select
              className={inputClass}
              name="projectAllocationId"
              required
              defaultValue={initialData.projectAllocationId ?? ""}
            >
              <option value="" disabled>
                Select project allocation
              </option>
              {projectAllocationIdOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="text-sm font-medium text-slate-700">
            Task title
            <input
              className={inputClass}
              name="taskTitle"
              type="text"
              defaultValue={initialData.taskTitle ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Task description
            <textarea
              className={inputClass}
              name="taskDescription"
              defaultValue={initialData.taskDescription || ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Task status
            <input
              className={inputClass}
              name="taskStatus"
              type="text"
              defaultValue={initialData.taskStatus ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Assigned score
            <input
              className={inputClass}
              name="assignedScore"
              type="number"
              step="any"
              defaultValue={initialData.assignedScore ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Earned score
            <input
              className={inputClass}
              name="earnedScore"
              type="number"
              step="any"
              defaultValue={initialData.earnedScore ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Start date
            <input
              className={inputClass}
              name="startDate"
              type="date"
              step="any"
              defaultValue={initialData.startDate ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Due date
            <input
              className={inputClass}
              name="dueDate"
              type="date"
              step="any"
              defaultValue={initialData.dueDate ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Completed date
            <input
              className={inputClass}
              name="completedDate"
              type="date"
              step="any"
              defaultValue={initialData.completedDate ?? ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Faculty remarks
            <textarea
              className={inputClass}
              name="facultyRemarks"
              defaultValue={initialData.facultyRemarks || ""}
            />
          </label>
          <label className="text-sm font-medium text-slate-700">
            Student remarks
            <textarea
              className={inputClass}
              name="studentRemarks"
              defaultValue={initialData.studentRemarks || ""}
            />
          </label>
        </div>
        <div className="flex justify-end gap-3">
          <Link
            href="/admin/project-task"
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

import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateProjectTask } from "@/service/projectTask.service";
import ProjectTaskForm from "../ProjectTaskForm";

export default function CreateProjectTaskPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateProjectTask(data);
    if (!result?.error) {
      revalidatePath("/admin/project-task");
      redirect("/admin/project-task");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <ProjectTaskForm mode="create" onSubmitAction={handleSubmit} />;
}

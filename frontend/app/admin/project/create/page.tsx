import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateProject } from "@/service/project.service";
import ProjectForm from "../ProjectForm";

export default function CreateProjectPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateProject(data);
    if (!result?.error) {
      revalidatePath("/admin/project");
      redirect("/admin/project");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <ProjectForm mode="create" onSubmitAction={handleSubmit} />;
}

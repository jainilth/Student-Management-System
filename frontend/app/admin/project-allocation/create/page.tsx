import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateProjectAllocation } from "@/service/projectAllocation.service";
import ProjectAllocationForm from "../ProjectAllocationForm";

export default function CreateProjectAllocationPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateProjectAllocation(data);
    if (!result?.error) {
      revalidatePath("/admin/project-allocation");
      redirect("/admin/project-allocation");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return (
    <ProjectAllocationForm
      mode="create"
      showEvaluationFields={false}
      onSubmitAction={handleSubmit}
    />
  );
}

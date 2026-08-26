import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { notFound, redirect } from "next/navigation";
import {
  GetProjectAllocationById,
  UpdateProjectAllocation,
} from "@/service/projectAllocation.service";
import ProjectAllocationForm from "../../ProjectAllocationForm";

type Props = { params: Promise<{ id: string }> };

export default async function EditProjectAllocationPage({ params }: Props) {
  const id = Number((await params).id);
  const response = await GetProjectAllocationById(id);
  if (response?.error)
    return (
      <div className="rounded-lg border border-red-200 bg-red-50 p-8 text-red-700">
        {response.error}
      </div>
    );
  const initialData = response?.data || response;
  if (!initialData) notFound();
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await UpdateProjectAllocation(id, data);
    if (!result?.error) {
      revalidatePath("/admin/project-allocation");
      redirect("/admin/project-allocation");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return (
    <ProjectAllocationForm
      initialData={initialData}
      mode="edit"
      onSubmitAction={handleSubmit}
    />
  );
}

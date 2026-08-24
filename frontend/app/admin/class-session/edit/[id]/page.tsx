import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { notFound, redirect } from "next/navigation";
import {
  GetClassSessionById,
  UpdateClassSession,
} from "@/service/classSession.service";
import ClassSessionForm from "../../ClassSessionForm";

type Props = { params: Promise<{ id: string }> };

export default async function EditClassSessionPage({ params }: Props) {
  const id = Number((await params).id);
  const response = await GetClassSessionById(id);
  if (response?.error)
    return (
      <div className="rounded-lg border border-red-200 bg-red-50 p-8 text-red-700">
        <strong>API Error:</strong> {response.error}
      </div>
    );
  const initialData = response?.data || response;
  if (!initialData) notFound();
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await UpdateClassSession(id, data);
    if (!result?.error) {
      revalidatePath("/admin/class-session");
      redirect("/admin/class-session");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return (
    <ClassSessionForm
      initialData={initialData}
      mode="edit"
      onSubmitAction={handleSubmit}
    />
  );
}

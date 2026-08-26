import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { notFound, redirect } from "next/navigation";
import { GetSubjectById, UpdateSubject } from "@/service/subject.service";
import SubjectForm from "../../SubjectForm";

type Props = { params: Promise<{ id: string }> };

export default async function EditSubjectPage({ params }: Props) {
  const id = Number((await params).id);
  const response = await GetSubjectById(id);
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
    const result = await UpdateSubject(id, data);
    if (!result?.error) {
      revalidatePath("/admin/subject");
      redirect("/admin/subject");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return (
    <SubjectForm
      initialData={initialData}
      mode="edit"
      onSubmitAction={handleSubmit}
    />
  );
}

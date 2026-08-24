import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { notFound, redirect } from "next/navigation";
import {
  GetAcademicProgramById,
  UpdateAcademicProgram,
} from "@/service/academicProgram.service";
import AcademicProgramForm from "../../AcademicProgramForm";

type Props = { params: Promise<{ id: string }> };

export default async function EditAcademicProgramPage({ params }: Props) {
  const id = Number((await params).id);
  const response = await GetAcademicProgramById(id);
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
    const result = await UpdateAcademicProgram(id, data);
    if (!result?.error) {
      revalidatePath("/admin/academic-program");
      redirect("/admin/academic-program");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return (
    <AcademicProgramForm
      initialData={initialData}
      mode="edit"
      onSubmitAction={handleSubmit}
    />
  );
}

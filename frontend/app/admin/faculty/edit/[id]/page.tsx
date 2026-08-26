import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { notFound, redirect } from "next/navigation";
import { GetFacultyById, UpdateFaculty } from "@/service/faculty.service";
import FacultyForm from "../../FacultyForm";

type Props = { params: Promise<{ id: string }> };

export default async function EditFacultyPage({ params }: Props) {
  const id = Number((await params).id);
  const response = await GetFacultyById(id);
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
    const result = await UpdateFaculty(id, data);
    if (!result?.error) {
      revalidatePath("/admin/faculty");
      redirect("/admin/faculty");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return (
    <FacultyForm
      initialData={initialData}
      mode="edit"
      onSubmitAction={handleSubmit}
    />
  );
}

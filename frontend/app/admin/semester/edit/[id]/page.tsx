import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { notFound, redirect } from "next/navigation";
import { GetSemesterById, UpdateSemester } from "@/service/semester.service";
import SemesterForm from "../../SemesterForm";

type Props = { params: Promise<{ id: string }> };

export default async function EditSemesterPage({ params }: Props) {
  const id = Number((await params).id);
  const response = await GetSemesterById(id);
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
    data.semesterNumber = Number(data.semesterNumber);
    data.isActive = formData.get("isActive") === "on";
    const result = await UpdateSemester(id, data);
    if (!result?.error) {
      revalidatePath("/admin/semester");
      redirect("/admin/semester");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return (
    <SemesterForm
      initialData={initialData}
      mode="edit"
      onSubmitAction={handleSubmit}
    />
  );
}

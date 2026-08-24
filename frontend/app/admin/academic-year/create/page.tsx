import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateAcademicYear } from "@/service/academicYear.service";
import AcademicYearForm from "../AcademicYearForm";

export default function CreateAcademicYearPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateAcademicYear(data);
    if (!result?.error) {
      revalidatePath("/admin/academic-year");
      redirect("/admin/academic-year");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <AcademicYearForm mode="create" onSubmitAction={handleSubmit} />;
}

import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateAcademicProgram } from "@/service/academicProgram.service";
import AcademicProgramForm from "../AcademicProgramForm";

export default function CreateAcademicProgramPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateAcademicProgram(data);
    if (!result?.error) {
      revalidatePath("/admin/academic-program");
      redirect("/admin/academic-program");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <AcademicProgramForm mode="create" onSubmitAction={handleSubmit} />;
}

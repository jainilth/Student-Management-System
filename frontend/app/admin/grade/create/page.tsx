import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateGrade } from "@/service/grade.service";
import GradeForm from "../GradeForm";

export default function CreateGradePage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateGrade(data);
    if (!result?.error) {
      revalidatePath("/admin/grade");
      redirect("/admin/grade");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <GradeForm mode="create" onSubmitAction={handleSubmit} />;
}

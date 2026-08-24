import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateSemesterResult } from "@/service/semesterResult.service";
import SemesterResultForm from "../SemesterResultForm";

export default function CreateSemesterResultPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateSemesterResult(data);
    if (!result?.error) {
      revalidatePath("/admin/semester-result");
      redirect("/admin/semester-result");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <SemesterResultForm mode="create" onSubmitAction={handleSubmit} />;
}

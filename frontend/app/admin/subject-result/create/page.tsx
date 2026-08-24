import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateSubjectResult } from "@/service/subjectResult.service";
import SubjectResultForm from "../SubjectResultForm";

export default function CreateSubjectResultPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateSubjectResult(data);
    if (!result?.error) {
      revalidatePath("/admin/subject-result");
      redirect("/admin/subject-result");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <SubjectResultForm mode="create" onSubmitAction={handleSubmit} />;
}

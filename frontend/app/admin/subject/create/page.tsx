import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateSubject } from "@/service/subject.service";
import SubjectForm from "../SubjectForm";

export default function CreateSubjectPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateSubject(data);
    if (!result?.error) {
      revalidatePath("/admin/subject");
      redirect("/admin/subject");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <SubjectForm mode="create" onSubmitAction={handleSubmit} />;
}

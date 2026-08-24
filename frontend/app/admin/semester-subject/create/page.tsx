import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateSemesterSubject } from "@/service/semesterSubject.service";
import SemesterSubjectForm from "../SemesterSubjectForm";

export default function CreateSemesterSubjectPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateSemesterSubject(data);
    if (!result?.error) {
      revalidatePath("/admin/semester-subject");
      redirect("/admin/semester-subject");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <SemesterSubjectForm mode="create" onSubmitAction={handleSubmit} />;
}

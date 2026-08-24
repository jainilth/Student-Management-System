import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateFacultySubject } from "@/service/facultySubject.service";
import FacultySubjectForm from "../FacultySubjectForm";

export default function CreateFacultySubjectPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateFacultySubject(data);
    if (!result?.error) {
      revalidatePath("/admin/faculty-subject");
      redirect("/admin/faculty-subject");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <FacultySubjectForm mode="create" onSubmitAction={handleSubmit} />;
}

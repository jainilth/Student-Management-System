import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateStudentSemester } from "@/service/studentSemester.service";
import StudentSemesterForm from "../StudentSemesterForm";

export default function CreateStudentSemesterPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateStudentSemester(data);
    if (!result?.error) {
      revalidatePath("/admin/student-semester");
      redirect("/admin/student-semester");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <StudentSemesterForm mode="create" onSubmitAction={handleSubmit} />;
}

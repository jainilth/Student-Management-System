import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateStudent } from "@/service/student.service";
import StudentForm from "../StudentForm";

export default function CreateStudentPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateStudent(data);
    if (!result?.error) {
      revalidatePath("/admin/student");
      redirect("/admin/student");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <StudentForm mode="create" onSubmitAction={handleSubmit} />;
}

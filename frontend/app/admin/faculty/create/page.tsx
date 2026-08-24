import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateFaculty } from "@/service/faculty.service";
import FacultyForm from "../FacultyForm";

export default function CreateFacultyPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateFaculty(data);
    if (!result?.error) {
      revalidatePath("/admin/faculty");
      redirect("/admin/faculty");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <FacultyForm mode="create" onSubmitAction={handleSubmit} />;
}

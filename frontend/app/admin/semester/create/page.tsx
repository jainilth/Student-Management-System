import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateSemester } from "@/service/semester.service";
import SemesterForm from "../SemesterForm";

export default function CreateSemesterPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    data.semesterNumber = Number(data.semesterNumber);
    data.isActive = formData.get("isActive") === "on";
    const result = await CreateSemester(data);
    if (!result?.error) {
      revalidatePath("/admin/semester");
      redirect("/admin/semester");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <SemesterForm mode="create" onSubmitAction={handleSubmit} />;
}

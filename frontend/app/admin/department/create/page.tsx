import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateDepartment } from "@/service/department.service";
import DepartmentForm from "../DepartmentForm";

export default function CreateDepartmentPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateDepartment(data);
    if (!result?.error) {
      revalidatePath("/admin/department");
      redirect("/admin/department");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <DepartmentForm mode="create" onSubmitAction={handleSubmit} />;
}

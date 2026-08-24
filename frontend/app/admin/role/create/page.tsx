import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateRole } from "@/service/role.service";
import RoleForm from "../RoleForm";

export default function CreateRolePage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateRole(data);
    if (!result?.error) {
      revalidatePath("/admin/role");
      redirect("/admin/role");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <RoleForm mode="create" onSubmitAction={handleSubmit} />;
}

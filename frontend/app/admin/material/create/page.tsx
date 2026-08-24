import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateMaterial } from "@/service/material.service";
import MaterialForm from "../MaterialForm";

export default function CreateMaterialPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateMaterial(data);
    if (!result?.error) {
      revalidatePath("/admin/material");
      redirect("/admin/material");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <MaterialForm mode="create" onSubmitAction={handleSubmit} />;
}

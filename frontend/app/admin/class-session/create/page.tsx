import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateClassSession } from "@/service/classSession.service";
import ClassSessionForm from "../ClassSessionForm";

export default function CreateClassSessionPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateClassSession(data);
    if (!result?.error) {
      revalidatePath("/admin/class-session");
      redirect("/admin/class-session");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <ClassSessionForm mode="create" onSubmitAction={handleSubmit} />;
}

import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateAttendance } from "@/service/attendance.service";
import AttendanceForm from "../AttendanceForm";

export default function CreateAttendancePage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateAttendance(data);
    if (!result?.error) {
      revalidatePath("/admin/attendance");
      redirect("/admin/attendance");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <AttendanceForm mode="create" onSubmitAction={handleSubmit} />;
}

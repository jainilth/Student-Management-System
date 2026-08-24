import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CreateAttendanceRecord } from "@/service/attendanceRecord.service";
import AttendanceRecordForm from "../AttendanceRecordForm";

export default function CreateAttendanceRecordPage() {
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await CreateAttendanceRecord(data);
    if (!result?.error) {
      revalidatePath("/admin/attendance-record");
      redirect("/admin/attendance-record");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return <AttendanceRecordForm mode="create" onSubmitAction={handleSubmit} />;
}

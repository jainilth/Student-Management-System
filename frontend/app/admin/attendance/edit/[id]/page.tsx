import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { notFound, redirect } from "next/navigation";
import {
  GetAttendanceById,
  UpdateAttendance,
} from "@/service/attendance.service";
import AttendanceForm from "../../AttendanceForm";

type Props = { params: Promise<{ id: string }> };

export default async function EditAttendancePage({ params }: Props) {
  const id = Number((await params).id);
  const response = await GetAttendanceById(id);
  if (response?.error)
    return (
      <div className="rounded-lg border border-red-200 bg-red-50 p-8 text-red-700">
        {response.error}
      </div>
    );
  const initialData = response?.data || response;
  if (!initialData) notFound();
  async function handleSubmit(formData: FormData) {
    "use server";
    const data = getAdminPayload(formData);
    const result = await UpdateAttendance(id, data);
    if (!result?.error) {
      revalidatePath("/admin/attendance");
      redirect("/admin/attendance");
    }
    return { error: result?.error || "The request could not be completed." };
  }
  return (
    <AttendanceForm
      initialData={initialData}
      mode="edit"
      onSubmitAction={handleSubmit}
    />
  );
}

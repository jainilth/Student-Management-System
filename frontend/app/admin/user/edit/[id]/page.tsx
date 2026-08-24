import { getAdminPayload } from "@/lib/admin-form-data";
import { revalidatePath } from "next/cache";
import { notFound, redirect } from "next/navigation";
import { GetAllRoles } from "@/service/role.service";
import { GetUserById, UpdateUser } from "@/service/user.service";
import UserForm from "../../UserForm";

type Props = { params: Promise<{ id: string }> };
type RoleRecord = { roleName: string; roleId: number };

export default async function EditUserPage({ params }: Props) {
    const id = Number((await params).id);
    if (!Number.isInteger(id) || id <= 0) notFound();

    const [userResponse, rolesResponse] = await Promise.all([
        GetUserById(id),
        GetAllRoles(),
    ]);

    if (userResponse?.error) {
        return (
            <div className="rounded-lg border border-red-200 bg-red-50 p-8 text-red-700">
                <strong>API Error:</strong> {userResponse.error}
            </div>
        );
    }

    const initialData = userResponse?.data || userResponse;
    if (!initialData) notFound();

    const roles = Array.isArray(rolesResponse?.data)
        ? rolesResponse.data.map((role: RoleRecord) => ({
            label: role.roleName,
            value: role.roleId,
        }))
        : [];

    async function handleSubmit(formData: FormData) {
        "use server";
        const data = getAdminPayload(formData);

        const response = await UpdateUser(id, data);
        if (!response?.error) {
            revalidatePath("/admin/user");
            redirect("/admin/user");
        }
    return { error: response?.error || "The request could not be completed." };
    }

    return (
        <UserForm
            initialData={initialData}
            roles={roles}
            mode="edit"
            onSubmitAction={handleSubmit}
        />
    );
}

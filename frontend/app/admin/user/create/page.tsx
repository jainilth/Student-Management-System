import { getAdminPayload } from "@/lib/admin-form-data";
import { GetAllRoles } from "@/service/role.service";
import { CreateUser } from "@/service/user.service";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import UserForm from "../UserForm";

type RoleRecord = { roleName: string; roleId: number };

export default async function CreateUserPage() {
    const rolesResponse = await GetAllRoles();
    const roleOptions = Array.isArray(rolesResponse?.data)
        ? rolesResponse.data.map((role: RoleRecord) => ({
            label: role.roleName,
            value: role.roleId,
        }))
        : [];

    const handleSubmit = async (formData: FormData) => {
        "use server";
        const data = getAdminPayload(formData);

        const res = await CreateUser(data);
        if (!res?.error) {
            revalidatePath("/admin/user");
            redirect("/admin/user");
        }
    return { error: res?.error || "The request could not be completed." };
    };

    return (
        <UserForm roles={roleOptions} mode="create" onSubmitAction={handleSubmit} />
    );
}

"use client";

import { useActionState } from "react";

type FormState = { error?: string } | void;
type AdminDeleteFormProps = {
    action: (formData: FormData) => Promise<FormState>;
    children: React.ReactNode;
};

export default function AdminDeleteForm({ action, children }: AdminDeleteFormProps) {
    const [state, formAction] = useActionState(
        async (_previousState: FormState, formData: FormData) => action(formData),
        undefined,
    );

    return (
        <div>
            <form action={formAction}>{children}</form>
            {state?.error && <p className="mt-2 text-right text-xs text-rose-600">{state.error}</p>}
        </div>
    );
}
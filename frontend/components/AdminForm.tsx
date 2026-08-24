"use client";

import { useActionState } from "react";

type FormState = { error?: string } | void;
type AdminFormProps = {
    action: (formData: FormData) => Promise<FormState>;
    className?: string;
    children: React.ReactNode;
};

export default function AdminForm({ action, className, children }: AdminFormProps) {
    const [state, formAction] = useActionState(
        async (_previousState: FormState, formData: FormData) => action(formData),
        undefined,
    );

    return (
        <form action={formAction} className={className}>
            {state?.error && (
                <div className="mb-5 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
                    {state.error}
                </div>
            )}
            {children}
        </form>
    );
}

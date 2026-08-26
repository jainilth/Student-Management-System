"use client";

import { useActionState, useEffect, useRef } from "react";
import { usePathname } from "next/navigation";

type FormState = { error?: string } | void;
type AdminFormProps = {
    action: (formData: FormData) => Promise<FormState>;
    className?: string;
    children: React.ReactNode;
    preserveValuesOnError?: boolean;
};

export default function AdminForm({ action, className, children, preserveValuesOnError = false }: AdminFormProps) {
    const pathname = usePathname();
    const formRef = useRef<HTMLFormElement>(null);
    const submittedValuesRef = useRef<FormData | null>(null);
    const storageKey = `admin-form-values:${pathname}`;

    const [state, formAction] = useActionState(
        async (_previousState: FormState, formData: FormData) => action(formData),
        undefined,
    );

    useEffect(() => {
        if (!preserveValuesOnError || !state?.error || !formRef.current || !submittedValuesRef.current) {
            if (!preserveValuesOnError || !state?.error || !formRef.current) {
                return;
            }

            try {
                const raw = sessionStorage.getItem(storageKey);
                if (!raw) {
                    return;
                }

                const parsed = JSON.parse(raw) as Record<string, string[]>;
                const restored = new FormData();
                for (const [name, values] of Object.entries(parsed)) {
                    for (const value of values) {
                        restored.append(name, value);
                    }
                }
                submittedValuesRef.current = restored;
            } catch {
                return;
            }
        }

        const form = formRef.current;
        for (const [name, rawValue] of submittedValuesRef.current.entries()) {
            const value = String(rawValue);
            const element = form.elements.namedItem(name);
            if (!element) {
                continue;
            }

            if (element instanceof RadioNodeList) {
                for (let i = 0; i < element.length; i += 1) {
                    const item = element.item(i);
                    if (item instanceof HTMLInputElement && item.type === "radio") {
                        item.checked = item.value === value;
                    }
                }
                continue;
            }

            if (element instanceof HTMLInputElement || element instanceof HTMLSelectElement || element instanceof HTMLTextAreaElement) {
                if (element instanceof HTMLInputElement && element.type === "checkbox") {
                    element.checked = value === "on" || value === "true" || value === "1";
                } else {
                    element.value = value;
                }
            }
        }

        try {
            sessionStorage.removeItem(storageKey);
        } catch {
            // Ignore session storage errors.
        }
    }, [preserveValuesOnError, state, storageKey]);

    useEffect(() => {
        if (!preserveValuesOnError || !state || state.error) {
            return;
        }

        try {
            sessionStorage.removeItem(storageKey);
        } catch {
            // Ignore session storage errors.
        }
    }, [preserveValuesOnError, state, storageKey]);

    return (
        <form
            ref={formRef}
            action={formAction}
            className={className}
            onSubmit={(event) => {
                if (preserveValuesOnError) {
                    const submitted = new FormData(event.currentTarget);
                    submittedValuesRef.current = submitted;

                    try {
                        const payload: Record<string, string[]> = {};
                        for (const [name, rawValue] of submitted.entries()) {
                            const value = String(rawValue);
                            if (!payload[name]) {
                                payload[name] = [];
                            }
                            payload[name].push(value);
                        }
                        sessionStorage.setItem(storageKey, JSON.stringify(payload));
                    } catch {
                        // Ignore session storage errors.
                    }
                }
            }}
        >
            {state?.error && (
                <div className="mb-5 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
                    {state.error}
                </div>
            )}
            {children}
        </form>
    );
}

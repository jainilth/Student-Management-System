type ApiErrorBody = {
    message?: unknown;
    title?: unknown;
    errors?: unknown;
};

export function getApiErrorMessage(body: unknown, fallback: string): string {
    if (!body || typeof body !== "object") return fallback;

    const errorBody = body as ApiErrorBody;
    if (typeof errorBody.message === "string" && errorBody.message) return errorBody.message;
    if (typeof errorBody.title === "string" && errorBody.title) return errorBody.title;

    if (Array.isArray(errorBody.errors)) {
        const messages = errorBody.errors.filter(
            (error): error is string => typeof error === "string" && error.length > 0,
        );
        if (messages.length > 0) return messages.join(", ");
    }

    if (errorBody.errors && typeof errorBody.errors === "object") {
        const messages = Object.entries(errorBody.errors)
            .flatMap(([field, errors]) => {
                const fieldErrors = Array.isArray(errors) ? errors : [errors];
                return fieldErrors
                    .filter((error): error is string => typeof error === "string" && error.length > 0)
                    .map((error) => `${field}: ${error}`);
            });
        if (messages.length > 0) return messages.join(" ");
    }

    return fallback;
}

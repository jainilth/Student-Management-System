type ApiErrorBody = {
    message?: unknown;
    title?: unknown;
    errors?: unknown;
};

export function getApiErrorMessage(body: unknown, fallback: string): string {
    if (!body || typeof body !== "object") return toUserFriendlyFallback(fallback);

    const errorBody = body as ApiErrorBody;
    const message = typeof errorBody.message === "string" && errorBody.message
        ? errorBody.message
        : typeof errorBody.title === "string" && errorBody.title
            ? errorBody.title
            : undefined;
    let details: string | undefined;

    if (Array.isArray(errorBody.errors)) {
        const messages = errorBody.errors.filter(
            (error): error is string => typeof error === "string" && error.length > 0,
        );
        if (messages.length > 0) details = messages.join(", ");
    }

    if (!details && errorBody.errors && typeof errorBody.errors === "object") {
        const messages = Object.entries(errorBody.errors)
            .flatMap(([field, errors]) => {
                const fieldErrors = Array.isArray(errors) ? errors : [errors];
                return fieldErrors
                    .filter((error): error is string => typeof error === "string" && error.length > 0)
                    .map((error) => `${field}: ${error}`);
            });
        if (messages.length > 0) details = messages.join(" ");
    }

    if (details && message?.toLowerCase() === "validation failed") {
        return `Please correct: ${details}`;
    }

    if (message && details) return `${message} ${details}`;
    if (message?.toLowerCase() === "an unexpected error occurred.") {
        return "Something went wrong on the server. Please try again.";
    }
    return message || details || toUserFriendlyFallback(fallback);
}

function toUserFriendlyFallback(fallback: string): string {
    if (fallback.toLowerCase().includes("fetch")) {
        return "We couldn't load this information. Please try again.";
    }
    if (fallback.toLowerCase().includes("create")) {
        return "We couldn't create this record. Please check the fields and try again.";
    }
    if (fallback.toLowerCase().includes("update")) {
        return "We couldn't save your changes. Please check the fields and try again.";
    }
    if (fallback.toLowerCase().includes("delete")) {
        return "We couldn't delete this record. Please try again.";
    }
    return fallback;
}

export async function getApiErrorMessageFromResponse(
    response: Response,
    fallback: string,
): Promise<string> {
    const body = await response.json().catch(() => undefined);
    console.error("API request failed", {
        status: response.status,
        statusText: response.statusText,
        body,
    });
    return getApiErrorMessage(body, fallback);
}

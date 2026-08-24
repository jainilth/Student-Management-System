const numericFieldPattern = /(?:Id|Year|Years|Semesters|Held|Attended|Point|Marks|Credits|Score|Size)$/;
const numericFieldNames = new Set(["semesterNumber"]);
const booleanFields = new Set(["isActive", "isActivate"]);

type AdminPayloadValue = string | number | boolean;

export function getAdminPayload(formData: FormData): Record<string, AdminPayloadValue> {
    const payload: Record<string, AdminPayloadValue> = {};

    for (const [name, value] of formData.entries()) {
        if (typeof value !== "string") continue;
        if (booleanFields.has(name)) {
            payload[name] = value === "on" || value === "true";
            continue;
        }
        if (numericFieldNames.has(name) || numericFieldPattern.test(name)) {
            payload[name] = Number(value);
            continue;
        }
        payload[name] = value;
    }

    for (const name of booleanFields) {
        if (!formData.has(name)) payload[name] = false;
    }

    return payload;
}

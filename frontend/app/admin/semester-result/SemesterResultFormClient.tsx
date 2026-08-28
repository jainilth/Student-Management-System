"use client";

import AdminForm from "@/components/AdminForm";
import Link from "next/link";
import { useState } from "react";

type SemesterResultFormClientProps = {
    initialData: Record<string, any>;
    onSubmitAction: (formData: FormData) => Promise<void | { error?: string }>;
    mode: "create" | "edit";
    studentSemesters: Record<string, any>[];
    semesterSubjects: Record<string, any>[];
};

const inputClass =
    "mt-2 block w-full rounded-lg border border-slate-200 bg-white px-3 py-2.5 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-600 focus:ring-2 focus:ring-emerald-100";

export default function SemesterResultFormClient({
    initialData,
    onSubmitAction,
    mode,
    studentSemesters,
    semesterSubjects,
}: SemesterResultFormClientProps) {
    const editing = mode === "edit";
    const initialStudentSemesterId = String(initialData.studentSemesterId ?? "");
    const [studentSemesterId, setStudentSemesterId] = useState(initialStudentSemesterId);
    const selectedStudentSemester = studentSemesters.find(
        (record) => String(record.studentSemesterId) === studentSemesterId,
    );
    const totalCredits = selectedStudentSemester
        ? semesterSubjects
            .filter(
                (record) =>
                    Number(record.programId) === Number(selectedStudentSemester.academicProgramId) &&
                    Number(record.semesterId) === Number(selectedStudentSemester.semesterId),
            )
            .reduce((total, record) => total + Number(record.credits || 0), 0)
        : "";

    return (
        <AdminForm
            action={onSubmitAction}
            preserveValuesOnError={!editing}
            className="space-y-6 rounded-xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8"
        >
            <div className="grid gap-5 sm:grid-cols-2">
                <label className="text-sm font-medium text-slate-700">
                    Student semester
                    <select
                        className={inputClass}
                        name="studentSemesterId"
                        required
                        value={studentSemesterId}
                        onChange={(event) => setStudentSemesterId(event.target.value)}
                    >
                        <option value="" disabled>
                            Select student semester
                        </option>
                        {studentSemesters.map((record) => (
                            <option key={record.studentSemesterId} value={record.studentSemesterId}>
                                {[record.studentName, record.academicProgramName, record.semesterName]
                                    .filter(Boolean)
                                    .join(" - ") || `Record ${record.studentSemesterId}`}
                            </option>
                        ))}
                    </select>
                </label>
                <label className="text-sm font-medium text-slate-700">
                    SGPA
                    <input className={inputClass} name="sgpa" type="number" step="any" defaultValue={editing ? initialData.sgpa ?? "" : ""} />
                </label>
                <label className="text-sm font-medium text-slate-700">
                    Total credits
                    <input className={inputClass} name="totalCredits" type="number" step="any" value={totalCredits} readOnly />
                </label>
                <label className="text-sm font-medium text-slate-700">
                    Earned credits
                    <input className={inputClass} name="earnedCredits" type="number" step="any" defaultValue={editing ? initialData.earnedCredits ?? "" : ""} />
                </label>
                <label className="text-sm font-medium text-slate-700">
                    Result status
                    <input className={inputClass} name="resultStatus" type="text" defaultValue={editing ? initialData.resultStatus ?? "" : ""} />
                </label>
            </div>
            <div className="flex justify-end gap-3">
                <Link href="/admin/semester-result" className="rounded-lg border border-slate-200 px-4 py-2.5 text-sm font-semibold text-slate-600">
                    Cancel
                </Link>
                <button className="rounded-lg bg-emerald-950 px-4 py-2.5 text-sm font-semibold text-white hover:bg-emerald-900" type="submit">
                    {editing ? "Save changes" : "Create record"}
                </button>
            </div>
        </AdminForm>
    );
}
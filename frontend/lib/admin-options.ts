import { GetAllAcademicPrograms } from "@/service/academicProgram.service";
import { GetAllAcademicYears } from "@/service/academicYear.service";
import { GetAllClassSessions } from "@/service/classSession.service";
import { GetAllDepartments } from "@/service/department.service";
import { GetAllFacultys } from "@/service/faculty.service";
import { GetAllGrades } from "@/service/grade.service";
import { GetAllProjects } from "@/service/project.service";
import { GetAllProjectAllocations } from "@/service/projectAllocation.service";
import { GetAllRoles } from "@/service/role.service";
import { GetAllSemesters } from "@/service/semester.service";
import { GetAllSemesterResults } from "@/service/semesterResult.service";
import { GetAllSemesterSubjects } from "@/service/semesterSubject.service";
import { GetAllStudents } from "@/service/student.service";
import { GetAllStudentSemesters } from "@/service/studentSemester.service";
import { GetAllSubjects } from "@/service/subject.service";
import { GetAllUsers } from "@/service/user.service";

export type AdminOption = { value: number; label: string };
type RecordData = Record<string, any>;

const optionSources: Record<string, () => Promise<any>> = {
  AcademicProgram: GetAllAcademicPrograms,
  AcademicYear: GetAllAcademicYears,
  ClassSession: GetAllClassSessions,
  Department: GetAllDepartments,
  Faculty: GetAllFacultys,
  Grade: GetAllGrades,
  Project: GetAllProjects,
  ProjectAllocation: GetAllProjectAllocations,
  Role: GetAllRoles,
  Semester: GetAllSemesters,
  SemesterResult: GetAllSemesterResults,
  SemesterSubject: GetAllSemesterSubjects,
  Student: GetAllStudents,
  StudentSemester: GetAllStudentSemesters,
  Subject: GetAllSubjects,
  User: GetAllUsers,
};

const optionFields: Record<string, { id: string; labels: string[] }> = {
  AcademicProgram: { id: "programId", labels: ["programName", "programCode"] },
  AcademicYear: { id: "academicYearId", labels: ["year"] },
  ClassSession: { id: "sessionId", labels: ["topic", "sessionDate"] },
  Department: {
    id: "departmentId",
    labels: ["departmentName", "departmentCode"],
  },
  Faculty: {
    id: "facultyId",
    labels: ["userName", "employeeNumber", "designation"],
  },
  Grade: { id: "gradeId", labels: ["gradeCode", "gradeName"] },
  Project: { id: "projectId", labels: ["title"] },
  ProjectAllocation: {
    id: "allocationId",
    labels: ["projectTitle", "studentName", "status"],
  },
  Role: { id: "roleId", labels: ["roleName"] },
  Semester: { id: "semesterId", labels: ["semesterName", "semesterNumber"] },
  SemesterResult: {
    id: "semesterResultId",
    labels: ["studentName", "resultStatus"],
  },
  SemesterSubject: {
    id: "semesterSubjectId",
    labels: ["subjectName", "programName", "semesterName"],
  },
  Student: { id: "studentId", labels: ["userName", "enrollmentNumber"] },
  StudentSemester: {
    id: "studentSemesterId",
    labels: ["studentName", "semesterName", "academicYear"],
  },
  Subject: { id: "subjectId", labels: ["subjectCode", "subjectName"] },
  User: { id: "userId", labels: ["userName", "email"] },
};

export async function getAdminOptions(entity: string): Promise<AdminOption[]> {
  const source = optionSources[entity];
  const fields = optionFields[entity];
  if (!source || !fields) return [];

  const response = await source();
  const records = Array.isArray(response?.data)
    ? response.data
    : Array.isArray(response)
      ? response
      : [];
  return records
    .map((record: RecordData) => ({
      value: Number(record[fields.id]),
      label:
        fields.labels
          .map((field) => record[field])
          .find(
            (value) => value !== undefined && value !== null && value !== "",
          )
          ?.toString() || `Record ${record[fields.id]}`,
    }))
    .filter((option: AdminOption) => Number.isFinite(option.value));
}

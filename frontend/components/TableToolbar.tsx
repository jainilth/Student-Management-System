import { Search, Filter, Download } from "lucide-react";

export default function TableToolbar() {
  return (
    <div className="flex flex-col gap-4 border-b border-slate-100 px-5 py-4 sm:flex-row sm:items-center sm:justify-between">
      <div className="relative">
        <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
        <input 
          type="text" 
          placeholder="Search..." 
          className="h-10 w-full rounded-lg border border-slate-200 bg-slate-50 pl-10 pr-4 text-sm text-slate-900 outline-none transition focus:border-emerald-500 focus:bg-white focus:ring-2 focus:ring-emerald-100 sm:w-64"
        />
      </div>
      <div className="flex items-center gap-3">
        <button type="button" className="flex items-center gap-2 rounded-lg border border-slate-200 bg-white px-4 py-2 text-sm font-medium text-slate-700 transition hover:bg-slate-50">
          <Filter className="h-4 w-4" />
          Status Filter
        </button>
        <button type="button" className="flex items-center gap-2 rounded-lg border border-slate-200 bg-white px-4 py-2 text-sm font-medium text-slate-700 transition hover:bg-slate-50">
          <Download className="h-4 w-4" />
          Export
        </button>
      </div>
    </div>
  );
}

const fs = require('fs');
const path = require('path');
const root = path.join('d:/jainil/Projects/Student-Management-System', 'frontend', 'app', 'admin');
function toPascalCase(input) {
  return input
    .replace(/[-_]/g, ' ')
    .replace(/([a-z])([A-Z])/g, ' ')
    .split(/\s+/)
    .filter(Boolean)
    .map((s) => s.charAt(0).toUpperCase() + s.slice(1))
    .join('');
}
const entities = fs.readdirSync(root, { withFileTypes: true })
  .filter((d) => d.isDirectory())
  .map((d) => d.name)
  .filter((name) => {
    const createPage = path.join(root, name, 'create', 'page.tsx');
    const editPage = path.join(root, name, 'edit', '[id]', 'page.tsx');
    const listPage = path.join(root, name, 'page.tsx');
    return fs.existsSync(createPage) && fs.existsSync(editPage) && fs.existsSync(listPage);
  });
for (const entity of entities) {
  const pascal = toPascalCase(entity);
  const createPath = path.join(root, entity, 'create', 'page.tsx');
  const editPath = path.join(root, entity, 'edit', '[id]', 'page.tsx');
  const listPath = path.join(root, entity, 'page.tsx');
  let createContent = fs.readFileSync(createPath, 'utf8');
  let editContent = fs.readFileSync(editPath, 'utf8');
  let listContent = fs.readFileSync(listPath, 'utf8');
  const entityNameMatch = createContent.match(/entityName=\"([^\"]+)\"/);
  const redirectPathMatch = createContent.match(/redirectPath=\"([^\"]+)\"/);
  const entityName = entityNameMatch ? entityNameMatch[1] : pascal;
  const redirectPath = redirectPathMatch ? redirectPathMatch[1] : /admin/;
  const formComponentPath = path.join(root, entity, ${pascal}Form.tsx);
  const formComponentContent = import EntityForm, { FormField } from '@/components/EntityForm';\n\ninterface FormProps {\n    fields: FormField[];\n    initialData?: any;\n    onSubmitAction: (formData: FormData) => Promise<any>;\n}\n\nexport default function Form({ fields, initialData, onSubmitAction }: FormProps) {\n    return (\n        <EntityForm\n            fields={fields}\n            initialData={initialData}\n            entityName=\"\"\n            onSubmitAction={onSubmitAction}\n            redirectPath=\"\"\n        />\n    );\n}\n;
  fs.writeFileSync(formComponentPath, formComponentContent, 'utf8');
  createContent = createContent
    .replace("import EntityForm from '@/components/EntityForm';", import Form from '../Form';)
    .replace(/<EntityForm/g, <Form)
    .replace(/^\s*entityName=\"[^\"]+\"\n/m, '')
    .replace(/^\s*redirectPath=\"[^\"]+\"\n/m, '');
  editContent = editContent
    .replace("import EntityForm from '@/components/EntityForm';", import Form from '../../Form';)
    .replace(/<EntityForm/g, <Form)
    .replace(/^\s*entityName=\"[^\"]+\"\n/m, '')
    .replace(/^\s*redirectPath=\"[^\"]+\"\n/m, '');
  fs.writeFileSync(createPath, createContent, 'utf8');
  fs.writeFileSync(editPath, editContent, 'utf8');
  const columnsMatch = listContent.match(/const columns = \[[\s\S]*?\n\s*\];/);
  const basePathMatch = listContent.match(/basePath=\"([^\"]+)\"/);
  const listEntityNameMatch = listContent.match(/entityName=\"([^\"]+)\"/);
  if (columnsMatch) {
    const columnsBlock = columnsMatch[0];
    const listBasePath = basePathMatch ? basePathMatch[1] : /admin/;
    const listEntityName = listEntityNameMatch ? listEntityNameMatch[1] : entityName;
    const listComponentPath = path.join(root, entity, ${pascal}List.tsx);
    const listComponentContent = import EntityList from '@/components/EntityList';\n\ninterface ListProps {\n    data: any[];\n}\n\n\n\nexport default function List({ data }: ListProps) {\n    return (\n        <EntityList\n            data={data}\n            columns={columns}\n            entityName=\"\"\n            basePath=\"\"\n        />\n    );\n}\n;
    fs.writeFileSync(listComponentPath, listComponentContent, 'utf8');
    listContent = listContent
      .replace("import EntityList from '@/components/EntityList';", import List from './List';)
      .replace(columnsBlock + '\n\n', '')
      .replace(/<EntityList[\s\S]*?\/>/, <List\n            data={Array.isArray(data) ? data : []}\n        />);
    fs.writeFileSync(listPath, listContent, 'utf8');
  }
}
console.log(Refactored  admin entities.);

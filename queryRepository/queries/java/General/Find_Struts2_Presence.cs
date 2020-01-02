CxList imports = base.Find_Import();

result = imports.FindByName("*.struts2*");
result.Add(imports.FindByName("*.xwork2*"));
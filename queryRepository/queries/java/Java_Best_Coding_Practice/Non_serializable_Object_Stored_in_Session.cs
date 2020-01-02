// All possible setAttribute methods - to session and to context
CxList setAttr = Set_Session_Attribute();
setAttr.Add(Set_Context_Attribute());

// Serializable classes
CxList serializable = All.InheritsFrom("Serializable");

// The data attribute of set attribute. We remove the Param type, since it has no relevant implementation
CxList setAttrParam = All.GetParameters(setAttr, 1);
setAttrParam -= setAttrParam.FindByType(typeof(Param));
setAttrParam -= setAttrParam.FindByType(typeof(StringLiteral));
setAttrParam -= setAttrParam.FindByMemberAccess("ResultSet.getString");

// All references of the parameter of setAttr
CxList setAttrParamRef = All.FindAllReferences(setAttrParam);
setAttrParamRef = All.GetByAncs(setAttrParamRef);

// Get all references of the serializable classes, and look at their father (to find declarators)
CxList paramDefinition = All.FindAllReferences(serializable).GetFathers();
// Find the relevant Declarators, when exist
CxList serializableParamDef = All.GetByAncs(paramDefinition).FindByType(typeof(Declarator));

// Leave only parameters that are not serializable (if we wouldn't have removed the "Param"
// type, we would have to remove it now)
setAttrParam -= setAttrParam.FindAllReferences(serializableParamDef);

CxList classDecl = Find_ClassDecl();
CxList nonSerializable = classDecl - serializable;
CxList attrParamDef = nonSerializable.FindDefinition(setAttrParam);

result = setAttrParam.FindAllReferences(attrParamDef);
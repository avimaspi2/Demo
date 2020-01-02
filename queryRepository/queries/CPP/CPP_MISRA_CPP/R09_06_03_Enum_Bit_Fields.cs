/*
MISRA CPP RULE 9-6-3
------------------------------
This query searches for bit fields of type enum

	The Example below shows code with vulnerability: 

		struct a
        {
            enum temp{x,y,z};
            temp bf:1;          //non-compliant
        }

*/

CxList decls=All.FindByType(typeof(FieldDecl));
CxList tr = All.FindByType(typeof(TypeRef));
CxList enumDecls= All.FindByType(typeof(ClassDecl)).GetClass(All.FindByType(typeof(EnumMemberDecl)));
CxList potentialBitFields = tr.FindAllReferences(enumDecls).GetFathers()*decls;

result = potentialBitFields.FindByRegex(@"[\}|\w]+?\s*?:\s*?\d+?\s*?;", false, false,false);
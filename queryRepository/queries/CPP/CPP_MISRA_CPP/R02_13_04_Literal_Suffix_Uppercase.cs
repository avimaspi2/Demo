/*
 MISRA CPP RULE 2-13-4
 ------------------------------
 This query searches for literal suffixes in undercase.
 
 The Example below shows code with vulnerability: 

      int main() 
		{
			int a = 0U;
			a = 0u;     //Non-compliant
			a = 0L;
			a = 0l;     //Non-compliant
			a = 0UL;
			a = 0Ul;    //Non-compliant
			a = 0uL;    //Non-compliant
			a = 0x12bU;
			a = 0x12bu; //Non-compliant
			float b = 1.2F;
			b = 2.4f;   //Non-compliant
			b = 1.2L;
			b = 2.4l;   //Non-compliant
		};

*/

CxList findInts = All.FindByRegex(@"\d+[b]?[UL]?[ul][UL]?\W", false, false,false);
findInts = findInts.FindByType(typeof(IntegerLiteral));
CxList findFloats = All.FindByRegex(@"\d+\.?\d*[fl]\W", false, false,false);
findFloats = findFloats.FindByType(typeof(RealLiteral));
result = findFloats + findInts;
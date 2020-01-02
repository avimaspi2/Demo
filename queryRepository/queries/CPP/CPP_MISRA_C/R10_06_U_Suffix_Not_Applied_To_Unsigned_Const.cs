/*
MISRA C RULE 10-6
------------------------------
This query searches for unsigned constants without the 'U' suffix

	The Example below shows code with vulnerability: 

use_uint64 ( 9223372036854775808 );
use_uint32 ( 0x80000000 );

*/

// System definitions
const int INT_BITS = 32;
const int LONG_BITS = 64;

// The limits on range of int and long, based on system definition
ulong MAX_SIGNED_INT = (ulong)Math.Pow(2, INT_BITS-1)-1;
ulong MAX_UNSIGNED_INT = (ulong)Math.Pow(2, INT_BITS)-1;
ulong MAX_SIGNED_LONG = (ulong) Math.Pow(2, LONG_BITS-1)-1;
ulong MAX_UNSIGNED_LONG = (ulong) Math.Pow(2, LONG_BITS)-1;

// decimal literals with no 'U' or 'u' suffix
CxList decimalLiterals = All.NewCxList();
CxList temp = All.FindByRegex(@"[^\w][1-9][0-9]*(L|l)?[^\wuU]", false, false, false, decimalLiterals);

// octal literals with no 'U' or 'u' suffix
CxList octalLiterals = All.NewCxList();
temp = All.FindByRegex(@"[^\w]0[0-7]*(L|l)?[^\wuU]", false, false, false, octalLiterals);

// hexadecimal literals with no 'U' or 'u' suffix
CxList hexaLiterals = All.NewCxList();
temp = All.FindByRegex(@"[^\w]0[x|X][0-9,a-fA-F]+(L|l)?[^G-Za-z0-9_uU]", false, false, false, hexaLiterals);

hexaLiterals.Add(octalLiterals);

CxList resultsText = All.NewCxList();

// go over the various found numeric literals, check if the underlying type is unsigned.

// add decimals
foreach (CxList cur in decimalLiterals){
	try{
		string str = (cur.data.GetByIndex(0) as CSharpGraph).FullName;
		str = str.Substring(1, str.Length - 2);
		char lastChar = str[str.Length - 1];
		if ((lastChar == 'L') || (lastChar == 'l')){
			str = str.Substring(0, str.Length - 1);
		}
		ulong number = Convert.ToUInt64(str);
		
		if (number > MAX_SIGNED_LONG){
			resultsText.Add(cur);
			continue;
		}
	}
	catch(Exception e){}	
}

// add octals/hexas
foreach (CxList cur in (hexaLiterals)){
	try{
		string str = (cur.data.GetByIndex(0) as CSharpGraph).FullName;
		str = str.Substring(3, str.Length - 4);
		char lastChar = str[str.Length - 1];
		if ((lastChar == 'L') || (lastChar == 'l')){
			str = str.Substring(0, str.Length - 1);
		}
		long numberLong = Int64.Parse(str, System.Globalization.NumberStyles.HexNumber);
		if (numberLong < 0){
			resultsText.Add(cur);
			continue;
		}
		if (!((lastChar == 'L') || (lastChar == 'l'))){
			ulong number = ((ulong) numberLong);
			if ((lastChar != 'L') && (lastChar != 'l')){
				if ((number > MAX_SIGNED_INT) && (number <= MAX_UNSIGNED_INT)){
					resultsText.Add(cur);
					continue;
				}
			}
		}
	}
	catch(Exception e){}	
}

result = All.FindByRegexSecondOrder(".", resultsText);
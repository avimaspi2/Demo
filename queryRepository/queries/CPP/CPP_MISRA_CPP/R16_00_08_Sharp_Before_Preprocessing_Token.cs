/* MISRA CPP RULE 16-0-8
 ------------------------------
 This query ensures that if the # token appears as the first token in line , then it
 shall be immediately followed by a preprocessing token.It also makes sure that  #else and #endif
 not followed by anything but spaces.
 
 The Example below shows code with vulnerability: 

      inf foo()
      {
           int x;
           #define X 8  //compliant
           # ifndef AAA //non-compliant - space after #
                 x=1;
           #else1       //non-compliant - non standard preprocessing token.
                 x=2;
           #endif a     //non-compliant - endif followed by "a" character.
      }
*/

// checks if there is an appearance of # at the beginning of the line which is not directly followed by a preprocessor token
// and in case of "endif" and "else" checks if there are only spaces and new lines following the directive.

string regexStr = @"^[\t ]*#(?!include\s|define\s|undef\s|ifndef\s|endif\s*\n+|else\s*\n+|elif\s|ifdef\s|if\s|line\s|error\s|pragma\s)";
List<string> cppExtensions = new List<string> {"*.cpp", "*.c", "*.cc", "*.cxx", "*.c++", "*.ec", "*.h", "*.hh", "*.hxx", "*.h++", "*.hpp"};

foreach(string extension in cppExtensions)
{
	result.Add(All.FindByRegexExt(regexStr, extension));
}
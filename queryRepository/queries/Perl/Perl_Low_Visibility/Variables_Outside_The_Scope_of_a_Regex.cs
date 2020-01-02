/*
source: https://www.securecoding.cert.org/confluence/display/perl/STR30-PL.+Do+not+use+capture+variables+outside+the+scope+of+a+regex

Perl's capture variables ($1, $2, etc.) are assigned the values of capture expressions after
a regular expression (regex) match has been found. If a regex fails to find a match, the contents
of the capture variables can remain undefined. The perlre manpage [Wall 2011] contains this note:
  NOTE: Failed matches in Perl do not reset the match variables, which makes it easier to write code that
        tests for a series of more specific cases and remembers the best match.

Consequently, the value of a capture variable can be indeterminate if a previous regex failed.
Always ensure that a regex was successful before reading its capture variables.

A regular exression should be checked for success before the capture variables are accessed.

Bad example:
  $data =~ /\[(\d*)\].+/;  # this regex will fail
  $time = 1;
  print "time is $time\n";

---

Good example:
  if ($data =~ /\[(\d*)\].+/) {  # this regex will fail
    $time = 1;
    print "time is $time\n";
  }
*/

CxList regex = Find_Regex();

// All the methods where we test regex
CxList regexMethods = All.GetMethod(regex);

// Find if statements that contain a test on the regexes
CxList testRegex = regex.GetByAncs(Find_Conditions());

// Remove the cases where we test when regexes fail before using their $1, $2 etx.
CxList not = testRegex.GetByAncs(All.FindByShortName("Not"));
not.Add(regex.FindByFathers(All.FindByType(typeof(IfStmt))).FindByRegex("unless")); // unless issues
regexMethods -= regexMethods.GetMethod(not);

// Get the if stmt and the while loop statement
testRegex = testRegex.GetAncOfType(typeof(IfStmt)) + testRegex.GetAncOfType(typeof(IterationStmt));

// Find variables
CxList dollar = All.FindByName("$*");
dollar = dollar.GetByAncs(regexMethods); // relevant only when we have regexes, not any parameters
CxList variables = dollar.FindByName("$1*");
variables.Add(dollar.FindByName("$2*"));
variables.Add(dollar.FindByName("$3*"));
variables.Add(dollar.FindByName("$4*"));
variables.Add(dollar.FindByName("$5*"));
variables.Add(dollar.FindByName("$6*"));
variables.Add(dollar.FindByName("$7*"));
variables.Add(dollar.FindByName("$8*"));
variables.Add(dollar.FindByName("$9*"));


// Find all regex variables that are not tested
result = variables - variables.GetByAncs(testRegex);

// Every Param has its value, so we don't need to return both of them
result -= result.FindByType(typeof(Param));
/*

see: https://www.securecoding.cert.org/confluence/display/perl/DCL30-PL.+Do+not+import+deprecated+modules

Deprecated
----------
Class::ISA
Pod::Plainer
Shell
Switch
Universal::isa
Universal::can
Universal::VERSION
File::PathConvert
*/

CxList imports = All.FindByType(typeof(Import));

result = 
	imports.FindByRegex("CLASS").FindByRegex(@"\(ISA\)") +
	imports.FindByRegex("POD").FindByRegex(@"\(Plainer\)") +
	imports.FindByRegex("Shell") +
	imports.FindByRegex("Switch") +
	imports.FindByRegex("UNIVERSAL").FindByRegex(@"\(isa\)") +
	imports.FindByRegex("UNIVERSAL").FindByRegex(@"\(can\)") +
	imports.FindByRegex("UNIVERSAL").FindByRegex(@"\(VERSION\)") +
	imports.FindByRegex("File").FindByRegex(@"\(PathConvert\)") +

	imports.FindByRegex("CLASS::ISA") +
	imports.FindByRegex("POD::Plainer") +
	imports.FindByRegex("UNIVERSAL::isa") +
	imports.FindByRegex("UNIVERSAL::can") +
	imports.FindByRegex("UNIVERSAL::VERSION") +
	imports.FindByRegex("File::PathConvert");
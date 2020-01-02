List<string> timePkgConstMembers = new List<string> {
	"Microsecond", "Millisecond", "Second", "Minute", "Hour",						// Time
	"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday",	// Days
	"January", "February", "March", "April", "May", "June",
	"July", "August", "September", "October", "November", "December",				// Months
	"Local", "UTC"																	// Read-only variables
	};
result = All.FindByMemberAccess("\"time\".*").FindByShortNames(timePkgConstMembers);
result = Find_Methods().FindByShortNames(new List<string>{
	"exec",     //exec
	"syscall",  // syscall
	"system",   // system && backticks
	"open",     // open
	"qx",       // qx
	"readpipe"  // readpipe
});
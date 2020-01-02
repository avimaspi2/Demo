/* 
 * This query will return valid nodes that will be used as sanitizers when finding
 *  good Salts, that are slices with at least a length of 32 and with random data
 *  from the built in go randomizer named "Read" from crypto/rand.
 *
 * The query finds flows from the creation of an slice with at least that size until
 *  the Read method. It will return the last node of the flow to be used as sanitizer.
*/

// Finding slices with length>32
// For now we only search for slices from make function.
// We should improve this with, for example, situations like the following one, where
//  evaluate the lenght:
//     var arraysalt = [5]byte{1, 5, 2, 3, 7}
//     salt := arraysalt[:]
CxList slices = Find_Not_In_Range("make", 0, 0, 31); //position needs to be 1!

// Finding arrays beeing filled with random bytes:
CxList randMethods = All.FindByMemberAccess("\"crypto/rand\".Read");

// Get flows that prove the creation of good arrays with random bytes, and return the last node:
CxList saltFlows = randMethods
	.InfluencedBy(slices)
	.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

// Return all "make" fuctions that start the founded flows.
CxList makeParameter = saltFlows.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);

result = Find_Methods().FindByParameters(makeParameter).GetAssignee();
/*
 * This query finds potential Denial of Service on the following situations:
 *  - If theres an input that can affect a loop, this can cause big loops, that 
 *   	may be problematic when allocating memory for example, like being out of memory;
 *  - If there are pointers that can be influenced by inputs, that situation may
 *      potential problems of memory access for that instance, ie accessing OS mem
 *      will do a segmentation violation. 
 *  - If there are array or slices access that may be influenced by inputs, they will
 *      be marked as result because there's a potencial of buffer overflow
 *  - If there's a DoS by sleep, a situation when an input is influencing a time.sleep
 *      and the program may be sleeping for a while. 
*/

CxList inputs = Find_Inputs();
CxList sinkNodes = All.NewCxList();

/* Find all situations where the number of iterations is influenced by inputs. */
CxList conditions = Find_Loop_Conditions();
sinkNodes.Add(conditions);

/* Find all situations where an input is influencing an unsafe Pointer */
CxList unsafePointers = All.FindByMemberAccess("\"unsafe\".Pointer");
CxList unsafePointersParams = All.GetParameters(unsafePointers);
sinkNodes.Add(unsafePointers);

/* Find all accesses to array or slice positions*/
sinkNodes.Add(Find_Array_Indexes());

/* Find DoS By sleep*/
CxList sleepMethod = All.FindByMemberAccess("\"time\".Sleep");
CxList sleepMethodParams = All.GetParameters(sleepMethod);
sinkNodes.Add(sleepMethodParams);


/*** Create the possible flows: ***/ 
result.Add(inputs.InfluencingOn(sinkNodes).ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow));
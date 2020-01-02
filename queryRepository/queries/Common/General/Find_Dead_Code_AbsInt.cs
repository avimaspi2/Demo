result = Find_Dead_Blocks_From_Conditions();
result.Add(Find_Unused_Private_Methods());
result.Add(Find_Code_After_Return());
result.Add(Find_Unreached_Switch_Case());
result.Add(Find_Catch_Block_Of_Empty_Try());
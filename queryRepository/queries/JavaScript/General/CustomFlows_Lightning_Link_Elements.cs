if(Lightning_Find_Aura_Cmp_And_App().Count > 0)
{
	cxXPath.AddSupportForExpressionLanguageForFramework("Lightning");
	Lightning_Link_Intercomponent_flow();
	Lightning_Connect_Attributes_To_Usage();
	
	Lightning_Add_Flows_To_Iteration();
	Lightning_Map_Component_To_Getter();
	
	Lightning_Map_Setter_To_Cmp();
	Lightning_Link_Events();
	Lightning_Map_Ss_To_Cs_Response();   
}
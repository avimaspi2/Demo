CxList allComponent = Lightning_Find_Aura_Cmp_And_App();
const int EXT_SIZE = 4;
const string CONTROLLER="Controller";
const string JS_EXT=".js";
const string PATH_SEP="\\";
Dictionary<string,string> mapComponentToController = new Dictionary<string,string>();
Dictionary<string,string> mapControllerToComponent = new Dictionary<string,string>();
foreach(CxList cmp in allComponent)
{
	string fileName = cmp.GetFirstGraph().LinePragma.FileName;
	string folderPath = fileName.Remove(fileName.LastIndexOf(PATH_SEP));
	string pathWithoutExt = fileName.Remove(fileName.Length - EXT_SIZE, EXT_SIZE);
	string onlyName = pathWithoutExt.Remove(0, pathWithoutExt.LastIndexOf(PATH_SEP) + 1);
	string controllerPath = folderPath +PATH_SEP+ onlyName + CONTROLLER + JS_EXT;
	
	fileName = fileName.ToLower();
	controllerPath = controllerPath.ToLower();
	
	if(!mapComponentToController.ContainsKey(fileName))
	{
		mapComponentToController.Add(fileName, controllerPath);
	}
	if(!mapControllerToComponent.ContainsKey(controllerPath))
	{
		mapControllerToComponent.Add(controllerPath, fileName);
	
	}
}
querySharedData.AddSharedData("Lightning_Cmp_To_Cont", mapComponentToController, true);
querySharedData.AddSharedData("Lightning_Cont_To_Cmp", mapControllerToComponent, true);
List<string> inputAnnotations = new List<string> {
		"RequestMapping",
		"RequestBody",
		"ModelAttribute",
		"RequestPart",
		"MessageMapping",
		"SendToUser",
		"SendTo",
		"SubscribeMapping",
		"PostMapping",
		"PutMapping",
		"DeleteMapping",
		"PatchMapping",
		"GetMapping"

		};
CxList controllers = Find_Controllers();

CxList customAttribute = Find_CustomAttribute();
	
CxList customAttributes = customAttribute.GetByAncs(controllers);

CxList spring_inputs = All.GetParameters(customAttributes.FindByShortNames(inputAnnotations).GetFathers());

spring_inputs -= spring_inputs.FindByType("Model*");
spring_inputs -= spring_inputs.FindByType("BindingResult");
spring_inputs -= spring_inputs.FindByType("Errors");
spring_inputs -= spring_inputs.FindByType("*session*");
spring_inputs -= spring_inputs.FindByType("*response*");

CxList springHeaders = customAttribute.FindByCustomAttribute("RequestHeader").GetFathers();

spring_inputs.Add(springHeaders);

result = spring_inputs;
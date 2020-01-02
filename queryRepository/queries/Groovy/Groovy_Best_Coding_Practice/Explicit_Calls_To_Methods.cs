// Find explicit calls to methods that's can be replaced in pure Groovy simple syntax
CxList method_calls = Find_Methods();

result = method_calls.FindByName("*.compareTo") -
	method_calls.FindByName("super.compareTo") + 
	method_calls.FindByName("*.equals") -
	method_calls.FindByName("super.equals") + 
	method_calls.FindByName("*.getAt") -
	method_calls.FindByName("super.getAt") + 
	method_calls.FindByName("*.leftShift") -
	method_calls.FindByName("super.leftShift") + 
	method_calls.FindByName("*.rightShift") -
	method_calls.FindByName("super.rightShift") + 	
	method_calls.FindByName("*.minus") -
	method_calls.FindByName("super.minus") + 
	method_calls.FindByName("*.multiply") -
	method_calls.FindByName("super.multiply") + 
	method_calls.FindByName("*.div") -
	method_calls.FindByName("super.div") +
	method_calls.FindByName("*.mod") -
	method_calls.FindByName("super.mod") + 
	method_calls.FindByName("*.or") -
	method_calls.FindByName("super.or") + 
	method_calls.FindByName("*.plus") -
	method_calls.FindByName("super.plus") + 
	method_calls.FindByName("*.power") -
	method_calls.FindByName("super.power") + 
	method_calls.FindByName("*.xor") -
	method_calls.FindByName("super.xor");
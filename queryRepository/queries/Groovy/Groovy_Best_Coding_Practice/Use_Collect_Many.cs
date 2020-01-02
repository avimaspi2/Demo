// In many case collectMany() yields the same result as collect.flatten().
// It is easier to understand and more clearly conveys the intent.
CxList method_calls = Find_Methods().FindByName("*.collect").GetFathers();
CxList method_calls_flatten = Find_Methods().FindByName("*.flatten");

CxList list = All.NewCxList(); 

foreach(CxList item in method_calls_flatten)
{
	if ((item.GetFathers() * method_calls).Count > 0) // there is a common father 
	{												  // which means that flatten is called after collect
		list.Add(item);
	}
}

result = list;
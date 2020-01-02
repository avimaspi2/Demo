//Classes implementing Map interface
string[] collections = new string[]{
	"Map",
	"MultiMap",
	"ImmutableMapAdaptor",
	"SynchronizedMap",	
	"LinkedHashMap",
	"ObservableMap",
	"OpenHashMap",
	"WeakHashMap",		
	"HashMap", 
	"ListMap",
	"SortedMap",
	"TreeMap",	
	"AbstractMap",
	"ConcurrentHashMap",
	"ConcurrentSkipListMap",
	"Hashtable",
	"IdentityHashMap",
	}; 

result = All.FindByTypes(collections) + All.FindByType(typeof(DictionaryCreateExpr));
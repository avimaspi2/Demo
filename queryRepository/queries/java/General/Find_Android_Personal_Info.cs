////////////////////////////////////////////////////////////////
// Find personal information in Android environment
// Geolocation, contacts, SMS messages, IMEI,
// call log, calendar
////////////////////////////////////////////////////////////////

// Device ID (IMEI):
CxList imei = All.FindByMemberAccess("TelephonyManager.getDeviceId");

// GPS location
CxList GPSInfo = All.FindByMemberAccess("Location.getLatitude");
GPSInfo.Add(All.FindByMemberAccess("Location.getLongitude"));

// Contacts
CxList getStrCursor = All.FindByMemberAccess("Cursor.getString");

CxList peopleInParam = All.FindByName("*Contacts.People*");
peopleInParam.Add(All.FindByShortName("ContactsContract"));

CxList contactInfoInput = peopleInParam.GetByAncs(getStrCursor);

// SMS Messages, Call log information, Calendar events
CxList uriParse = All.FindByMemberAccess("Uri.parse");
CxList contentUri = All.GetParameters(uriParse, 0);
CxList contentData = All.NewCxList();
foreach(CxList cu in contentUri)
{
	if(cu.GetName().StartsWith("content://sms"))
	{
		contentData.Add(cu); // SMS data
	}
	else if(cu.GetName().StartsWith("content://call_log"))
	{
		contentData.Add(cu); // Call log
	}
	else if(cu.GetName().StartsWith("content://com.android.calendar"))
	{
		contentData.Add(cu); // Calendar
	}
}
CxList contentRead = getStrCursor.DataInfluencedBy(contentData) * getStrCursor;


result = GPSInfo;
result.Add(contactInfoInput);
result.Add(contentRead);
result.Add(imei);
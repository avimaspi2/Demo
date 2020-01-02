CxList strings = Find_Strings();
CxList capabilities = All.FindByMemberAccess("CAPABILITY.NAME");

CxList idCap = strings.FindByShortName("ID_CAP_*");

CxList networkCap = idCap.FindByShortName("ID_CAP_NETWORKING");
CxList locationCap = idCap.FindByShortName("ID_CAP_LOCATION");
CxList contactsCap = idCap.FindByShortName("ID_CAP_CONTACTS");
CxList storageCap = idCap.FindByShortName("ID_CAP_REMOVABLE_STORAGE");
CxList cameraCap = idCap.FindByShortName("ID_CAP_ISV_CAMERA");
CxList microphoneCap = idCap.FindByShortName("ID_CAP_MICROPHONE");
CxList walletCap = idCap.FindByShortName("ID_CAP_WALLET");
CxList wInstCap = idCap.FindByShortName("ID_CAP_WALLET_PAYMENTINSTRUMENTS");
CxList wSecureCap = idCap.FindByShortName("ID_CAP_WALLET_SECUREELEMENT");
CxList phoneCap = idCap.FindByShortName("ID_CAP_PHONEDIALER");
CxList mediaPhotoCap = idCap.FindByShortName("ID_CAP_MEDIALIB_PHOTO");
CxList mdeiaAudioCap = idCap.FindByShortName("ID_CAP_MEDIALIB_AUDIO");
CxList mediaPlayCap = idCap.FindByShortName("ID_CAP_MEDIALIB_PLAYBACK");

CxList caps = All.NewCxList();
caps.Add(networkCap,
	locationCap,
	contactsCap,
	storageCap,
	cameraCap,
	microphoneCap,
	walletCap,
	wInstCap,
	wSecureCap,
	phoneCap,
	mediaPhotoCap,
	mdeiaAudioCap,
	mediaPlayCap);
	
CxList permissions = caps.DataInfluencingOn(capabilities);

//finds each permission request
CxList networkPermission = permissions * networkCap;
CxList gpsPermission = permissions * locationCap;
CxList contactsPermission = permissions * contactsCap;
CxList externalStoragePermission = permissions * storageCap;
CxList cameraPermission = permissions * cameraCap;
CxList microphonePermission = permissions * microphoneCap;
CxList walletPermission = permissions * walletCap;
CxList walletPaymentInstrumentPermission = permissions * wInstCap;
CxList walletSecureElementPermission = permissions * wSecureCap;
CxList dialerPermission = permissions * phoneCap;
CxList photoPermission = permissions * mediaPhotoCap;
CxList audioPermission = permissions * (mdeiaAudioCap + mediaPlayCap);


//find all uses of capabilities
CxList usingNetwork = All.FindByTypes(new String[] {
	"HttpWebRequest",
	"HttpWebResponse",
	"WebClient",
	"Socket",
	"WebBrowser"});
usingNetwork.Add(All.FindByMemberAccess("Launcher.LaunchUri*"));

CxList usingGPS = All.FindByTypes(new String[] {
	"GeoCoordinate",
	"GeoCoordinateWatcher",
	"Geolocator",
	"Geoposition"});

CxList usingContacts = All.FindByType("Contacts");

CxList usingExternalStorage = All.FindByTypes(new String[] {
	"ExternalStorageDevice",
	"ExternalStorage"});

CxList usingCamera = All.FindByTypes(new String[] {
	"PhotoCamera",
	"AudioVideoCaptureDevice",
	"VideoCaptureDevice"});

CxList usingMicrophone = All.FindByTypes(new String[] {
	"AudioVideoCaptureDevice",
	"AudioCaptureDevice",
	"Microphone"});

CxList usingWallet = All.FindByTypes(new String[] {
	"Wallet*",
	"SecureElement*"});

CxList usingWalletPaymentInstrument = All.FindByTypes(new String[] {
	"PaymentInstrument",
	"OnlinePaymentInstrument"});

CxList usingWalletSecureElement = All.FindByTypes(new String[] {
	"SecureElementSession",
	"SecureElementChannel",
	"SecureElementReader"});

CxList usingDialer = All.FindByType("PhoneCallTask");

CxList usingPhoto = All.FindByMemberAccess("MediaLibrary.Pictures");
usingPhoto.Add(All.FindByMemberAccess("MediaLibrary.RootPictureAlbum"));
usingPhoto.Add(All.FindByMemberAccess("MediaLibrary.SavedPictures"));
usingPhoto.Add(All.FindByMemberAccess("MediaLibrary.GetPictureFromToken"));
usingPhoto.Add(All.FindByMemberAccess("MediaLibrary.SavePicture*"));

CxList usingAudio = All.FindByMemberAccess("MediaLibrary.Albums");
usingAudio.Add(All.FindByMemberAccess("MediaLibrary.Artists"));
usingAudio.Add(All.FindByMemberAccess("MediaLibrary.Genres"));

// Application Required capability access but not uses it
if ((networkPermission.Count > 0) && (usingNetwork.Count == 0))
{
	result.Add(networkPermission);
}

if ((gpsPermission.Count > 0) && (usingGPS.Count == 0))
{
	result.Add(gpsPermission);
}
										 
if ((contactsPermission.Count > 0) && (usingContacts.Count == 0))
{
	result.Add(contactsPermission);
}

if ((externalStoragePermission.Count > 0) && (usingExternalStorage.Count == 0))
{
	result.Add(externalStoragePermission);
}

if ((cameraPermission.Count > 0) && (usingCamera.Count == 0))
{
	result.Add(cameraPermission);
}

if ((microphonePermission.Count > 0) && (usingMicrophone.Count == 0))
{
	result.Add(microphonePermission);
}

if ((walletPermission.Count > 0) && (usingWallet.Count == 0))
{
	result.Add(walletPermission);
}

if ((walletPaymentInstrumentPermission.Count > 0) && (usingWalletPaymentInstrument.Count == 0))
{
	result.Add(walletPaymentInstrumentPermission);
}

if ((walletSecureElementPermission.Count > 0) && (usingWalletSecureElement.Count == 0))
{
	result.Add(walletSecureElementPermission);
}

if ((dialerPermission.Count > 0) && (usingDialer.Count == 0))
{
	result.Add(dialerPermission);
}

if ((photoPermission.Count > 0) && (usingPhoto.Count == 0))
{
	result.Add(photoPermission);
}

if ((audioPermission.Count > 0) && (usingAudio.Count == 0))
{
	result.Add(audioPermission);
}
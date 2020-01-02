/*
	This query looks for missing resource deallocations/releases on the iOS platform.
    There are many frameworks available: add new ones as needed.

	REFERENCES:
	Full list of frameworks:
	https://developer.apple.com/library/ios/documentation/Miscellaneous/Conceptual/iPhoneOSTechOverview/iPhoneOSFrameworks/iPhoneOSFrameworks.html
*/


/*	
	CoreFoundation Framework
	https://developer.apple.com/library/ios/documentation/CoreFoundation/Reference/CoreFoundation_Collection/index.html#//apple_ref/doc/uid/TP40003849

	Note: CoreFoundation uses a reference-counting mechanism when it comes to memory allocation. In that
	regard, 'CFRelease' is +/- like C's stdlib 'free', by decrementing the reference-count of a given
	object; and, conversely, 'CFRetain' increments.
*/

// CFAllocator
result.Add(Find_Missing_Resource_Release("CFAllocatorAllocate", -1, "CFAllocatorDeallocate", 1));

// CFArray
result.Add(Find_Missing_Resource_Release("CFArrayCreate", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFArrayCreateCopy", "CFRelease", 0));

// CFAttributedString
result.Add(Find_Missing_Resource_Release("CFAttribuedStringCreate", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFAttribuedStringCreateCopy", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFAttribuedStringCreateWithSubstring", -1, "CFRelease", 0));

// CFBag
result.Add(Find_Missing_Resource_Release("CFBagCreate", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFBagCreateCopy", -1, "CFRelease", 0));

// CFBinaryHeap
result.Add(Find_Missing_Resource_Release("CFBinaryHeapCreate", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFBinaryHeapCreateCopy", -1, "CFRelease", 0));

// CFBitVector
result.Add(Find_Missing_Resource_Release("CFBitVectorCreate", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFBitVectorCreateCopy", -1, "CFRelease", 0));

// CFBundle
result.Add(Find_Missing_Resource_Release("CFBundleCreate", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFBundleCreateBundlesFromDirectory", -1, "CFRelease", 0)); // NOTE: The output is of CFArrayRef type.

// CFCalendar
result.Add(Find_Missing_Resource_Release("CFCalendarCopyCurrent", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFCalendarCreateWithIdentifier", -1, "CFRelease", 0));

// CFCharacterSet
result.Add(Find_Missing_Resource_Release("CFCharacterSetCreateCopy", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFCharacterSetCreateInvertedSet", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFCharacterSetCreateWithCharactersInRange", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFCharacterSetCreateWithCharactersInString", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFCharacterSetCreateWithBitmapRepresentation", -1, "CFRelease", 0));

// CFData
result.Add(Find_Missing_Resource_Release("CFDataCreate", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFDataCreateCopy", -1, "CFRelease", 0));

// CFDate
result.Add(Find_Missing_Resource_Release("CFDateCreate", -1, "CFRelease", 0));

// CFDateFormatter
result.Add(Find_Missing_Resource_Release("CFDateFormatterCreate", -1, "CFRelease", 0));

// CFDictionary
result.Add(Find_Missing_Resource_Release("CFDictionaryCreate", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFDictionaryCreateCopy", -1, "CFRelease", 0));

// CFError
result.Add(Find_Missing_Resource_Release("CFErrorCreate", -1, "CFRelease", 0));

// CFFileDescriptor
result.Add(Find_Missing_Resource_Release("CFFileDescriptorCreate", -1, "CFRelease", 0));

// CFLocale
result.Add(Find_Missing_Resource_Release("CFLocaleCreate", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFLocaleCreateCopy", -1, "CFRelease", 0));

// CFMachPort
result.Add(Find_Missing_Resource_Release("CFMachPortCreate", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFMachPortCreateWithPort", -1, "CFRelease", 0));

// CFMessagePort
result.Add(Find_Missing_Resource_Release("CFMessagePortCreateLocal", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFMessagePortCreateRemote", -1, "CFRelease", 0));

// CFMutableArray
result.Add(Find_Missing_Resource_Release("CFArrayCreateMutable", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFArrayCreateMutableCopy", -1, "CFRelease", 0));

// CFMutableAttribuedString
result.Add(Find_Missing_Resource_Release("CFAttribuedStringCreateMutable", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFAttribuedStringCreateMutableCopy", -1, "CFRelease", 0));

// CFMutableBag
result.Add(Find_Missing_Resource_Release("CFBagCreateMutable", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFBagCreateMutableCopy", -1, "CFRelease", 0));

// CFMutableBitVector
result.Add(Find_Missing_Resource_Release("CFBitVectorCreateMutable", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFBitVectorCreateMutableCopy", -1, "CFRelease", 0));

// CFMutableCharacterSet
result.Add(Find_Missing_Resource_Release("CFCharacterSetCreateMutable", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFCharacterSetCreateMutableCopy", -1, "CFRelease", 0));

// CFMutableData
result.Add(Find_Missing_Resource_Release("CFDataCreateMutable", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFDataCreateMutableCopy", -1, "CFRelease", 0));

// CFMutableDictionary
result.Add(Find_Missing_Resource_Release("CFDictionaryCreateMutable", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFDictionaryCreateMutableCopy", -1, "CFRelease", 0));

// CFMutableSet
result.Add(Find_Missing_Resource_Release("CFSetCreateMutable", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFSetCreateMutableCopy", -1, "CFRelease", 0));

// CFMutableString
result.Add(Find_Missing_Resource_Release("CFStringCreateMutable", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateMutableCopy", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateMutableWithExternalCharactersNoCopy", -1, "CFRelease", 0));

// CFNumber
result.Add(Find_Missing_Resource_Release("CFNumberCreate", -1, "CFRelease", 0));

// CFNumberFormatter
result.Add(Find_Missing_Resource_Release("CFNumberFormatterCreate", -1, "CFRelease", 0));

// CFPlugIn
result.Add(Find_Missing_Resource_Release("CFPlugInCreate", -1, "CFRelease", 0));

// CFPropertyList
result.Add(Find_Missing_Resource_Release("CFPropertyListCreateWithData", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFPropertyListCreateWithStream", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFPropertyListCreateDeepCopy", -1, "CFRelease", 0));

// CFReadStream
result.Add(Find_Missing_Resource_Release("CFReadStreamCreateWithBytesNoCopy", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFReadStreamCreateWithFile", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFReadStreamOpen", 0, "CFReadStreamClose", 0));

// CFRunLoopObserver
result.Add(Find_Missing_Resource_Release("CFRunLoopObserverCreateWithHandler", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFRunLoopObserverCreate", -1, "CFRelease", 0));

// CFRunLoopSource
result.Add(Find_Missing_Resource_Release("CFRunLoopSourceCreate", -1, "CFRelease", 0));

// CFRunLoopTimer
result.Add(Find_Missing_Resource_Release("CFRunLoopTimerCreateWithHandler", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFRunLoopTimerCreate", -1, "CFRelease", 0));

// CFSet
result.Add(Find_Missing_Resource_Release("CFSetCreate", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFSetCreateCopy", -1, "CFRelease", 0));

// CFSocket
result.Add(Find_Missing_Resource_Release("CFSocketCreate", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFSocketCreateConnectedToSocketSignature", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFSocketCreateWithNative", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFSocketCreateWithSocketSignature", -1, "CFRelease", 0));

// CFString
result.Add(Find_Missing_Resource_Release("CFSTR", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateArrayBySeparatingStrings", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateByCombiningStrings", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateCopy", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateFromExternalRepresentation", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateWithBytes", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateWithBytesNoCopy", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateWithCharacters", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateWithCharactersNoCopy", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateWithCString", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateWithCStringNoCopy", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateWithFormat", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateWithFormatAndArguments", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateWithPascalString", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateWithPascalStringNoCopy", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFStringCreateWithSubstring", -1, "CFRelease", 0));

// CFStringTokenizer
result.Add(Find_Missing_Resource_Release("CFStringTokenizerCreate", -1, "CFRelease", 0));

// CFTimeZone
result.Add(Find_Missing_Resource_Release("CFTimeZoneCreateWithName", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFTimeZoneCreateWithTimeIntervalFromGMT", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFTimeZoneCreate", -1, "CFRelease", 0));

// CFTree
result.Add(Find_Missing_Resource_Release("CFTreeCreate", -1, "CFRelease", 0));

// CFURL
result.Add(Find_Missing_Resource_Release("CFURLCopyAbsoluteURL", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFURLCreateAbsoluteURLWithBytes", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFURLCreateByResolvingBookmarkData", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFURLCreateCopyAppendingPathComponent", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFURLCreateCopyAppendingPathExtension", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFURLCreateCopyDeletingLastPathComponent", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFURLCreateCopyDeletingPathExtension", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFURLCreateFilePathURL", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFURLCreateFileReferenceURL", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFURLCreateFromFileSystemRepresentation", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFURLCreateFromFileSystemRepresentationRelativeToBase", -1, "CFRelease", 0));
// CFURLCreateFromFSRef (obsoleted)
result.Add(Find_Missing_Resource_Release("CFURLCreateWithBytes", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFURLCreateWithFileSystemPath", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFURLCreateWithFileSystemPathRelativeToBase", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFURLCreateWithString", -1, "CFRelease", 0));

// CFUUID
result.Add(Find_Missing_Resource_Release("CFUUIDCreate", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFUUIDCreateFromString", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFUUIDCreateFromUUIDBytes", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFUUIDCreateWithBytes", -1, "CFRelease", 0));

// CFWriteStream
result.Add(Find_Missing_Resource_Release("CFWriteStreamCreateWithAllocatedBuffers", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFWriteStreamCreateWithBuffer", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFWriteStreamCreateWithFile", -1, "CFRelease", 0));
result.Add(Find_Missing_Resource_Release("CFWriteStreamOpen", 0, "CFWriteStreamClose", 0));
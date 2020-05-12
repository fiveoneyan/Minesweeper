#import "iOSNativeURL.h"

@implementation iOSNativeURL
+(void) OpenURL:(NSString *) urlString {
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString:urlString]];
}

extern "C" {
    void _TAG_iOSNativeURL_OpenURL(char* urlString){
        [iOSNativeURL OpenURL: [DataConvertor charToNSString:urlString]];
    }
}

@end

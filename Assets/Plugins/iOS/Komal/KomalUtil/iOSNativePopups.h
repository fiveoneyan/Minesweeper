#import <Foundation/Foundation.h>
#import "DataConvertor.h"

@interface iOSNativePopups : NSObject

+ (void) unregisterAllertView;
+ (void) dismissCurrentAlert;
+ (void) showMessage: (NSString *) title message: (NSString*) msg okTitle:(NSString*) b1;
+ (void) showDialog: (NSString *) title message: (NSString*) msg yesTitle:(NSString*) b1 noTitle: (NSString*) b2;
+ (void) showRateUsPopUp: (NSString *) title message: (NSString*) msg b1: (NSString*) b1 b2: (NSString*) b2 b3: (NSString*) b3;

@end

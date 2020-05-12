
#ifndef UnityiOSTapticPlugin_h
#define UnityiOSTapticPlugin_h

#import <UIKit/UIKit.h>


@interface UnityiOSTapticPlugin : NSObject{
}
+ (UnityiOSTapticPlugin*) shared;
- (void) notification:(UINotificationFeedbackType) type;
- (void) selection;
- (void) impact:(UIImpactFeedbackStyle) style;
+ (BOOL) isSupport;
@end

#endif /* UnityiOSTapticPlugin_h */

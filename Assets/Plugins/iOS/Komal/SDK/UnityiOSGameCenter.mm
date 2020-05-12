#import "UnityiOSGameCenter.h"
@implementation UnityiOSGameCenter

+(void)FiveStars {
    if([SKStoreReviewController respondsToSelector:@selector(requestReview)]) {
        // iOS 10.3 以上支持
        [SKStoreReviewController requestReview];
    }else{
        // 不支持 iOS 10.3 之前
    }
}

#if defined(__cplusplus)
extern "C"{
#endif
    void _TAG_UnityiOSGameCenter_FiveStars() {
        [UnityiOSGameCenter FiveStars];
    }
#if defined(__cplusplus)
}
#endif

@end

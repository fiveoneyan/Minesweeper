//
//  UnityiOSTapticPlugin.mm
//

#import "UnityiOSTapticPlugin.h"

#pragma mark - UnityiOSTapticPlugin

@interface UnityiOSTapticPlugin ()
@property (nonatomic, strong) UINotificationFeedbackGenerator* notificationGenerator;
@property (nonatomic, strong) UISelectionFeedbackGenerator* selectionGenerator;
@property (nonatomic, strong) NSArray<UIImpactFeedbackGenerator*>* impactGenerators;
@end

@implementation UnityiOSTapticPlugin

static UnityiOSTapticPlugin * _shared;

+ (UnityiOSTapticPlugin*) shared {
    @synchronized(self) {
        if(_shared == nil) {
            _shared = [[self alloc] init];
        }
    }
    return _shared;
}

- (id) init {
    if (self = [super init])
    {
        if([UINotificationFeedbackGenerator class]){
            self.notificationGenerator = [UINotificationFeedbackGenerator new];
            [self.notificationGenerator prepare];
            
            self.selectionGenerator = [UISelectionFeedbackGenerator new];
            [self.selectionGenerator prepare];
            
            self.impactGenerators = @[
                 [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleLight],
                 [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleMedium],
                 [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleHeavy],
            ];
            for(UIImpactFeedbackGenerator* impact in self.impactGenerators) {
                [impact prepare];
            }
        }
    }
    return self;
}

- (void) dealloc {
    self.notificationGenerator = NULL;
    self.selectionGenerator = NULL;
    self.impactGenerators = NULL;
}

- (void) notification:(UINotificationFeedbackType)type {
    if(self.notificationGenerator){
        [self.notificationGenerator notificationOccurred:type];
    }
}


- (void) selection {
    if(self.selectionGenerator){
        [self.selectionGenerator selectionChanged];
    }
}

- (void) impact:(UIImpactFeedbackStyle)style {
    if(self.impactGenerators){
        [self.impactGenerators[(int) style] impactOccurred];
    }
}

+ (BOOL) isSupport {
    // http://stackoverflow.com/questions/39564510/check-if-device-supports-uifeedbackgenerator-in-ios-10
    
    // Private API
    // NSNumber* support = [[UIDevice currentDevice] valueForKey:@"_feedbackSupportLevel"];
    // return support.intValue == 2;

    if ([UINotificationFeedbackGenerator class]) {
        return YES;
    }
    return NO;
    
}

@end

#pragma mark - Unity Bridge

extern "C" {
    void _TAG_Unity_iOSTapticNotification(int type) {
        [[UnityiOSTapticPlugin shared] notification:(UINotificationFeedbackType) type];
    }
    
    void _TAG_Unity_iOSTapticSelection() {
        [[UnityiOSTapticPlugin shared] selection];
    }
    
    void _TAG_Unity_iOSTapticImpact(int style) {
        [[UnityiOSTapticPlugin shared] impact:(UIImpactFeedbackStyle) style];
    }
    
    bool _TAG_Unity_iOSTapticIsSupport() {
        return [UnityiOSTapticPlugin isSupport];
    }
}

#import <Foundation/Foundation.h>
#import "iOSNativeVibrate.h"
#import <AudioToolbox/AudioToolbox.h>

@implementation iOSNativeVibrate
+(void)Vibrate: (NSString *) vibrateTyp {
    if([vibrateTyp isEqualToString: @"NORMAL"]){
        //长震
        AudioServicesPlaySystemSound(kSystemSoundID_Vibrate);
    }else if([vibrateTyp isEqualToString: @"PEEK"]){
        //3D Touch 中 Peek 震动反馈
        AudioServicesPlaySystemSound(1519);
    }else if([vibrateTyp isEqualToString: @"POP"]){
        //3D Touch 中 Pop 震动反馈
        AudioServicesPlaySystemSound(1520);
    }else if([vibrateTyp isEqualToString: @"CONTINUE"]){
        //连续三次振动
        AudioServicesPlaySystemSound(1521);
    }else{
        // do nothing
    }
}

extern "C" {
    void _TAG_iOSNativeVibrate_Vibrate(char* vibrateTyp) {
        [iOSNativeVibrate Vibrate: [DataConvertor charToNSString:vibrateTyp]];
    }
}
@end


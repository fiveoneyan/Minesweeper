//
//  Copyright (c) 2015 IronSource. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "IronSource/ISBaseAdapter+Internal.h"
#import "IronSource/ISGlobals.h"

static NSString * const ChartboostAdapterVersion = @"4.1.7";
static NSString * GitHash = @"66454182e";

//System Frameworks For Chartboost Adapter

@import AVFoundation;
@import CoreGraphics;
@import Foundation;
@import StoreKit;
@import UIKit;
@import WebKit;

@interface ISChartboostAdapter : ISBaseAdapter

@end

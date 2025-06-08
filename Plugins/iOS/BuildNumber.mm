#import <Foundation/Foundation.h>

extern "C" {
    const char* _GetIOSBuildNumber()
    {
        @autoreleasepool {
            NSString* buildNumber = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleVersion"];
            return [buildNumber UTF8String];
        }
    }
}
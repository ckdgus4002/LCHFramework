#import <Foundation/Foundation.h>

extern "C" {
    const char* GetIosBuildNumber()
    {
        @autoreleasepool {
            NSString* buildNumber = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleVersion"];
            return [buildNumber UTF8String];
        }
    }
}
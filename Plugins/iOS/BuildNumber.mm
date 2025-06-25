#import <Foundation/Foundation.h>
#include <string.h>

extern "C" {
    const char* GetiOSBuildNumber()
    {
        @autoreleasepool {
            NSString* buildNumber = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleVersion"];
            return strdup([buildNumber UTF8String]);
        }
    }
}
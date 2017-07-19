#import "UnityAppController.h"

@interface MemoryAppController : UnityAppController
- (void)applicationDidReceiveMemoryWarning:(UIApplication*)application;
@end

@implementation MemoryAppController
- (void)applicationDidReceiveMemoryWarning:(UIApplication*)application {
    printf_console("Memory warning called\n");
    UnitySendMessage("BackendObject", "ReceivedMemoryWarning","");
}
@end

IMPL_APP_CONTROLLER_SUBCLASS(MemoryAppController)
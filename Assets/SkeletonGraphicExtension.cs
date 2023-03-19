#if Spine
using Spine.Unity;
#endif

namespace LCHFramework.Extensions
{ 
    public static class SkeletonGraphicExtension
    {
#if Spine
        // public static void SetSkinContinuously(this SkeletonGraphic skeletonGraphic, string skinName)
        // {
        //     skeletonGraphic.Skeleton.SetSkin(skinName);
        //     skeletonGraphic.Skeleton.SetSlotsToSetupPose();
        //     skeletonGraphic.initialSkinName = skinName;
        // }
#endif
    }
}
#if Spine
using Spine.Unity;

namespace LCHFramework.Extensions
{ 
    public static class SkeletonGraphicExtension
    {
        public static void SetSkinContinuously(this SkeletonGraphic skeletonGraphic, string skinName)
        {
            skeletonGraphic.Skeleton.SetSkin(skinName);
            skeletonGraphic.Skeleton.SetSlotsToSetupPose();
            skeletonGraphic.initialSkinName = skinName;
        }
    }
}
#endif

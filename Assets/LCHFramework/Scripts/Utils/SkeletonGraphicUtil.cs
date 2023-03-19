#if Spine
using Spine.Unity;
#endif

namespace JSchool.Modules.Common.LCH.Utils
{
    public static class SkeletonGraphicUtil
    {
#if Spine
        public static void SetContinuouslySkin(SkeletonGraphic skeletonGraphic, string skinName)
        {
            skeletonGraphic.Skeleton.SetSkin(skinName);
            skeletonGraphic.Skeleton.SetSlotsToSetupPose();
            skeletonGraphic.initialSkinName = skinName;
        }
#endif
    }
}
using Spine.Unity;

namespace JSchool.Modules.Common.LCH.Utils
{
    public static class SkeletonGraphicUtil
    {
        public static void SetContinuouslySkin(SkeletonGraphic skeletonGraphic, string skinName)
        {
            skeletonGraphic.Skeleton.SetSkin(skinName);
            skeletonGraphic.Skeleton.SetSlotsToSetupPose();
            skeletonGraphic.initialSkinName = skinName;
        }
    }
}
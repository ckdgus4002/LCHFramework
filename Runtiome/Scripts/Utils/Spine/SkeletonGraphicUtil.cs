#if Spine
using Spine.Unity;

namespace LCHFramework.Utils.Spine
{
    public static class SkeletonGraphicUtil
    {
        public static void SetContinuouslySkin(SkeletonGraphic skeletonGraphic, string skinName)
            => skeletonGraphic.SetContinuouslySkin(skinName);
    }
}
#endif

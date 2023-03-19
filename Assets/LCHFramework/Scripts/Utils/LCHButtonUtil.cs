using System.Collections.Generic;
using System.Linq;

namespace LCHFramework.Utils
{
    public static class LCHButtonUtil
    {
        public const float GuideAnimationDuration = .5f;
        public const int GuideAnimationLoopNumber = 4;
        public const float WrongAnimationDuration = .5f + .5f;
        
        
        public static float GuideAnimationTotalDuration => GuideAnimationDuration * GuideAnimationLoopNumber;
        
        public static bool IsCorrected<T>(T button, IEnumerable<T> answers, IEnumerable<T> defaultAnswers) => IsCorrected(button, answers, IsAnswered(button, defaultAnswers));
        
        public static bool IsCorrected<T>(T button, IEnumerable<T> answers, bool isAnswered) => isAnswered && !answers.Contains(button);

        public static bool IsAnswered<T>(T button, IEnumerable<T> defaultAnswers) => defaultAnswers.Contains(button);
    }
}
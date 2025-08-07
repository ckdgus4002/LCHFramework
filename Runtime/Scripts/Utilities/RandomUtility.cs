using System;
using System.Linq;

namespace LCHFramework.Utilities
{
    public static class RandomUtility
    {
        public static int Sign => new Random().NextDouble() < .5f ? -1 : 1;
        
        
        
        public static int ChooseWithProbabilities(params float[] probabilities)
        {
            var result = -1;
            var randomPoint = new Random().NextDouble() * probabilities.Sum();  
            for (var i = 0; i < probabilities.Length; i++)
                if (i < probabilities.Length - 1 && probabilities[i] < randomPoint)
                    randomPoint -= probabilities[i];
                else
                {
                    result = i;
                    break;
                }

            return result;
        }
    }
}

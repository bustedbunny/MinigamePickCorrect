using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MinigamePickCorrect
{
    public static class ListUtility
    {
        public static T RandomElementFromList<T>(List<T> list)
        {
            return list[RandomListIndex(list)];
        }
        public static T RandomElementFromList<T>(List<T> list, out int ind)
        {
            ind = RandomListIndex(list);
            T obj = list.ElementAt(ind);
            return obj;
        }
        private static int RandomListIndex<T>(List<T> list)
        {
            return Random.Range(0, list.Count);
        }
    }
}


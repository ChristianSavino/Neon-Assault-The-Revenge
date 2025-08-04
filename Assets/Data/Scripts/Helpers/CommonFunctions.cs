using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Helpers
{
    public static class CommonFunctions
    {
        public static void SetGameObjectParent(Transform parent, IEnumerable<GameObject> children)
        {
            foreach (var child in children)
            {
                if (child != null)
                {
                    child.transform.SetParent(parent, true);
                }
            }
        }

        public static Transform FindChild(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name.Contains(name))
                {
                    return child;
                }
                var result = FindChild(child, name);
                if (result != null)
                {
                    return result;
                }
                    
            }
            return null;
        }
    }
}

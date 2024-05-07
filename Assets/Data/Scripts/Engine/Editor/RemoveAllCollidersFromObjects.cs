using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Keru.Scripts.Engine.Editor
{
    public class RemoveAllCollidersFromObjects : MonoBehaviour
    {
        [MenuItem("Tools/Remove all colliders from scene %#p")]
        static public void removeColliderAndApplyPrefabChanges()
        {
            var collidersDestroyed = 0;
            var objectsWithColliders = 0;
            
                var gameObjects = FindObjectsOfType<GameObject>();
            foreach (var gameObject in gameObjects)
            {
                if(gameObject.activeInHierarchy && gameObject.tag == "Untagged")
                {
                    var colliders = gameObject.GetComponents<Collider>();
                    objectsWithColliders += colliders.Any() ? 1 : 0;
                    foreach (var collider in colliders)
                    {
                        collidersDestroyed++;
                        DestroyImmediate(collider);
                    }
                }
            }

            print($"Results:\nGameObjects With Colliders: {objectsWithColliders}\nColliders Removed: {collidersDestroyed}");
        }
    }
}
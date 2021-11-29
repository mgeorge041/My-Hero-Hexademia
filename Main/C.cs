using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    // Game wide functions
    public class C
    {
        // Destroy game objects depending on running mode
        public static T DestroyGameObject<T>(T obj) where T : Object
        {
            if (obj != null)
            {
                if (Application.isEditor)
                {
                    Object.DestroyImmediate(obj);
                }
                else
                {
                    Object.Destroy(obj);
                }
            }

            return null;
        }

        // Destroy game objects depending on running mode
        public static T Destroy<T>(T obj) where T : MonoBehaviour
        {
            if (obj != null)
            {
                if (Application.isEditor)
                {
                    Object.DestroyImmediate(obj.gameObject);
                }
                else
                {
                    Object.Destroy(obj.gameObject);
                }
            }

            return null;
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Game.Actions
{
    public class Action : MonoBehaviour
    {
        public virtual void SetUp(int paramater, float floatParameter, string stringParameter, GameObject gameObjectParameter, bool boolParameter)
        {

        }
        
        public virtual void Execute(GameObject target = null)
        {

        }
    }
}
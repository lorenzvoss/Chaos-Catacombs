using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Conditions
{
    /// <summary>
    /// It is a perception condition to check if the objective is close depending on a given distance.
    /// </summary>
    [Condition("MyConditions/IsInShootDistance")]
    [Help("Checks whether a target is in shooting distance")]
    public class IsInShootDistance : GOCondition
    {
        ///<value>Input Target Parameter to to check the distance.</value>
        [InParam("target")]
        [Help("Target to check the distance")]
        public GameObject target;

        ///<value>Input minimum distance to player in order to shoot.</value>
        [InParam("minDistance")]
        [Help("The maximun distance to consider that the target is close")]
        public float minDistance;

        ///<value>Input maximun distance to player in order to shoot.</value>
        [InParam("maxDistance")]
        [Help("The maximun distance to consider that the target is close")]
        public float maxDistance;
    
        /// <summary>
        /// Checks whether a shooting distance is reached,
        /// calculates the magnitude between the gameobject and the target and then compares with the min and max distance.
        /// </summary>
        /// <returns>True if the magnitude between the gameobject and the target is in the given range.</returns>
        public override bool Check()
		{
            return ((gameObject.transform.position - target.transform.position).sqrMagnitude < maxDistance * maxDistance && 
                    (gameObject.transform.position - target.transform.position).sqrMagnitude > minDistance * minDistance)    ;
		}
    }
}
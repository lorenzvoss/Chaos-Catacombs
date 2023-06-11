using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using BBUnity.Actions;        // GOAction

namespace BBCore.Actions
{

    /// <summary>
    /// Implementation of the wait action using busy-waiting (spinning).
    /// </summary>
    [Action("MyActions/AsureKinematic")]
    [Help("Sets IsKinematic Variable to true.")]
    public class AsureKinematic : GOAction
    {
        private Rigidbody rb;

        /// <summary>Sets isKinematic to true, to asure that the enemy isn't affected  from physics after a jump attack.</summary>
        public override void OnStart()
        {
            rb = gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}

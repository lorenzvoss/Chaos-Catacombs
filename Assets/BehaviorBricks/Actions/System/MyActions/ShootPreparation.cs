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
    [Action("MyActions/ShootPreparation")]
    [Help("Action that success after a period of time.")]
    public class ShootPreparation : GOAction
    {
        ///<value>Input Amount of time to wait (in seconds) Parameter.</value>
        [InParam("seconds", DefaultValue = 0.5f)]
        [Help("Amount of time to wait (in seconds)")]
        public float seconds;

        ///<value>Target to watch while waiting.</value>
        [InParam("target")]
        [Help("Target to watch")]
        public GameObject target;

        private float elapsedTime;
        private float movementDirection;

        private Animator animator;

        /// <summary>Initialization Method of WaitForSeconds.</summary>
        /// <remarks>Initializes the elapsed time to 0.</remarks>
        public override void OnStart()
        {
            animator = gameObject.GetComponent<Animator>();
            animator.SetBool("isWalking", false);

            elapsedTime = 0;
            movementDirection = Random.Range(0, 2) == 0 ? -1 : 1;
        }

        /// <summary>Method of Update of WaitForSeconds.</summary>
        /// <remarks>Increase the elapsed time and check if you have exceeded the waiting time has ended.</remarks>
        public override TaskStatus OnUpdate()
        {
            gameObject.transform.LookAt(target.transform);
            float translation = 1.5f * movementDirection * Time.deltaTime;
            gameObject.transform.Translate(translation, 0f, 0f);
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= seconds)
                return TaskStatus.COMPLETED;
            return TaskStatus.RUNNING;
        }
    }
}

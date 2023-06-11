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
    [Action("MyActions/Stunn")]
    [Help("Action that success after a period of time.")]
    public class Stunn : GOAction
    {
        ///<value>Time the gameObject is stunned</value>
        [InParam("stunnTime")]
        [Help("Time the gameObject is stunned")]
        public float stunnTime;
        private float stunnTimer;
        private bool stunnTheEnemy;
        private Rigidbody rb;
        

        /// <summary>Initialization Method of WaitForSeconds.</summary>
        /// <remarks>Initializes the elapsed time to 0.</remarks>
        public override void OnStart()
        {
            Debug.Log("Stunn Komponente erreicht");
            rb = gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            stunnTimer = stunnTime;
            stunnTheEnemy = !gameObject.GetComponent<Enemy_Large_Behavior>().hitThePlayerInLastXSec;
        }

        /// <summary>Method of Update of WaitForSeconds.</summary>
        /// <remarks>Increase the elapsed time and check if you have exceeded the waiting time has ended.</remarks>
        public override TaskStatus OnUpdate()
        {
            Debug.Log(stunnTheEnemy);
            if(stunnTheEnemy && stunnTimer >= 0)
            {
                stunnTimer -= Time.deltaTime;
                Debug.Log("Stunn RUNNING");
                return TaskStatus.RUNNING;
            }
            else{
                gameObject.GetComponent<Enemy_Large_Behavior>().hitThePlayerInLastXSec = !stunnTheEnemy;
                Debug.Log("Stunn COMPLETED");
                return TaskStatus.COMPLETED;
            }
        }
    }
}

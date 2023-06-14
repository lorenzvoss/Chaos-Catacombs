using UnityEngine;
using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using BBUnity.Actions;        // GOAction

namespace BBCore.Actions
{
    /// <summary>
    /// 
    /// </summary>
    [Action("MyAction/JumpAttack")]
    [Help("")]
    public class JumpAttack : GOAction
    {
        ///<value>Input target Parameter.</value>
        // Define target at which will be shot.
        [InParam("target")]
        public GameObject target;

        public float jumpForce = 1500f;
        
        private Vector3 lastPlayerPosition;
        private Rigidbody rb;
        private float jumpTimer;
        private float finishTime;



        /// <summary>Initialization method of DoneShootOnce.</summary>
        /// <remarks>If the shootPoint is not established, we look for the shooting point.</remarks>

        // Initialization method. If not established, we look for the shooting point.
        public override void OnStart()
        {
            jumpTimer = 0f;
            finishTime = 2f;
            rb = gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            // Speichere die Position des Spielers
            lastPlayerPosition = target.transform.position;

            // Berechne die Richtung vom aktuellen Objekt zum Spieler
            Vector3 direction = lastPlayerPosition - gameObject.transform.position;
            direction.Normalize();
            // Führe den Sprung aus
            rb.AddForce(direction * jumpForce, ForceMode.Impulse);
            
        }

        public override TaskStatus OnUpdate()
        {            
            jumpTimer += Time.deltaTime;

            if(jumpTimer >= finishTime)
            {
                rb.isKinematic = true;
                gameObject.GetComponent<Enemy_Large_Behavior>().jumpAttackFinished = true;
                Debug.Log("JumpAttackFinished wurde auf true gesetzt!");
                return TaskStatus.COMPLETED;
            }
            
            return TaskStatus.RUNNING;
        }  


    }

} // namespace
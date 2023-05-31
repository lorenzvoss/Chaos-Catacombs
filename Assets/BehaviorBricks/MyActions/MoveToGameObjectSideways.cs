using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to move towards the given goal using a NavMeshAgent.
    /// </summary>
    [Action("MyActions/MoveToGameObjectSideways")]
    [Help("Moves the game object towards a given target by using a NavMeshAgent")]
    public class MoveToGameObjectSideways : GOAction
    {
        ///<value>Input target game object towards this game object will be moved Parameter.</value>
        [InParam("target")]
        [Help("Target game object towards this game object will be moved")]
        public GameObject target;

        private UnityEngine.AI.NavMeshAgent navAgent;

        private Transform targetTransform;

        private Transform gameObjectTransform;

        public float jumpDelay = 2f;
        private float nextJumpTime;
        private bool isMovingRight;

        /// <summary>Initialization Method of MoveToGameObject.</summary>
        /// <remarks>Check if GameObject object exists and NavMeshAgent, if there is no NavMeshAgent, the default one is added.</remarks>
        public override void OnStart()
        {
            if (target == null)
            {
                Debug.LogError("The movement target of this game object is null", gameObject);
                return;
            }
            targetTransform = target.transform;

            navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogWarning("The " + gameObject.name + " game object does not have a Nav Mesh Agent component to navigate. One with default values has been added", gameObject);
                navAgent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            }
			navAgent.SetDestination(targetTransform.position);
            
            #if UNITY_5_6_OR_NEWER
                navAgent.isStopped = false;
            #else
                navAgent.Resume();
            #endif

            nextJumpTime = Time.time + jumpDelay;
            isMovingRight = true;
        }

        /// <summary>Method of Update of MoveToGameObject.</summary>
        /// <remarks>Verify the status of the task, if there is no objective fails, if it has traveled the road or is near the goal it is completed
        /// y, the task is running, if it is still moving to the target.</remarks>
        public override TaskStatus OnUpdate()
        {
            gameObjectTransform = gameObject.GetComponent<Transform>();
            if(Vector3.Distance(gameObjectTransform.position, targetTransform.position) >= 5){
                //Sidejump implementation
                if(Time.time >= nextJumpTime)
                {   
                    if(isMovingRight)
                    {
                        gameObjectTransform.position = gameObjectTransform.position + (Quaternion.Euler(0,90,0) * (gameObjectTransform.position - targetTransform.position))*0.5f;
                        nextJumpTime= Time.time + jumpDelay;
                    }
                    else{
                        gameObjectTransform.position = gameObjectTransform.position - (Quaternion.Euler(0,90,0) * (gameObjectTransform.position - targetTransform.position))*0.5f;
                        nextJumpTime= Time.time + jumpDelay;
                    }
                    isMovingRight = !isMovingRight;
                }
            }
                if (target == null)
                    return TaskStatus.FAILED;
                if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
                    return TaskStatus.COMPLETED;
                else if (navAgent.destination != targetTransform.position)
                    navAgent.SetDestination(targetTransform.position);
                return TaskStatus.RUNNING;
            
        }
        /// <summary>Abort method of MoveToGameObject </summary>
        /// <remarks>When the task is aborted, it stops the navAgentMesh.</remarks>
        public override void OnAbort()
        {

        #if UNITY_5_6_OR_NEWER
            if(navAgent!=null)
                navAgent.isStopped = true;
        #else
            if (navAgent!=null)
                navAgent.Stop();
        #endif

        }
    }
}

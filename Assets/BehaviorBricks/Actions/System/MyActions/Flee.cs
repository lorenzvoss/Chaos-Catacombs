using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to move the GameObject to a given position.
    /// </summary>
    [Action("MyActions/Flee")]
    [Help("Moves the game object to a given position by using a NavMeshAgent")]
    public class Flee : GOAction
    {
        ///<value>Input target from which to flee.</value>
        [InParam("target")]
        [Help("Target from which the gameObject will flee")]
        public Transform target;

        ///<value>Input target from which to flee.</value>
        [InParam("distanceToArchieve")]
        [Help("Distance which the gameObject will try to establish")]
        public float distanceToAchieve;

        private UnityEngine.AI.NavMeshAgent navAgent;

        /// <summary>Initialization Method of MoveToPosition.</summary>
        /// <remarks>Check if there is a NavMeshAgent to assign a default one and assign the destination to the NavMeshAgent the given position.</remarks>
        public override void OnStart()
        {
            navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogWarning("The " + gameObject.name + " game object does not have a Nav Mesh Agent component to navigate. One with default values has been added", gameObject);
                navAgent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            }
            target = GameObject.FindWithTag("Player").transform;
            #if UNITY_5_6_OR_NEWER
                navAgent.isStopped = false;
            #else
                navAgent.Resume();
            #endif
        }

        /// <summary>Method of Update of Flee </summary>
        /// <remarks>Check the status of the task, if it has traveled the road or is close to the goal it is completed
        /// and otherwise it will remain in operation.</remarks>
        public override TaskStatus OnUpdate()
        {
            Vector3 fleeDirection = gameObject.transform.position - target.position;
            fleeDirection.Normalize();
            Vector3 fleeTarget = gameObject.transform.position + fleeDirection;
            navAgent.SetDestination(fleeTarget);

            if (Vector3.Distance(gameObject.transform.position, target.position) > distanceToAchieve)
                return TaskStatus.COMPLETED;

            return TaskStatus.RUNNING;
        }

        /// <summary>Abort method of MoveToPosition.</summary>
        /// <remarks>When the task is aborted, it stops the navAgentMesh.</remarks>
        public override void OnAbort()
        {
#if UNITY_5_6_OR_NEWER
            if(navAgent!=null)
                navAgent.isStopped = true;
#else
            if (navAgent != null)
                navAgent.Stop();
            #endif
        }
    }
}

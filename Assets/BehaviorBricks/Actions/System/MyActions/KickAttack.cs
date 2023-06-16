using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using BBUnity.Actions;        // GOAction

namespace BBUnity.Actions
{
    [Action("MyActions/KickAttack")]
    public class KickAttack : GOAction
    {
        private Animator animator;
        private UnityEngine.AI.NavMeshAgent navAgent;
 
        private Vector3 targetPosition;
        private float oldSpeed;
        private float oldAcceleration;


        public override void OnStart()
        {
            animator = gameObject.GetComponent<Animator>();
            navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            targetPosition = GameObject.FindWithTag("Player").transform.position;

            animator.SetBool("isWalking", true);

            //Alten Variablen des NavAgents speichern
            oldSpeed = navAgent.speed;
            oldAcceleration = navAgent.acceleration;
        

            // Variablen des NavAgents setzen:
            navAgent.SetDestination(targetPosition);
            navAgent.acceleration = 100;
            navAgent.speed = 13;
            //navAgent.stoppingDistance = 10f;
        }


        public override TaskStatus OnUpdate()
        {
            if(!navAgent.pathPending && navAgent.remainingDistance > navAgent.stoppingDistance + 5)
            {
                targetPosition = GameObject.FindWithTag("Player").transform.position;
                navAgent.SetDestination(targetPosition);

            }
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
            {    
                animator.SetTrigger("isKicking");
                animator.SetBool("isWalking", false);
                //Werte des navAgent zurücksetzen
                navAgent.speed = oldSpeed;

                
                return TaskStatus.COMPLETED;
            }
            return TaskStatus.RUNNING;
        }
    }
}

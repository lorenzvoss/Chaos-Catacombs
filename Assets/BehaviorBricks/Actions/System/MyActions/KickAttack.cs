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
        private GameObject player;


        public override void OnStart()
        {
            animator = gameObject.GetComponent<Animator>();
            navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            player = GameObject.FindWithTag("Player");
            targetPosition = player.transform.position;
            navAgent.enabled = true;
            animator.SetBool("isWalking", true);
            

            //Alten Variablen des NavAgents speichern
            oldSpeed = navAgent.speed;
            oldAcceleration = navAgent.acceleration;
        

            // Variablen des NavAgents setzen:
            navAgent.SetDestination(targetPosition);
            navAgent.acceleration = 100;
            navAgent.speed = 20;
            //navAgent.stoppingDistance = 10f;
        }


        public override TaskStatus OnUpdate()
        {
            if(!navAgent.pathPending && navAgent.remainingDistance > navAgent.stoppingDistance + 8)
            {
                targetPosition = GameObject.FindWithTag("Player").transform.position;
                navAgent.SetDestination(targetPosition);

            }
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
            {    
                if(checkHitPlayer())
                {
                    player.GetComponent<PlayerHealth>().isHitByKick = true;
                }
                animator.SetTrigger("isKicking");
                animator.SetBool("isWalking", false);
                //Werte des navAgent zurücksetzen
                navAgent.speed = oldSpeed;
                navAgent.enabled = false;
                
                return TaskStatus.COMPLETED;
            }
            return TaskStatus.RUNNING;
        }

        private bool checkHitPlayer()
        { 
            GameObject temp = gameObject.GetComponent<SphereCollider>().gameObject;
            Vector3 correctedPlayerPos = player.transform.position + new Vector3(0f,1f,0f);
            Vector3 direction = correctedPlayerPos - temp.transform.position ;
            RaycastHit hit;
            if (Physics.Raycast(temp.transform.position, direction, out hit, 3f))
            {
                Debug.Log(hit.collider.gameObject);
                Debug.Log(player);
                if (hit.collider.gameObject == player)
                {
                    Debug.Log("TRUE");
                    return true;
                }
            }
            Debug.Log("FALSE");
            return false;
            
        }
    }
}

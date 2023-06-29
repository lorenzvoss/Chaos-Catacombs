using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using BBUnity.Actions;        // GOAction

namespace BBUnity.Actions
{
    [Action("MyActions/SprintToRandPos")]
    public class SprintToRandPos : GOAction
    {
        private Animator animator;
        private GameObject player;
        private float minDistanceToPlayer;
        private float distanceToPlayer;
        private UnityEngine.AI.NavMeshAgent navAgent;
         private float oldSpeed;
        private float oldAcceleration;

        public override void OnStart()
        {
            animator = gameObject.GetComponent<Animator>();
            player = GameObject.FindWithTag("Player");
            minDistanceToPlayer = 10f;
            navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            animator.SetBool("isWalking", true);
            navAgent.enabled = true;

            Vector3 randomPosition;
            UnityEngine.AI.NavMeshHit navMeshHit;
            
            // Suche eine zufällige Position, die auf dem NavMesh liegt und einen minimalen Abstand zum Spieler hat
            do
            {
                // Generiere eine zufällige Position
                randomPosition = Random.insideUnitSphere * 50;
                randomPosition += player.transform.position;

                // Berechne den Abstand zwischen der generierten Position und dem Spieler
                distanceToPlayer = Vector3.Distance(randomPosition, player.transform.position);

                // Überprüfe, ob die Position auf dem NavMesh liegt
            } while (!UnityEngine.AI.NavMesh.SamplePosition(randomPosition, out navMeshHit, minDistanceToPlayer, UnityEngine.AI.NavMesh.AllAreas) || distanceToPlayer < minDistanceToPlayer);

            navAgent.SetDestination(randomPosition);

            oldSpeed = navAgent.speed;
            oldAcceleration = navAgent.acceleration;
            navAgent.acceleration = 100;
            navAgent.speed = 13;
        }


        public override TaskStatus OnUpdate()
        {
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance + 0.7f)
            {    
                animator.SetBool("isWalking", false);
            }
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
            {    
                animator.SetBool("isWalking", false);
                navAgent.speed = oldSpeed;
                navAgent.enabled = false;
                gameObject.transform.LookAt(player.transform);
                return TaskStatus.COMPLETED;
            }
            return TaskStatus.RUNNING;
        }
    }
}
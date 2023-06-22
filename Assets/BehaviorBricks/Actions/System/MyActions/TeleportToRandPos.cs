using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using BBUnity.Actions;        // GOAction

namespace BBUnity.Actions
{
    [Action("MyActions/TeleportToRandPos")]
    public class TeleportToRandPos : GOAction
    {
        private Animator animator;
        private GameObject player;
        private float minDistanceToPlayer;
        private float distanceToPlayer;

        public override void OnStart()
        {
            animator = gameObject.GetComponent<Animator>();
            player = GameObject.FindWithTag("Player");
            minDistanceToPlayer = 10f;

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

            // Teleportiere das GameObject an die zufällige Position
            gameObject.transform.position = navMeshHit.position;

            //Rotation zum Spieler hin anpassen
            gameObject.transform.LookAt(player.transform);
        }


        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
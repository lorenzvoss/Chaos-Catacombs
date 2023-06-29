using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using BBUnity.Actions;        // GOAction

namespace BBUnity.Actions
{
    [Action("MyActions/SkyAttack")]
    public class SkyAttack : GOAction
    {
        private Animator animator;
        private UnityEngine.AI.NavMeshAgent navAgent;
 
        private Vector3 skyPosition;
        private Vector3 targetPosition;
        private float oldSpeed;
        private float oldAcceleration;
        private bool isLaunching;
        private float launchSpeed;
        private float attackSpeed;
        private bool reachedSkyPosition;
        private GameObject player;


        public override void OnStart()
        {
            animator = gameObject.GetComponent<Animator>();
            navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            player = GameObject.FindWithTag("Player");
            targetPosition = player.transform.position;
            targetPosition.y = 0f;
            skyPosition = (gameObject.transform.position + targetPosition)/2  + new Vector3(0f, 15f, 0f);
            isLaunching = true;
            launchSpeed = 20f;
            attackSpeed = 20f;
            reachedSkyPosition = false;

            animator.SetBool("isWalking", false);
            animator.SetTrigger("isJumping");

        }


        public override TaskStatus OnUpdate()
        {
            if (isLaunching)
            {    
                navAgent.enabled = false;
                gameObject.transform.LookAt(player.transform);
                
                if(gameObject.transform.position == skyPosition)
                {
                    reachedSkyPosition = true;
                }

                if(!reachedSkyPosition)
                {    
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, skyPosition, launchSpeed * Time.deltaTime);
                }else{
                    attackSpeed += 1f;
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, attackSpeed * Time.deltaTime);

                    if(gameObject.transform.position == targetPosition)
                    {
                        isLaunching = false;
                    }
                }
            }else{
                gameObject.transform.LookAt(player.transform);
                
                player.GetComponent<PlayerHealth>().isHitByJump = true;
                player.GetComponent<PlayerHealth>().damageShockWave = CalculateDamage();
                //navAgent.enabled = true;
                return TaskStatus.COMPLETED;    
            }
            return TaskStatus.RUNNING;
        }

        private int CalculateDamage()
        {
            float distanceToPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position);
            int maxDamage = 20;
            float maxDamageDistance = 5f;

            if(distanceToPlayer < maxDamageDistance)
            {
                float distanceRatio = 1 - (distanceToPlayer / maxDamageDistance);
                int damage = Mathf.RoundToInt(maxDamage * distanceRatio);
                Debug.Log("Distanz: " + distanceToPlayer);
                Debug.Log("Damage: " + damage);
                return damage;
            }

            return 0;
        }
    }
}

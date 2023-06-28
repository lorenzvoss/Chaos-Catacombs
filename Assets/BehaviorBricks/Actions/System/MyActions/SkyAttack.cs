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


        public override void OnStart()
        {
            animator = gameObject.GetComponent<Animator>();
            navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            targetPosition = GameObject.FindWithTag("Player").transform.position;
            skyPosition = (gameObject.transform.position + targetPosition)/2  + new Vector3(0f, 10f, 0f);
            isLaunching = true;
            launchSpeed = 15f;
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
                gameObject.transform.LookAt(GameObject.FindWithTag("Player").transform);
                
                if(gameObject.transform.position == skyPosition)
                {
                    reachedSkyPosition = true;
                }

                if(!reachedSkyPosition)
                {    
                    launchSpeed -= 0.1f;
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, skyPosition, launchSpeed * Time.deltaTime);
                }else{
                    attackSpeed += 0.1f;
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, attackSpeed * Time.deltaTime);

                    if(gameObject.transform.position == targetPosition)
                    {
                        isLaunching = false;
                    }
                }
            }else{
                gameObject.transform.LookAt(GameObject.FindWithTag("Player").transform);
                navAgent.enabled = true;
                return TaskStatus.COMPLETED;    
            }
            return TaskStatus.RUNNING;
        }
    }
}

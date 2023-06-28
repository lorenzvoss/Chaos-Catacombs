using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using BBUnity.Actions;        // GOAction

namespace BBUnity.Actions
{
    [Action("MyActions/LaserPreparation")]
    public class LaserPreparation : GOAction
    {
        private Animator animator;
        private float timer;
        private float duration;

        public override void OnStart()
        {
            animator = gameObject.GetComponent<Animator>();
            animator.SetBool("isShooting", true);
            animator.SetBool("isWalking", false);
            timer = 0f;
            duration = 2f;
        }


        public override TaskStatus OnUpdate()
        {
            gameObject.transform.LookAt(GameObject.FindWithTag("Player").transform);
            if( timer >= duration)
            {
                return TaskStatus.COMPLETED;
            }
            timer += Time.deltaTime;
            return TaskStatus.RUNNING;
        }
    }
}

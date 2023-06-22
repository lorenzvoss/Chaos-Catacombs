using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using BBUnity.Actions;        // GOAction

namespace BBUnity.Actions
{
    [Action("MyActions/ShootLaserBeams")]
    public class ShootLaserBeams : GOAction
    {
        [InParam("laserPrefab")]
        public GameObject laserPrefab;

        private Animator animator;
        private GameObject player;
        private GameObject shootpointLeft;
        private GameObject shootpointRight;
        private int numberOfLasers;
        private int shotLasers;
        private float fireDelay;
        private float beamDuration;
        private bool shootFromLeftSide;

        public override void OnStart()
        {
            animator = gameObject.GetComponent<Animator>();
            player = GameObject.FindWithTag("Player");
            //laserPrefab = Resources.Load<GameObject>("/Assets/Prefabs/Laserbeam.prefab");
            shootpointLeft = GameObject.Find("ShootpointLeft");
            shootpointRight = GameObject.Find("ShootpointRight");
            numberOfLasers = 100;
            shotLasers = 0;
            //fireDelay = 0.2f;
            shootFromLeftSide = true;

            Debug.Log(laserPrefab);
            Debug.Log(shootpointLeft);
            Debug.Log(shootpointRight);
        }


        public override TaskStatus OnUpdate()
        {
            if(shotLasers < numberOfLasers)
            {

                if(shootFromLeftSide)
                {
                    GameObject.Instantiate(laserPrefab, shootpointLeft.transform.position, Quaternion.identity);
                }else{
                    GameObject.Instantiate(laserPrefab, shootpointRight.transform.position, Quaternion.identity);
                }

            }else{
                return TaskStatus.COMPLETED; 
            }
            return TaskStatus.RUNNING;
        }
    }
}

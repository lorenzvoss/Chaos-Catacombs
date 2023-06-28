using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using BBUnity.Actions;        // GOAction

namespace BBUnity.Actions
{
    [Action("MyActions/ShootLaserBeams")]
    public class ShootLaserBeams : GOAction
    {
        //[InParam("laserPrefab")]
        //public GameObject laserPrefab;

        private Animator animator;
        private GameObject player;
        private GameObject shootpointLeft;
        private GameObject shootpointRight;
        private float beamDuration;
        private float beamTimer;
        private LineRenderer lineRendererLeft;
        private LineRenderer lineRendererRight;
        private Vector3 endPos;

        ///TODOS
        /// - Laser hört noch am Spieler auf
        /// - Größe vom Laser anpassen
        /// - Laser Vorbereitung (Drehung zum Spieler, Schussvorlauf)

        public override void OnStart()
        {
            animator = gameObject.GetComponent<Animator>();
            player = GameObject.FindWithTag("Player");
            shootpointLeft = GameObject.Find("ShootpointLeft");
            shootpointRight = GameObject.Find("ShootpointRight");
            lineRendererLeft = shootpointLeft.GetComponent<LineRenderer>();
            lineRendererRight = shootpointRight.GetComponent<LineRenderer>();
            beamTimer = 0f;
            beamDuration = 3f;
            endPos = player.transform.position + new Vector3(0f, 0.75f, 0f);
            animator.SetBool("isShooting", true);
            gameObject.transform.LookAt(player.transform);
        }


        public override TaskStatus OnUpdate()
        {
            if( beamTimer <= beamDuration)
            {
                beamTimer += Time.deltaTime;
                shootLaser();
                return TaskStatus.RUNNING;
            }
            else{
                animator.SetBool("isShooting", false);
                lineRendererLeft.enabled = false;
                lineRendererRight.enabled = false;
                return TaskStatus.COMPLETED;
            }
        }

        public void shootLaser()
        {
            RaycastHit hit;
            Vector3 startPosLeft = shootpointLeft.transform.position;
            Vector3 startPosRight = shootpointRight.transform.position;

            if (Physics.Raycast(startPosLeft, (endPos - startPosLeft).normalized , out hit))
            {
                // Kollision mit einem GameObject wurde erkannt

                // Zeige den Raycast visuell mit dem Line Renderer
                lineRendererLeft.enabled = true;
                Draw3DRay(lineRendererLeft ,shootpointLeft.transform.position, hit.point); 
            }
            if (Physics.Raycast(startPosRight, (endPos - startPosRight).normalized, out hit))
            {
                // Kollision mit einem GameObject wurde erkannt

                // Zeige den Raycast visuell mit dem Line Renderer
                lineRendererRight.enabled = true;
                Draw3DRay(lineRendererRight ,shootpointRight.transform.position, hit.point);
            }
            
        }

        public void Draw3DRay(LineRenderer ren, Vector3 startPos, Vector3 endPos)
        {
            ren.SetPosition(0, startPos);
            ren.SetPosition(1, endPos);
        }
    }
}

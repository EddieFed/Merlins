using UnityEngine;
using UnityEngine.AI;

namespace __ProjectMain.Scripts.Game
{
    public class ClickRaycast : MonoBehaviour
    {
        public Camera cam;
        public NavMeshAgent agent;
    
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    //move to destination
                    agent.SetDestination(hit.point);
                }
            }
        }
    }
}

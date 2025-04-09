using UnityEngine;

namespace __ProjectMain.Scripts
{
    public class Flock : MonoBehaviour
    {
        private float speed;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
        }

        // Update is called once per frame
        void Update()
        {
            this.transform.Translate(0,0, speed * Time.deltaTime);
        }
    }
}

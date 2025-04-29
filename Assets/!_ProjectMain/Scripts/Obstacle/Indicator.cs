using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace __ProjectMain.Scripts.Obstacle
{
    public class Indicator : MonoBehaviour
    {
        private const float RotationalVelocity = 150.0f;
        
        [SerializeField] private GameObject indicatorPrefab;
        public GameObject indicatorInstance = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                indicatorInstance = Instantiate(indicatorPrefab, this.transform.position + (Vector3.up * 5), Quaternion.Euler(new Vector3(90, 0, 0)));
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Destroy(indicatorInstance);
            }
        }
        
        private void Update()
        {
            if (indicatorInstance == null)
            {
                return;
            }
            
            indicatorInstance.transform.RotateAround(indicatorInstance.transform.position, Vector3.up, RotationalVelocity * Time.deltaTime);
        }
    }
}
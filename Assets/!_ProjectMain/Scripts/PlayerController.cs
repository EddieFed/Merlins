using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace __ProjectMain.Scripts
{
    public class PlayerController : NetworkBehaviour
    {
        private void Update()
        {
            Vector3 movement = new Vector3(0f, 0f, 0f);
            if (Input.GetKey(KeyCode.W))
            {
                movement += new Vector3(10f, 0f, 10f);
            }
            if (Input.GetKey(KeyCode.S))
            {
                movement += new Vector3(-10f, 0f, -10f);
            }
            if (Input.GetKey(KeyCode.A))
            {
                movement += new Vector3(-10f, 0f, 10f);
            }
            if (Input.GetKey(KeyCode.D))
            {
                movement += new Vector3(10f, 0f, -10f);
            }
            transform.Translate(movement * Time.deltaTime);
        }
    }
}

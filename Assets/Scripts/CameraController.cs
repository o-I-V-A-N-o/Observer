using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public void RotateCamera()
        {
            StartCoroutine(MoveCamera());
        }

        private IEnumerator MoveCamera()
        {
            FindObjectOfType<CellsPool>().LockedGame(true);
            float speed = 5f;
            float angle = speed;
            
            while (angle <= 180)
            {
                angle += speed;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, speed);
                yield return new WaitForSeconds(0.02f);
            }
            FindObjectOfType<CellsPool>().LockedGame(false);
        }
    }
}

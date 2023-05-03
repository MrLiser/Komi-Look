using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mfarm.Transition
{
    public class Teleport : MonoBehaviour
    {
        public string sceneToGo;

        public Vector3 positonToGo;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            
            if(collision.CompareTag("Player"))
            {
                EventHandler.CallTransitonEvent(sceneToGo, positonToGo);
            }
        }
    }

}

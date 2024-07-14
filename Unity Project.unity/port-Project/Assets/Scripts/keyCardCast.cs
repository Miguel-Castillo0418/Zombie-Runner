using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace keyCards
{
    public class keyCardCast : MonoBehaviour
    {
        [SerializeField] private int rayLength = 5;
        [SerializeField] private LayerMask layerMaskinteractable;
        [SerializeField] private string excludeLayerName = null;

        private keyCardController rcOBJ;
        [SerializeField] private KeyCode doorKey = KeyCode.E;

        private string interactableTag = "InteractiveObj";
        private bool doOnce;
        private void Update()
        {
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);

            int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskinteractable.value;

            if (Physics.Raycast(transform.position, fwd, out hit, rayLength, mask))
            {
                if (hit.collider.CompareTag(interactableTag))
                {
                    if (!doOnce)
                    {
                        rcOBJ = hit.collider.gameObject.GetComponent<keyCardController>();
                    }
                    doOnce = true;
                    if (Input.GetKeyDown(doorKey))
                    {
                        rcOBJ.ObjInteraction();
                    }
                }
            }
            else
            {
                doOnce = false;
            }
        }
    }
}


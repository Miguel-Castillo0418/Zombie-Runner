using KeyCards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace keyCards
{
    public class keyCardController : MonoBehaviour
    {
        [SerializeField] private bool door = false;
        [SerializeField] private bool key = false;
        [SerializeField] private KeycardInventory _keycards = null;
        [SerializeField] GameObject keycard;

        private lockedDoorController doorObj;
        private void Awake()
        {
            keycard = GameObject.FindGameObjectWithTag("keycardInv");
            _keycards = keycard.GetComponent<KeycardInventory>();
        }
        void Start()
        {
            if (door)
            {
                doorObj = GetComponent<lockedDoorController>();
            }
        }
        public void ObjInteraction()
        {
            if (door)
            {
                doorObj.PlayAnim();
            }
            else if (key)
            {
                _keycards.hasKeyCard = true;
                gameObject.SetActive(false);
            }
        }
    }
}
using KeyCards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace keyCards
{
    public class keyCardController : MonoBehaviour
    {
        [SerializeField] private bool door = false;
        [SerializeField] private bool codedDoor = false;
        [SerializeField] private bool key = false;
        [SerializeField] private KeycardInventory _keycards = null;
        [SerializeField] GameObject keycard;

        private lockedDoorController doorObj;
        private lockedDoorController codedDoorObj;
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
            else if (codedDoor)
            {
                codedDoorObj = GetComponent<lockedDoorController>();
            }
        }
        public void ObjInteraction()
        {
            if (door)
            {
                doorObj.PlayAnim();
            }
            else if (codedDoor)
            {
                codedDoorObj.PlayAnim();
            }
            else if (key)
            {
                AudioManager.instance.keyPickup();
                _keycards.hasKeyCard = true;
                gameObject.SetActive(false);
                doorObj.PlayAnim();
            }
        }
    }
}
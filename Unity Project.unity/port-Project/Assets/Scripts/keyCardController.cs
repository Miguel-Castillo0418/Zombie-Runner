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

        [SerializeField] private bool redDoor = false;
        [SerializeField] private bool lightBlueDoor = false;
        [SerializeField] private bool purpleDoor = false;
        [SerializeField] private bool darkBlueDoor = false;

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
            // Initializing new doors
            else if (redDoor || lightBlueDoor || purpleDoor || darkBlueDoor)
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
            else if (codedDoor)
            {
                codedDoorObj.PlayAnim();
            }
            else if (key)
            {
                AudioManager.instance.keyPickup();
                _keycards.hasKeyCard = true;
                gameObject.SetActive(false);
            }
            // Interaction for new doors
            else if (redDoor && _keycards.hasRedKey)
            {
                doorObj.PlayAnim();
            }
            else if (lightBlueDoor && _keycards.hasLightBlueKey)
            {
                doorObj.PlayAnim();
            }
            else if (purpleDoor && _keycards.hasPurpleKey)
            {
                doorObj.PlayAnim();
            }
            else if (darkBlueDoor && _keycards.hasDarkBlueKey)
            {
                doorObj.PlayAnim();
            }
        }
    }
}

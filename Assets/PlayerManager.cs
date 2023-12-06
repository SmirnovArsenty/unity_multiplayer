using UnityEngine;
using UnityEngine.EventSystems;

// using Photon.Pun;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Com.MyCompany.MyGame
{
    /// <summary>
    /// Player manager.
    /// </summary>
    public class PlayerManager : MonoBehaviour//PunCallbacks, IPunObservable
    {
        #region Private Fields

        public GameObject prefab;

        [SerializeField]
        public GameObject tank;

        private ChobiAssets.KTP.ID_Control_CS idControl;
        private ChobiAssets.KTP.Wheel_Control_CS wheelControl;
#if !UNITY_ANDROID && !UNITY_IPHONE
        private ChobiAssets.KTP.Wheel_Control_Input_01_Desktop_CS inputScript;
#else
        private ChobiAssets.KTP.Wheel_Control_Input_02_Mobile_CS inputScript;
#endif
        #endregion

        #region MonoBehaviour CallBacks

        void Awake()
        {
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            if (tank == null) {
                Thread.Sleep(1000);
                // tank = PhotonNetwork.Instantiate(prefab.name, new Vector3(0, 0, 0), Quaternion.identity, 0);

                SetupControls();
            }
        }

        void SetupControls()
        {
            idControl = tank.GetComponent<ChobiAssets.KTP.ID_Control_CS>();
            wheelControl = idControl.GetComponent<ChobiAssets.KTP.Wheel_Control_CS>();
            if (wheelControl == null) {
                return;
            }
#if !UNITY_ANDROID && !UNITY_IPHONE
            inputScript = wheelControl.GetComponent<ChobiAssets.KTP.Wheel_Control_Input_01_Desktop_CS>();
#else
            inputScript = wheelControl.GetComponent<ChobiAssets.KTP.Wheel_Control_Input_02_Mobile_CS>();
#endif
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {
            if (wheelControl == null || inputScript == null) {
                SetupControls();
            }
            if (tank == null) {
                return;
            }
            if (tank.GetComponent<ChobiAssets.KTP.Damage_Control_CS>() == null)
            {
                return;
            }
            // trigger Beams active state
            // if (beams != null && IsFiring != beams.activeInHierarchy)
            // {
            //     beams.SetActive(IsFiring);
            // }

            // if (photonView.IsMine)
            // {
            //     if (tank.GetComponent<ChobiAssets.KTP.Damage_Control_CS>().currentDurability <= 0f)
            //     {
            //         GameManager.Instance.LeaveRoom();
            //     }
            // }
        }

        #endregion

        #region IPunObservable implementation

        // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        // {
        //     if (stream.IsWriting)
        //     {
        //         // We own this player: send the others our data
        //         stream.SendNext(tank.GetComponent<ChobiAssets.KTP.Damage_Control_CS>().currentDurability);
        //     }
        //     else
        //     {
        //         // Network player, receive data
        //         tank.GetComponent<ChobiAssets.KTP.Damage_Control_CS>().currentDurability = (float)stream.ReceiveNext();
        //     }
        // }

        #endregion
    }
}
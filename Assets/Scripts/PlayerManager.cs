using UnityEngine;
using UnityEngine.EventSystems;

using Photon.Pun;

using System.Collections;

namespace Com.MyCompany.MyGame
{
    /// <summary>
    /// Player manager.
    /// Handles fire Input and Beams.
    /// </summary>
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Private Fields

        // [Tooltip("The Beams GameObject to control")]
        // [SerializeField]
        // private GameObject beams;
        public GameObject prefab;
        private GameObject tank;
        // public float Health;
        //True, when the user is firing
        bool IsFiring;
        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // if (beams == null)
            // {
            //     Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            // }
            // else
            // {
            //     beams.SetActive(false);
            // }
        }

        void Start()
        {
            tank = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {
            ProcessInputs();

            float health = tank.GetComponent<ChobiAssets.KTP.Damage_Control_CS>().Get_Health();
            // trigger Beams active state
            // if (beams != null && IsFiring != beams.activeInHierarchy)
            // {
            //     beams.SetActive(IsFiring);
            // }

            if (photonView.IsMine)
            {
                ProcessInputs();
                if (health <= 0f)
                {
                    GameManager.Instance.LeaveRoom();
                }
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>
        void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!IsFiring)
                {
                    IsFiring = true;
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                if (IsFiring)
                {
                    IsFiring = false;
                }
            }
        }

        #endregion

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(IsFiring);
            }
            else
            {
                // Network player, receive data
                this.IsFiring = (bool)stream.ReceiveNext();
            }
        }

        #endregion
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChobiAssets.KTP
{

    public class Game_Controller_CS : MonoBehaviour
    {
        /*
		 * This script is attached to the "Game_Controller" in the scene.
		 * This script controls the physics settings, the layers collision settings and the cursor state in the scene.
		 * Also the general functions such as quit and pause are controlled by this script.
		*/

        [Header("General functions settings")]
        [Tooltip("Prefab for touch controls.")] public GameObject touchControlsPrefab;
        [Tooltip("These gameobjects will be removed on mobile platforms.")] public GameObject[] uselessObjectsOnMobiles;
#if !UNITY_ANDROID && !UNITY_IPHONE
        [Tooltip("Show cursor or not.")] public bool showCursor = false;
#endif


        [HideInInspector] public List<ID_Control_CS> idScriptsList = new List<ID_Control_CS>();
        [HideInInspector] public int currentID = 0;
        bool isPaused;
#if UNITY_ANDROID || UNITY_IPHONE
        bool isPauseButtonDown;
        bool isSwitchButtonDown;
#else
        bool storedCursorVisible;
#endif

        [HideInInspector] public static Game_Controller_CS instance;


        void Awake()
        {
            instance = this;

            // Layer settings.
            Layer_Settings_CS.Layers_Collision_Settings();

            // Set the frame rate.
            if (General_Settings_CS.fixFrameRate)
            {
                QualitySettings.vSyncCount = 0;
#if !UNITY_ANDROID && !UNITY_IPHONE
                Application.targetFrameRate = General_Settings_CS.targetFrameRate;
#else
                Application.targetFrameRate = General_Settings_CS.targetFrameRateMobile;
#endif
            }

            // Set the Fixed Timestep in the scene.
#if !UNITY_ANDROID && !UNITY_IPHONE
            Time.fixedDeltaTime = General_Settings_CS.fixedTimestep;
#else
            Time.fixedDeltaTime = General_Settings_CS.fixedTimestepMobile;
#endif

#if UNITY_ANDROID || UNITY_IPHONE
            // Instantiate the touch controls prefab.
            if (touchControlsPrefab)
            {
                Instantiate(touchControlsPrefab);
            }

            // Remove the useless objects on mobile platforms.
            for (int i = 0; i < uselessObjectsOnMobiles.Length; i++)
            {
                Destroy(uselessObjectsOnMobiles[i]);
            }
#else
            // Set the cursor state.
            Switch_Cursor(showCursor);
#endif
        }


        public void Receive_ID_Script(ID_Control_CS idScript)
        { // Called from "ID_Control_CS" in tanks in the scene, when the tank is spawned.
            // Store the "ID_Control_CS".
            idScriptsList.Add(idScript);
        }


        void Update()
        {
            // Quit.
            if (Key_Bindings_CS.IsQuitKeyDown())
            {
                Application.Quit();
                return;
            }

            // Control the cursor state.
#if !UNITY_ANDROID && !UNITY_IPHONE
            if (Key_Bindings_CS.IsSwitchCursorModeKeyDown())
            {
                Switch_Cursor(Cursor.visible == false);
            }
#endif
        }


        public void Switch_Cursor(bool isVisible)
        {
            if (isVisible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }



        public void Remove_ID(ID_Control_CS idScript)
        { // Called from "ID_Control_CS", when the tank is removed from the scene.
            // Remove the "ID_Control_CS" from the list.
            idScriptsList.Remove(idScript);
        }

    }
}
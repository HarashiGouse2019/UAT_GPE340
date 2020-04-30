using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagTracker : MonoBehaviour
{
    private static FlagTracker Instance;
    /*Keep track of the status of flags in the Enviornment Map*/

    [Serializable]
    public class FlagStatus
    {
        [SerializeField] private FlagID flagIndex;
        [SerializeField] private float percentageTilCapture = 0f;
        [SerializeField] private bool captured;

       
        private const bool YES = true;

        //The default gain amount
        private const float GAIN = 0.01f;

        private const float CAPTURE_VALUE = 1f;

        public void UpdatePercentage()
        {
            //Only update percentage if not captured
            if (!captured)
            {
                //Turn on our flag capture meter
                GetCaptureMeter().gameObject.SetActive(true);

                //The moddedGain will depend on how many flags the player has.
                //This value is what makes it take longer to capture a flag.
                float moddedGain = (float)Instance.capturedFlags.Count + 1f;

                //We have moddedGain increase exponentially to really show that it's harder to capture a flag.
                percentageTilCapture += GAIN / (moddedGain * moddedGain);

                //Update Capture Meter
                GetCaptureMeter().value = percentageTilCapture;

                //Check if capture meter is 100%
                CheckStatus();
            }
                
        }

        public void SetID(FlagID _value)
        {
            flagIndex = _value;
        }

        public void SetAsCaptured()
        {
            //Update Player Spawn Position
            GameManager.UpdatePlayerSpawnerPosition(flagIndex.gameObject.transform.position);

            //Turn off our capture meter
            GetCaptureMeter().gameObject.SetActive(false);

            //Say that "YES!!! WE CAPTURED IT!!"
            captured = YES;

            //Print that we got it, and what flag id it was.
            GameManager.PrintInfoLog("Flag " + flagIndex.id + " has been captured");

            //Add to our captured flags
            Instance.capturedFlags.Add(flagIndex);

            //Set this flag to inactive.
            flagIndex.gameObject.SetActive(false);
        }
        public FlagID GetFlagID() => flagIndex;
        public float GetPercentageTilCapture() => percentageTilCapture;
        public bool Captured() => captured;
        void CheckStatus()
        {
            if (percentageTilCapture >= CAPTURE_VALUE)
                SetAsCaptured();
        }
    }

    //Information of existing IDs
    public List<FlagStatus> existingID = new List<FlagStatus>();

    //Captured Flags
    public List<FlagID> capturedFlags = new List<FlagID>();

    //Capturing Metter
    public Slider captureMeter;

    // Start is called before the first frame update
    void Awake()
    {
        #region SINGLETON
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    void Start()
    {
        //Scan all flags in current area
        ScanFlags();
    }

    /// <summary>
    /// Scans all existing flags in the current Environmental Map
    /// </summary>
    void ScanFlags()
    {
        //Find all flags in scene
        FlagID[] existingFlags = FindObjectsOfType<FlagID>();

        //Start of with an assigning value of 0
        int assignedID = 0;

        foreach (FlagID flagID in existingFlags)
        {
            //We assing id to the flag
            flagID.id = assignedID;

            //Register it
            RegisterID(flagID);

            //Then increase our assigned value, since the previous one is already useds
            assignedID++;
        }
    }

    /// <summary>
    /// Register a FlagID
    /// </summary>
    /// <param name="_flagID"></param>
    public static void RegisterID(FlagID _flagID)
    {
        //If list has data, we'll add more
        if (Instance.existingID.Count != 0)
        {
            FlagStatus newStatus = new FlagStatus();
            newStatus.SetID(_flagID);
            Instance.existingID.Add(newStatus);
            return;
        }
        //If not, this means there's no data.
        else
        {
            //This also means this is the first registry
            FlagStatus newStatus = new FlagStatus();
            newStatus.SetID(_flagID);
            Instance.existingID.Add(newStatus);
            return;
        }
    }

    public static FlagStatus DetectFlag(FlagID _flag)
    {
        //We'll search for the matching flag
        foreach (FlagStatus flagStatus in Instance.existingID)
        {
            //If it matches, we'll return the object to whoever needs it
            if (flagStatus.GetFlagID().id == _flag.id)
                return flagStatus;
        }

        //Otherwise, we found nothing
        return null;
    }

    public static Slider GetCaptureMeter() => Instance.captureMeter;

    public static List<FlagID> GetCapturedFlags() => Instance.capturedFlags;
}

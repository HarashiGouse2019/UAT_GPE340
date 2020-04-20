using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFlagCase : MonoBehaviour
{
    private static BaseFlagCase Instance;

    /*Place all captured flags into this case*/
    public static List<FlagID> flagAmmount = new List<FlagID>();

    //The max value
    private const int FLAGCAPACITY = 5;

    //The amount of flags left
    public static int FlagsLeft = FLAGCAPACITY;

    //The percentage
    public static float percentageUntilFlagInserted = 0f;

    //The gain
    public const float GAIN = 0.01f;

    //Inserted Value
    public static float INSERTED_FLAG_VALUE = 1f;

    //Inserted
    public static bool Inserted = false;

    public const bool YES = true;

    void Awake()
    {
        Instance = this;
    }

    public static void AddFlagIntoCase(FlagID _flag)
    {
        flagAmmount.Add(_flag);
        FlagsLeft = FLAGCAPACITY - flagAmmount.Count;
        GameManager.PrintInfoLog("Flag " + _flag.id + "has been inserted");
        CheckStatus();
    }

    public static void CheckStatus()
    {
        if(FlagsLeft == 0)
        {
            GameManager.PrintInfoLog("VICTORY IS YOURS!!!");
            GameManager.SetResultsValue(FlagsLeft);
            GameCameraControls.ReturnToInitialPosition();
            UINavigator.Instance.Goto("RESULTS");
        }
    }
}

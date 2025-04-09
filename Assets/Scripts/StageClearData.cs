using UnityEngine;

public static class StageClearData
{
    public static bool[] stageClear = new bool[5];

    public static void ResetAll()
    {
        for (int i = 0; i < stageClear.Length; i++)
        {
            stageClear[i] = false;
        }
    }


}

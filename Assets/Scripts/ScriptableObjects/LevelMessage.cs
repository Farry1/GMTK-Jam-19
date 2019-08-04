using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LevelDescription", menuName = "Custom/LevelDescription")]
public class Levelmessage : ScriptableObject
{    
    public string levelNumber;
    public string levelName;
    public string levelDescription;

    public string additionalGameOverText;
}
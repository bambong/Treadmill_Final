using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnInfo : MonoBehaviour
{
    public string TargetName { get { return targetName; } }
    [SerializeField] string targetName;
}

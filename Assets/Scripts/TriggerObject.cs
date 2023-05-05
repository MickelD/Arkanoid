using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerObject : MonoBehaviour
{
    public abstract void OnPickUpItem(PlayerMovement player);
}

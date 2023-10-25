using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Activatable> activatables = new List<Activatable>();

    protected void Activate() => activatables.ForEach(x => x.Activate());
}

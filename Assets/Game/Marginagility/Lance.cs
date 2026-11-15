using Marginagility;
using UnityEngine;

public class Lance : MonoBehaviour
{
    [SerializeField] MarginagilityPawn _owner;

    public MarginagilityPawn GetOwner() {
        return _owner;
    }
}

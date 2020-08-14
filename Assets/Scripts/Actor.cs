using UnityEngine;

// This class contains general information describing an actor (player or enemies).
// It is mostly used for AI detection logic and determining if an actor is friend or foe
public class Actor : MonoBehaviour
{
    [Tooltip(
        "Represents the affiliation (or team) of the actor. Actors of the same affiliation are friendly to eachother")]
    public int affiliation;

    [Tooltip("Represents point where other actors will aim when they attack this actor")]
    public Transform aimPoint;
}
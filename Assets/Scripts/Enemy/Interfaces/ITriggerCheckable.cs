using UnityEngine;

public interface ITriggerCheckable
{
    bool IsWithinStrikingDistance { get; set; }
    
    bool SetStrikingDistance(bool isWithinStrikingDistance);
}
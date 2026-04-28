using Unity.Cinemachine;
using UnityEngine;

public class MultiplayerTargetGroupManager : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup targetGroup;

    [SerializeField] private float defaultWeight = 1.0f;
    [SerializeField] private float defaultRadius = 2.0f;


    void Awake()
    {
        if (targetGroup == null)
        {
            targetGroup = FindFirstObjectByType<CinemachineTargetGroup>();
        }    
    }

    public void RegisterTarget(Transform target)
    {
        if (targetGroup == null || target == null)
            return;

        targetGroup.AddMember(target,defaultWeight,defaultRadius);
    }

    public void UnregisterTarget(Transform target)
    {
        if (targetGroup == null || target == null)
            return;

        targetGroup.RemoveMember(target);
    }

}

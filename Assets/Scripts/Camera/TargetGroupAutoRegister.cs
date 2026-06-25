using UnityEngine;

public class TargetGroupAutoRegister : MonoBehaviour
{

    [SerializeField] public MultiplayerTargetGroupManager targetGroupManager;

    public static TargetGroupAutoRegister instance;


    void Start()
    {
        instance  = this;

        targetGroupManager = FindFirstObjectByType<MultiplayerTargetGroupManager>();

        if (targetGroupManager != null)
        {
            targetGroupManager.RegisterTarget(transform);
        }

    }

    public void OnDestroy()
    {
        if (targetGroupManager != null) {targetGroupManager.UnregisterTarget(transform); }
    }


}

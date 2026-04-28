using UnityEngine;

public class TargetGroupAutoRegister : MonoBehaviour
{

    [SerializeField] MultiplayerTargetGroupManager targetGroupManager;

    void Start()
    {
        targetGroupManager = FindFirstObjectByType<MultiplayerTargetGroupManager>();

        if (targetGroupManager != null)
        {
            targetGroupManager.RegisterTarget(transform);
        }

    }

    private void OnDestroy()
    {
        if (targetGroupManager != null) {targetGroupManager.UnregisterTarget(transform); }
    }


}

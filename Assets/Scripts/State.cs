 using UnityEngine;

public abstract class State
{
    public abstract float ts { get; }

    public virtual void Enter(GameManager manager)
    {
        Time.timeScale = ts;
    }

}

public class GameplayState : State
{
    public override float ts => 1f;

    public override void Enter(GameManager manager)
    {
        base.Enter(manager);

        manager.StartUI.SetActive(false);
    }
}

public class PauseState : State
{
    public override float ts => 0f;

    public override void Enter(GameManager manager)
    {
        base.Enter(manager);

       // manager.PauseUI.SetActive(true);
    }
}

public class UpgradeState : State
{
    public override float ts => 0f;

    public override void Enter(GameManager manager)
    {
        base.Enter(manager);

       // manager.PauseUI.SetActive(false);
    }
}

public class StartState : State
{
    public override float ts => 1f;

    public override void Enter(GameManager manager)
    {
        base.Enter(manager);

        manager.StartUI.SetActive(true);
    }
}


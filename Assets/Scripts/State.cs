using JetBrains.Annotations;
using UnityEngine;
using System.Collections.Generic;

public abstract class State
{
    public abstract float ts { get;}

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
        
        manager.WonUI.SetActive(false);
        manager.UpgradeUI.SetActive(false);
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
    public override float ts => 1f;

    public override void Enter(GameManager manager)
    {
        base.Enter(manager);

       manager.StartUI.SetActive(false);
       manager.WonUI.SetActive(false);
       manager.UpgradeUI.SetActive(true);


    //    foreach(PlayerController player in manager.spawnedPlayers)
    //    {
    //     Deck deck  = player.GetComponent<Deck>();
    //      if (deck == null) continue;

    //      List<ScriptableCard> offer = CardOfferSystem.Instance.GenerateOffer(player);
    //      CardChoiceUI.Instance.ShowForPlayer(player, offer, deck);
    //    }
    UpgradeManager.Instance.StartUpgradePhase();
    }
}

public class StartState : State
{
    public override float ts => 1f;

    public override void Enter(GameManager manager)
    {
        base.Enter(manager);

        manager.WonUI.SetActive(false);
       manager.UpgradeUI.SetActive(false);
        manager.StartUI.SetActive(true);
    }
}

public class WonState : State
{
    public override float ts => 1f;

    public override void Enter(GameManager manager)
    {
        base.Enter(manager);

        manager.WonUI.SetActive(true);
       manager.UpgradeUI.SetActive(false);
        manager.StartUI.SetActive(false);
    }
}
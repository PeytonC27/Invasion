using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    TMP_Text phaseText;
    TMP_Text infoText;
    TMP_Text manaText;

    // Start is called before the first frame update
    void Start()
    {
        Transform child = transform.GetChild(0);
        phaseText = child.GetChild(0).GetComponent<TMP_Text>();
        infoText = child.GetChild(1).GetComponent<TMP_Text>();
        manaText = child.GetChild(2).GetComponent<TMP_Text>();
    }

    public void UpdateManaDisplay(int mana, bool isActionPhase)
    {
        if (!isActionPhase)
            manaText.text = "";
        else
            manaText.text = "Current Mana: " + mana;
    }

    /// <summary>
    /// Updates the phase display on the right side of the screen
    /// </summary>
    /// <param name="cardPhase"></param>
    /// <param name="actionPhase"></param>
    public void UpdatePhaseDisplay(bool cardPhase, bool actionPhase)
    {
        if (cardPhase)
            phaseText.text = "Card Phase";
        else if (actionPhase)
            phaseText.text = "Action Phase";
        else
            phaseText.text = "Enemy Phase";
    }

    /// <summary>
    /// Updates the info display when the user hovers their mouse over a card
    /// </summary>
    /// <param name="card">The card to display the info of</param>
    /// <param name="phaseActivation">The phase that enables the text</param>
    public void UpdateInfoDisplay(Card card, bool isActionPhase)
    {
        if (card is null || !isActionPhase)
        {
            infoText.text = "";
            return;
        }
        else if (card is HeroCard)
        {
            HeroCard hero = card as HeroCard;
            infoText.text = string.Format("Name: {0}\nDefense: {1}\nAttack: {2}\nOn Cooldown: {3}\nDamage Buff: {4}\nProtected: {5}",
                hero.Name,
                hero.Defense,
                hero.Damage,
                hero.OnCooldown,
                hero.Empower,
                hero.Protected
            );
        }
        else if (card is EnemyCard)
        {
            if (card is Goblin)
                (card as Goblin).UpdateGobDamage(transform.parent.GetChild(0).GetComponent<Board>());
            EnemyCard enemy = card as EnemyCard;
            infoText.text = string.Format("Name: {0}\nHealth: {1}\nAttack: {2}\nMove Speed: {3}",
                enemy.Name,
                enemy.Health,
                enemy.Damage,
                enemy.MoveSpeed
            );
        }
    }
}

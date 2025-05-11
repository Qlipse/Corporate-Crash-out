using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Updates the BattleHud object to reflect player's health bar, name, and level.
public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    Person _person;

    public void SetData(Person person)
    {
        _person = person;
        nameText.text = person.Base.Name;
        levelText.text = "Lvl " + person.Level;
        hpBar.SetHP((float) person.HP / person.MaxHp);
    }

    public IEnumerator UpdateHP() 
    {
        yield return hpBar.SetHPSmooth((float)_person.HP / _person.MaxHp);
    }

    public void UpdateLevel()
    {
        levelText.text = "Lvl " + _person.Level;
    }
}

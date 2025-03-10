using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleChar : MonoBehaviour
{
    [SerializeField] PersonBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerChar;

    public Person Person { get ; set; }

    Image image;
    Vector3 oriPos;
    Color oriColor;

    // Called once before application is even ran.
    // Sets image to the Person char.
    // Records the image's local position (relative position)
    // Records the image's color
    private void Awake()
    {
        image = GetComponent<Image>();
        oriPos = image.transform.localPosition;
        oriColor = image.color;
    }

    // Fetches data from connected objects through SerializeField.
    public void Setup()
    {
        Person = new Person(_base, level);
        if(isPlayerChar)
            image.sprite = Person.Base.BackSprite;
        else
            image.sprite = Person.Base.FrontSprite;
        image.color = oriColor;
        PlayEnterAnimation();
    }

    // Depending on whether it is a PlayerChar or not and performing the proper Enter Animation.
    public void PlayEnterAnimation()
    {
        if (isPlayerChar)
        {
            image.transform.localPosition = new Vector3(-500f, oriPos.y);
        } else 
        {
            image.transform.localPosition = new Vector3(500f, oriPos.y);
        }

        image.transform.DOLocalMoveX(oriPos.x, 1f);
    }

    // Depending on whether it is a PlayerChar or not and performing the proper Attack Animation.
    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerChar)
            sequence.Append(image.transform.DOLocalMoveX(oriPos.x + 50f, 0.25f));
        else
            sequence.Append(image.transform.DOLocalMoveX(oriPos.x + -50f, 0.25f));

        sequence.Append(image.transform.DOLocalMoveX(oriPos.x, 0.25f));
    }

    // When called, the current Char blinks gray to show damage has been done.
    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(oriColor, 0.1f));
    }

    // When called, the current Char moves downwards and fades away.
    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(oriPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }
}
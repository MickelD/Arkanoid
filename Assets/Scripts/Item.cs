using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : TriggerObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private bool _expiresInstantly;
    [SerializeField] private float _itemDuration;

    protected PlayerMovement p_playerMovement;

    public override void OnPickUpItem(PlayerMovement player)
    {
        p_playerMovement = player;

        ExecuteAction();
    }

    public virtual void ExecuteAction()
    {
        if (_expiresInstantly)
        {
            ExitAction();
        }
        else
        {
            StartCoroutine(DurationCounter());
        }
    }

    public virtual void ExitAction()
    {

        Destroy(gameObject);
    }

    private IEnumerator DurationCounter()
    {
        yield return new WaitForSeconds(_itemDuration);

        ExitAction();
    }
}

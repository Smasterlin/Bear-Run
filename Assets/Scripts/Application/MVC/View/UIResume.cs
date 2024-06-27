using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIResume : View
{
    [SerializeField] Image img_count;
    [SerializeField] Sprite[] sprite_counts;
    GameModel gm;
    public override string Name { get { return Const.V_Resume; } }

    private void Awake()
    {
        gm = GetModel<GameModel>();
    }
    public override void HandleEvent(string name, object data)
    {
        
    }
    public void StartCount()
    {
        Show();
        StartCoroutine(StartCountCor());
    }
    IEnumerator StartCountCor()
    {
        int index = 3;
        while (index > 0)
        {
            index--;
            img_count.sprite = sprite_counts[index];
            yield return new WaitForSeconds(1);
            if (index <= 0)
            {
                break;
            }
        }
        SendEvent(Const.E_ContinueGame);
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.Serialization;

public class HorizontalCardHolder : MonoBehaviour
{
    [FormerlySerializedAs("selectedCard")]
    [SerializeField]
    private CardInput selectedCardInput;
    [FormerlySerializedAs("hoveredCard")]
    [SerializeReference]
    private CardInput hoveredCardInput;

    [SerializeField]
    private GameObject dudSlotPrefab;
    [SerializeField]
    private GameObject roseSlotPrefab;
    [SerializeField]
    private GameObject skullSlotPrefab;
    private RectTransform rect;

    [Header("Spawn Settings")]
    [SerializeField]
    private int rosesToSpawn = 3;
    public List<CardInput> cards;

    bool isCrossing = false;
    [SerializeField]
    private bool tweenCardReturn = true;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        for (int i = 0; i < rosesToSpawn; i++)
        {
            Instantiate(roseSlotPrefab, transform);
        }

        Instantiate(dudSlotPrefab, transform);
        Instantiate(skullSlotPrefab, transform);

        rect = GetComponent<RectTransform>();
        cards = GetComponentsInChildren<CardInput>().ToList();

        int cardCount = 0;

        foreach (CardInput card in cards)
        {
            card.PointerEnterEvent.AddListener(CardPointerEnter);
            card.PointerExitEvent.AddListener(CardPointerExit);
            card.BeginDragEvent.AddListener(BeginDrag);
            card.EndDragEvent.AddListener(EndDrag);
            card.name = cardCount.ToString();
            cardCount++;
        }

        StartCoroutine(Frame());

        IEnumerator Frame()
        {
            yield return new WaitForSecondsRealtime(.1f);
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].cardVisual != null)
                    cards[i].cardVisual.UpdateIndex(transform.childCount);
            }
        }
    }

    private void BeginDrag(CardInput cardInput)
    {
        selectedCardInput = cardInput;
    }


    void EndDrag(CardInput cardInput)
    {
        if (selectedCardInput == null)
            return;

        selectedCardInput.transform
            .DOLocalMove(selectedCardInput.selected ? new Vector3(0, selectedCardInput.selectionOffset, 0) : Vector3.zero,
                tweenCardReturn ? .15f : 0).SetEase(Ease.OutBack);

        rect.sizeDelta += Vector2.right;
        rect.sizeDelta -= Vector2.right;

        selectedCardInput = null;
    }

    void CardPointerEnter(CardInput cardInput)
    {
        hoveredCardInput = cardInput;
    }

    void CardPointerExit(CardInput cardInput)
    {
        hoveredCardInput = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (hoveredCardInput != null)
            {
                Destroy(hoveredCardInput.transform.parent.gameObject);
                cards.Remove(hoveredCardInput);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            foreach (CardInput card in cards)
            {
                card.Deselect();
            }
        }

        if (selectedCardInput == null)
            return;

        if (isCrossing)
            return;

        for (int i = 0; i < cards.Count; i++)
        {
            if (selectedCardInput.transform.position.x > cards[i].transform.position.x)
            {
                if (selectedCardInput.ParentIndex() < cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }

            if (selectedCardInput.transform.position.x < cards[i].transform.position.x)
            {
                if (selectedCardInput.ParentIndex() > cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }
        }
    }

    void Swap(int index)
    {
        isCrossing = true;

        Transform focusedParent = selectedCardInput.transform.parent;
        Transform crossedParent = cards[index].transform.parent;

        cards[index].transform.SetParent(focusedParent);
        cards[index].transform.localPosition =
            cards[index].selected ? new Vector3(0, cards[index].selectionOffset, 0) : Vector3.zero;
        selectedCardInput.transform.SetParent(crossedParent);

        isCrossing = false;

        if (cards[index].cardVisual == null)
            return;

        bool swapIsRight = cards[index].ParentIndex() > selectedCardInput.ParentIndex();
        cards[index].cardVisual.Swap(swapIsRight ? -1 : 1);

        //Updated Visual Indexes
        foreach (CardInput card in cards)
        {
            card.cardVisual.UpdateIndex(transform.childCount);
        }
    }
}
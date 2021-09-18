using DartsGames;
using System;
using System.Collections;
using UnityEngine;

[ExtendEditor]
public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private WorldGrid loadingTilesGrid;
    [SerializeField]
    private Vector3 fullSize, shrinkedSize;
    [SerializeField]
    private float animationTime, cooldownBetweenColumns;

    public void ShowScreen(Action OnComplete = null)
    {
        loadingTilesGrid.gameObject.SetActive(true);
        StartCoroutine(AnimateGrid(fullSize, OnComplete));
    }

    public void HideScreen(Action OnComplete = null) => StartCoroutine(AnimateGrid(shrinkedSize, () => {
        loadingTilesGrid.gameObject.SetActive(false);
        OnComplete?.Invoke();
        }));

    private IEnumerator AnimateGrid(Vector3 newSize, Action onComplete)
    {
        var w = loadingTilesGrid.Width;
        var count = loadingTilesGrid.GridObjects.Count;

        for(int i = 0; i < count; i += w)
        {
            for(int y = 0; y < w; y++)
            {
                var tween = LeanTween.scale(loadingTilesGrid.GridObjects[i + y],
                    newSize, animationTime).setIgnoreTimeScale(true);

                if (i == count - w && y == w - 1)
                    tween.setOnComplete(onComplete);
            }

            yield return new WaitForSecondsRealtime(cooldownBetweenColumns);
        }
    }
}
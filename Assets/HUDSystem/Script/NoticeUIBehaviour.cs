using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationTween;

public class NoticeUIBehaviour : MonoBehaviour
{
    RectTransform _transform;
    public RectTransform noticeRect;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NoticeLiftSpan());
    }

    public IEnumerator NoticeLiftSpan()
    {
        float time = 0;
        float duration = 0.25f;
        while(time < duration)
        {
            float x = Mathf.Lerp(-400f, 0, Tween.EaseInOut(time/duration, 5f, 3f));
            Vector2 newPos = new Vector2(x, 0f);
            noticeRect.anchoredPosition = newPos;
            yield return null;
            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        float time2 = 0;
        float duration2 = 0.75f;
        while (time2 < duration2)
        {
            float x = Mathf.Lerp(0, -400f, Tween.EaseOut(time2 / duration2, 3f));
            Vector2 newPos = new Vector2(x, 0f);
            noticeRect.anchoredPosition = newPos;
            yield return null;
            time2 += Time.deltaTime;
            yield return null;
        }

        Destroy(_transform.gameObject);
    }
}

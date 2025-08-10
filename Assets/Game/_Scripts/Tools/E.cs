using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public static class E
{
    public static Vector3 Clamp(Vector3 value, float min, float max)
    {
        return new Vector3(
            Mathf.Clamp(value.x, min, max),
            Mathf.Clamp(value.y, min, max),
            Mathf.Clamp(value.z, min, max)
        );
    }

    public static Vector3 FindClosest(Vector3 value, Vector3[] values)
    {
        var minDistance = float.MaxValue;
        var minIndex = 0;
        for (var i = 0; i < values.Length; i++)
        {
            var distance = Vector3.Distance(value, values[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                minIndex = i;
            }
        }

        return values[minIndex];
    }

    public static Vector3 FindClosest(Vector3 value, List<Vector3> values)
    {
        var minDistance = float.MaxValue;
        var minIndex = 0;
        for (var i = 0; i < values.Count; i++)
        {
            var distance = Vector3.Distance(value, values[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                minIndex = i;
            }
        }

        return values[minIndex];
    }

    public static UniTask ToUniTask(this Animator animator, string stateName, int layerIndex = 0,
        CancellationToken cancellationToken = default)
    {
        if (animator == null || !animator.isActiveAndEnabled)
        {
            return UniTask.CompletedTask;
        }

        if (!animator.HasState(layerIndex, Animator.StringToHash(stateName)))
        {
            Debug.LogError($"State {stateName} not found in Animator");
            return UniTask.CompletedTask;
        }

        var completionSource = new UniTaskCompletionSource();

        CheckAnimationState().Forget();

        return completionSource.Task.AttachExternalCancellation(cancellationToken);

        async UniTaskVoid CheckAnimationState()
        {
            try
            {
                await UniTask.WaitUntil(() =>
                        animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName),
                    cancellationToken: cancellationToken);

                await UniTask.WaitWhile(() =>
                {
                    var stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
                    return stateInfo.IsName(stateName) && stateInfo.normalizedTime < 1.0f;
                }, cancellationToken: cancellationToken);

                completionSource.TrySetResult();
            }
            catch (System.Exception ex)
            {
                completionSource.TrySetException(ex);
            }
        }
    }

    public static UniTask ToUniTask(this AnimationClip clip, CancellationToken cancellationToken = default, float delay = 0f, float timeScale = 1.0f)
    {
        if (!clip || clip.length <= 0)
            return UniTask.CompletedTask;

        return UniTask.Delay(
            (int)((delay + clip.length) * 1000 / timeScale),
            DelayType.Realtime,
            cancellationToken: cancellationToken
        );
    }
    
    public static UniTask ToUniTask(this Tween tween, CancellationToken cancellationToken = default)
    {
        if (tween == null || !tween.active)
            return UniTask.CompletedTask;

        var completionSource = new UniTaskCompletionSource();

        // Если твин уже завершен
        if (tween.IsComplete())
        {
            completionSource.TrySetResult();
            return completionSource.Task;
        }

        // Подписываемся на завершение твина
        tween.OnKill(() =>
        {
            completionSource.TrySetResult();
        });

        // Обрабатываем отмену через токен
        cancellationToken.Register(() =>
        {
            if (tween.IsActive())
            {
                tween.Kill();
            }
        });

        return completionSource.Task.AttachExternalCancellation(cancellationToken);
    }

    /// <summary>
    /// Конвертирует Sequence в UniTask
    /// </summary>
    public static UniTask ToUniTask(this Sequence sequence, CancellationToken cancellationToken = default)
    {
        return ((Tween)sequence).ToUniTask(cancellationToken);
    }
}
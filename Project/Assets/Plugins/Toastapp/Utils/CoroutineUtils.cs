using UnityEngine;
using System;
using System.Collections;

public static class CoroutineUtils {

    /**
     * Usage: StartCoroutine(CoroutineUtils.DelaySeconds(action, delay))
     * For example:
     *     StartCoroutine(CoroutineUtils.DelaySeconds(
     *         () => DebugUtils.Log("2 seconds past"),
     *         2);
     */
    public static IEnumerator DelaySeconds(Action action, float delay) {
        yield return new WaitForSeconds(delay);
        action();
    }
}
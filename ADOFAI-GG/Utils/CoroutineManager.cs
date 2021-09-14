using System;
using System.Collections;
using UnityEngine;

namespace ADOFAI_GG.Utils {
    public class CoroutineTools {
        public static void WaitCoroutine(IEnumerator func) {
            while (func.MoveNext()) {
                if (func.Current != null) {
                    IEnumerator num;
                    try {
                        num = (IEnumerator) func.Current;
                    } catch (InvalidCastException) {
                        if (func.Current is WaitForSeconds)
                            Debug.LogWarning("Skipped call to WaitForSeconds. Use WaitForSecondsRealtime instead.");
                        return; // Skip WaitForSeconds, WaitForEndOfFrame and WaitForFixedUpdate
                    }

                    WaitCoroutine(num);
                }
            }
        }
    }
}
using DG.Tweening;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ADOFAI_GG.Presentation.Patches.Intro {
    internal static class IntroButtonPatches {
        [HarmonyPatch(typeof(scnLevelSelect), "Start")]
        private static class ScnLevelSelectStart {
            public const float FloorPosX = 8;
            public const float FloorPosY = -3;

            private static void Postfix() {
                var parent = GameObject.Find("outer ring");
                var gemParent = GameObject.Find("XtraGem");
                var gem = gemParent.transform.Find("MovingGem_Top").gameObject;
                var f = parent.transform.Find("FloorCalibration").gameObject;
                var obj = Object.Instantiate(gem, parent.transform);
                obj.name = "FloorAdofaiGG";
                var floor = obj.GetComponent<scrFloor>();
                var movingfloor = obj.GetComponent<scrMenuMovingFloor>();
                Object.Destroy(obj.GetComponent<ffxCallFunction>());
                obj.transform.position = new Vector3(FloorPosX, FloorPosY);
                floor.isportal = true;
                floor.levelnumber = -12345678;
                obj.transform.Find("SpecIcon").gameObject.SetActive(false);
                var spriteRenderer = obj.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = Sprite.Create(Assets.Bundle.LoadAsset<Texture2D>("gggem"),
                    new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f));
                spriteRenderer.color = Color.red;
                DOTween.To((value => spriteRenderer.color = Color.Lerp(
                        new Color(1, 0.25f, 0.25f, spriteRenderer.color.a),
                        new Color(0.25f, 0.25f, 1, spriteRenderer.color.a), value)), 0, 1, 2).SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);

                /*var textParent = GameObject.Find("Canvas World");
                var textObj = Object.Instantiate(textParent.transform.Find("Calibration"), textParent.transform);
                textObj.position = new Vector3(FloorPosX - 0.5f, FloorPosY, 0);
                textObj.name = "AdofaiGG";
                textObj.GetComponent<scrTextChanger>().desktopText = "ADOFAI.GG";
                textObj.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;*/
            }
        }

        [HarmonyPatch(typeof(scnLevelSelect), "Update")]
        private static class ScnLevelSelectUpdate {
            private static void Postfix() {
                if (RDEditorUtils.CheckForKeyCombo(true, true, KeyCode.A)) {
                    GCS.sceneToLoad = "ADOFAIGG_MAIN";
                    scrController.instance.StartLoadingScene(WipeDirection.StartsFromRight);
                }
            }
        }

        [HarmonyPatch(typeof(scrController), "OnLandOnPortal")]
        private static class ScrControllerPortalTravelAction {
            private static bool Prefix(int portalDestination) {
                if (portalDestination == -12345678) {
                    GCS.sceneToLoad = "ADOFAIGG_MAIN";
                    scrController.instance.StartLoadingScene(WipeDirection.StartsFromRight);

                    return false;
                }

                return true;
            }
        }
    }
}
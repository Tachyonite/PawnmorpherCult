// MutagenicEntropyGizmo.cs created by Iron Wolf for PawnmorpherCult on 04/21/2020 8:18 AM
// last updated 04/21/2020  8:18 AM

using System.Collections.Generic;
using JetBrains.Annotations;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace PawnmorpherCult.AbilitySys
{
    /// <summary>
    /// gui for the mutagenic entropy gizmo 
    /// </summary>
    /// <seealso cref="Verse.Gizmo" />
    public class MutagenicEntropyGizmo : Gizmo
    {
        private const string LimitedIconPath = "UI/Icons/EntropyLimit/Limited";

        private const string UnlimitedIconPath = "UI/Icons/EntropyLimit/Unlimited";

        public MutagenicEntropyGizmo([NotNull] MutagenicEntropyTracker tracker)
        {
            _tracker = tracker;
            order = -100f;
            _limitedTexture = ContentFinder<Texture2D>.Get(LimitedIconPath);
            _unlimitedTexture = ContentFinder<Texture2D>.Get(UnlimitedIconPath);
        }

        private static readonly Texture2D FullBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.55f, 0.84f));

        private static readonly Texture2D OverLimitBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.75f, 0.2f, 0.15f));

        private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

        private readonly MutagenicEntropyTracker _tracker;
        private readonly Texture2D _limitedTexture;
        private readonly Texture2D _unlimitedTexture;

        private void DrawThreshold(Rect rect, float percent)
        {
            Rect rect2 = default(Rect);
            rect2.x = rect.x + rect.width * percent - 1f;
            rect2.y = rect.y;
            rect2.width = 2f;
            rect2.height = 6f;
            Rect position = rect2;
            if (_tracker.EntropyRelativeValue < percent)
            {
                GUI.DrawTexture(position, BaseContent.GreyTex);
            }
            else
            {
                GUI.DrawTexture(position, BaseContent.BlackTex);
            }
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
        {
            Rect rect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            Rect rect2 = rect.ContractedBy(6f);
            Widgets.DrawWindowBackground(rect);
            Text.Font = GameFont.Tiny;
            Rect rect3 = rect2;
            rect3.height = Text.LineHeight;
            Widgets.Label(rect3, "PsychicEntropy".Translate());
            if (_tracker.Parent.IsColonistPlayerControlled)
            {
                float num = 32f;
                float num2 = 6f;
                float num3 = rect2.height / 2f - num + num2;
                float num4 = rect2.width - num;
                Rect rect4 = new Rect(rect2.x + num4, rect2.y + num3, num, num);
                if (Widgets.ButtonImage(rect4, _tracker.AllowOverflow ? _limitedTexture: _unlimitedTexture))
                {
                    _tracker.AllowOverflow = !_tracker.AllowOverflow;
                    if (_tracker.AllowOverflow)
                    {
                        SoundDefOf.Tick_Low.PlayOneShotOnCamera();
                    }
                    else
                    {
                        SoundDefOf.Tick_High.PlayOneShotOnCamera();
                    }
                }
                TooltipHandler.TipRegionByKey(rect4, "PawnTooltipPsychicEntropyLimit");
            }
           
            Rect rect6 = rect2;
            rect6.yMin = rect2.y + rect2.height / 2f + 4f;
            float entropyRelativeValue = _tracker.EntropyRelativeValue;
            Widgets.FillableBar(rect6, Mathf.Min(entropyRelativeValue, 1f), FullBarTex, EmptyBarTex, doBorder: false);
            if (_tracker.EntropyLevel > _tracker.MaxEntropy)
            {
                Widgets.FillableBar(rect6, Mathf.Min(entropyRelativeValue - 1f, 1f), OverLimitBarTex, FullBarTex, doBorder: false);
                foreach (KeyValuePair<PsychicEntropySeverity, float> entropyThreshold in Pawn_PsychicEntropyTracker.EntropyThresholds)
                {
                    if (entropyThreshold.Value > 1f && entropyThreshold.Value < 2f)
                    {
                        DrawThreshold(rect6, entropyThreshold.Value - 1f);
                    }
                }
            }
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(rect6, _tracker.EntropyLevel.ToString("F0") + " / " + _tracker.MaxEntropy.ToString("F0"));
            Text.Anchor = TextAnchor.UpperLeft;
            if (Mouse.IsOver(rect2))
            {
                TooltipHandler.TipRegion(tip: new TipSignal(delegate
                {
                    float f = _tracker.EntropyLevel / _tracker.RecoveryRatePerSecond;
                    return string.Format("PawnTooltipPsychicEntropy".Translate(), Mathf.Round(_tracker.EntropyLevel), Mathf.Round(_tracker.MaxEntropy), _tracker.RecoveryRate.ToString("0.0"), 30f, Mathf.Round(f));
                }, Gen.HashCombineInt(_tracker.GetHashCode(), 133877), TooltipPriority.Pawn), rect: rect2);
            }
            return new GizmoResult(GizmoState.Clear);
        }

        public override float GetWidth(float maxWidth)
        {
            return 170f;
        }
    }
}
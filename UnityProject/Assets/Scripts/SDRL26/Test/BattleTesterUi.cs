using System.Linq;
using TMPro;
using UnityEngine;

namespace SDRL26
{
    public class BattleTesterUi : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private BattleTester _battleTester;

        private float _nextStateTime = 1;

        private void Start()
        {
            if (_battleTester.IsBattleInitialized())
            {
                ObserveBattle();
            }
            else
            {
                _battleTester.OnBattleInitialized.AddListener(ObserveBattle);
            }
        }

        private void Update()
        {
            if (!_battleTester) return;
            if (_battleTester.Battle == null) return;
            if (_nextStateTime > _battleTester.BattleTime) return;

            AddStateText();
            _nextStateTime = _battleTester.BattleTime + 1;
        }

        private void OnDestroy()
        {
            if (_battleTester != null && _battleTester.Battle != null)
            {
                _battleTester.Battle.OnStarted.RemoveListener(HandleBattleStarted);
            }

            Battler.OnTargetsEvaluated.RemoveListener(HandleBattlerTargetsEvaluated);
        }

        private void ObserveBattle()
        {
            _battleTester.Battle.OnStarted.AddListener(HandleBattleStarted);
            Battler.OnTargetsChanged.AddListener(HandleBattlerTargetsEvaluated);
            Battler.OnActionsPerformed.AddListener(HandleActionPerformed);
        }

        private void HandleActionPerformed(Battler battler)
        {
            AddText(
                $"{battler.DisplayName} performed: {string.Join(", ", battler.Actions.Select(t => t.DebugString))} on {string.Join(", ", battler.Targets.Select(t => t.DisplayName))}",
                _battleTester.Battle.IsInPlayerTeam(battler) ? Color.green : Color.red);
        }

        private void HandleBattlerTargetsEvaluated(Battler battler)
        {
            AddText($"{battler.DisplayName}'s targets changed: {string.Join(", ", battler.Targets.Select(t => t.DisplayName))}",
                _battleTester.Battle.IsInPlayerTeam(battler) ? Color.green : Color.red);
        }

        private void HandleBattleStarted() => AddText("Battle started", Color.white);

        private void AddStateText()
        {
            AddText("Player Team: " + string.Join(", ", _battleTester.Battle.PlayerTeam.Battlers.Select(t => $"{t.DisplayName} ({t.Health.CurrentHealth}HP)")), Color.cyan);
            AddText("Opponent Team: " + string.Join(", ", _battleTester.Battle.OpponentTeam.Battlers.Select(t => $"{t.DisplayName} ({t.Health.CurrentHealth}HP)")), Color.cyan);
        }

        private void AddText(string text, Color color) => _text.text = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>[{GetBattleTime()}] {text}</color><br>{_text.text}";

        private string GetBattleTime()
        {
            var battleTime = _battleTester.BattleTime;

            var millis = (int)(battleTime % 1 * 1000);
            var seconds = (int)battleTime % 60;
            var minutes = (int)battleTime / 60;

            return $"{minutes:00}:{seconds:00}.{millis:000}";
        }
    }
}
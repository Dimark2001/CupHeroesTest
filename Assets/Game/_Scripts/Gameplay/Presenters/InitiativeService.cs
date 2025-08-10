using System.Collections.Generic;
using UnityEngine;

public class InitiativeService
{
    private class ParticipantData
    {
        public CharacterPresenter Presenter;
        public float WaitTime;
        public float InverseSpeed => 1f / Mathf.Max(0.01f, Presenter.Model.Stats.Value.AttackSpeed);
    }

    private readonly List<ParticipantData> _participants = new();

    public void RegisterParticipant(CharacterPresenter character)
    {
        if (_participants.Exists(p => p.Presenter == character)) return;

        _participants.Add(new ParticipantData
        {
            Presenter = character,
            WaitTime = 1f / character.Model.Stats.Value.AttackSpeed
        });
    }

    public void UnregisterParticipant(CharacterPresenter character)
    {
        _participants.RemoveAll(p => p.Presenter == character);
    }

    public CharacterPresenter GetNextTurnCharacter()
    {
        if (_participants.Count == 0) return null;

        var next = _participants[0];
        float minWait = next.WaitTime;

        for (int i = 1; i < _participants.Count; i++)
        {
            if (_participants[i].WaitTime < minWait)
            {
                minWait = _participants[i].WaitTime;
                next = _participants[i];
            }
        }

        foreach (var participant in _participants)
        {
            participant.WaitTime -= minWait;
        }

        next.WaitTime = next.InverseSpeed;

        return next.Presenter;
    }

    public void DebugLogParticipants()
    {
        foreach (var p in _participants)
        {
            Debug.Log($"{p.Presenter}: Speed={p.Presenter.Model.Stats.Value.AttackSpeed} WaitTime={p.WaitTime}");
        }
    }
}
using System.Text.Json.Serialization;

namespace TCGBase
{
    public enum CharacterTargetType
    {
        CurrCharacter,
        NextCharacter,
        LastCharacter,
        ClosestCharacter,
        AnyCharacter,
        AllCharacter,
        Team
    }
    public record class CharacterTargetRecord
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CharacterTargetType Type { get; }
        public bool Except { get; }
        public string? CertainName { get; }

        public CharacterTargetRecord(CharacterTargetType type = CharacterTargetType.CurrCharacter, bool except = false, string? certainName = null)
        {
            Type = type;
            Except = except;
            CertainName = certainName;
        }
        public List<int> GetCharacters(AbstractTeam team)
        {
            List<int> chars = new();
            if (team.CurrCharacter != -1)
            {
                switch (Type)
                {
                    case CharacterTargetType.CurrCharacter:
                        chars.Add(team.CurrCharacter);
                        break;
                    case CharacterTargetType.NextCharacter:
                        chars.Add((team.CurrCharacter + 1) % team.Characters.Length);
                        break;
                    case CharacterTargetType.LastCharacter:
                        chars.Add((team.CurrCharacter - 1 + team.Characters.Length) % team.Characters.Length);
                        break;
                    case CharacterTargetType.ClosestCharacter:
                        if (team.Characters[team.Enemy.CurrCharacter].Alive)
                        {
                            chars.Add(team.Enemy.CurrCharacter);
                        }
                        else
                        {
                            for (int i = 0; i < team.Characters.Length; i++)
                            {
                                if (team.Characters[i].Alive)
                                {
                                    chars.Add(i);
                                    break;
                                }
                            }
                        }
                        break;
                    case CharacterTargetType.AnyCharacter:
                        for (int i = 0; i < team.Characters.Length; i++)
                        {
                            if (team.Characters[(team.CurrCharacter + i) % team.Characters.Length].Alive)
                            {
                                chars.Add(i);
                                break;
                            }
                        }
                        break;
                    case CharacterTargetType.AllCharacter:
                        chars.AddRange(Enumerable.Range(0, team.Characters.Length));
                        break;
                    case CharacterTargetType.Team:
                        chars.Add(-1);
                        break;
                }
                if (CertainName != null)
                {
                    chars = chars.Where(index =>
                    {
                        var card = team.Characters[index].Card;
                        return $"{card.Namespace}:{card.NameID}" == CertainName;
                    }).ToList();
                }
                if (Except)
                {
                    chars = Enumerable.Range(0, team.Characters.Length).Except(chars).ToList();
                }
            }
            return chars;
        }
    }
}

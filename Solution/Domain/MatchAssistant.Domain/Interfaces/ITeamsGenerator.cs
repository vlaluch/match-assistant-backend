﻿using MatchAssistant.Domain.Core.Entities;
using System.Collections.Generic;

namespace MatchAssistant.Domain.Interfaces
{
    public interface ITeamsGenerator
    {
        IEnumerable<Player>[] Generate(ICollection<Player> gameParticipants, TeamGenerationAlgorithm algorithm);
    }
}

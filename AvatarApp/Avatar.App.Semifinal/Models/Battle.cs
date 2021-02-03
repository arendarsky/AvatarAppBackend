using System;
using System.Collections.Generic;
using System.Linq;

namespace Avatar.App.Semifinal.Models
{
    public class Battle
    {
        public long Id { get; set; }
        public DateTime EndDate { get; set; }
        public int WinnersNumber { get; set; }
        public IEnumerable<Semifinalist> Participants { get; set; }

        public int TotalVotesNumber => Participants.SelectMany(participant => participant.GetBattleVotes(Id)).Count();

        public bool IsVoteLimitExpired(long userId)
        {
            return Participants.SelectMany(participant => participant.GetBattleVotes(Id))
                .Count(vote => vote.UserId == userId) >= WinnersNumber;
        }

        public IEnumerable<BattleVote> GetParticipantsVotes(long participantId)
        {
            return Participants.FirstOrDefault(participant => participant.Id == participantId)?.GetBattleVotes(Id);
        }
    }
}

using System;
using System.Collections.Generic;

namespace P03_FootballBetting.Data.Models
{
	public class Game
	{
		public Game()
		{
			PlayerStatistics = new List<PlayerStatistic>();
			Bets = new List<Bet>();
		}

		public int GameId { get; set; }

		public double AwayTeamBetRate { get; set; }

		public int AwayTeamGoals { get; set; }

		public double DrawBetRate { get; set; }

		public double HomeTeamBetRate { get; set; }

		public int HomeTeamGoals { get; set; } 

		public string Result { get; set; }

		public DateTime DateTime { get; set; }

		public int HomeTeamId { get; set; }
		public Team HomeTeam { get; set; }

		public int AwayTeamId { get; set; }
		public Team AwayTeam { get; set; }

		public ICollection<PlayerStatistic> PlayerStatistics { get; set; }
		public ICollection<Bet> Bets { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pizzeon_server.Models;

namespace Pizzeon_server.Processors {
	public class ScoreboardProcessor {

		private IRepository _repository;

		public ScoreboardProcessor (IRepository repository) {
			_repository = repository;
		}

		public IEnumerable<PlayerStatsView> GetAllPlayerStatsSingle() {
			return this._repository.GetAllPlayerStatsSingle().Result;
		}

		public IEnumerable<PlayerStatsView> GetAllPlayerStatsMulti() {
			return this._repository.GetAllPlayerStatsMulti().Result;
		}

		public IEnumerable<PlayerStatsView> GetTopPlayerStatsSingle(int number, int page) {
			return _repository.GetTopPlayerStatsSingle(number, page).Result;

		}

		public IEnumerable<PlayerStatsView> GetTopPlayerStatsMulti (int number, int page) {
			return _repository.GetTopPlayerStatsMulti(number, page).Result;
		}

	}
}

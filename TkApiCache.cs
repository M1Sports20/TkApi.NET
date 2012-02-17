#region License
/*
This file is part of TkApi.NET project.

TkApi.NET is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

TkApi.NET is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with TkApi.NET.  If not, see <http://www.gnu.org/licenses/>.

Copyright (c) 2011-2012, Michael D. Spradling (mike@mspradling.com)
*/
#endregion

using System;
using TkApi.DataStructures;
using System.Collections.Generic;

namespace TkApi {
	public class TkApiCache : TkApiRaw {
		private const uint RetryCount = 7; // Count
		private const uint RetryDelay = 100; // Milliseconds
		private uint _cacheTimeout = 0; // How long to cache results for in ms
		
		private class TkCacheData {
			public TKBaseType Data = null;
			public string Key = String.Empty;
			public DateTime AccessTime = new DateTime(0);
		}
		
		public TkApiCache(string consumerKey, string consumerSecret, string accessToken, string accessSecret):
			base(consumerKey, consumerSecret, accessToken, accessSecret) {
		}
		public TkApiCache(string consumerKey, string consumerSecret, string accessToken, string accessSecret, bool allowTrades):
			base(consumerKey, consumerSecret, accessToken, accessSecret, allowTrades) {
		}
		
		private TKBaseType RetryFunc(Func<TKBaseType> func, uint retries, uint retryTimeout) {
			TKBaseType result = default(TKBaseType);
			while (retries > 0) {
		   	    result = func();	
				if (result != null) {
					if (result.Type == null && (result.Error == null || result.Error == "Success")) // Tk provides to many forms of success/errors.
						break;
				}
				retries--;
				System.Threading.Thread.Sleep((int)retryTimeout);
			}
			if (retries == 0) // Failed
				throw new Exception("Failed to get web request.");
			
			return result;
		}
		
		/// <summary>
		/// Number of Milliseconds to cache Tradeking results.
		/// Only results are cached, no write transactoins. 
		/// </summary>
		/// <value>
		/// The cache timeout in milliseconds.
		/// </value>
		public uint CacheTimeout {
			get {
				return _cacheTimeout;
			}
			set {
				_cacheTimeout = value;
			}
		}
		
		private TkCacheData _accounts = new TkCacheData();
		public override Accounts GetAccounts() {
			TimeSpan ts = DateTime.Now - _accounts.AccessTime;
			if (ts.TotalMilliseconds > CacheTimeout) {
				RetryFunc(() => _accounts.Data = base.GetAccounts(), RetryCount, RetryDelay);
				_accounts.AccessTime = DateTime.Now;
			}
			return (Accounts)_accounts.Data;
		}
		
		private TkCacheData _accountsBalances = new TkCacheData();
		public override AccountsBalance GetAccounts_Balances() {
			TimeSpan ts = DateTime.Now - _accountsBalances.AccessTime;
			if (ts.TotalMilliseconds > CacheTimeout) {
				RetryFunc(() => _accountsBalances.Data = base.GetAccounts_Balances(), RetryCount, RetryDelay);
				_accountsBalances.AccessTime = DateTime.Now;
			}
			return (AccountsBalance)_accountsBalances.Data;
		}
		
		private List<TkCacheData> _accountsSingle = new List<TkCacheData>();
		public override AccountsSingle GetAccounts (string accountNumber) {
			TkCacheData cache = null;
		
			foreach (TkCacheData c in _accountsSingle) {
				if (c.Key == accountNumber) {
					cache = c;
					break;
				}
			}
			// Add if not found
			if (cache == null) {
				cache = new TkCacheData();
				cache.Key = accountNumber;
				_accountsSingle.Add(cache);
			}
					
			TimeSpan ts = DateTime.Now - cache.AccessTime;
			if (ts.TotalMilliseconds > CacheTimeout) {
				RetryFunc(() => cache.Data = base.GetAccounts(accountNumber), RetryCount, RetryDelay);
				cache.AccessTime = DateTime.Now;
			}
			return (AccountsSingle)cache.Data;
		}
		
		private List<TkCacheData> _accountsBalancesSingle = new List<TkCacheData>();
		public override AccountsBalancesSingle GetAccounts_Balances (string accountNumber) {
			TkCacheData cache = null;
		
			foreach (TkCacheData c in _accountsBalancesSingle) {
				if (c.Key == accountNumber) {
					cache = c;
					break;
				}
			}
			// Add if not found
			if (cache == null) {
				cache = new TkCacheData();
				cache.Key = accountNumber;
				_accountsBalancesSingle.Add(cache);
			}
					
			TimeSpan ts = DateTime.Now - cache.AccessTime;
			if (ts.TotalMilliseconds > CacheTimeout) {
				RetryFunc(() => cache.Data = base.GetAccounts_Balances(accountNumber), RetryCount, RetryDelay);
				cache.AccessTime = DateTime.Now;
			}
			return (AccountsBalancesSingle)cache.Data;
		}
		
		private List<TkCacheData> _accounts_History = new List<TkCacheData>();
		public override AccountsHistory GetAccounts_History(string accountNumber, AccountsHistory_Request_Range range, AccountsHistory_Request_Transactions transactions) {
			TkCacheData cache = null;
			string key = accountNumber + range.ToString() + transactions.ToString();
			
			foreach (TkCacheData c in _accounts_History) {
				if (c.Key == key) {
					cache = c;
					break;
				}
			}
			// Add if not found
			if (cache == null) {
				cache = new TkCacheData();
				cache.Key = key;
				_accounts_History.Add(cache);
			}
					
			TimeSpan ts = DateTime.Now - cache.AccessTime;
			if (ts.TotalMilliseconds > CacheTimeout) {
				RetryFunc(() => cache.Data = base.GetAccounts_History(accountNumber, range, transactions), RetryCount, RetryDelay);
				cache.AccessTime = DateTime.Now;
			}
			return (AccountsHistory)cache.Data;
		}
		
		private List<TkCacheData> _accountsHoldings = new List<TkCacheData>();
		public override AccountsHoldings GetAccount_Holdings(string accountNumber) {
			TkCacheData cache = null;
		
			foreach (TkCacheData c in _accountsHoldings) {
				if (c.Key == accountNumber) {
					cache = c;
					break;
				}
			}
			// Add if not found
			if (cache == null) {
				cache = new TkCacheData();
				cache.Key = accountNumber;
				_accountsHoldings.Add(cache);
			}
					
			TimeSpan ts = DateTime.Now - cache.AccessTime;
			if (ts.TotalMilliseconds > CacheTimeout) {
				RetryFunc(() => cache.Data = base.GetAccount_Holdings(accountNumber), RetryCount, RetryDelay);
				cache.AccessTime = DateTime.Now;
			}
			return (AccountsHoldings)cache.Data;
		}
		
		private List<TkCacheData> _accountsOrders = new List<TkCacheData>();
		public override Orders GetAccounts_Orders (string accountNumber) {
			TkCacheData cache = null;
		
			foreach (TkCacheData c in _accountsOrders) {
				if (c.Key == accountNumber) {
					cache = c;
					break;
				}
			}
			// Add if not found
			if (cache == null) {
				cache = new TkCacheData();
				cache.Key = accountNumber;
				_accountsOrders.Add(cache);
			}
					
			TimeSpan ts = DateTime.Now - cache.AccessTime;
			if (ts.TotalMilliseconds > CacheTimeout) {
				RetryFunc(() => cache.Data = base.GetAccounts_Orders(accountNumber), RetryCount, RetryDelay);
				cache.AccessTime = DateTime.Now;
			}
			return (Orders)cache.Data;
		}
		
		private TkCacheData _marketClock = new TkCacheData();
		public override MarketClock GetMarket_Clock() {
			TimeSpan ts = DateTime.Now - _marketClock.AccessTime;
			if (ts.TotalMilliseconds > CacheTimeout) {
				RetryFunc(() => _marketClock.Data = base.GetMarket_Clock(), RetryCount, RetryDelay);
				_marketClock.AccessTime = DateTime.Now;
			}
			return (MarketClock)_marketClock.Data;
		}
		
		
		/*public override void GetMarket_Quotes (string symbol, string watchlist, bool delayed)
		{
			TkCacheData a = new TkCacheData();
			// TODO:
			RetryFunc(() => a.Data = base.GetMarket_Quotes(symbol, watchlist, delayed), RetryCount, RetryDelay);
		}*/
	
		private TkCacheData _memberProfile = new TkCacheData();
		public override MemberProfile GetMember_Profile() {
			TimeSpan ts = DateTime.Now - _memberProfile.AccessTime;
			if (ts.TotalMilliseconds > CacheTimeout) {
				RetryFunc(() => _memberProfile.Data = base.GetMember_Profile(), RetryCount, RetryDelay);
				_memberProfile.AccessTime = DateTime.Now;
			}
			return (MemberProfile)_memberProfile.Data;
		}
		
		private TkCacheData _utilityVersion = new TkCacheData();
		public override UtilityVersion GetUtility_Version() {
			TimeSpan ts = DateTime.Now - _memberProfile.AccessTime;
			if (ts.TotalMilliseconds > CacheTimeout) {
				RetryFunc(() => _utilityVersion.Data = base.GetUtility_Version(), RetryCount, RetryDelay);
				_utilityVersion.AccessTime = DateTime.Now;
			}
			return (UtilityVersion)_utilityVersion.Data;
		}
		
		private TkCacheData _utilityStatus = new TkCacheData();
		public override UtilityStatus GetUtility_Status () {
			TimeSpan ts = DateTime.Now - _utilityStatus.AccessTime;
			if (ts.TotalMilliseconds > CacheTimeout) {
				RetryFunc(() => _utilityVersion.Data = base.GetUtility_Status(), RetryCount, RetryDelay);
				_utilityStatus.AccessTime = DateTime.Now;
			}
			return (UtilityStatus)_utilityStatus.Data;
		}

		private TkCacheData _watchlists = new TkCacheData();
		public override Watchlists GetWatchlists() {
			TimeSpan ts = DateTime.Now - _watchlists.AccessTime;
			if (ts.TotalMilliseconds > CacheTimeout) {
				RetryFunc(() => _watchlists.Data = base.GetWatchlists(), RetryCount, RetryDelay);
				_watchlists.AccessTime = DateTime.Now;
			}
			return (Watchlists)_watchlists.Data;
		}

		private List<TkCacheData> _watchlistItems = new List<TkCacheData>();
		public override WatchlistsItems GetWatchlists(string id) {
			TkCacheData cache = null;
		
			foreach (TkCacheData c in _watchlistItems) {
				if (c.Key == id) {
					cache = c;
					break;
				}
			}
			// Add if not found
			if (cache == null) {
				cache = new TkCacheData();
				cache.Key = id;
				_watchlistItems.Add(cache);
			}
					
			TimeSpan ts = DateTime.Now - cache.AccessTime;
			if (ts.TotalMilliseconds > CacheTimeout) {
				RetryFunc(() => cache.Data = base.GetWatchlists(id), RetryCount, RetryDelay);
				cache.AccessTime = DateTime.Now;
			}
			return (WatchlistsItems)cache.Data;
		}
	}
}

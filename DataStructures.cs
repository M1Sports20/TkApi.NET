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

Copyright (c) 2011, Michael D. Spradling (mike@mspradling.com)
*/
#endregion

using System;
using Newtonsoft.Json;

namespace TkApi
{
	#region JSON.NET Converters
	internal class ObjectSometimesArrayConverter<T>: JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof(T)) || (objectType == typeof(T[]));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    	{
            if (reader.TokenType == JsonToken.StartArray)
            {
            	T[] s = serializer.Deserialize<T[]>(reader);
            	return s;
            }
            else
            {
                T s = serializer.Deserialize<T>(reader);
                return new T[] { s };
            }
        }
		
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
	#endregion
	
	namespace DataStructures
	{
		# region Classes for serialization of user/balance
		public class UserBalance
		{
			[JsonProperty("@id")]
			public string Id { get; set; }
	
			[JsonProperty("accountbalance")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<UserBalance_AccountBalance>))]
			public UserBalance_AccountBalance[] AccountBalance { get; set; }
			
			[JsonProperty("totalbalance")]
			public UserBalance_TotalBalance TotalBalance { get; set; }
		}
		
		public class UserBalance_TotalBalance
		{
			[JsonProperty("accountvalue")]
			public string AccountValue { get; set; }
		}
	
		public class UserBalance_AccountBalance
		{
			[JsonProperty("account")]		
			public string AccountNumber { get; set; }
	
			[JsonProperty("accountname")]
			public string AccountName { get; set; }
	
			[JsonProperty("accountvalue")]
			public string AccountValue { get; set; }
		}
		#endregion
		
		#region Classes for serialization of user/summary
		public class UserSummary
		{
			[JsonProperty("@id")]
			public string Id { get; set; }
			
			[JsonProperty("accounts")]
			public UserSummary_Accounts Accounts { get; set; }	
		}
		
		public class UserSummary_Accounts
		{
			[JsonProperty("accountsummary")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<UserSummary_Accounts_AccountSummary>))]
			public UserSummary_Accounts_AccountSummary[] Summary { get; set; }
		}
		
		public class UserSummary_Accounts_AccountSummary
		{
			[JsonProperty("account")]
			public string Account { get; set; }
		
			[JsonProperty("accountbalance")]
			public UserSummary_Accounts_AccountBalance AccountBalance { get; set; }
			
			[JsonProperty("accountholdings")]
			public UserSummary_Accounts_AccountHoldings AccountHoldings { get; set; }
		}
		
		public class UserSummary_Accounts_AccountBalance
		{
			[JsonProperty("account")]
			public string Account { get; set; }
			
			[JsonProperty("accountvalue")]
			public string AccountValue { get; set; }
			
			[JsonProperty("fedcall")]
			public string FedCall { get; set; }
			
			[JsonProperty("housecall")]
			public string HouseCall { get; set; }
			
			[JsonProperty("money")]
			public UserSummary_Accounts_AccountBalance_Money Money;
			
			[JsonProperty("securities")]
			public UserSummary_Accounts_AccountBalance_Securities Security;
		}
		
		public class UserSummary_Accounts_AccountBalance_Money
		{
			[JsonProperty("accruedinterest")]
			public string AccruedInterest { get; set; }
		
			[JsonProperty("cash")]
			public string Cash { get; set; }
		
			[JsonProperty("cashavailable")]
			public string CashAvailable { get; set; }
		
			[JsonProperty("marginbalance")]
			public string MarginBalance { get; set; }
		
			[JsonProperty("mmf")]
			public string Mmf { get; set; }
		
			[JsonProperty("total")]
			public string Total { get; set; }
		
			[JsonProperty("uncleareddeposits")]
			public string UnclearedDeposits { get; set; }
	
			[JsonProperty("unsettledfunds")]
			public string UnsettledFunds { get; set; }
		
			[JsonProperty("yield")]
			public string Yield { get; set; }
		}
		
		public class UserSummary_Accounts_AccountBalance_Securities
		{
			[JsonProperty("longoptions")]
			public string LongOptions { get; set; }
			
			[JsonProperty("longstocks")]
			public string LongStocks { get; set; }	
			
			[JsonProperty("options")]
			public string Options { get; set; }	
			
			[JsonProperty("shortoptions")]
			public string ShortOptions { get; set; }	
			
			[JsonProperty("shortstocks")]
			public string ShortStocks { get; set; }	
			
			[JsonProperty("stocks")]
			public string Stocks { get; set; }	
			
			[JsonProperty("total")]
			public string Total { get; set; }	
		}
		
		public class UserSummary_Accounts_AccountHoldings
		{
			[JsonProperty("displaydata")]
			public UserSummary_Accounts_AccountHoldings_DisplayData DisplayData { get; set; }
			
			[JsonProperty("holding")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<UserSummary_Accounts_AccountSummary_AccountHoldings_Holding>))]
			public UserSummary_Accounts_AccountSummary_AccountHoldings_Holding[] Holdings { get; set; }
			
			[JsonProperty("totalsecurities")]
			public string TotalSecurities { get; set; }	
		}
		
		public class UserSummary_Accounts_AccountHoldings_DisplayData
		{
			[JsonProperty("totalsecurities")]
			public string TotalSecurities { get; set; }			
		}
		
		public class UserSummary_Accounts_AccountSummary_AccountHoldings_Holding
		{
			[JsonProperty("accounttype")]
			public string AccountType { get; set; }
			
			[JsonProperty("costbasis")]
			public string CostBasis { get; set; }
			
			[JsonProperty("displaydata")]
			public UserSummary_Accounts_AccountSummary_AccountHoldings_Holding_DisplayData DisplayData { get; set; }
					
			[JsonProperty("gainloss")]
			public string GainLoss { get; set; }
			
			[JsonProperty("instrument")]
			public UserSummary_Accounts_AccountSummary_AccountHoldings_Holding_Instrument Instrument { get; set; }
					
			[JsonProperty("marketvalue")]
			public string MarketValue { get; set; }
					
			[JsonProperty("marketvaluechange")]
			public string MarketValueChange { get; set; }
							
			[JsonProperty("price")]
			public string Price { get; set; }
					
			[JsonProperty("purchaseprice")]
			public string PurchasePrice { get; set; }
					
			[JsonProperty("qty")]
			public string Qty { get; set; }
	
			[JsonProperty("quote")]
			public UserSummary_Accounts_AccountSummary_AccountHoldings_Holding_Quote Quote { get; set; }
					
			[JsonProperty("underlying")]
			public string Underlying { get; set; }
		}
		
		public class UserSummary_Accounts_AccountSummary_AccountHoldings_Holding_DisplayData
		{
			[JsonProperty("accounttype")]
			public string AccountType { get; set; }
	
			[JsonProperty("assetclass")]
			public string AssetClass { get; set; }
			
			[JsonProperty("change")]
			public string Change { get; set; }
			
			[JsonProperty("costbasis")]
			public string CostBasis { get; set; }
			
			[JsonProperty("desc")]
			public string Desc { get; set; }
	
			[JsonProperty("lastprice")]
			public string LastPrice { get; set; }
			
			[JsonProperty("marketvalue")]
			public string MarketValue { get; set; }
			
			[JsonProperty("marketvaluechange")]
			public string MarketValueChange { get; set; }
			
			[JsonProperty("qty")]
			public string Qty { get; set; }
			
			[JsonProperty("symbol")]
			public string Symbol { get; set; }
		}
		
		public class UserSummary_Accounts_AccountSummary_AccountHoldings_Holding_Instrument
		{
			[JsonProperty("cusip")]
			public string CusIp { get; set; }
			
			[JsonProperty("desc")]
			public string Desc { get; set; }
			
			[JsonProperty("factor")]
			public string Factor { get; set; }
			
			[JsonProperty("sectyp")]
			public string SECTyp { get; set; }
			
			[JsonProperty("sym")]
			public string Sym { get; set; }
		}
			
		public class UserSummary_Accounts_AccountSummary_AccountHoldings_Holding_Quote
		{
			[JsonProperty("change")]
			public string Change { get; set; }
		
			[JsonProperty("lastprice")]
			public string LastPrice { get; set; }
		
		}
		#endregion
	
		#region Classes for serialization of user/profile
		public class UserProfile
		{
			[JsonProperty("@id")]
			public string Id { get; set; }
			
			[JsonProperty("userdata")]
			public UserProfile_UserData UserData { get; set; }	
		}
		
		public class UserProfile_UserData
		{
			[JsonProperty("account")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<UserProfile_UserData_Account>))]
			public UserProfile_UserData_Account[] Account { get; set; }
			
			[JsonProperty("disabled")]
			public string Disabled { get; set; }
			
			[JsonProperty("resetpassword")]
			public string ResetPassword { get; set; }
			
			[JsonProperty("resettradingpassword")]
			public string ResetTradingPassword { get; set; }
			
			[JsonProperty("userprofile")]
			public UserProfile_UserData_UserProfile UserProfile { get; set; }
		}
		
		public class UserProfile_UserData_Account
		{
			[JsonProperty("account")]
			public string Account { get; set; }
			
			[JsonProperty("fundtrading")]
			public string FundTrading { get; set; }
			
			[JsonProperty("ira")]
			public string IRA { get; set; }
			
			[JsonProperty("margintrading")]
			public string MarginTrading { get; set; }
			
			[JsonProperty("nickname")]
			public string Nickname { get; set; }
			
			[JsonProperty("optionlevel")]
			public string OptionLevel { get; set; }
			
			[JsonProperty("shared")]
			public string Shared { get; set; }
			
			[JsonProperty("stocktrading")]
			public string StockTrading { get; set; }
		}
		
		public class UserProfile_UserData_UserProfile
		{
			[JsonProperty("entry")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<UserProfile_UserData_UserProfile_Entry>))]
			public UserProfile_UserData_UserProfile_Entry[] Entry{ get; set; }
		}
		
		public class UserProfile_UserData_UserProfile_Entry
		{
			[JsonProperty("name")]
			public string Name { get; set; }
			
			[JsonProperty("value")]
			public string Value { get; set; }
		}
		#endregion
		
		#region Classes for serialization of account/history		
		public enum AccountHistory_Request_Range
		{
			All,
			Today,
			Current_Week,
			Current_Month,
			Last_Month
		}
		
		public enum AccountHistory_Request_Transactions
		{
			All,
			Bookkeeping,
			Trade
		}
		
		public class AccountHistory
		{
			[JsonProperty("@id")]
			public string Id { get; set; }
			
			[JsonProperty("totalrows")]
			public string TotalRows { get; set; }
			
			[JsonProperty("transactions")]
			public AccountHistory_Transactions Transactions { get; set; }
		}
		
		public class AccountHistory_Transactions
		{		
			[JsonProperty("transaction")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<AccountHistory_Transaction>))]
			public AccountHistory_Transaction[] Transaction { get; set; }
		}
		
		public class AccountHistory_Transaction
		{
			[JsonProperty("activity")]
			public string Activity { get; set; }
			
			[JsonProperty("amount")]
			public string Amount { get; set; }
			
			[JsonProperty("date")]
			public string Date { get; set; }
			
			[JsonProperty("desc")]
			public string Desc { get; set; }
			
			[JsonProperty("symbol")]
			public string Symbol { get; set; }
			
			[JsonProperty("transaction")]
			public AccountHistory_Transaction_Transaction Transaction { get; set; }
		}
			
		public class AccountHistory_Transaction_Transaction
		{
			[JsonProperty("accounttype")]
			public string AccountType { get; set; }
			
			[JsonProperty("commission")]
			public string Commission { get; set; }
			
			[JsonProperty("description")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<string>))]
			public string[] Description { get; set; }
			
			[JsonProperty("fee")]
			public string Fee { get; set; }
			
			[JsonProperty("price")]
			public string Price { get; set; }
			
			[JsonProperty("quantity")]
			public string Quantity { get; set; }
			
			[JsonProperty("secfee")]
			public string SECFee { get; set; }
	
			[JsonProperty("security")]
			public AccountHistory_Transaction_Transaction_Security Security { get; set; }
			
			[JsonProperty("settlementdate")]
			public string SettlementDate { get; set; }
			
			[JsonProperty("side")]
			public string Side { get; set; }
			
			[JsonProperty("source")]
			public string Source { get; set; }
			
			[JsonProperty("tradedate")]
			public string TradeDate { get; set; }
			
			[JsonProperty("transactionid")]
			public string TransactionId { get; set; }
			
		}
		
		public class AccountHistory_Transaction_Transaction_Security
		{
			[JsonProperty("cusip")]
			public string CusIp { get; set; }
			
			[JsonProperty("id")]
			public string Id { get; set; }
			
			[JsonProperty("sectyp")]
			public string SECTyp { get; set; }
	
			[JsonProperty("sym")]
			public string Symbol { get; set; }
		}
		#endregion
		
		#region Classes for serialization of user/watchlists
		public class UserWatchlistsList 
		{
			[JsonProperty("@id")]
			public string Id { get; set; }

			[JsonProperty("watchlists")]
			public UserWatchlistsList_Watchlists WatchLists { get; set; }
		}

		public class UserWatchlistsList_Watchlists
		{
			[JsonProperty("watchlist")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<UserWatchlistsList_Watchlists_Watchlist>))]
			public UserWatchlistsList_Watchlists_Watchlist[] WatchList { get; set; }
		}

		public class UserWatchlistsList_Watchlists_Watchlist
		{
			[JsonProperty("id")]
			public string Id { get; set; }
		}

		public class UserWatchlistsGet
		{
			[JsonProperty("@id")]
			public string Id { get; set; }

			[JsonProperty("watchlists")]
			public UserWatchlistsGet_Watchlists WatchLists { get; set; }
		}

		public class UserWatchlistsGet_Watchlists
		{
			[JsonProperty("watchlist")]
			public UserWatchListsGet_Watchlists_Watchlist WatchList { get; set; }
		}

		public class UserWatchListsGet_Watchlists_Watchlist
		{
			[JsonProperty("watchlistitem")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<UserWatchListsGet_Watchlists_Watchlist_Watchlistitem>))]
			public UserWatchListsGet_Watchlists_Watchlist_Watchlistitem[] Watchlistitem;
		}
		
		public class UserWatchListsGet_Watchlists_Watchlist_Watchlistitem
		{
			[JsonProperty("costbasis")]
			public string CostBasis { get; set; }

			[JsonProperty("instrument")]
			public UserWatchListsGet_Watchlists_Watchlist_Instrument Instrument { get; set; }

			[JsonProperty("qty")]
			public string Qty { get; set; }
		}

		public class UserWatchListsGet_Watchlists_Watchlist_Instrument
		{
			[JsonProperty("sym")]
			public string Sym { get; set; }
		}
		
		public class UserWatchlistsItem
		{
			public string CostBasis { get; set; }
			public string Qty { get; set; }
			public UserWatchlistsItem_Instrument Instrument { get; set; }
		}
		
		public class UserWatchlistsItem_Instrument
		{
			public string Sym;
		}
		#endregion
		
		#region Classes for serialization of account/balances
		public class AccountBalances
		{
			[JsonProperty("@id")]
			public string Id { get; set; }
			
			[JsonProperty("accountbalance")]
			public AccountBalances_AccountBalance AccountBalance { get; set; }
		}
		
		public class AccountBalances_AccountBalance
		{
			[JsonProperty("account")]
			public string Account { get; set; }
	
			[JsonProperty("accountvalue")]
			public string AccountValue { get; set; }

			[JsonProperty("cashbalance")]
			public string CashBalance { get; set; }
			
			[JsonProperty("cashmv")]
			public string CashMV { get; set; }
			
			[JsonProperty("fedcall")]
			public string FedCall { get; set; }
	
			[JsonProperty("housecall")]
			public string HouseCall { get; set; }

			[JsonProperty("marginbalance")]
			public string MarginBalance { get; set; }
			
			[JsonProperty("marginmv")]
			public string MarginMV { get; set; }
					
			[JsonProperty("money")]
			public AccountBalances_Money Money { get; set; }
			
			[JsonProperty("securities")]
			public AccountBalances_Securities Securities { get; set; }
			
			[JsonProperty("shortbalance")]
			public string ShortBalance { get; set;}
			
			[JsonProperty("shortmv")]
			public string ShortMV { get; set; }		
		}
		
		public class AccountBalances_Money
		{
			[JsonProperty("accruedinterest")]
			public string AccruedInterest { get; set; }

			[JsonProperty("cash")]
			public string Cash { get; set; }
			
			[JsonProperty("cashavailable")]
			public string CashAvailable { get; set; }
			
			[JsonProperty("marginbalance")]
			public string MarginBalance { get; set; }
			
			[JsonProperty("mmf")]
			public string MMF { get; set; }
			
			[JsonProperty("total")]
			public string Total { get; set; }
			
			[JsonProperty("uncleareddeposits")]
			public string UnclearedDeposits { get; set; }
			
			[JsonProperty("unsettledfunds")]
			public string UnsettledFunds { get; set; }
			
			[JsonProperty("yield")]
			public string Yield { get; set; }
		}
		
		public class AccountBalances_Securities
		{
			[JsonProperty("longoptions")]
			public string LongOptions { get; set; }

			[JsonProperty("longstocks")]
			public string LongStocks { get; set; }
			
			[JsonProperty("options")]
			public string Options { get; set; }
			
			[JsonProperty("shortoptions")]
			public string ShortOptions { get; set; }
			
			[JsonProperty("shortstocks")]
			public string ShortStocks { get; set; }
			
			[JsonProperty("stocks")]
			public string Stocks { get; set; }
			
			[JsonProperty("total")]
			public string Total { get; set; }	
		}
		#endregion
	
		#region Classes for serialization of account/holdings
		public class AccountHoldings
		{
			[JsonProperty("@id")]
			public string Id { get; set; }
			
			[JsonProperty("accountholdings")]
			public AccountHoldings_AccountHoldings AccountHolding { get; set; }
		}
		
		public class AccountHoldings_AccountHoldings
		{
			[JsonProperty("holding")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<AccountHoldings_AccountHoldings_Holding>))]
			public AccountHoldings_AccountHoldings_Holding[] Holding { get; set; }
		}
		
		public class AccountHoldings_AccountHoldings_Holding
		{
			[JsonProperty("accounttype")]
			public string AccountType { get; set; }
		
			[JsonProperty("costbasis")]
			public string CostBasis { get; set; }
			
			[JsonProperty("gainloss")]
			public string GainLoss { get; set; }
		
			[JsonProperty("instrument")]
			public AccountHoldings_AccountHoldings_Holding_Instrument Instrument { get; set; }
		
			[JsonProperty("marketvalue")]
			public string MarketValue { get; set; }
		
			[JsonProperty("marketvaluechange")]
			public string MarketValueChange { get; set; }
			
			[JsonProperty("price")]
			public string Price { get; set; }
		
			[JsonProperty("purchaseprice")]
			public string PurchasePrice { get; set; }
		
			[JsonProperty("qty")]
			public string Qty { get; set; }

			[JsonProperty("quote")]
			public AccountHoldings_AccountHoldings_Holding_Quote Quote { get; set; }
		
			[JsonProperty("underlying")]
			public string Underlying { get; set; }
		}
		
		public class AccountHoldings_AccountHoldings_Holding_Instrument
		{
			[JsonProperty("cusip")]
			public string CusIp { get; set; }
		
			[JsonProperty("desc")]
			public string Desc { get; set; }
		
			[JsonProperty("factor")]
			public string Factor { get; set; }
		
			[JsonProperty("sectyp")]
			public string SECTyp { get; set; }
		
			[JsonProperty("sym")]
			public string Sym { get; set; }
		}
		
		public class AccountHoldings_AccountHoldings_Holding_Quote
		{
			[JsonProperty("change")]
			public string Change { get; set; }
		
			[JsonProperty("lastprice")]
			public string LastPrice { get; set; }
		}
		#endregion
	
		#region Classes for serialization of account/status
		public class AccountStatus
		{
			[JsonProperty("@id")]
			public string Id { get; set; }
			
			[JsonProperty("orderstatus")]
			public AccountStatus_OrderStatus OrderStatus { get; set; }
		}
		
		public class AccountStatus_OrderStatus
		{
			[JsonProperty("order")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<AccountStatus_OrderStatus_Order>))]
			public AccountStatus_OrderStatus_Order[] Order { get; set; }
		}
		
		public class AccountStatus_OrderStatus_Order
		{
			[JsonProperty("fixmlmessage")]
			public string FixMLMessage { get; set; }
		}
		#endregion
		
		#region Classes for serialization of account/holdings
		public class AccountSummary
		{
			[JsonProperty("@id")]
			public string Id { get; set; }
			
			[JsonProperty("accountbalance")]
			public AccountSummary_AccountBalance AccountBalance { get; set; }
			
			[JsonProperty("accountholdings")]
			public AccountSummary_AccountHoldings AccountHoldings { get; set; }
		}
		
		public class AccountSummary_AccountBalance
		{
			[JsonProperty("account")]
			public string Account { get; set; }
			
			[JsonProperty("accountvalue")]
			public string AccountValue { get; set; }
			
			[JsonProperty("cashbalance")]
			public string CashBalance { get; set; }
			
			[JsonProperty("cashmv")]
			public string CashMV { get; set; }
			
			[JsonProperty("fedcall")]
			public string FedCall { get; set; }
						
			[JsonProperty("housecall")]
			public string HouseCall { get; set; }
			
			[JsonProperty("marginbalance")]
			public string MarginBalance { get; set; }

			[JsonProperty("marginmv")]
			public string MarginMV { get; set; }
			
			[JsonProperty("money")]
			public AccountSummary_AccountBalance_Money Money { get; set; }
			
			[JsonProperty("openbuyvalue")]
			public string OpenBuyValue { get; set; }

			[JsonProperty("securities")]
			public AccountSummary_AccountBalance_Securities Securities { get; set; }	

			[JsonProperty("shortbalance")]
			public string ShortBalance { get; set; }

			[JsonProperty("shortmv")]
			public string ShortMV { get; set; }			
		}
		
		public class AccountSummary_AccountBalance_Money
		{
			[JsonProperty("accruedinterest")]
			public string AccruedInterest { get; set; }
						
			[JsonProperty("cash")]
			public string Cash { get; set; }
						
			[JsonProperty("cashavailable")]
			public string CashAvailable { get; set; }
						
			[JsonProperty("marginbalance")]
			public string MarginBalance { get; set; }
						
			[JsonProperty("mmf")]
			public string MMF { get; set; }
						
			[JsonProperty("total")]
			public string Total { get; set; }
						
			[JsonProperty("uncleareddeposits")]
			public string UnclearedDeposits { get; set; }
						
			[JsonProperty("unsettledfunds")]
			public string UnsettledFunds { get; set; }
						
			[JsonProperty("yield")]
			public string Yield { get; set; }
		}
		
		public class AccountSummary_AccountBalance_Securities
		{
			[JsonProperty("longoptions")]
			public string LongOptions { get; set; }
			
			[JsonProperty("longstocks")]
			public string LongStocks { get; set; }
			
			[JsonProperty("options")]
			public string Options { get; set; }
			
			[JsonProperty("shortoptions")]
			public string ShortOptions { get; set; }
			
			[JsonProperty("shortstocks")]
			public string ShortStocks { get; set; }
			
			[JsonProperty("stocks")]
			public string Stocks { get; set; }
			
			[JsonProperty("total")]
			public string Total { get; set; }			
		}
	
		public class AccountSummary_AccountHoldings
		{
			[JsonProperty("displaydata")]
			public AccountSummary_AccountHoldings_DisplayData DisplayData { get; set; }
			
			[JsonProperty("holding")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<AccountSummary_AccountHoldings_Holding>))]
			public AccountSummary_AccountHoldings_Holding[] Holding { get; set; }
			
			[JsonProperty("totalsecurities")]
			public string TotalSecurities { get; set; }
		}
		
		public class AccountSummary_AccountHoldings_DisplayData
		{
			[JsonProperty("totalsecurities")]
			public string TotalSecurities { get; set; }
		}
		
		public class AccountSummary_AccountHoldings_Holding
		{
			[JsonProperty("accounttype")]
			public string AccountType { get; set; }
		
			[JsonProperty("costbasis")]
			public string CostBasis { get; set; }
			
			[JsonProperty("displaydata")]
			public AccountSummary_AccountHoldings_Holding_DisplayData DisplayData { get; set; }
			
			[JsonProperty("gainloss")]
			public string GainLoss { get; set; }
		
			[JsonProperty("instrument")]
			public AccountSummary_AccountHoldings_Holding_Instrument Instrument { get; set; }
		
			[JsonProperty("marketvalue")]
			public string MarketValue { get; set; }
		
			[JsonProperty("marketvaluechange")]
			public string MarketValueChange { get; set; }
			
			[JsonProperty("price")]
			public string Price { get; set; }
		
			[JsonProperty("purchaseprice")]
			public string PurchasePrice { get; set; }
		
			[JsonProperty("qty")]
			public string Qty { get; set; }

			[JsonProperty("quote")]
			public AccountSummary_AccountHoldings_Holding_Quote Quote { get; set; }
		
			[JsonProperty("underlying")]
			public string Underlying { get; set; }
		}
		
		public class AccountSummary_AccountHoldings_Holding_DisplayData
		{
			[JsonProperty("accounttype")]
			public string AccountType { get; set; }

			[JsonProperty("assetclass")]
			public string AssetClass { get; set; }

			[JsonProperty("change")]
			public string Change { get; set; }

			[JsonProperty("costbasis")]
			public string CostBasis { get; set; }

			[JsonProperty("desc")]
			public string Desc { get; set; }

			[JsonProperty("lastprice")]
			public string LastPrice { get; set; }

			[JsonProperty("marketvalue")]
			public string MarketValue { get; set; }

			[JsonProperty("marketvaluechange")]
			public string MarketValueChange { get; set; }

			[JsonProperty("qty")]
			public string Qty { get; set; }

			[JsonProperty("symbol")]
			public string Symbol { get; set; }
		}
		
		public class AccountSummary_AccountHoldings_Holding_Instrument
		{
			[JsonProperty("cusip")]
			public string CusIp { get; set; }
		
			[JsonProperty("desc")]
			public string Desc { get; set; }
		
			[JsonProperty("factor")]
			public string Factor { get; set; }
		
			[JsonProperty("sectyp")]
			public string SECTyp { get; set; }
		
			[JsonProperty("sym")]
			public string Sym { get; set; }
		}
		
		public class AccountSummary_AccountHoldings_Holding_Quote
		{
			[JsonProperty("change")]
			public string Change { get; set; }
		
			[JsonProperty("lastprice")]
			public string LastPrice { get; set; }
		}
		#endregion
	
		#region Classes for serialization of trade/preview
		public class TradePreviewResponse
		{
			[JsonProperty("@id")]
			public string Id { get; set; }
			
			[JsonProperty("warning")]
			public TradePreviewResponse_Warning Warning { get; set; }		  
			
			[JsonProperty("estcommission")]
			public string EstCommission { get; set; }
		
			[JsonProperty("marginrequirement")]
			public string MarginRequirement { get; set; }
			
			[JsonProperty("principal")]
			public string Principal { get; set; }
			
			[JsonProperty("quotes")]
			public TradePreviewResponse_Quotes Quotes { get; set; }
			
			[JsonProperty("secfee")]
			public string SECFee { get; set; }
		}
		
		public class TradePreviewResponse_Warning
		{
			[JsonProperty("warningcode")]
			public string WarningCode { get; set; }
			
			[JsonProperty("warningtext")]
			public string WarningText { get; set; }
		}
		
		public class TradePreviewResponse_Quotes
		{
			[JsonProperty("instrumentquote")]
			public TradePreviewResponse_Quotes_InstrumentQuote InstrumentQuote { get; set; }
		}
		
		public class TradePreviewResponse_Quotes_InstrumentQuote
		{
			[JsonProperty("displaydata")]
			public TradePreviewResponse_Quotes_InstrumentQuote_DisplayData DisplayData { get; set; }
			
			[JsonProperty("greeks")]
			public string Greeks { get; set; }
			
			[JsonProperty("instrument")]
			public TradePreviewResponse_Quotes_InstrumentQuote_Instrument Instrument { get; set; }
				
			[JsonProperty("quote")]
			public TradePreviewResponse_Quotes_InstrumentQuote_Quote Quote { get; set; }
			
			[JsonProperty("unknownsymbol")]
			public string UnknownSymbol { get; set; }
		}
		
		public class TradePreviewResponse_Quotes_InstrumentQuote_DisplayData
		{
			[JsonProperty("askprice")]
			public string AskPrice { get; set; }
			
			[JsonProperty("asksize")]
			public string AskSize { get; set; }
			
			[JsonProperty("bidprice")]
			public string BidPrice { get; set; }
			
			[JsonProperty("bidsize")]
			public string BidSize { get; set; }
			
			[JsonProperty("change")]
			public string Change { get; set; }
			
			[JsonProperty("contracthigh")]
			public string ContractHigh { get; set; }
			
			[JsonProperty("contractlow")]
			public string ContractLow { get; set; }
			
			[JsonProperty("contractsize")]
			public string ContractSize { get; set; }
			
			[JsonProperty("delta")]
			public string Delta { get; set; }
			
			[JsonProperty("desc")]
			public string Desc { get; set; }
			
			[JsonProperty("dividendamount")]
			public string DividendAmount { get; set; }
			
			[JsonProperty("dividendexdate")]
			public string DividendExDate { get; set; }
			
			[JsonProperty("dividendpaydate")]
			public string DividendPayDate { get; set; }
			
			[JsonProperty("dividendyield")]
			public string DividendYield { get; set; }
			
			[JsonProperty("earningspershare")]
			public string EarningsPerShare { get; set; }
			
			[JsonProperty("exch")]
			public string Exch { get; set; }
			
			[JsonProperty("expiry")]
			public string Expiry { get; set; }
			
			[JsonProperty("gamma")]
			public string Gamma { get; set; }
			
			[JsonProperty("high52price")]
			public string High52Price { get; set; }
			
			[JsonProperty("highprice")]
			public string HighPrice { get; set; }
			
			[JsonProperty("impvolatility")]
			public string ImpVolatility { get; set; }
			
			[JsonProperty("lastclosingprice")]
			public string LastClosingPrice { get; set; }
			
			[JsonProperty("lastprice")]
			public string LastPrice { get; set; }
			
			[JsonProperty("low52price")]
			public string Low52Price { get; set; }
			
			[JsonProperty("lowprice")]
			public string LowPrice { get; set; }
			
			[JsonProperty("nav")]
			public string Nav { get; set; }
			
			[JsonProperty("openinterest")]
			public string OpenInterest { get; set; }
			
			[JsonProperty("openprice")]
			public string OpenPrice { get; set; }
			
			[JsonProperty("optval")]
			public string OptVal { get; set; }
			
			[JsonProperty("optiontype")]
			public string OptionType { get; set; }
			
			[JsonProperty("pctchange")]
			public string PctChange { get; set; }
			
			[JsonProperty("priceearningsratio")]
			public string PriceEarningsRatio { get; set; }
			
			[JsonProperty("rho")]
			public string Rho { get; set; }
			
			[JsonProperty("sessionvolume")]
			public string SessionVolume { get; set; }
			
			[JsonProperty("strike")]
			public string Strike { get; set; }
			
			[JsonProperty("symbol")]
			public string Symbol { get; set; }
			
			[JsonProperty("theta")]
			public string Theta { get; set; }
			
			[JsonProperty("unknownsymbol")]
			public string UnknownSymbol { get; set; }
			
			[JsonProperty("vega")]
			public string Vega { get; set; }
			
			[JsonProperty("volatility")]
			public string Volatility { get; set; }
			
			[JsonProperty("volume")]
			public string Volume { get; set; }
		}
		
		public class TradePreviewResponse_Quotes_InstrumentQuote_Instrument
		{
			[JsonProperty("desc")]
			public string Desc { get; set; }
			
			[JsonProperty("exch")]
			public string Exch { get; set; }
			
			[JsonProperty("sectyp")]
			public string SECType { get; set; }
			
			[JsonProperty("sym")]
			public string Sym { get; set; }
		}
		
		public class TradePreviewResponse_Quotes_InstrumentQuote_Quote
		{
			[JsonProperty("askprice")]
			public string AskPrice { get; set; }
			
			[JsonProperty("bidprice")]
			public string BidPrice { get; set; }
			
			[JsonProperty("change")]
			public string Change { get; set; }
			
			[JsonProperty("extendedquote")]
			public TradePreviewResponse_Quotes_InstrumentQuote_Quote_ExtendedQuote ExtendedQuote { get; set; }
			
			[JsonProperty("lastprice")]
			public string LastPrice { get; set; }
			
			[JsonProperty("pctchange")]
			public string PctChange { get; set; }
		}
		
		public class TradePreviewResponse_Quotes_InstrumentQuote_Quote_ExtendedQuote
		{
			[JsonProperty("asksize")]
			public string AskSize { get; set; }
			
			[JsonProperty("bidsize")]
			public string BidSize { get; set; }
			
			[JsonProperty("dividenddata")]
			public TradePreviewResponse_Quotes_InstrumentQuote_Quote_ExtendedQuote_DividendData DividendData { get; set; }
			
			[JsonProperty("earningspershare")]
			public string EarningsPerShare { get; set; }
			
			[JsonProperty("high52price")]
			public string High52Price { get; set; }
			
			[JsonProperty("highprice")]
			public string HighPrice { get; set; }
			
			[JsonProperty("low52price")]
			public string Low52Price { get; set; }
			
			[JsonProperty("lowprice")]
			public string LowPrice { get; set; }
			
			[JsonProperty("openprice")]
			public string OpenPrice { get; set; }
			
			[JsonProperty("prevclose")]
			public string PrevClose { get; set; }
			
			[JsonProperty("priceearningsratio")]
			public string PriceEarningsRatio { get; set; }
			
			[JsonProperty("sessionvolume")]
			public string SessionVolume { get; set; }
			
			[JsonProperty("volume")]
			public string Volume { get; set; }
		}
		
		public class TradePreviewResponse_Quotes_InstrumentQuote_Quote_ExtendedQuote_DividendData
		{
			[JsonProperty("amt")]
			public string Amt { get; set; }
		
			[JsonProperty("exdate")]
			public string ExDate { get; set; }
		
			[JsonProperty("yield")]
			public string Yield { get; set; }
		}
		#endregion
		
		#region Classes for serialization of trade/submit
		public class TradeSubmitOverrideResponse
		{
			[JsonProperty("@id")]
			public string Id { get; set; }
			
			[JsonProperty("clientorderid")]
			public string ClientOrderId { get; set; }
			
			[JsonProperty("orderstatus")]
			public string OrderStatus { get; set; }
		}
		
		public class TradeSubmitResponse
		{
			[JsonProperty("@id")]
			public string Id { get; set; }
			
			[JsonProperty("warning")]
			public TradeSubmitResponse_Warning Warning { get; set; }		  
			
			[JsonProperty("estcommission")]
			public string EstCommission { get; set; }
		
			[JsonProperty("marginrequirement")]
			public string MarginRequirement { get; set; }
			
			[JsonProperty("principal")]
			public string Principal { get; set; }
			
			[JsonProperty("quotes")]
			public TradeSubmitResponse_Quotes Quotes { get; set; }
			
			[JsonProperty("secfee")]
			public string SECFee { get; set; }
		}
			
		public class TradeSubmitResponse_Warning
		{
			[JsonProperty("warningcode")]
			public string WarningCode { get; set; }
			
			[JsonProperty("warningtext")]
			public string WarningText { get; set; }
		}
		
		public class TradeSubmitResponse_Quotes
		{
			[JsonProperty("instrumentquote")]
			public TradeSubmitResponse_Quotes_InstrumentQuote InstrumentQuote { get; set; }
		}
		
		public class TradeSubmitResponse_Quotes_InstrumentQuote
		{
			[JsonProperty("greeks")]
			public string Greeks { get; set; }
			
			[JsonProperty("instrument")]
			public TradeSubmitResponse_Quotes_InstrumentQuote_Instrument Instrument { get; set; }
				
			[JsonProperty("quote")]
			public TradeSubmitResponse_Quotes_InstrumentQuote_Quote Quote { get; set; }
			
			[JsonProperty("unknownsymbol")]
			public string UnknownSymbol { get; set; }
		}
		
		public class TradeSubmitResponse_Quotes_InstrumentQuote_Instrument
		{
			[JsonProperty("desc")]
			public string Desc { get; set; }
			
			[JsonProperty("exch")]
			public string Exch { get; set; }
			
			[JsonProperty("sectyp")]
			public string SECType { get; set; }
			
			[JsonProperty("sym")]
			public string Sym { get; set; }
		}
		
		public class TradeSubmitResponse_Quotes_InstrumentQuote_Quote
		{
			[JsonProperty("askprice")]
			public string AskPrice { get; set; }
			
			[JsonProperty("bidprice")]
			public string BidPrice { get; set; }
			
			[JsonProperty("change")]
			public string Change { get; set; }
			
			[JsonProperty("extendedquote")]
			public TradeSubmitResponse_Quotes_InstrumentQuote_Quote_ExtendedQuote ExtendedQuote { get; set; }
			
			[JsonProperty("lastprice")]
			public string LastPrice { get; set; }
			
			[JsonProperty("pctchange")]
			public string PctChange { get; set; }
		}
		
		public class TradeSubmitResponse_Quotes_InstrumentQuote_Quote_ExtendedQuote
		{
			[JsonProperty("asksize")]
			public string AskSize { get; set; }
			
			[JsonProperty("bidsize")]
			public string BidSize { get; set; }
			
			[JsonProperty("dividenddata")]
			public TradeSubmitResponse_Quotes_InstrumentQuote_Quote_ExtendedQuote_DividendData DividendData { get; set; }
			
			[JsonProperty("earningspershare")]
			public string EarningsPerShare { get; set; }
			
			[JsonProperty("high52price")]
			public string High52Price { get; set; }
			
			[JsonProperty("highprice")]
			public string HighPrice { get; set; }
			
			[JsonProperty("low52price")]
			public string Low52Price { get; set; }
			
			[JsonProperty("lowprice")]
			public string LowPrice { get; set; }
			
			[JsonProperty("openprice")]
			public string OpenPrice { get; set; }
			
			[JsonProperty("prevclose")]
			public string PrevClose { get; set; }
			
			[JsonProperty("priceearningsratio")]
			public string PriceEarningsRatio { get; set; }
			
			[JsonProperty("sessionvolume")]
			public string SessionVolume { get; set; }
			
			[JsonProperty("volume")]
			public string Volume { get; set; }
		}
		
		public class TradeSubmitResponse_Quotes_InstrumentQuote_Quote_ExtendedQuote_DividendData
		{
			[JsonProperty("amt")]
			public string Amt { get; set; }
		
			[JsonProperty("exdate")]
			public string ExDate { get; set; }
		
			[JsonProperty("yield")]
			public string Yield { get; set; }
		}
		#endregion
		
		#region Classes for serialization of trade/quote
		public class TradeQuote
		{
			[JsonProperty("@id")]
			public string Id { get; set; }
			
			[JsonProperty("quotes")]
			public TradeQuote_Quotes Quotes { get; set; }
		}
		
		public class TradeQuote_Quotes
		{
			[JsonProperty("instrumentquote")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<TradeQuote_Quotes_InstrumentQuote>))]
			public TradeQuote_Quotes_InstrumentQuote[] InstrumentQuote { get; set; }
		}
		
		public class TradeQuote_Quotes_InstrumentQuote
		{
			[JsonProperty("greeks")]
			public string Greeks { get; set; }
			
			[JsonProperty("instrument")]
			public TradeQuote_Quotes_InstrumentQuote_Instrument Instrument { get; set; }
				
			[JsonProperty("quote")]
			public TradeQuote_Quotes_InstrumentQuote_Quote Quote { get; set; }
			
			[JsonProperty("unknownsymbol")]
			public string UnknownSymbol { get; set; }
		}
		
		public class TradeQuote_Quotes_InstrumentQuote_Instrument
		{
			[JsonProperty("desc")]
			public string Desc { get; set; }
			
			[JsonProperty("exch")]
			public string Exch { get; set; }
			
			[JsonProperty("sectyp")]
			public string SECType { get; set; }
			
			[JsonProperty("sym")]
			public string Sym { get; set; }
		}
		
		public class TradeQuote_Quotes_InstrumentQuote_Quote
		{
			[JsonProperty("askprice")]
			public string AskPrice { get; set; }
			
			[JsonProperty("bidprice")]
			public string BidPrice { get; set; }
			
			[JsonProperty("change")]
			public string Change { get; set; }
			
			[JsonProperty("extendedquote")]
			public TradeQuote_Quotes_InstrumentQuote_Quote_ExtendedQuote ExtendedQuote { get; set; }
			
			[JsonProperty("lastprice")]
			public string LastPrice { get; set; }
			
			[JsonProperty("pctchange")]
			public string PctChange { get; set; }
		}
		
		public class TradeQuote_Quotes_InstrumentQuote_Quote_ExtendedQuote
		{
			[JsonProperty("asksize")]
			public string AskSize { get; set; }
			
			[JsonProperty("bidsize")]
			public string BidSize { get; set; }
			
			[JsonProperty("dividenddata")]
			public TradeQuote_Quotes_InstrumentQuote_Quote_ExtendedQuote_DividendData DividendData { get; set; }
			
			[JsonProperty("earningspershare")]
			public string EarningsPerShare { get; set; }
			
			[JsonProperty("high52price")]
			public string High52Price { get; set; }
			
			[JsonProperty("highprice")]
			public string HighPrice { get; set; }
			
			[JsonProperty("low52price")]
			public string Low52Price { get; set; }
			
			[JsonProperty("lowprice")]
			public string LowPrice { get; set; }
			
			[JsonProperty("openprice")]
			public string OpenPrice { get; set; }
			
			[JsonProperty("prevclose")]
			public string PrevClose { get; set; }
			
			[JsonProperty("priceearningsratio")]
			public string PriceEarningsRatio { get; set; }
			
			[JsonProperty("sessionvolume")]
			public string SessionVolume { get; set; }
			
			[JsonProperty("volume")]
			public string Volume { get; set; }
		}
		
		public class TradeQuote_Quotes_InstrumentQuote_Quote_ExtendedQuote_DividendData
		{
			[JsonProperty("amt")]
			public string Amt { get; set; }
		
			[JsonProperty("exdate")]
			public string ExDate { get; set; }
		
			[JsonProperty("yield")]
			public string Yield { get; set; }
		}
		#endregion
	}
}

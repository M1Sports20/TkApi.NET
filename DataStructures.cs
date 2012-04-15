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
using Newtonsoft.Json;
using System.Reflection;


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
	
	namespace DataStructures {
		public enum AccountsHistory_Range {
			All,
			Today,
			Current_Week,
			Current_Month,
			Last_Month
		}
		public enum AccountsHistory_Transactions {
			All,
			Bookkeeping,
			Trade
		}
		
		public class TKBaseType {
			[JsonProperty("@id")]
			public string Id { get; set; }
			
			[JsonProperty("type")]
			public string Type { get; set; }
			
			[JsonProperty("name")]
			public string Name { get; set; }
			
			[JsonProperty("description")]
			public string Description { get; set; }
			
			[JsonProperty("path")] 
			public string Path { get; set; }
			
			[JsonProperty("error")] // Some errors are here
			public string Error { get; set; }
			
			
			public static bool operator ==(TKBaseType lhs, TKBaseType rhs) {
				if (ReferenceEquals(lhs, rhs)) return true;
				
				// Cast to object to prevent recursion
				if ((object)lhs == null || (object)rhs == null) return false;
			
				return lhs.Equals(rhs);
			}
			public static bool operator !=(TKBaseType lhs, TKBaseType rhs) {
			 	return !(lhs == rhs);
			}
			public override bool Equals(object obj) {
				if (obj == null) return false; // Must be here to adhere to Microsoft's standard for overriding Equals
				
				if (obj.GetType() != this.GetType()) return false; // Are they the same object type
				
				bool equal = true;
				
				foreach (PropertyInfo myPropertyInfo in this.GetType().GetProperties()) {
					// Skip Id.  This is unique for every transaction
					if (myPropertyInfo.Name != "Id") {
		                foreach (PropertyInfo objPropertyInfo in obj.GetType().GetProperties()) {
		                    if (myPropertyInfo.Name == objPropertyInfo.Name) {
								object myProp = myPropertyInfo.GetValue(this, null);
								object objProp = objPropertyInfo.GetValue(obj, null);
								if (myProp != null && objProp != null) {
							    	equal = myProp.ToString() == objProp.ToString();
								} else if (myProp == null && objProp == null) {
									// Do nothing
								} else {
									equal = false;
								}
								
		                        if (!equal) break;
		                    }
		                }
		                if (!equal) break;
					}
	            }
				return equal;
			}
		}
		
		public class Accounts: TKBaseType {
			[JsonProperty("accounts")]
			public TAccounts Accounts2 { get; set; }	
			public class TAccounts {
				[JsonProperty("accountsummary")]
				[JsonConverter(typeof(ObjectSometimesArrayConverter<TAccountSummary>))]
				public TAccountSummary[] AccountSummary { get; set; }
				public class TAccountSummary	{
					[JsonProperty("account")]
					public string Account { get; set; }
				
					[JsonProperty("accountbalance")]
					public TAccountBalance AccountBalance { get; set; }
					public class TAccountBalance	{
						[JsonProperty("account")]
						public string Account { get; set; }
						
						[JsonProperty("accountvalue")]
						public string AccountValue { get; set; }
						
						[JsonProperty("cashbalance")]
						public string CashBalance { get; set; }
					
						[JsonProperty("cashmv")]
						public string cashmv { get; set; }
						
						[JsonProperty("fedcall")]
						public string FedCall { get; set; }
						
						[JsonProperty("housecall")]
						public string HouseCall { get; set; }
						
						[JsonProperty("marginbalance")]
						public string MarginBalance { get; set; }
						
						[JsonProperty("marginmv")]
						public string MarginMv { get; set; }
						
						[JsonProperty("money")]
						public TMoney Money;
						public class TMoney {
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
						
						[JsonProperty("openbuyvalue")]
						public string OpenBuyValue { get; set; }
								
						[JsonProperty("securities")]
						public TSecurities Security;
						public class TSecurities {
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
						
						[JsonProperty("shortbalance")]
						public string ShortBalance { get; set; }
				
						[JsonProperty("shortmv")]
						public string ShortMv { get; set; }
					}
					
					[JsonProperty("accountholdings")]
					public TAccountHoldings AccountHoldings { get; set; }
					public class TAccountHoldings {
						[JsonProperty("displaydata")]
						public TDisplayData DisplayData { get; set; }
						public class TDisplayData {
							[JsonProperty("totalsecurities")]
							public string TotalSecurities { get; set; }			
						}
						
						[JsonProperty("holding")]
						[JsonConverter(typeof(ObjectSometimesArrayConverter<THolding>))]
						public THolding[] Holdings { get; set; }
						public class THolding	{
							[JsonProperty("accounttype")]
							public string AccountType { get; set; }
							
							[JsonProperty("costbasis")]
							public string CostBasis { get; set; }
							
							[JsonProperty("displaydata")]
							public TDisplayData DisplayData { get; set; }
							public class TDisplayData	{
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
									
							[JsonProperty("gainloss")]
							public string GainLoss { get; set; }
							
							[JsonProperty("instrument")]
							public TInstrument Instrument { get; set; }
							public class TInstrument {
								[JsonProperty("cusip")]
								public string CusIp { get; set; }
								
								[JsonProperty("desc")]
								public string Desc { get; set; }
								
								[JsonProperty("factor")]
								public string Factor { get; set; }
								
								[JsonProperty("matdt")]
								public string MatDt { get; set; }
								
								[JsonProperty("mmy")]
								public string Mmy { get; set; }
								
								[JsonProperty("mult")]
								public string Mult { get; set; }
								
								[JsonProperty("sectyp")]
								public string SECTyp { get; set; }
								
								[JsonProperty("strkpx")]
								public string StrkPx { get; set; }
								
								[JsonProperty("sym")]
								public string Sym { get; set; }
							}
							
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
							public TQuote Quote { get; set; }
							public class TQuote {
								[JsonProperty("change")]
								public string Change { get; set; }
							
								[JsonProperty("lastprice")]
								public string LastPrice { get; set; }
							
							}
							
							[JsonProperty("underlying")]
							public string Underlying { get; set; }
						}
						
						[JsonProperty("totalsecurities")]
						public string TotalSecurities { get; set; }	
					}
				}
			}
		}

		public class AccountsBalance: TKBaseType {	
			[JsonProperty("accountbalance")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<TAccountBalance>))]
			public TAccountBalance[] AccountBalance { get; set; }
			public class TAccountBalance {
				[JsonProperty("account")]		
				public string AccountNumber { get; set; }
		
				[JsonProperty("accountname")]
				public string AccountName { get; set; }
		
				[JsonProperty("accountvalue")]
				public string AccountValue { get; set; }
			}
			
			[JsonProperty("totalbalance")]
			public TTotalBalance TotalBalance { get; set; }
			public class TTotalBalance	{
				[JsonProperty("accountvalue")]
				public string AccountValue { get; set; }
			}
		}
			
		public class AccountsSingle : TKBaseType {
			[JsonProperty("accountbalance")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<Accounts.TAccounts.TAccountSummary.TAccountBalance>))]
			public Accounts.TAccounts.TAccountSummary.TAccountBalance[] AccountBalance { get; set; }
			
			[JsonProperty("accountholdings")]
			public Accounts.TAccounts.TAccountSummary.TAccountHoldings AccountHoldings { get; set; }
		}
		
		public class AccountsBalancesSingle: TKBaseType {
			[JsonProperty("accountbalance")]
			public Accounts.TAccounts.TAccountSummary.TAccountBalance AccountBalance { get; set; }
		}
		
		public class AccountsHistory: TKBaseType {
			[JsonProperty("totalrows")]
			public string TotalRows { get; set; }
			
			[JsonProperty("transactions")]
			public TTransactions Transactions { get; set; }
			public class TTransactions
			{		
				[JsonProperty("transaction")]
				[JsonConverter(typeof(ObjectSometimesArrayConverter<TTransaction>))]
				public TTransaction[] Transaction { get; set; }
				public class TTransaction
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
					public TTTransaction Transaction { get; set; }
					public class TTTransaction
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
						public TSecurity Security { get; set; }
						public class TSecurity {
							[JsonProperty("cusip")]
							public string CusIp { get; set; }
							
							[JsonProperty("id")]
							public string Id { get; set; }
							
							[JsonProperty("sectyp")]
							public string SECTyp { get; set; }
					
							[JsonProperty("sym")]
							public string Symbol { get; set; }
						}
						
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
				}
			}
		}
		
		public class AccountsHoldings: TKBaseType {
			[JsonProperty("accountholdings")]
			public TAccountHoldings AccountHoldings{ get; set; }
			public class TAccountHoldings {
				[JsonProperty("holding")]
				[JsonConverter(typeof(ObjectSometimesArrayConverter<Accounts.TAccounts.TAccountSummary.TAccountHoldings.THolding>))]
				public Accounts.TAccounts.TAccountSummary.TAccountHoldings.THolding[] Holdings { get; set; }
			}
		}
		
		public class Orders: TKBaseType {
			[JsonProperty("orderstatus")]
			public TOrderStatus OrderStatus { get; set; }
			public class TOrderStatus {
				[JsonProperty("order")]
				[JsonConverter(typeof(ObjectSometimesArrayConverter<TOrder>))]
				public TOrder[] Order { get; set; }
				public class TOrder {
					[JsonProperty("fixmlmessage")]
					public string FixmlMessage { get; set; }
				}
			}
		}
			
		public class OrdersPost: TKBaseType {
			[JsonProperty("clientorderid")]
			public string ClientOrderId { get; set; }
		
			[JsonProperty("orderstatus")]
			public string OrderStatus { get; set; }
			
			[JsonProperty("warning")]
			public TWarning Warning { get; set; }
			public class TWarning {
				[JsonProperty("warningcode")]
				public string WarningCode { get; set; }
				
				[JsonProperty("warningtext")]
				public string WarningText { get; set; }	
			}
			
			[JsonProperty("estcommission")]
			public string EstCommission { get; set; }
			
			[JsonProperty("marginrequirement")]
			public string MarginRequirement { get; set; }
			
			[JsonProperty("netamt")]
			public string NetAmt { get; set; }
			
			[JsonProperty("principal")]
			public string Principal { get; set; }
			
			[JsonProperty("quotes")]
			Quotess.TQuotes Quotes { get; set; }
				
			[JsonProperty("secfee")]
			public string SecFee { get; set; }
		}
		
		public class MarketClock: TKBaseType {
			[JsonProperty("date")]
			public string Date { get; set; }

			[JsonProperty("status")]
			public TStatus Status { get; set; }
			public class TStatus {
				[JsonProperty("current")]
				public string Current { get; set; }
			}
			
			[JsonProperty("message")]
			public string Message { get; set; }
			
			[JsonProperty("unixtime")]
			public string UnixTime { get; set; }
		}
		
		public class MarketExtQuotes: TKBaseType {
			[JsonProperty("quotes")]
			public TQuotes Quotes { get; set; }
			public class TQuotes {
				[JsonProperty("quote")]
				[JsonConverter(typeof(ObjectSometimesArrayConverter<TQuote>))]
				public TQuote[] Quote { get; set; }
				public class TQuote {
					[JsonProperty("adp_100")]
					public string adp_100 { get; set; }
					
					[JsonProperty("adp_200")]
					public string adp_200 { get; set; }
					
					[JsonProperty("adp_50")]
					public string adp_50 { get; set; }
					
					[JsonProperty("adv_21")]
					public string adv_21 { get; set; }
					
					[JsonProperty("adv_30")]
					public string adv_30 { get; set; }
					
					[JsonProperty("adv_90")]
					public string adv_90 { get; set; }
					
					[JsonProperty("ask")]
					public string ask { get; set; }
					
					[JsonProperty("ask_time")]
					public string ask_time { get; set; }
					
					[JsonProperty("asksz")]
					public string asksz { get; set; }
					
					[JsonProperty("basis")]
					public string basis { get; set; }
					
					[JsonProperty("beta")]
					public string beta { get; set; }
					
					[JsonProperty("bid")]
					public string bid { get; set; }
					
					[JsonProperty("bid_time")]
					public string bid_time { get; set; }
					
					[JsonProperty("bidsz")]
					public string bidsz { get; set; }
					
					[JsonProperty("bidtick")]
					public string bidtick { get; set; }
					
					[JsonProperty("chg")]
					public string chg { get; set; }
					
					[JsonProperty("chg_sign")]
					public string chg_sign { get; set; }
					
					[JsonProperty("chg_t")]
					public string chg_t { get; set; }
					
					[JsonProperty("cl")]
					public string cl { get; set; }
					
					[JsonProperty("contract_size")]
					public string contract_size { get; set; }
					
					[JsonProperty("cusip")]
					public string cusip { get; set; }
					
					[JsonProperty("date")]
					public string date { get; set; }
					
					[JsonProperty("datetime")]
					public string datetime { get; set; }
					
					[JsonProperty("days_to_expiration")]
					public string days_to_expiration { get; set; }
					
					[JsonProperty("div")]
					public string div { get; set; }
					
					[JsonProperty("divexdate")]
					public string divexdate { get; set; }
					
					[JsonProperty("divfreq")]
					public string divfreq { get; set; }
					
					[JsonProperty("divpaydt")]
					public string divpaydt { get; set; }
					
					[JsonProperty("dollar_value")]
					public string dollar_value { get; set; }
					
					[JsonProperty("eps")]
					public string eps { get; set; }
					
					[JsonProperty("exch")]
					public string exch { get; set; }
					
					[JsonProperty("exch_desc")]
					public string exch_desc { get; set; }
					
					[JsonProperty("hi")]
					public string hi { get; set; }
					
					[JsonProperty("iad")]
					public string iad { get; set; }
					
					[JsonProperty("idelta")]
					public string idelta { get; set; }
					
					[JsonProperty("igamma")]
					public string igamma { get; set; }
					
					[JsonProperty("imp_volatility")]
					public string imp_volatility { get; set; }
					
					[JsonProperty("incr_vl")]
					public string incr_vl { get; set; }
					
					[JsonProperty("irho")]
					public string irho { get; set; }
					
					[JsonProperty("issue_desc")]
					public string issue_desc { get; set; }
					
					[JsonProperty("itheta")]
					public string itheta { get; set; }
					
					[JsonProperty("ivega")]
					public string ivega { get; set; }
					
					[JsonProperty("last")]
					public string last { get; set; }
					
					[JsonProperty("lo")]
					public string lo { get; set; }
					
					[JsonProperty("name")]
					public string name { get; set; }
					
					[JsonProperty("op_delivery")]
					public string op_delivery { get; set; }
					
					[JsonProperty("op_flag")]
					public string op_flag { get; set; }
					
					[JsonProperty("op_style")]
					public string op_style { get; set; }
					
					[JsonProperty("op_subclass")]
					public string op_subclass { get; set; }
					
					[JsonProperty("openinterest")]
					public string openinterest { get; set; }
					
					[JsonProperty("opn")]
					public string opn { get; set; }
					
					[JsonProperty("opt_val")]
					public string opt_val { get; set; }
					
					[JsonProperty("pchg")]
					public string pchg { get; set; }
					
					[JsonProperty("pchg_sign")]
					public string pchg_sign { get; set; }
					
					[JsonProperty("pcls")]
					public string pcls { get; set; }
					
					[JsonProperty("pe")]
					public string pe { get; set; }
					
					[JsonProperty("phi")]
					public string phi { get; set; }
					
					[JsonProperty("plo")]
					public string plo { get; set; }
					
					[JsonProperty("popn")]
					public string popn { get; set; }
					
					[JsonProperty("pr_adp_100")]
					public string pr_adp_100 { get; set; }
					
					[JsonProperty("pr_adp_200")]
					public string pr_adp_200 { get; set; }
					
					[JsonProperty("pr_adp_50")]
					public string pr_adp_50 { get; set; }
					
					[JsonProperty("pr_date")]
					public string pr_date { get; set; }
					
					[JsonProperty("pr_openinterest")]
					public string pr_openinterest { get; set; }
					
					[JsonProperty("prbook")]
					public string prbook { get; set; }
					
					[JsonProperty("prchg")]
					public string prchg { get; set; }
					
					[JsonProperty("prem_mult")]
					public string prem_mult { get; set; }
					
					[JsonProperty("put_call")]
					public string put_call { get; set; }
					
					[JsonProperty("pvol")]
					public string pvol { get; set; }
					
					[JsonProperty("qcond")]
					public string qcond { get; set; }
					
					[JsonProperty("rootsymbol")]
					public string rootsymbol { get; set; }
					
					[JsonProperty("secclass")]
					public string secclass { get; set; }
					
					[JsonProperty("sesn")]
					public string sesn { get; set; }
					
					[JsonProperty("sho")]
					public string sho { get; set; }
					
					[JsonProperty("strikeprice")]
					public string strikeprice { get; set; }
					
					[JsonProperty("symbol")]
					public string symbol { get; set; }
					
					[JsonProperty("tcond")]
					public string tcond { get; set; }
					
					[JsonProperty("timestamp")]
					public string timestamp { get; set; }
					
					[JsonProperty("tr_num")]
					public string tr_num { get; set; }
					
					[JsonProperty("tradetick")]
					public string tradetick { get; set; }
					
					[JsonProperty("trend")]
					public string trend { get; set; }
					
					[JsonProperty("under_cusip")]
					public string under_cusip { get; set; }
					
					[JsonProperty("undersymbol")]
					public string undersymbol { get; set; }
					
					[JsonProperty("vl")]
					public string vl { get; set; }
					
					[JsonProperty("volatility12")]
					public string volatility12 { get; set; }
					
					[JsonProperty("vwap")]
					public string vwap { get; set; }
					
					[JsonProperty("wk52hi")]
					public string wk52hi { get; set; }
					
					[JsonProperty("wk52hidate")]
					public string wk52hidate { get; set; }
					
					[JsonProperty("wk52lo")]
					public string wk52lo { get; set; }
					
					[JsonProperty("wk52lodate")]
					public string wk52lodate { get; set; }
					
					[JsonProperty("xdate")]
					public string xdate { get; set; }
					
					[JsonProperty("xday")]
					public string xday { get; set; }
					
					[JsonProperty("xmonth")]
					public string xmonth { get; set; }
					
					[JsonProperty("xyear")]
					public string xyear { get; set; }
					
					[JsonProperty("yield")]
					public string yield { get; set; }
				}
			}
		}
		
		public class MarketOptionsStrikes: TKBaseType {
			[JsonProperty("prices")]
			public TPrices Prices { get; set; }
			public class TPrices  {
				[JsonProperty("price")]
				[JsonConverter(typeof(ObjectSometimesArrayConverter<string>))]
				public string[] Price { get; set; }
			}
		}
		
		public class MarketOptionsExpirations: TKBaseType {
			[JsonProperty("expirationdates")]
			public TExpirationDates ExpirationDates { get; set; }
			public class TExpirationDates  {
				[JsonProperty("date")]
				[JsonConverter(typeof(ObjectSometimesArrayConverter<string>))]
				public string[] Date { get; set; }
			}
		}
		
		public class Quotess: TKBaseType {
			[JsonProperty("quotes")]
			public TQuotes Quotes { get; set; }
			public class TQuotes
			{
				[JsonProperty("instrumentquote")]
				[JsonConverter(typeof(ObjectSometimesArrayConverter<TInstrumentQuote>))]
				public TInstrumentQuote[] InstrumentQuote { get; set; }
				public class TInstrumentQuote
				{
					[JsonProperty("displaydata")]
					public TDisplayData DisplayData { get; set; }
					public class TDisplayData {
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
					
					[JsonProperty("greeks")]
					public TGreeks Greeks { get; set; }
					public class TGreeks {
						[JsonProperty("delta")]
						public string Delta { get; set; }
						
						[JsonProperty("impvolatility")]
						public string ImpVolatility { get; set; }
					}
					
					[JsonProperty("instrument")]
					public TInstrument Instrument { get; set; }
					public class TInstrument {
						[JsonProperty("desc")]
						public string Desc { get; set; }
						
						[JsonProperty("exch")]
						public string Exch { get; set; }
						
						[JsonProperty("matdt")]
						public string MatDt { get; set; }
						
						[JsonProperty("putcall")]
						public string PutCall { get; set; }
						
						[JsonProperty("sectyp")]
						public string SecTyp { get; set; }
									
						[JsonProperty("strkpx")]
						public string StrkPx { get; set; }
									
						[JsonProperty("sym")]
						public string Sym { get; set; }
					}
					
					[JsonProperty("quote")]
					public TQuote Quote { get; set; }
					public class TQuote {
						[JsonProperty("askprice")]
						public string AskPrice { get; set; }
						
						[JsonProperty("bidprice")]
						public string BidPrice { get; set; }
						
						[JsonProperty("change")]
						public string Change { get; set; }
						
						[JsonProperty("extendedquote")]
						public TExtendedQuote ExtendedQuote { get; set; }
						public class TExtendedQuote
						{
							[JsonProperty("asksize")]
							public string AskSize { get; set; }
							
							[JsonProperty("bidsize")]
							public string BidSize { get; set; }
							
							[JsonProperty("contracthigh")]
							public string ContractHigh { get; set; }
							
							[JsonProperty("contractlow")]
							public string ContractLow { get; set; }
							
							[JsonProperty("contractsize")]
							public string ContractSize { get; set; }
							
							[JsonProperty("delta")]
							public string Delta { get; set; }
							
							[JsonProperty("dividenddata")]
							public TDividendData DividendData { get; set; }
							public class TDividendData
							{
								[JsonProperty("amt")]
								public string Amt { get; set; }
							
								[JsonProperty("exdate")]
								public string ExDate { get; set; }
								
								[JsonProperty("paydate")]
								public string PayDate { get; set; }
							
								[JsonProperty("yield")]
								public string Yield { get; set; }
							}
							
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
							
							[JsonProperty("openinterest")]
							public string OpenInterest { get; set; }
							
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
						
						[JsonProperty("lastprice")]
						public string LastPrice { get; set; }
						
						[JsonProperty("pctchange")]
						public string PctChange { get; set; }
					}
					
					[JsonProperty("underlying")]
					public TUnderlying Underlying { get; set; }
					public class TUnderlying {
						[JsonProperty("desc")]
						public string Desc { get; set; }
						
						[JsonProperty("exch")]
						public string Exch { get; set; }
						
						[JsonProperty("sym")]
						public string Sym { get; set; }
					}
				
					[JsonProperty("undlyquote")]
					public TQuote UndlyQuote { get; set; }
		
					[JsonProperty("unknownsymbol")]
					public string UnknownSymbol { get; set; }
				}
			}
		}

		public class MemberProfile: TKBaseType {
			[JsonProperty("userdata")]
			public TUserData UserData { get; set; }
			public class TUserData {
				[JsonProperty("account")]
				public TAccount[] Account { get; set; }
				public class TAccount {
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
				
				[JsonProperty("disabled")]
				public string Disabled { get; set; }
						
				[JsonProperty("resetpassword")]
				public string ResetPassword { get; set; }
							
				[JsonProperty("resettradingpassword")]
				public string ResetTradingPassword { get; set; }
							
				[JsonProperty("userprofile")]
				public TUserProfile UserProfile { get; set; }
				public class TUserProfile {
					[JsonProperty("entry")]
					public TEntry[] Entry { get; set; }
					public class TEntry {
						[JsonProperty("name")]
						public string Name { get; set; }
					
						[JsonProperty("value")]
						public string Value { get; set; }
					}
				}
			}
		}
		
		public class UtilityDocumentation: TKBaseType {
			[JsonProperty("base")]
			public TBase Base { get; set; }
			public class TBase {
				[JsonProperty("uri")]
				public string Uri { get; set; }
				
				[JsonProperty("homepage")]
				public string Homepage { get; set; }
				
				[JsonProperty("parameters")]
				public TParameters Parameters { get; set; }
				public class TParameters {
					[JsonProperty("param")]
					[JsonConverter(typeof(ObjectSometimesArrayConverter<TParam>))]
					public TParam[] Param { get; set; }
					public class TParam {
						[JsonProperty("@name")]
						public string Name { get; set; }
						
						[JsonProperty("@optional")]
						public string Optional { get; set; }
						
						[JsonProperty("@type")]
						public string Type { get; set; }
						
						[JsonProperty("@value")]
						public string Value { get; set; }
						
						[JsonProperty("@default")]
						public string Default { get; set; }
						
						[JsonProperty("description")]
						public string Description { get; set; }
						
						[JsonProperty("example")]
						public string Example { get; set; }
					}
				}
			}
			
			[JsonProperty("endpoints")]
			public TEndPoints EndPoints { get; set; }	
			public class TEndPoints {
				[JsonProperty("client")]
				[JsonConverter(typeof(ObjectSometimesArrayConverter<TClient>))]
				public TClient[] Client { get; set; }
				public class TClient {
					[JsonProperty("@name")]
					public string Name { get; set; }
					
					[JsonProperty("@method")]
					public string Method { get; set; }
					
					[JsonProperty("uri")]
					public string Uri { get; set; }
					
					[JsonProperty("description")]
					public string Description { get; set; }
					
					[JsonProperty("parameters")]
					public TParameters Parameters { get; set; }
					public class TParameters {
						[JsonProperty("default")]
						TDefault Default { get; set; }
						public class TDefault {
							[JsonProperty("param")]
							[JsonConverter(typeof(ObjectSometimesArrayConverter<UtilityDocumentation.TBase.TParameters.TParam>))]
							public UtilityDocumentation.TBase.TParameters.TParam[] Param { get; set; }
						}
						[JsonProperty("required")]
						TRequired Required { get; set; }
						public class TRequired {
							[JsonProperty("@count")]
							public string Count { get; set; }
						
							[JsonProperty("parm")]
							[JsonConverter(typeof(ObjectSometimesArrayConverter<UtilityDocumentation.TBase.TParameters.TParam>))]
							public UtilityDocumentation.TBase.TParameters.TParam[] Param { get; set; }
						}
						
						[JsonProperty("optional")]
						TOptional Optional { get; set; }
						public class TOptional {
							[JsonProperty("param")]
							[JsonConverter(typeof(ObjectSometimesArrayConverter<UtilityDocumentation.TBase.TParameters.TParam>))]
							public UtilityDocumentation.TBase.TParameters.TParam[] Param { get; set; }
						}
					}
				}
			}
		}
		
		public class UtilityStatus: TKBaseType {		
			[JsonProperty("time")]
			public string Time { get; set; }
		}

		public class UtilityVersion: TKBaseType {
			[JsonProperty("version")]
			public string Version { get; set; }
		}
		
		public class Watchlists: TKBaseType {
			[JsonProperty("watchlists")]
			public TWatchlists WatchLists { get; set; }
			public class TWatchlists {
				[JsonProperty("watchlist")]
				[JsonConverter(typeof(ObjectSometimesArrayConverter<TWatchlist>))]
				public TWatchlist[] WatchList { get; set; }
				public class TWatchlist {
					[JsonProperty("id")]
					public string Id { get; set; }
				}
			}
		}

		public class WatchlistsItems:  TKBaseType {
			[JsonProperty("watchlists")]
			public TWatchlists Watchlists { get; set; }
			public class TWatchlists {
				[JsonProperty("watchlist")]
				public TWatchlist WatchList { get; set; }
				public class TWatchlist {
					[JsonProperty("watchlistitem")]
					[JsonConverter(typeof(ObjectSometimesArrayConverter<TWatchlistItem>))]
					public TWatchlistItem[] WatchlistItem;
					public class TWatchlistItem {
						[JsonProperty("costbasis")]
						public string CostBasis { get; set; }
			
						[JsonProperty("instrument")]
						public TInstrument Instrument { get; set; }
						public class TInstrument
						{
							[JsonProperty("matdt")]
							public string MatDt { get; set; }
				
							[JsonProperty("putcall")]
							public string PutCall { get; set; }
				
							[JsonProperty("sectyp")]
							public string SecTyp { get; set; }
				
							[JsonProperty("strkpx")]
							public string StrkPx { get; set; }
				
							[JsonProperty("sym")]
							public string Sym { get; set; }
						}
			
						[JsonProperty("qty")]
						public string Qty { get; set; }
					}
				}
			}
		}
	}
}

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
		}
		
		#region Accounts
		public class Accounts: TKBaseType {
			[JsonProperty("accounts")]
			public Accounts_Accounts Accounts2 { get; set; }	
		}
		
		public class Accounts_Accounts {
			[JsonProperty("accountsummary")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<Accounts_Accounts_AccountSummary>))]
			public Accounts_Accounts_AccountSummary[] AccountSummary { get; set; }
		}
		
		public class Accounts_Accounts_AccountSummary	{
			[JsonProperty("account")]
			public string Account { get; set; }
		
			[JsonProperty("accountbalance")]
			public Accounts_Accounts_AccountBalance AccountBalance { get; set; }
			
			[JsonProperty("accountholdings")]
			public Accounts_Accounts_AccountHoldings AccountHoldings { get; set; }
		}
		
		public class Accounts_Accounts_AccountBalance	{
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
			public Accounts_Accounts_AccountBalance_Money Money;
			
			[JsonProperty("openbuyvalue")]
			public string OpenBuyValue { get; set; }
					
			[JsonProperty("securities")]
			public Accounts_Accounts_AccountBalance_Securities Security;
			
			[JsonProperty("shortbalance")]
			public string ShortBalance { get; set; }
	
			[JsonProperty("shortmv")]
			public string ShortMv { get; set; }
		}
		
		public class Accounts_Accounts_AccountBalance_Money {
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
		
		public class Accounts_Accounts_AccountBalance_Securities {
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
		
		public class Accounts_Accounts_AccountHoldings {
			[JsonProperty("displaydata")]
			public Accounts_Accounts_AccountHoldings_DisplayData DisplayData { get; set; }
			
			[JsonProperty("holding")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<Accounts_Accounts_AccountSummary_AccountHoldings_Holding>))]
			public Accounts_Accounts_AccountSummary_AccountHoldings_Holding[] Holdings { get; set; }
			
			[JsonProperty("totalsecurities")]
			public string TotalSecurities { get; set; }	
		}
		
		public class Accounts_Accounts_AccountHoldings_DisplayData {
			[JsonProperty("totalsecurities")]
			public string TotalSecurities { get; set; }			
		}
		
		public class Accounts_Accounts_AccountSummary_AccountHoldings_Holding	{
			[JsonProperty("accounttype")]
			public string AccountType { get; set; }
			
			[JsonProperty("costbasis")]
			public string CostBasis { get; set; }
			
			[JsonProperty("displaydata")]
			public Accounts_Accounts_AccountSummary_AccountHoldings_Holding_DisplayData DisplayData { get; set; }
					
			[JsonProperty("gainloss")]
			public string GainLoss { get; set; }
			
			[JsonProperty("instrument")]
			public Accounts_Accounts_AccountSummary_AccountHoldings_Holding_Instrument Instrument { get; set; }
					
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
			public Accounts_Accounts_AccountSummary_AccountHoldings_Holding_Quote Quote { get; set; }
					
			[JsonProperty("underlying")]
			public string Underlying { get; set; }
		}
		
		public class Accounts_Accounts_AccountSummary_AccountHoldings_Holding_DisplayData	{
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
		
		public class Accounts_Accounts_AccountSummary_AccountHoldings_Holding_Instrument {
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
			
		public class Accounts_Accounts_AccountSummary_AccountHoldings_Holding_Quote {
			[JsonProperty("change")]
			public string Change { get; set; }
		
			[JsonProperty("lastprice")]
			public string LastPrice { get; set; }
		
		}
		#endregion
		
		# region Accounts/Balances
		public class AccountsBalance: TKBaseType {	
			[JsonProperty("accountbalance")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<AccountsBalance_AccountBalance>))]
			public AccountsBalance_AccountBalance[] AccountBalance { get; set; }
			
			[JsonProperty("totalbalance")]
			public AccountsBalance_TotalBalance TotalBalance { get; set; }
		}
		public class AccountsBalance_AccountBalance {
			[JsonProperty("account")]		
			public string AccountNumber { get; set; }
	
			[JsonProperty("accountname")]
			public string AccountName { get; set; }
	
			[JsonProperty("accountvalue")]
			public string AccountValue { get; set; }
		}
		public class AccountsBalance_TotalBalance	{
			[JsonProperty("accountvalue")]
			public string AccountValue { get; set; }
		}
		#endregion
			
		#region Accounts/:id
		public class AccountsSingle : TKBaseType {
			[JsonProperty("accountbalance")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<Accounts_Accounts_AccountBalance>))]
			public Accounts_Accounts_AccountBalance[] AccountBalance { get; set; }
			
			[JsonProperty("accountholdings")]
			public Accounts_Accounts_AccountHoldings AccountHoldings { get; set; }
		}
		#endregion
		
		#region Accounts/:id/Balances
		public class AccountsBalancesSingle: TKBaseType {
			[JsonProperty("accountbalance")]
			public Accounts_Accounts_AccountBalance AccountBalance { get; set; }
		}
		#endregion
		
		#region Accounts/:id/History
		public enum AccountsHistory_Request_Range {
			All,
			Today,
			Current_Week,
			Current_Month,
			Last_Month
		}
		
		public enum AccountsHistory_Request_Transactions {
			All,
			Bookkeeping,
			Trade
		}
		
		public class AccountsHistory: TKBaseType {
			[JsonProperty("totalrows")]
			public string TotalRows { get; set; }
			
			[JsonProperty("transactions")]
			public AccountsHistory_Transactions Transactions { get; set; }
		}
		
		public class AccountsHistory_Transactions
		{		
			[JsonProperty("transaction")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<AccountsHistory_Transaction>))]
			public AccountsHistory_Transaction[] Transaction { get; set; }
		}
		
		public class AccountsHistory_Transaction
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
			public AccountsHistory_Transaction_Transaction Transaction { get; set; }
		}
			
		public class AccountsHistory_Transaction_Transaction
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
			public AccountsHistory_Transaction_Transaction_Security Security { get; set; }
			
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
		
		public class AccountsHistory_Transaction_Transaction_Security
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
		
		#region Accounts/:id/Holdings
		public class AccountsHoldings: TKBaseType {
			[JsonProperty("accountholdings")]
			public AccountsHoldings_AccountHoldings AccountHoldings{ get; set; }
		}
		public class AccountsHoldings_AccountHoldings {
			[JsonProperty("holding")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<Accounts_Accounts_AccountSummary_AccountHoldings_Holding>))]
			public Accounts_Accounts_AccountSummary_AccountHoldings_Holding[] Holdings { get; set; }
		}
		#endregion
		
		#region Accounts/:id/Orders (GET)
		public class Orders: TKBaseType {
			[JsonProperty("orderstatus")]
			public Orders_OrderStatus OrderStatus { get; set; }
		}
		
		public class Orders_OrderStatus {
			[JsonProperty("order")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<Orders_OrderStatus_Order>))]
			public Orders_OrderStatus_Order[] Order { get; set; }
		}
		
		public class Orders_OrderStatus_Order {
			[JsonProperty("fixmlmessage")]
			public string FixmlMessage { get; set; }
		}
		#endregion
		#region Accounts/:id/Orders (POST)
		public class OrdersPost: TKBaseType {
			[JsonProperty("clientorderid")]
			public string ClientOrderId { get; set; }
		
			[JsonProperty("orderstatus")]
			public string OrderStatus { get; set; }
			
			[JsonProperty("warning")]
			public OrdersPost_Warning Warning { get; set; }
			
			[JsonProperty("estcommission")]
			public string EstCommission { get; set; }
			
			[JsonProperty("marginrequirement")]
			public string MarginRequirement { get; set; }
			
			[JsonProperty("netamt")]
			public string NetAmt { get; set; }
			
			[JsonProperty("principal")]
			public string Principal { get; set; }
			
			[JsonProperty("quotes")]
			Quotes_Quotes Quotes { get; set; }
				
			[JsonProperty("secfee")]
			public string SecFee { get; set; }
		}
		public class OrdersPost_Warning {
			[JsonProperty("warningcode")]
			public string WarningCode { get; set; }
			
			[JsonProperty("warningtext")]
			public string WarningText { get; set; }	
		}
		#endregion
		
		#region Market/Chains
		public class MarketChain: TKBaseType {
			[JsonProperty("putcalls")]
			public Quotes_Quotes PutCalls { get; set; }
		}
		#endregion
		
		#region Market/Clock
		public class MarketClock: TKBaseType {
			[JsonProperty("date")]
			public string Date { get; set; }
		
			[JsonProperty("status")]
			public MarketClock_Status Status { get; set; }
			
			[JsonProperty("message")]
			public string Message { get; set; }
			
			[JsonProperty("unixtime")]
			public string UnixTime { get; set; }
		}
		public class MarketClock_Status {
			[JsonProperty("current")]
			public string Current { get; set; }
		}
		#endregion	
		
		#region Market/Quotes
		public class Quotess: TKBaseType {
			[JsonProperty("quotes")]
			public Quotes_Quotes Quotes { get; set; }
		}
		
		public class Quotes_Quotes
		{
			[JsonProperty("instrumentquote")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<Quotes_Quotes_InstrumentQuote>))]
			public Quotes_Quotes_InstrumentQuote[] InstrumentQuote { get; set; }
		}
		public class Quotes_Quotes_InstrumentQuote
		{
			[JsonProperty("displaydata")]
			public Quotes_Quotes_InstrumentQuote_DisplayData DisplayData { get; set; }
		
			[JsonProperty("greeks")]
			public Quotes_Quotes_InstrumentQuote_Greeks Greeks { get; set; }
			
			[JsonProperty("instrument")]
			public Quotes_Quotes_InstrumentQuote_Instrument Instrument { get; set; }
				
			[JsonProperty("quote")]
			public Quotes_Quotes_InstrumentQuote_Quote Quote { get; set; }
			
			[JsonProperty("underlying")]
			public Quotes_Quotes_InstrumentQuote_Underlying Underlying { get; set; }
			
			[JsonProperty("undlyquote")]
			public Quotes_Quotes_InstrumentQuote_Quote UndlyQuote { get; set; }
			
			[JsonProperty("unknownsymbol")]
			public string UnknownSymbol { get; set; }
		}
		public class Quotes_Quotes_InstrumentQuote_DisplayData {
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
		public class Quotes_Quotes_InstrumentQuote_Greeks {
			[JsonProperty("delta")]
			public string Delta { get; set; }
			
			[JsonProperty("impvolatility")]
			public string ImpVolatility { get; set; }
		}
		public class Quotes_Quotes_InstrumentQuote_Instrument {
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
		public class Quotes_Quotes_InstrumentQuote_Quote {
			[JsonProperty("askprice")]
			public string AskPrice { get; set; }
			
			[JsonProperty("bidprice")]
			public string BidPrice { get; set; }
			
			[JsonProperty("change")]
			public string Change { get; set; }
			
			[JsonProperty("extendedquote")]
			public Quotes_Quotes_InstrumentQuote_Quote_ExtendedQuote ExtendedQuote { get; set; }
			
			[JsonProperty("lastprice")]
			public string LastPrice { get; set; }
			
			[JsonProperty("pctchange")]
			public string PctChange { get; set; }
		}
		public class Quotes_Quotes_InstrumentQuote_Quote_ExtendedQuote
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
			public Quotes_Quotes_InstrumentQuote_Quote_ExtendedQuote_DividendData DividendData { get; set; }
			
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
		public class Quotes_Quotes_InstrumentQuote_Quote_ExtendedQuote_DividendData
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
		public class Quotes_Quotes_InstrumentQuote_Underlying {
			[JsonProperty("desc")]
			public string Desc { get; set; }
			
			[JsonProperty("exch")]
			public string Exch { get; set; }
			
			[JsonProperty("sym")]
			public string Sym { get; set; }
		}
		
		#endregion
		
		#region Member/Profile
		public class MemberProfile: TKBaseType {
			[JsonProperty("userdata")]
			public MemberProfile_UserData UserData { get; set; }
		}
		public class MemberProfile_UserData {
			[JsonProperty("account")]
			public MemberProfile_UserData_Account[] Account { get; set; }
			
			[JsonProperty("disabled")]
			public string Disabled { get; set; }
					
			[JsonProperty("resetpassword")]
			public string ResetPassword { get; set; }
						
			[JsonProperty("resettradingpassword")]
			public string ResetTradingPassword { get; set; }
						
			[JsonProperty("userprofile")]
			public MemberProfile_UserData_UserProfile UserProfile { get; set; }
		}
		public class MemberProfile_UserData_Account {
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
		public class MemberProfile_UserData_UserProfile {
			[JsonProperty("entry")]
			public MemberProfile_UserData_UserProfile_Entry[] Entry { get; set; }
		}
		public class MemberProfile_UserData_UserProfile_Entry {
			[JsonProperty("name")]
			public string Name { get; set; }
		
			[JsonProperty("value")]
			public string Value { get; set; }
		}
		#endregion
		
		#region Utility/Documentation
		public class UtilityDocumentation: TKBaseType {
			[JsonProperty("base")]
			public UtilityDocumentation_Base Base { get; set; }
			
			[JsonProperty("endpoints")]
			public UtilityDocumentation_EndPoints EndPoints { get; set; }	
		}
		public class UtilityDocumentation_Base {
			[JsonProperty("uri")]
			public string Uri { get; set; }
			
			[JsonProperty("homepage")]
			public string Homepage { get; set; }
			
			[JsonProperty("parameters")]
			public UtilityDocumentation_Base_Parameters Parameters { get; set; }
		}
		public class UtilityDocumentation_Base_Parameters {
			[JsonProperty("param")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<UtilityDocumentation_Base_Parameters_Param>))]
			public UtilityDocumentation_Base_Parameters_Param[] Param { get; set; }
		}
		public class UtilityDocumentation_Base_Parameters_Param {
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
		public class UtilityDocumentation_EndPoints {
			[JsonProperty("client")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<UtilityDocumentation_EndPoints_Client>))]
			public UtilityDocumentation_EndPoints_Client[] Client { get; set; }
		}
		public class UtilityDocumentation_EndPoints_Client {
			[JsonProperty("@name")]
			public string Name { get; set; }
			
			[JsonProperty("@method")]
			public string Method { get; set; }
			
			[JsonProperty("uri")]
			public string Uri { get; set; }
			
			[JsonProperty("description")]
			public string Description { get; set; }
			
			[JsonProperty("parameters")]
			public UtilityDocumentation_EndPoints_Client_Parameters Parameters { get; set; }
		}
		public class UtilityDocumentation_EndPoints_Client_Parameters {
			[JsonProperty("default")]
			UtilityDocumentation_EndPoints_Client_Parameters_Default Default { get; set; }
			
			[JsonProperty("required")]
			UtilityDocumentation_EndPoints_Client_Parameters_Required Required { get; set; }
			
			[JsonProperty("optional")]
			UtilityDocumentation_EndPoints_Client_Parameters_Optional Optional { get; set; }
		}
		public class UtilityDocumentation_EndPoints_Client_Parameters_Default {
			[JsonProperty("param")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<UtilityDocumentation_Base_Parameters_Param>))]
			public UtilityDocumentation_Base_Parameters_Param[] Param { get; set; }
		}
		public class UtilityDocumentation_EndPoints_Client_Parameters_Required {
			[JsonProperty("@count")]
			public string Count { get; set; }
		
			[JsonProperty("parm")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<UtilityDocumentation_Base_Parameters_Param>))]
			public UtilityDocumentation_Base_Parameters_Param[] Param { get; set; }
		}
		public class UtilityDocumentation_EndPoints_Client_Parameters_Optional {
			[JsonProperty("param")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<UtilityDocumentation_Base_Parameters_Param>))]
			public UtilityDocumentation_Base_Parameters_Param[] Param { get; set; }
		}
		#endregion
		
		#region Utility/Status
		public class UtilityStatus: TKBaseType {		
			[JsonProperty("time")]
			public string Time { get; set; }
		}
		#endregion
		
		#region Utility/Version
		public class UtilityVersion: TKBaseType {
			[JsonProperty("version")]
			public string Version { get; set; }
		}
		#endregion
		
		#region Watchlists
		public class Watchlists: TKBaseType {
			[JsonProperty("watchlists")]
			public Watchlists_Watchlists WatchLists { get; set; }
		}

		public class Watchlists_Watchlists {
			[JsonProperty("watchlist")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<Watchlists_Watchlists_Watchlist>))]
			public Watchlists_Watchlists_Watchlist[] WatchList { get; set; }
		}

		public class Watchlists_Watchlists_Watchlist {
			[JsonProperty("id")]
			public string Id { get; set; }
		}
		#endregion

		#region Watchlists/:id
		public class WatchlistsItems:  TKBaseType {
			[JsonProperty("watchlists")]
			public WatchlistsItems_Watchlists Watchlists { get; set; }
		}

		public class WatchlistsItems_Watchlists {
			[JsonProperty("watchlist")]
			public WatchlistsItems_Watchlists_Watchlist WatchList { get; set; }
		}

		public class WatchlistsItems_Watchlists_Watchlist {
			[JsonProperty("watchlistitem")]
			[JsonConverter(typeof(ObjectSometimesArrayConverter<WatchlistsItems_Watchlists_Watchlist_WatchlistItem>))]
			public WatchlistsItems_Watchlists_Watchlist_WatchlistItem[] WatchlistItem;
		}
		
		public class WatchlistsItems_Watchlists_Watchlist_WatchlistItem {
			[JsonProperty("costbasis")]
			public string CostBasis { get; set; }

			[JsonProperty("instrument")]
			public WatchlistsItems_Watchlists_Watchlist_WatchlistItem_Instrument Instrument { get; set; }

			[JsonProperty("qty")]
			public string Qty { get; set; }
		}

		public class WatchlistsItems_Watchlists_Watchlist_WatchlistItem_Instrument
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
		#endregion
	}
}

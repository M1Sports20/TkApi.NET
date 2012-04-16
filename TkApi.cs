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
using System.Collections.Generic;
using TkApi.DataStructures;
using TkApi.FixMl;

namespace TkApi {
	public class Order {
		private string _fixMl;
	
		public Order(string fixml) {
			_fixMl = fixml;
		}
		public string FixMl {
			get {
				return _fixMl;
			}
		}
		public string Text {
			get {
				return FixmlParse.GetText(_fixMl);
			}
		}
		public PositionEffect_t PositionEffect {
			get {
				return FixmlParse.GetPositionEffect(_fixMl);
			}
		}
		public DateTime TransactTime {
			get {
				return FixmlParse.GetTransactTime(_fixMl);
			}
		}
		public DateTime TradeDate {
			get {
				return FixmlParse.GetTradeDate(_fixMl);
			}
		}
		public double LeavesQuantity {
			get {
				return FixmlParse.GetLeavesQuantity(_fixMl);
			}
		}
		public TimeInForce_t TimeInForce {
			get {
				return FixmlParse.GetTimeInForce(_fixMl);
			}
		}
		public double Price {
			get {
				return FixmlParse.GetPrice(_fixMl);
			}
		}
		public OrderType_t OrderType {
			get {
				return FixmlParse.GetOrderType(_fixMl);
			}
		}
		public SideOfOrder_t SideOfOrder {
			get {
				return FixmlParse.GetSideOfOrder(_fixMl);
			}
		}
		public AccountType_t AccountType {
			get {
				return FixmlParse.GetAccountType(_fixMl);
			}
		}
		public string Account {
			get {
				return FixmlParse.GetAccount(_fixMl);
			}
		}
		public OrderStatus_t OrderStatus {
			get {
				return FixmlParse.GetOrderStatus(_fixMl);
			}
		}
		public string Id {
			get {
				return FixmlParse.GetId(_fixMl);
			}
		}
		public string OrderId {
			get {
				return FixmlParse.GetOrderId(_fixMl);
			}
		}
		public string SecurityDescription {
			get {
				return FixmlParse.GetSecurityDescription(_fixMl);
			}
		}
		public double ContractMultiplier {
			get {
				return FixmlParse.GetContractMultiplier(_fixMl);
			}
		}
		public double StrikePrice {
			get {
				return FixmlParse.GetStrikePrice(_fixMl);
			}
		}
		public DateTime MaturityDate {
			get {
				return FixmlParse.GetMaturityDate(_fixMl);
			}
		}
		public DateTime MaturityMonthYear {
			get {
				return FixmlParse.GetMaturityMonthYear(_fixMl);
			}
		}
		public SecurityType_t SecurityType {
			get {
				return FixmlParse.GetSecurityType(_fixMl);
			}
		}
		public string Symbol {
			get {
				return FixmlParse.GetSymbol(_fixMl);
			}
		}
		public string UnderlyingSymbol {
			get {
				return FixmlParse.GetUnderlyingSymbol(_fixMl);
			}
		}
		public double Quantity {
			get {
				return FixmlParse.GetQuantity(_fixMl);
			}
		}	
	}
		
	public class TkApi {
		private const uint CacheTimeout = 1000; // One second(s)
		private TkRestCache tk = null;
		
		public enum MarketStatus_t {
			Open,
			Closed
		}
		
		
		public TkApi(string consumerKey, string consumerSecret, string accessToken, string accessSecret) {
			tk = new TkRestCache(consumerKey, consumerSecret, accessToken, accessSecret);
			tk.CacheTimeout = CacheTimeout;
		}
		
		public TkApi(TkRestCache tk) {
			this.tk = tk;
			tk.CacheTimeout = CacheTimeout; // TODO: We should not modify the CacheTimeout
		}
		
		#region Market/Clock Accessors
		public DateTime GetMarketTime() {
			MarketClock market = tk.GetMarket_Clock();
			return DateTime.Parse(market.Date);
		}
		public MarketStatus_t GetMarketState(out string message) {
			MarketClock market = tk.GetMarket_Clock();
			message = market.Message;
			return (MarketStatus_t)Enum.Parse(typeof(MarketStatus_t), market.Status.Current, true);
		}
		#endregion
		#region Member/Profile Accessors
		public bool GetAccountAllowsMutualFund(uint accountNumber) {
		    MemberProfile.TUserData.TAccount[] accounts = tk.GetMember_Profile().UserData.Account;
			foreach (MemberProfile.TUserData.TAccount account in accounts) {
				if (Convert.ToUInt32(account.Account) == accountNumber) {
					return Convert.ToBoolean(account.FundTrading);
				}
			}
			throw new InvalidOperationException("Failed to find if account allows mutual funds for account number: " + accountNumber.ToString());
		}
		public bool GetIsAccountIRA(uint accountNumber) {
			MemberProfile.TUserData.TAccount[] accounts = tk.GetMember_Profile().UserData.Account;
			foreach (MemberProfile.TUserData.TAccount account in accounts) {
				if (Convert.ToUInt32(account.Account) == accountNumber) {
					return Convert.ToBoolean(account.IRA);
				}
			}
			throw new InvalidOperationException("Failed to find if account is an IRA for account number: " + accountNumber.ToString());
		}
		public bool GetAccountAllowsMarginTrading(uint accountNumber) {
			MemberProfile.TUserData.TAccount[] accounts = tk.GetMember_Profile().UserData.Account;
			foreach (MemberProfile.TUserData.TAccount account in accounts) {
				if (Convert.ToUInt32(account.Account) == accountNumber) {
					return Convert.ToBoolean(account.MarginTrading);
				}
			}
			throw new InvalidOperationException("Failed to find if margin trading is allowed for account number: " + accountNumber.ToString());
		}
		public string GetAccountNickname(uint accountNumber) {
			MemberProfile.TUserData.TAccount[] accounts = tk.GetMember_Profile().UserData.Account;
			foreach (MemberProfile.TUserData.TAccount account in accounts) {
				if (Convert.ToUInt32(account.Account) == accountNumber) {
					return account.Nickname;
				}
			}
			throw new InvalidOperationException("Failed to find account nickname for account number: " + accountNumber.ToString());
		}
		public uint GetAccountOptionLevel(uint accountNumber) {
			MemberProfile.TUserData.TAccount[] accounts = tk.GetMember_Profile().UserData.Account;
			foreach (MemberProfile.TUserData.TAccount account in accounts) {
				if (Convert.ToUInt32(account.Account) == accountNumber) {
					return Convert.ToUInt32(account.OptionLevel);
				}
			}
			throw new InvalidOperationException("Failed to find option level for account number: " + accountNumber.ToString());
		}
		public bool GetIsAccountAllowsStock(uint accountNumber) {
			MemberProfile.TUserData.TAccount[] accounts = tk.GetMember_Profile().UserData.Account;
			foreach (MemberProfile.TUserData.TAccount account in accounts) {
				if (Convert.ToUInt32(account.Account) == accountNumber) {
					return Convert.ToBoolean(account.StockTrading);
				}
			}
			throw new InvalidOperationException("Failed to find if account allows stocks for account number: " + accountNumber.ToString());
		}
		public bool IsUserAccountEnabled() {
			MemberProfile member = tk.GetMember_Profile();
			return Convert.ToBoolean(member.UserData.Disabled);
		}
		public bool IsResetPasswordEnabled() {
			MemberProfile member = tk.GetMember_Profile();
			return Convert.ToBoolean(member.UserData.ResetPassword);
		}
		public bool IsResetTradingPasswordEnabled() {
			MemberProfile member = tk.GetMember_Profile();
			return Convert.ToBoolean(member.UserData.ResetPassword);
		}
		public string GetUserMiddleInitial() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "primaryMiddleInitial") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get middle initial");
		}
		public string GetUserLastName() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "primaryLastName") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get last name");
		}
		public string GetLoginId() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "login_id") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get login ID");
		}
		public string GetZipCode() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "zip") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get zip code");
		}
		public string GetUserFirstName() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "primaryFirstName") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get first name");
		}
		public bool IsRealTimeQuoteEnabled() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "rtq") {
					if (profile.Value == "Y")
						return true;
					else
						return false;
				}
			}
			throw new InvalidOperationException("Failed to get status to see if realtime quotes are enabled");
		}
		public string GetUserEmailAddress1() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "emailAddress1") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get email address1");
		}
		public string GetUserEmailAddress2() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "emailAddress2") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get email address2");
		}
		public uint GetLogoutExpireMinutes() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "expireMins") {
					return Convert.ToUInt32(profile.Value);
				}
			}
			throw new InvalidOperationException("Failed to get logout expiring timeout");
		}
		public uint GetDefaultAccountNumber() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "defaultAccount") {
					return Convert.ToUInt32(profile.Value);
				}
			}
			throw new InvalidOperationException("Failed to get default account number");
		}
		public bool IsTradingPasswordEnabled() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "useTradingPassword") {
					if (profile.Value == "Y")
						return true;
					else
						return false;
				}
			}
			throw new InvalidOperationException("Failed to get if user would like to type password when trading");
		}
		public bool GetUserHaveNewMessage() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "NewMsg") {
					if (profile.Value == "Y")
						return true;
					else
						return false;
				}
			}
			throw new InvalidOperationException("Failed to get status if user has new message");
		}
		public bool GetUserSkipOrderPreview() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "skiporderPreview") {
					if (profile.Value == "Y")
						return true;
					else
						return false;
				}
			}
			throw new InvalidOperationException("Failed to get if user would like to skip order preview screen");
		}
		public DateTime GetUserLastDownload(uint accountNumber) {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "ofxLastDownload-" + accountNumber.ToString()) {
					return DateTime.Parse(profile.Value);
				}
			}
			throw new InvalidOperationException("Failed to get if user last download.");
		}
		public bool GetUserAgreedToFdic() {
			// TODO, Test, i don't know if this is a bool
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "AGREEMENT_FDIC") {
					if (profile.Value == "Y")
						return true;
					else
						return false;
				}
			}
			throw new InvalidOperationException("Failed to get if user agreed to FDIC agreement");
		}
		public string GetUserTradingDefaults() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "tradingDefaults") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get users trading defaults");
		}
		public string GetUserOptionChainDefaults() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile.TUserData.TUserProfile.TEntry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "optionChainDefaults") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get users option chain defaults");
		}
		#endregion
		#region Utility/Status Accessors
		public DateTime GetTkServerTime() {
			UtilityStatus status = tk.GetUtility_Status();
			return DateTime.Parse(status.Time.Substring(0,status.Time.LastIndexOf(' ')));
		}
		#endregion
		#region Utility/Version Accessors
		public string GetApiVersion() {
			UtilityVersion version = tk.GetUtility_Version();
			return version.Version;
		}
		#endregion
		
		/// <summary>
		/// Gets orders that have been partially filled or completely filled for the day.
		/// </summary>
		/// <returns>
		/// The executed orders.
		/// </returns>
		/// <param name='accountNumber'>
		/// Account Number.
		/// </param>
		public List<Order> GetExecutedOrders(string accountNumber) {
			List<Order> openOrders = new List<Order>();
			Orders orders = tk.GetAccounts_Orders(accountNumber);
			if (orders == null || orders.OrderStatus == null) return openOrders;
			
			foreach (Orders.TOrderStatus.TOrder orderStatus in orders.OrderStatus.Order) {
				Order order = new Order(orderStatus.FixmlMessage);
				if (order.OrderStatus == OrderStatus_t.PartiallyFilled ||
				    order.OrderStatus == OrderStatus_t.Filled) {
					openOrders.Add(order);	
				}
			}
			return openOrders;
		}
	}
}


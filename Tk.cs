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

namespace TkApi {
	public class Tk {
		private const uint CacheTimeout = 1000; // One second(s)
		private TkApiCache tk = null;
		
		public enum MarketStatus {
			Open,
			Closed
		};
		
		public Tk (string consumerKey, string consumerSecret, string accessToken, string accessSecret) {
			tk = new TkApiCache(consumerKey, consumerSecret, accessToken, accessSecret);
			tk.CacheTimeout = CacheTimeout;
		}
		
		#region Market/Clock Accessors
		public DateTime GetMarketTime() {
			MarketClock market = tk.GetMarket_Clock();
			return DateTime.Parse(market.Date);
		}
		public MarketStatus GetMarketState(out string message) {
			MarketClock market = tk.GetMarket_Clock();
			message = market.Message;
			return (MarketStatus)Enum.Parse(typeof(MarketStatus), market.Status.Current, true);
		}
		#endregion
		#region Member/Profile Accessors
		public bool GetAccountAllowsMutualFund(uint accountNumber) {
			MemberProfile_UserData_Account[] accounts = tk.GetMember_Profile().UserData.Account;
			foreach (MemberProfile_UserData_Account account in accounts) {
				if (Convert.ToUInt32(account.Account) == accountNumber) {
					return Convert.ToBoolean(account.FundTrading);
				}
			}
			throw new InvalidOperationException("Failed to find if account allows mutual funds for account number: " + accountNumber.ToString());
		}
		public bool GetIsAccountIRA(uint accountNumber) {
			MemberProfile_UserData_Account[] accounts = tk.GetMember_Profile().UserData.Account;
			foreach (MemberProfile_UserData_Account account in accounts) {
				if (Convert.ToUInt32(account.Account) == accountNumber) {
					return Convert.ToBoolean(account.IRA);
				}
			}
			throw new InvalidOperationException("Failed to find if account is an IRA for account number: " + accountNumber.ToString());
		}
		public bool GetAccountAllowsMarginTrading(uint accountNumber) {
			MemberProfile_UserData_Account[] accounts = tk.GetMember_Profile().UserData.Account;
			foreach (MemberProfile_UserData_Account account in accounts) {
				if (Convert.ToUInt32(account.Account) == accountNumber) {
					return Convert.ToBoolean(account.MarginTrading);
				}
			}
			throw new InvalidOperationException("Failed to find if margin trading is allowed for account number: " + accountNumber.ToString());
		}
		public string GetAccountNickname(uint accountNumber) {
			MemberProfile_UserData_Account[] accounts = tk.GetMember_Profile().UserData.Account;
			foreach (MemberProfile_UserData_Account account in accounts) {
				if (Convert.ToUInt32(account.Account) == accountNumber) {
					return account.Nickname;
				}
			}
			throw new InvalidOperationException("Failed to find account nickname for account number: " + accountNumber.ToString());
		}
		public uint GetAccountOptionLevel(uint accountNumber) {
			MemberProfile_UserData_Account[] accounts = tk.GetMember_Profile().UserData.Account;
			foreach (MemberProfile_UserData_Account account in accounts) {
				if (Convert.ToUInt32(account.Account) == accountNumber) {
					return Convert.ToUInt32(account.OptionLevel);
				}
			}
			throw new InvalidOperationException("Failed to find option level for account number: " + accountNumber.ToString());
		}
		public bool GetIsAccountAllowsStock(uint accountNumber) {
			MemberProfile_UserData_Account[] accounts = tk.GetMember_Profile().UserData.Account;
			foreach (MemberProfile_UserData_Account account in accounts) {
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
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "primaryMiddleInitial") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get middle initial");
		}
		public string GetUserLastName() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "primaryLastName") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get last name");
		}
		public string GetLoginId() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "login_id") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get login ID");
		}
		public string GetZipCode() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "zip") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get zip code");
		}
		public string GetUserFirstName() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "primaryFirstName") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get first name");
		}
		public bool IsRealTimeQuoteEnabled() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
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
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "emailAddress1") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get email address1");
		}
		public string GetUserEmailAddress2() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "emailAddress2") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get email address2");
		}
		public uint GetLogoutExpireMinutes() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "expireMins") {
					return Convert.ToUInt32(profile.Value);
				}
			}
			throw new InvalidOperationException("Failed to get logout expiring timeout");
		}
		public uint GetDefaultAccountNumber() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "defaultAccount") {
					return Convert.ToUInt32(profile.Value);
				}
			}
			throw new InvalidOperationException("Failed to get default account number");
		}
		public bool IsTradingPasswordEnabled() {
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
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
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
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
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
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
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "ofxLastDownload-" + accountNumber.ToString()) {
					return DateTime.Parse(profile.Value);
				}
			}
			throw new InvalidOperationException("Failed to get if user last download.");
		}
		public bool GetUserAgreedToFdic() {
			// TODO, Test, i don't know if this is a bool
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
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
			//Todo Break this up more
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
				if (profile.Name == "tradingDefaults") {
					return profile.Value;
				}
			}
			throw new InvalidOperationException("Failed to get users trading defaults");
		}
		public string GetUserOptionChainDefaults() {
			//TODO Break this up more
			MemberProfile member = tk.GetMember_Profile();
			foreach (MemberProfile_UserData_UserProfile_Entry profile in member.UserData.UserProfile.Entry) {
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
		#region Utility/Versoin Accessors
		public string GetApiVersion() {
			UtilityVersion version = tk.GetUtility_Version();
			return version.Version;
		}
		#endregion
	}
}


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
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TkApi.DataStructures;

namespace TkApi
{
	public class TkApiRaw
	{		
		private const string AppKeyHeader = "TKI_APPKEY";
		private const string UserKeyHeader = "TKI_USERKEY";
		private const string TimeStampHeader = "TKI_TIMESTAMP";
		private const string SignatureHeader = "TKI_SIGNATURE";
		private const string ContentTypeHeader = "application/xml";
		private const string RequestMethod = "POST";
		private const string RateLimitUsedHeader = "X-RateLimit-Used";
		private const string RateLimitExpireHeader = "X-RateLimit-Expire";
		private const string RateLimitLimitHeader = "X-RateLimit-Limit";
		private const string RateLimitRemainingHeader = "X-RateLimit-Remaining";
		private const string BaseUrl = "https://tkapi.tradeking.com/beta/";

		public enum DataFormat
		{
			XML,
			JSON
		}
		
		private string AppKey;
		private string UserKey;
		private string Secret;
		private DataFormat AcceptTypeHeader;
		private uint InternalRateLimitUsed;
		private DateTime InternalRateLimitExpire;
		private uint InternalRateLimitLimit;
		private uint InternalRateLimitRemaining;

		
		/// <summary>
		/// Number of requests sent against the current limit
		/// </summary>
		public uint RateLimitUsed
		{
			get { return InternalRateLimitUsed; }
			internal set { InternalRateLimitUsed = value; }
		}
		
		/// <summary>
		/// When the current limit will expire
		/// </summary>
		public DateTime RateLimitExpire
		{
			get { return InternalRateLimitExpire; }
			internal set { InternalRateLimitExpire = value; }
		}
		
		/// <summary>
		/// Total number of requests allowed in the call limit
		/// </summary>
		public uint RateLimitLimit
		{
			get { return InternalRateLimitLimit; }
			internal set { InternalRateLimitLimit = value; }
		}
		
		/// <summary>
		/// Number of requests allowed against the current limit
		/// </summary>
		public uint RateLimitRemaining
		{
			get { return InternalRateLimitRemaining; }
			internal set { InternalRateLimitRemaining = value; }
		}
		
				
		/// <summary>
		/// Sets the Response data format sent from TK's Servers.
		/// </summary>
		public DataFormat ResponseType
		{
			get { return AcceptTypeHeader; }
			set { AcceptTypeHeader = value; }
		}
		
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="appKey">Application Key provided by tradeking</param>
		/// <param name="userKey">User Key provided by tradeking</param>
		/// <param name="secret">User Secret provided by tradeking</param>
		public TkApiRaw(string appKey, string userKey, string secret)
		{
			if (String.IsNullOrEmpty(appKey))
				throw new ArgumentNullException("appKey");
			if (String.IsNullOrEmpty(userKey))
				throw new ArgumentNullException("userKey");
			if (String.IsNullOrEmpty(secret))
				throw new ArgumentNullException("secret");

			AppKey = appKey;
			UserKey = userKey;
			Secret = secret;
			
			ResponseType = DataFormat.JSON;
		}
		
		/// <summary>
		/// This function returns the Users Balances Call
		/// </summary>
		/// <returns></returns>
		public UserBalance GetUserBalance()
		{
			string response = SendRequestSync(BaseUrl + "user/balances", "");
			UserBalance obj = JsonConvert.DeserializeObject<UserBalance>(response);
			return obj;		
		}
		
		/// <summary>
		/// This function returns the User Summary Call
		/// </summary>
		/// <returns></returns>
		public UserSummary GetUserSummary()
		{
			string response = SendRequestSync(BaseUrl + "user/summary", "");
			UserSummary obj = JsonConvert.DeserializeObject<UserSummary>(response);
			return obj;					
		}
		
		/// <summary>
		/// This function returns the User Profile Call
		/// </summary>
		/// <returns></returns>
		public UserProfile GetUserProfile()
		{
			string response = SendRequestSync(BaseUrl + "user/profile", "");
			UserProfile obj = JsonConvert.DeserializeObject<UserProfile>(response);
			return obj;								
		}
		
		/// <summary>
		/// This function returns the Account History Call
		/// </summary>
		/// <param name="accountNumber"></param>
		/// <param name="range"></param>
		/// <param name="transactions"></param>
		/// <returns></returns>
		public AccountHistory GetAccountHistory(string accountNumber, AccountHistory_Request_Range range, AccountHistory_Request_Transactions transactions)
		{
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException("accountNumber");
					
			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<account>" + accountNumber + "</account>");
			request.Append("<history>");
			request.Append("<range>" + range.ToString().ToLower() + "</range>");
			request.Append("<transactions>" + transactions.ToString().ToLower() + "</transactions>");
			request.Append("</history></request>");
			
			string response = SendRequestSync(BaseUrl + "account/history", request.ToString());
			AccountHistory obj = JsonConvert.DeserializeObject<AccountHistory>(response);
			return obj;
		}

		/// <summary>
		/// This function returns the User Watchlist (list) Call
		/// </summary>
		/// <returns></returns>
		public UserWatchlistsList GetUserWatchlistsList()
		{
			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<watchlist action=\"list\"></watchlist>");
			request.Append("</request>");

			string response = SendRequestSync(BaseUrl + "user/watchlists", request.ToString());
			UserWatchlistsList obj = JsonConvert.DeserializeObject<UserWatchlistsList>(response);
			return obj;								
		}

		/// <summary>
		/// This function returns the User Watchlist (get) Call
		/// </summary>
		/// <param name="listName"></param>
		/// <returns></returns>
		public UserWatchlistsGet GetUserWatchlistGet(string listName)
		{
			if (string.IsNullOrEmpty(listName))
				throw new ArgumentNullException("listName");

			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<watchlist action=\"get\">");
			request.Append("<id>" + listName + "</id>");
			request.Append("</watchlist>");
			request.Append("</request>");

			string response = SendRequestSync(BaseUrl + "user/watchlists", request.ToString());
			UserWatchlistsGet obj = JsonConvert.DeserializeObject<UserWatchlistsGet>(response);
			return obj;								
		}

		/// <summary>
		/// This function returns the User Watchlist (create) Call
		/// </summary>
		/// <param name="listName"></param>
		/// <param name="list"></param>
		/// <returns></returns>
		public UserWatchlistsList UserWatchlistsCreate(string listName, UserWatchlistsItem[] list)
		{
			if (string.IsNullOrEmpty(listName))
				throw new ArgumentNullException("listName");
			if (list == null)
				throw new ArgumentNullException("list");

			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<watchlist action=\"create\">");
			request.Append("<id>" + listName + "</id>");

			foreach (UserWatchlistsItem l in list)
			{
				if (string.IsNullOrEmpty(l.CostBasis))
					throw new ArgumentNullException("list[*].CostBasis");
				if (string.IsNullOrEmpty(l.Qty))
					throw new ArgumentNullException("list[*].Qty");
				if (l.Instrument == null)
					throw new ArgumentNullException("list[*].Instrument");
				if (string.IsNullOrEmpty(l.Instrument.Sym))
					throw new ArgumentNullException("list[*].Instrument.Sym");

				request.Append("<watchlistItem>");
				request.Append("<costBasis>" + l.CostBasis + "</costBasis>");
				request.Append("<qty>" + l.Qty + "</qty>");
				request.Append("<instrument><sym>" + l.Instrument.Sym + "</sym></instrument>");
				request.Append("</watchlistItem>");
			}

			request.Append("</watchlist>");
			request.Append("</request>");

			string response = SendRequestSync(BaseUrl + "user/watchlists", request.ToString());
			UserWatchlistsList obj = JsonConvert.DeserializeObject<UserWatchlistsList>(response);
			return obj;								
		}

		/// <summary>
		/// This function returns the user watchlist (update) Call
		/// </summary>
		/// <param name="listName"></param>
		/// <param name="list"></param>
		/// <returns></returns>
		public UserWatchlistsList UserWatchlistsUpdate(string listName, UserWatchlistsItem[] list)
		{
			if (string.IsNullOrEmpty(listName))
				throw new ArgumentNullException("listName");

			if (list == null)
				throw new ArgumentNullException("list");

			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<watchlist action=\"update\">");
			request.Append("<id>" + listName + "</id>");

			foreach (UserWatchlistsItem l in list)
			{
				if (string.IsNullOrEmpty(l.CostBasis))
					throw new ArgumentNullException("list[*].CostBasis");
				if (string.IsNullOrEmpty(l.Qty))
					throw new ArgumentNullException("list[*].Qty");
				if (l.Instrument == null)
					throw new ArgumentNullException("list[*].Instrument");
				if (string.IsNullOrEmpty(l.Instrument.Sym))
					throw new ArgumentNullException("list[*].Instrument.Sym");

				request.Append("<watchlistItem>");
				request.Append("<costBasis>" + l.CostBasis + "</costBasis>");
				request.Append("<qty>" + l.Qty + "</qty>");
				request.Append("<instrument><sym>" + l.Instrument.Sym + "</sym></instrument>");
				request.Append("</watchlistItem>");
			}

			request.Append("</watchlist>");
			request.Append("</request>");

			string response = SendRequestSync(BaseUrl + "user/watchlists", request.ToString());
			UserWatchlistsList obj = JsonConvert.DeserializeObject<UserWatchlistsList>(response);
			return obj;								
		}

		/// <summary>
		/// This function returns the user watchlist (delete) Call
		/// </summary>
		/// <param name="listName"></param>
		/// <returns></returns>
		public UserWatchlistsList UserWatchlistsDelete(string listName)
		{
			if (string.IsNullOrEmpty(listName))
				throw new ArgumentNullException("listName");

			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<watchlist action=\"delete\">");
			request.Append("<id>" + listName + "</id>");
			request.Append("</watchlist>");
			request.Append("</request>");

			string response = SendRequestSync(BaseUrl + "user/watchlists", request.ToString());
			UserWatchlistsList obj = JsonConvert.DeserializeObject<UserWatchlistsList>(response);
			return obj;								
		}

		
		/// <summary>
		/// This function returns the Account Balances Call
		/// </summary>
		/// <param name="accountNumber"></param>
		/// <returns></returns>
		public AccountBalances GetAccountBalances(string accountNumber)
		{
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException("accountNumber");
			
			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<account>" + accountNumber + "</account>");
			request.Append("</request>");
			
			string response = SendRequestSync(BaseUrl + "account/balances", request.ToString());
			AccountBalances obj = JsonConvert.DeserializeObject<AccountBalances>(response);
			return obj;								
		}

		/// <summary>
		/// This function returns the Account Holdings Call
		/// </summary>
		/// <param name="accountNumber"></param>
		/// <returns></returns>
		public AccountHoldings GetAccountHoldings(string accountNumber)
		{
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException("accountNumber");
			
			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<account>" + accountNumber + "</account>");
			request.Append("</request>");
			
			string response = SendRequestSync(BaseUrl + "account/holdings", request.ToString());
			AccountHoldings obj = JsonConvert.DeserializeObject<AccountHoldings>(response);
			return obj;								
		}
		
		/// <summary>
		/// This function returns the Account Status Call
		/// </summary>
		/// <param name="accountNumber"></param>
		/// <returns></returns>
		public AccountStatus GetAccountStatus(string accountNumber)
		{
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException("accountNumber");
			
			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<account>" + accountNumber + "</account>");
			request.Append("</request>");
			
			string response = SendRequestSync(BaseUrl + "account/status", request.ToString());
			AccountStatus obj = JsonConvert.DeserializeObject<AccountStatus>(response);
			return obj;								
		}
		
		/// <summary>
		/// This function returns the Account Summary Call
		/// </summary>
		/// <param name="accountNumber"></param>
		/// <returns></returns>
		public AccountSummary GetAccountSummary(string accountNumber)
		{
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException("accountNumber");
			
			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<account>" + accountNumber + "</account>");
			request.Append("</request>");
			
			string response = SendRequestSync(BaseUrl + "account/summary", request.ToString());
			AccountSummary obj = JsonConvert.DeserializeObject<AccountSummary>(response);
			return obj;								
		}
		
		/// <summary>
		/// This function does a Trade Preview Call
		/// </summary>
		/// <param name="accountNumber"></param>
		/// <param name="fixml"></param>
		/// <returns></returns>
		public TradePreviewResponse TradePreview(string accountNumber, string fixml)
		{
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException("accountNumber");
			if (string.IsNullOrEmpty(fixml))
				throw new ArgumentNullException("fixml");
			
			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<account>" + accountNumber + "</account>");
			request.Append("<trade>");
			request.Append("<fixml><![CDATA[" + fixml + "]]></fixml>");
			request.Append("</trade>");
			request.Append("</request>");
			
			string response = SendRequestSync(BaseUrl + "trade/preview", request.ToString());
			TradePreviewResponse obj = JsonConvert.DeserializeObject<TradePreviewResponse>(response);
			return obj;									
		}
		
		/// <summary>
		/// This function returns the TradeSubmit Call
		/// </summary>
		/// <param name="accountNumber"></param>
		/// <param name="fixml"></param>
		/// <returns></returns>
		public TradeSubmitResponse TradeSubmit(string accountNumber, string fixml)
		{
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException("accountNumber");
			if (string.IsNullOrEmpty(fixml))
				throw new ArgumentNullException("fixml");
			
			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<account>" + accountNumber + "</account>");
			request.Append("<trade>");
			request.Append("<fixml><![CDATA[" + fixml + "]]></fixml>");
			request.Append("</trade>");
			request.Append("</request>");
			
			string response = SendRequestSync(BaseUrl + "trade/submit", request.ToString());
			TradeSubmitResponse obj = JsonConvert.DeserializeObject<TradeSubmitResponse>(response);
			return obj;									
		}
		
		/// <summary>
		/// This function returns the TradeSubmit Call
		/// </summary>
		/// <param name="accountNumber"></param>
		/// <param name="fixml"></param>
		/// <param name="overrideWarnings"></param>
		/// <returns></returns>
		public TradeSubmitOverrideResponse TradeSubmit(string accountNumber, string fixml, bool overrideWarnings)
		{
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException("accountNumber");
			if (string.IsNullOrEmpty(fixml))
				throw new ArgumentNullException("fixml");
			if (overrideWarnings == false)
				throw new ArgumentException("This call is used to Override Warnings, if you don't want to override warnings use another TradeSubmit call");
			
			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<account>" + accountNumber + "</account>");
			request.Append("<trade>");
			request.Append("<override>true</override>");
			request.Append("<fixml><![CDATA[" + fixml + "]]></fixml>");
			request.Append("</trade>");
			request.Append("</request>");
			
			string response = SendRequestSync(BaseUrl + "trade/submit", request.ToString());
			TradeSubmitOverrideResponse obj = JsonConvert.DeserializeObject<TradeSubmitOverrideResponse>(response);
			return obj;									
		}
		
		/// <summary>
		/// This function returns the Trade Quote Call
		/// </summary>
		/// <param name="accountNumber"></param>
		/// <param name="delayed"></param>
		/// <param name="symbols"></param>
		/// <returns></returns>
		public TradeQuote GetTradeQuote(string accountNumber, bool delayed, string[] symbols)
		{
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException("accountNumber");
			if (symbols == null)
				throw new ArgumentNullException("symbols");
			
			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<account>" + accountNumber + "</account>");
			request.Append("<quote>");
			request.Append("<delayed>" + delayed.ToString() + "</delayed>");
			request.Append("<symbols>");
			foreach (string symbol in symbols)
			{
				if (string.IsNullOrEmpty(symbol))
					throw new ArgumentNullException("symbols[*]");
				    
				request.Append("<symbol>" + symbol + "</symbol>");
			}
			request.Append("</symbols>");
			request.Append("</quote>");
			request.Append("</request>");
			
			string response = SendRequestSync(BaseUrl + "trade/quotes", request.ToString());
			TradeQuote obj = JsonConvert.DeserializeObject<TradeQuote>(response);
			return obj;									
		}

		/// <summary>
		/// This function returns the Trade Quote Call
		/// </summary>
		/// <param name="accountNumber"></param>
		/// <param name="delayed"></param>
		/// <param name="symbols"></param>
		/// <returns></returns>
		public TradeQuote GetTradeQuote(string accountNumber, bool delayed, string watchlist)
		{
			if (string.IsNullOrEmpty(accountNumber))
				throw new ArgumentNullException("accountNumber");
			if (string.IsNullOrEmpty(watchlist))
				throw new ArgumentNullException("watchlist");
			
			StringBuilder request = new StringBuilder();
			request.Append("<request>");
			request.Append("<account>" + accountNumber + "</account>");
			request.Append("<quote>");
			request.Append("<delayed>" + delayed.ToString() + "</delayed>");
			request.Append("<watchlist>" + watchlist + "</watchlist>");
			request.Append("</quote>");
			request.Append("</request>");
			
			string response = SendRequestSync(BaseUrl + "trade/quotes", request.ToString());
			TradeQuote obj = JsonConvert.DeserializeObject<TradeQuote>(response);
			return obj;									
		}
		
		#region Private Functions
		/// <summary>
		/// Gets the local time as a Unix timestamp (seconds since 01/01/1970)
		/// </summary>
		/// <returns>Unix Timestamp (LocalTime)</returns>
		private double GetTimeStamp()
		{
			TimeSpan ts = (DateTime.Now - new DateTime(1970,1,1,0,0,0).ToLocalTime());
			return Math.Floor(ts.TotalSeconds);
		}
		
		/// <summary>
		/// Converts a Unix Timestamp to a .Net DateTime (LocalTime)
		/// </summary>
		/// <param name="timestamp">Unix timestamp (seconds since 01/01/1970)</param>
		/// <returns>Local Time</returns>
		private DateTime ConvertUnixTimeStamp(double timestamp)
		{
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
   			return origin.AddSeconds(timestamp);
		}
		
		/// <summary>
		/// Creates a signature for an XML request
		/// </summary>
		/// <param name="request">XML Request To send</param>
		/// <param name="unixTimestamp"Unix Timestamp (LocalTime)</param>
		/// <returns></returns>
		private string GetBodySignature(string request, double unixTimestamp)
		{	
			// Generate the base64 encoded request body
			// then create an hmaccmd5 hash object using the secret
			// then hash the base64 data
			// create the 32-bit hex version of the hash
			string base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(request + Convert.ToString(unixTimestamp)));
			System.Security.Cryptography.HMACMD5 hmacmd5 = 
				new System.Security.Cryptography.HMACMD5(System.Text.Encoding.ASCII.GetBytes(Secret));
			byte[] data = System.Text.Encoding.ASCII.GetBytes(base64);
			byte[] hashdata = hmacmd5.ComputeHash(data);
			string signature = BitConverter.ToString(hashdata).Replace("-", "").ToLower();
			return signature;
		}
		
		/// <summary>
		/// Send a request and recieves the response.
		/// </summary>
		/// <param name="url">URL to send the Request too</param> 
		/// <param name="request">XML string of the request to send</param>
		/// <returns>String of response</returns>
		/// TODO: Handle Web Exceptions
		private string SendRequestSync(string url, string request)
		{
			if (String.IsNullOrEmpty(url))
				throw new ArgumentNullException("url");
			if (request == null)
				throw new ArgumentNullException("request");
			
			// Data used for Web Request
			double unixTimestamp = GetTimeStamp();
			string signature = GetBodySignature(request, unixTimestamp);

			// Setup WebRequest
			System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
			byte[] data = System.Text.Encoding.UTF8.GetBytes(request);
			
			webRequest.Method = RequestMethod;
			webRequest.Headers.Add(UserKeyHeader, UserKey);
			webRequest.Headers.Add(AppKeyHeader, AppKey);
			webRequest.Headers.Add(TimeStampHeader, Convert.ToString(unixTimestamp));
			webRequest.Headers.Add(SignatureHeader, signature);
			if (ResponseType == DataFormat.JSON)
				webRequest.Accept = "application/json";
			else
				webRequest.Accept = "application/xml";
			webRequest.ContentType = ContentTypeHeader;
			webRequest.ContentLength = data.Length;

			// Create IO stream to send the request
			Stream sendStream = webRequest.GetRequestStream();
			sendStream.Write(data, 0, data.Length);
			sendStream.Close();
			
			// Create a WebResponse to capture the reponse.
			System.Net.WebResponse webResponse = webRequest.GetResponse();
			Stream recieveStream = webResponse.GetResponseStream();
			
			// Save TradeKing Headers for later use.
			RateLimitExpire = ConvertUnixTimeStamp(Convert.ToDouble(webResponse.Headers[RateLimitExpireHeader]));
			RateLimitLimit = Convert.ToUInt32(webResponse.Headers[RateLimitLimitHeader]);
			RateLimitRemaining = Convert.ToUInt32(webResponse.Headers[RateLimitRemainingHeader]);
			RateLimitUsed = Convert.ToUInt32(webResponse.Headers[RateLimitUsedHeader]);
			
			StreamReader responseReader = new StreamReader(recieveStream);
			string response = responseReader.ReadToEnd();
			
			responseReader.Close();
			recieveStream.Close();
			webResponse.Close();
			
			// If XML was the response type convert it to JSON now to allow the rest of the class to be common
			if (ResponseType == DataFormat.XML)
			{
				System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
				doc.LoadXml(response);
				response = JsonConvert.SerializeXmlNode(doc);
			}
			
			response = StripResponseProperty(response);
			return response;
		}
		
		private string StripResponseProperty(string json)
		{
			if (String.IsNullOrEmpty(json))
				throw new ArgumentNullException("json");
			
			System.Text.RegularExpressions.Regex reg = 
				new System.Text.RegularExpressions.Regex(@"{.*""response"":(.*)}", 
				System.Text.RegularExpressions.RegexOptions.Singleline);
			
			System.Text.RegularExpressions.Match m = reg.Match(json);
			if (m.Success)
			{
				return m.Groups[1].Value;
			}
			
			return string.Empty;
		}
		#endregion
	}
}

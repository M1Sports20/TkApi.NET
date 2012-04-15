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
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TkApi.DataStructures;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using OAuth;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace TkApi {	
	public class TkRest {
		private enum RequestFormat {
			XML,
			JSON,
			JSONP,
		}
		private enum RequestMethod {
			GET,
			POST,
			DELETE,
		}
		
		private const string BaseUri = "https://api.tradeking.com/v1/";
		private const string OAuthVersion = "1.0";
		private const string TkVersion = "1.0-RC1";
		private const string OAuthSignatureMethod = "HMAC-SHA1";
		private  RequestFormat Format = RequestFormat.JSON;
		private const string RateLimitUsedHeader = "X-RateLimit-Used";
		private const string RateLimitExpireHeader = "X-RateLimit-Expire";
		private const string RateLimitLimitHeader = "X-RateLimit-Limit";
		private const string RateLimitRemainingHeader = "X-RateLimit-Remaining";

		private string _consumerKey = "";
		private string _consumerSecret = "";
		private string _accessToken = "";
		private string _accessSecret = "";
		private string _x_rate_limit_used;
		private string _x_rate_limit_expire;
		private string _x_rate_limit_limit;
		private string _x_rate_limit_remaining;
		private bool allowWrite = true;
		
		public string ConsumerKey {
			get { return _consumerKey; }
			set { 
				if (value == null) {
					throw new ArgumentNullException("ConsumerKey");
				} else {
					_consumerKey = value;
				}
			}
		}
		public string ConsumerSecret {
			get { return _consumerSecret; }
			set { 
				if (value == null) {
					throw new ArgumentNullException("ConsumerSecret");
				} else {
					_consumerSecret = value;
				}
			}
		}
		public string AccessToken {
			get { return _accessToken; }
			set { 
				if (value == null) {
					throw new ArgumentNullException("AccessToken");
				} else {
					_accessToken = value;
				}
			}
		}
		public string AccessSecret {
			get { return _accessSecret; }
			set { 
				if (value == null) {
					throw new ArgumentNullException("AccessSecret");
				} else {
					_accessSecret = value;
				}
			}
		}
		public string XRateLimitUsed {
			get { return _x_rate_limit_used; }
			internal set { 
				if (value == null) {
					throw new ArgumentNullException("XRateLimitUsed");
				} else {
					_x_rate_limit_used = value;
				}
			}
		}
		public string XRateLimitExpire {
			get { return _x_rate_limit_expire; }
			internal set { 
				if (value == null) {
					throw new ArgumentNullException("XRateLimitExpire");
				} else {
					_x_rate_limit_expire = value;
				}
			}
		}
		public string XRateLimitLimit {
			get { return _x_rate_limit_limit; }
			internal set { 
				if (value == null) {
					throw new ArgumentNullException("XRateLimitLimit");
				} else {
					_x_rate_limit_limit = value;
				}
			}
		}
		public string XRateLimitRemaining {
			get { return _x_rate_limit_remaining; }
			internal set { 
				if (value == null) {
					throw new ArgumentNullException("XRateLimitRemaining");
				} else {
					_x_rate_limit_remaining = value;
				}
			}
		}
		
		private class RequestData {
			public RequestMethod method;
			public string call;
			public bool authRequired;
			public NameValueCollection queryParams;
			public NameValueCollection headerParams;
			public string postData;
	
			public RequestData(RequestMethod method, string call, bool authRequired, NameValueCollection q, NameValueCollection h, string postData) {
				this.method = method;
				this.call = call;
				this.authRequired = authRequired;
				this.queryParams = q;
				this.headerParams = h;
				this.postData = postData;
			}
		}
		
		public TkRest(string consumerKey, string consumerSecret, string accessToken, string accessSecret) {
			ConsumerKey = consumerKey;
			ConsumerSecret = consumerSecret;
			AccessToken = accessToken;
			AccessSecret = accessSecret;
			
			// Verify support for server version
			UtilityVersion version = GetUtility_Version();
			if (version.Version != TkVersion) {
				allowWrite = false;
				throw new NotSupportedException("TKAPI.NET only supports version " + TkVersion + ".  Server is using version " + version.Version + ".");
			}
		}
		public TkRest(string consumerKey, string consumerSecret, string accessToken, string accessSecret, bool allowTrades) {
			ConsumerKey = consumerKey;
			ConsumerSecret = consumerSecret;
			AccessToken = accessToken;
			AccessSecret = accessSecret;
			allowWrite = allowTrades;
		}
		
		public virtual Accounts GetAccounts() {
			RequestData req = new RequestData(RequestMethod.GET, "accounts", true, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<Accounts>(response);
		}
		public virtual AccountsBalance GetAccounts_Balances() {
			RequestData req = new RequestData(RequestMethod.GET, "accounts/balances", true, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<AccountsBalance>(response);
		}
		public virtual AccountsSingle GetAccounts(string accountNumber) {
			RequestData req = new RequestData(RequestMethod.GET, "accounts/" + accountNumber, true, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<AccountsSingle>(response);
		}
		public virtual AccountsBalancesSingle GetAccounts_Balances(string accountNumber) {
			RequestData req = new RequestData(RequestMethod.GET, "accounts/" + accountNumber + "/balances", true, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<AccountsBalancesSingle>(response);
		}
		public virtual AccountsHistory GetAccounts_History(string accountNumber, AccountsHistory_Range range, AccountsHistory_Transactions transactions) {
			NameValueCollection q = new NameValueCollection();
			q.Add("range", range.ToString().ToLower());
			q.Add("transactions", transactions.ToString().ToLower());
			
			RequestData req = new RequestData(RequestMethod.GET, "accounts/" + accountNumber + "/history", true, q, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<AccountsHistory>(response);
		}
		public virtual AccountsHoldings GetAccount_Holdings(string accountNumber) {
			RequestData req = new RequestData(RequestMethod.GET, "accounts/" + accountNumber + "/holdings", true, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<AccountsHoldings>(response);
		}
		public virtual Orders GetAccounts_Orders(string accountNumber) {
			RequestData req = new RequestData(RequestMethod.GET, "accounts/" + accountNumber + "/orders", true, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<Orders>(response);
		}
		public OrdersPost PostAccounts_Order(string accountNumber, string fixml, string tradePassword, bool tradeOverRide = false) {
			NameValueCollection h = new NameValueCollection();
			h.Add("TKI_OVERRIDE", tradeOverRide.ToString().ToLower());
			if (!String.IsNullOrEmpty(tradePassword)) {	
				h.Add("TKI_TRADEPASS", tradePassword);
			}
				
			RequestData req = new RequestData(RequestMethod.POST, "accounts/" + accountNumber + "/orders", true, null, h, fixml);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<OrdersPost>(response);
		}
		public OrdersPost PostAccount_OrderPreview(string accountNumber, string fixml) {
			RequestData req = new RequestData(RequestMethod.POST, "accounts/" + accountNumber + "/orders/preview", true, null, null, fixml);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<OrdersPost>(response);
		}
		public virtual MarketClock GetMarket_Clock() {
			RequestData req = new RequestData(RequestMethod.GET, "market/clock", false, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<MarketClock>(response);
		}
		public virtual MarketExtQuotes GetMarket_ExtQuotes(string symbol) {
			string postData = "symbols=" + symbol;

			RequestData req = new RequestData(RequestMethod.POST, "market/ext/quotes", true, null, null, postData);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<MarketExtQuotes>(response);
		}
		// TODO: // Will this call the other one in cahched version
		public MarketExtQuotes GetMarket_ExtQuotes(string[] symbols) {
			StringBuilder symbol = new StringBuilder();
			foreach (string idx in symbols) {
				symbol.Append(idx + ",");
			}
			
			// Remove last comma
			string s = symbol.ToString();
			s = s.Substring(0,s.Length - 1);

			return GetMarket_ExtQuotes(s);
		}		
		// TODO: MarketOptionsSearch
		public virtual MarketOptionsStrikes GetMarket_OptionStrikes(string symbol) {
			NameValueCollection q = new NameValueCollection();
			q.Add("symbol", symbol);
		
			RequestData req = new RequestData(RequestMethod.GET, "market/options/strikes", true, q, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<MarketOptionsStrikes>(response);
		}
		public virtual MarketOptionsExpirations GetMarket_OptionExpirations(string symbol) {
			NameValueCollection q = new NameValueCollection();
			q.Add("symbol", symbol);
		
			RequestData req = new RequestData(RequestMethod.GET, "market/options/expirations", true, q, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<MarketOptionsExpirations>(response);
		}
		// TODO: MarketToplists
		public virtual MemberProfile GetMember_Profile() {
			RequestData req = new RequestData(RequestMethod.GET, "member/profile", true, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<MemberProfile>(response);
		}
		public virtual UtilityDocumentation GetUtility_Documentation() {
			RequestData req = new RequestData(RequestMethod.GET, "utility/documentation", false, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<UtilityDocumentation>(response);
		}
		public virtual UtilityStatus GetUtility_Status() {
			RequestData req = new RequestData(RequestMethod.GET, "utility/status", false, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<UtilityStatus>(response);
		}
		public virtual UtilityVersion GetUtility_Version() {
			RequestData req = new RequestData(RequestMethod.GET, "utility/version", false, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<UtilityVersion>(response);
		}
		public virtual Watchlists GetWatchlists() {
			RequestData req = new RequestData(RequestMethod.GET, "watchlists", true, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<Watchlists>(response);
		}
		public Watchlists PostWatchlists(string id, string symbols) {
			string postData;
			postData = "id=" + id;
			postData += "&symbols=" + symbols;

			RequestData req = new RequestData(RequestMethod.POST, "watchlists", true, null, null, postData);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<Watchlists>(response);
		}
		public Watchlists PostWatchlists(string id, string[] symbols) {
			StringBuilder symbol = new StringBuilder();
			foreach (string idx in symbols) {
				symbol.Append(idx + ",");
			}
			
			// Remove last comma
			string s = symbol.ToString();
			s = s.Substring(0,s.Length - 1);
			return PostWatchlists(id, s);
		}
		public virtual WatchlistsItems GetWatchlists(string id) {
			RequestData req = new RequestData(RequestMethod.GET, "watchlists/" + id, true, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<WatchlistsItems>(response);
		}
		public Watchlists DeleteWatchlists(string id) {
			RequestData req = new RequestData(RequestMethod.DELETE, "watchlists/" + id, true, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<Watchlists>(response);
		}
		public Watchlists PostWatchlist(string id, string symbol) {
			string postData = "symbols=" + symbol;
			RequestData req = new RequestData(RequestMethod.POST, "watchlists/" + id + "/symbols", true, null, null, postData);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<Watchlists>(response);
		}
		public Watchlists PostWatchlist(string id, string[] symbols) {
			StringBuilder symbol = new StringBuilder();
			foreach (string idx in symbols) {
				symbol.Append(idx + ",");
			}
			
			// Remove last comma
			string s = symbol.ToString();
			s = s.Substring(0,s.Length - 1);
			return PostWatchlist(id, s);
		}
		public Watchlists DeleteWatchList(string id, string symbol) {
			RequestData req = new RequestData(RequestMethod.DELETE, "watchlists/" + id + "/symbols/" + symbol, true, null, null, null);
			string response = RequestSync(req);
			return JsonConvert.DeserializeObject<Watchlists>(response);
		}
		
		private static bool Validator (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
			return true;
		}
		private string RequestSync(RequestData req) {
			if (req.method.ToString() != "GET" && !allowWrite) {
				return "";
			}
			
			// Build URI
			StringBuilder sbUri = new StringBuilder(BaseUri);
			sbUri.Append(req.call);
			sbUri.Append("." + Format.ToString().ToLower());
			
			// Query Params
			if (req.queryParams != null) {
				sbUri.Append("?");
				foreach (string idx in req.queryParams.AllKeys) {
					sbUri.AppendFormat("{0}={1}&", Uri.EscapeDataString(idx), Uri.EscapeDataString(req.queryParams[idx]));
				}
				sbUri.Remove(sbUri.Length - 1, 1);
			}
			
			Uri uri = new Uri(sbUri.ToString());
			
			// Create Request
			OAuthBase oAuth = new OAuthBase();
			string nonce = oAuth.GenerateNonce();
			string timeStamp = oAuth.GenerateTimeStamp();
			string normRequestUrl;
			string normRequestParam;
			
			string sig = oAuth.GenerateSignature(uri, ConsumerKey, ConsumerSecret, AccessToken, AccessSecret, 
				req.method.ToString(), timeStamp, nonce, out normRequestUrl, out normRequestParam);
			
			ServicePointManager.ServerCertificateValidationCallback = Validator;
			// normRequestParam += "&oauth_signature=" + sig;
			//Console.WriteLine(normRequestUrl + normRequestParam);
			HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(sbUri.ToString());
			webReq.Method = req.method.ToString();
			if (req.authRequired) {
				StringBuilder authHdr = new StringBuilder();
	            authHdr.Append("OAuth ");
	            authHdr.AppendFormat("oauth_consumer_key=\"{0}\",", HttpUtility.UrlEncode(ConsumerKey));
	            authHdr.AppendFormat("oauth_token=\"{0}\",", HttpUtility.UrlEncode(AccessToken));
	            authHdr.AppendFormat("oauth_signature_method=\"{0}\",", HttpUtility.UrlEncode(OAuthSignatureMethod));
	            authHdr.AppendFormat("oauth_signature=\"{0}\",", HttpUtility.UrlEncode(sig));
	            authHdr.AppendFormat("oauth_timestamp=\"{0}\",", HttpUtility.UrlEncode(timeStamp));
	            authHdr.AppendFormat("oauth_nonce=\"{0}\",", HttpUtility.UrlEncode(nonce));
	            authHdr.AppendFormat("oauth_version=\"{0}\",", HttpUtility.UrlEncode(OAuthVersion));
				webReq.Headers.Add("Authorization", authHdr.ToString());
			}
			
			// Header Params
			if (req.headerParams != null) {
				webReq.Headers.Add(req.headerParams);
			}
			
			// POST Data
			if (!String.IsNullOrEmpty(req.postData)) {
				
				ASCIIEncoding objEncoding = new ASCIIEncoding();
				byte[] objBytes = objEncoding.GetBytes(req.postData);
				webReq.ContentLength = objBytes.Length;
				webReq.ContentType = "application/x-www-form-urlencoded";
				Stream postStream = webReq.GetRequestStream();
				postStream.Write(objBytes, 0, objBytes.Length);
			}
			
			// Send web request and colleciton response
			WebResponse webResp = webReq.GetResponse();
			Stream respStream = webResp.GetResponseStream();
			StreamReader respReader = new StreamReader(respStream);
			string resp = respReader.ReadToEnd();
			
			// Save Response Headers
			string header;
			header = webResp.Headers[RateLimitUsedHeader];
			if (!String.IsNullOrEmpty(header)) XRateLimitUsed = header;
			header = webResp.Headers[RateLimitExpireHeader];
			if (!String.IsNullOrEmpty(header)) XRateLimitExpire = header;
			header = webResp.Headers[RateLimitLimitHeader];
			if (!String.IsNullOrEmpty(header)) XRateLimitLimit = header;
			header = webResp.Headers[RateLimitRemainingHeader];
			if (!String.IsNullOrEmpty(header)) XRateLimitRemaining = header;
			
			respReader.Close();
			respStream.Close();
			webResp.Close();
			
			//Console.WriteLine(resp);
			return StripResponseProperty(resp);
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
	
			throw new FormatException("Unknown Response from TK Server");
		}
	}
}
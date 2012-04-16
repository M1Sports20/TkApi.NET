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
using System.Text;
using System.Text.RegularExpressions;

#region Internal Doc
/*
Acct:      Account number needs to be passed with all order requests.
AcctTyp:   Only used for closing short positions, "Buy to Cover" orders should include this attribute as AcctTyp="5".
CFI:       Abbreviation for "classification of financial instrument", used for options to distinguish "OC" for call
           option or "OP" for put option.
Mat:       Represents the expiration date of a option. Needs to be in the format of "YYYY‐MM‐ DDT00:00:00.000‐05:00".
           For single leg orders, this attribute tag changes from Mat to MatDt.
MatDt:     Represents the expiration date of a option. Needs to be in the format of "YYYY‐MM‐ DDT00:00:00.000‐05:00".
           For multiple leg orders, this attribute tag changes from MatDt to Mat.
MMY:       Expiration of the option in the format of YYYYMM.
OrigID:    Order ID that needs to be passed for any change or cancel requests. Note: for Multi‐leg orders, use tag
           OrigClOrdID instead of OrigID.
PosEfct:   Used for options, option legs require and attribute of "O" for opening or "C" for closing.
Px:        Price for price type if needed. This attribute would be required for limits (Typ = "2") or stop limits
           (Typ="4").
SecTyp:    Security type attribute is needed. "CS" for common stock or "OPT" for option.
Side:      Side of market as "1" ‐ Buy, "2" ‐ Sell, "5" ‐ Sell Short. Buy to cover orders are attributed as buy
           orders with Side="1".
Strk:      Strike price of option contract. This tag changes from Strk to StrkPx for single leg orders.
StrkPx:    Strike price of option contract. This tag changes from StrkPx to Strk for multi‐leg orders.
Sym:       Ticker symbol of underlying security. This is utilized for stock, option, & multi‐leg orders.
TmInForce: Time in force, possible values include "0" ‐ Day Order, "1" ‐ GTC Order, "2" ‐ Market on Close. Not
           applicable when Typ="1" (market order).
Typ:       Price Type as "1" ‐ Market, "2" ‐ Limit", "3" ‐ Stop, "4" Stop Limit, or "P" for trailing stop.
ExecInst:  Used for trailing stop orders. Value of ExecInst="a" needs to be passed.
OfstTyp:   Used for trailing stop orders. Value of OfstTyp="0" needs to be passed. The offset value of "0" denotes
           a "price" offset from the PegPxTyp field below. The offset value of "1" denotes a "basis point" offset
           from the PegPxTyp field below (used as a percentage offset).
PegPxTyp:  Used for trailing stop orders defining type of peg (price used) for trailing. In this case, PegPxTyp="1"
           references "last price" of security.
OfstVal:   Used for trailing stop orders. Signed value needs to be passed for amount of offset value combined with
           the PegPxTyp & OfstTyp fields. Negative values are normally used for sell trailing stops so the trigger
           trails below current price. Positive values are normally used for buy trailing stops so the trigger
           trails above the current price. For example, assuming an OfstTyp ="0", a sell order with a OfstVal of
           ‐.50 will trigger if the current price falls by more than .50 of its last highest value since the order
           was placed. OfstType="1" would require the signed value for a percentage. For example, OfstVal="5" would
           represent a 5% increase in price before a buy trailing stop is triggered.
*/
#endregion
namespace TkApi {
	namespace FixMl {
		public enum PositionEffect_t {
			Close = 'C',
			FIFO = 'F',
			Open = 'O',
			Rolled = 'R',
			CloseButNotifyOnOpen = 'N',
			Default = 'D'
		}
		public enum TimeInForce_t {
			Day = '0',
			GoodTillCancel = '1',
			AtTheOpening = '2',
			ImmediateOrCancel = '3',
			FillOrKill = '4',
			GoodTillCrossing = '5',
			GoodTillDate = '6',
			AtTheClose = '7',
			GoodThroughCrossing = '8',
			AtCrossing = '9'
		}
		public enum OrderType_t {
			Market = '1',
			Limit = '2',
			Stop = '3',
			StopLimit = '4',
			[Obsolete("Deprecated FIX.4.3")]
			MarketOnClose = '5',
			WithOrWithout = '6',
			[Obsolete("Deprecated FIX.4.4")]
			LimitOrBetter = '7',
			LimitWithOrWithout = '8',
			OnBasis = '9',
			[Obsolete("Deprecated FIX.4.4")]
			OnClose = 'A',
			[Obsolete("Deprecated FIX.4.3")]
			LimitOnClose = 'B',
			[Obsolete("Deprecated FIX.4.3")]
			ForexMarket = 'C',
			PreviouslyQuoted = 'D',
			PreviouslyIndicated = 'E',
			[Obsolete("Deprecated FIX.4.3")]
			ForexLimit = 'F',
			ForexSwap = 'G',
			[Obsolete("Deprecated FIX.4.3")]
			ForexPreviouslyQuote = 'H',
			Funari = 'I',
			MarketIfTouched = 'J',
			MarketWithLeftOverAsLimit = 'K',
			PreviousFundValuationPoint = 'L',
			NextFundValuationPoint = 'M',
			Pegged = 'P',
			CounterOrderSelection = 'Q'			
		}
		public enum SideOfOrder_t {
			Buy = '1',
			Sell = '2',
			BuyMinus = '3',
			SellPlus = '4',
			SellShort = '5',
			SellShortExempt = '6',
			Undisclosed = '7',
			Cross = '8',
			CrossShort = '9',
			CrossShortExempt = 'A',
			AsDefined = 'B',
			Opposite = 'C',
			Subscribe = 'D',
			Redeem = 'E',
			Lend = 'F',
			Borrow = 'G'
		}
		public enum AccountType_t {
			CarriedCustomerSide = '1',
			CarriedNonCustomerSide = '2',
			HouseTrader = '3',
			FloorTrader = '4',
			CarriedNonCustomerSideCrossMargined = '6',
			HouseTraderCrossMargined = '7',
			JointBackOfficeAccount = '8'
		}
		public enum OrderStatus_t {
			New = '0',
			PartiallyFilled = '1',
			Filled = '2',
			DoneForDay = '3',
			Canceled = '4',
			[Obsolete("Deprecated FIX.4.3")]
			Replaced = '5',
		    PendingCancel = '6',
		    Stopped = '7',
		    Rejected = '8',
		    Suspended = '9',
		    PendingNew = 'A',
		    Calculated = 'B',
		    Expired = 'C',
		    AcceptedForBidding = 'D',
		    PendingReplace = 'E'
		}
		public enum SecurityType_t {
			// TODO: Not Complete
			CS,  // Common Stock
			PS,  // Preferred Stock
			OPT, // Options
			OOC  // Options on Combo
		}
		public enum Cfi_t {
			// TODO: Not Complete
			ES, // Equity Common Shares
			EM, // Equity Miscellaneous or Other
			EP, // Equity Preferred Shares
			EU, // Equity Units
			D,  // Debt (Fixed Income)
			DC, // Debt Convertible Bond
			F,  // Future
			MRC, // Misc, Referential Instrument, Currentcy
			MRI, // Misc, Referential Instrument, Index
			MRR, // Misc, Referential Instrument, Interest Rate
			OC, // Option - Call
			OP, // Option - Put
			RW, // Right Warrent
		}
		
		public static class Fixml {
			private const string HEADER = @"<FIXML xmlns=""http://www.fixprotocol.org/FIXML-5-0-SP2"">";
			private const string FOOTER = @"</FIXML>";
			
	
	
			public static string StockOrder(string accountNumber, SideOfOrder_t side, string symbol, int shares, 
			                                   TimeInForce_t time, OrderType_t type, double limitPrice) {
				StringBuilder fixml = new StringBuilder(HEADER);
				
				// TODO: Support Buy To Cover
				fixml.AppendFormat(@"<Order TmInForce=""{0}"" Typ=""{1}"" Side=""{2}"" Px=""{3}"" Acct=""{4}"">",
				                   time.GetHashCode(), type.GetHashCode(), side.GetHashCode(), limitPrice, accountNumber);
				fixml.AppendFormat(@"<Instrmt SecTyp=""{0}"" Sym=""{1}""/>", SecurityType_t.CS.ToString(), symbol);
				fixml.AppendFormat(@"<OrdQty Qty=""{0}""/>", shares.ToString());
				fixml.AppendFormat(@"</Order>");
				fixml.AppendFormat(FOOTER);
				
				return fixml.ToString();
			}
			public static string OptionOrder(string accountNumber, SideOfOrder_t side, string underlying, Cfi_t cfi,
			                                     PositionEffect_t positionEffect, DateTime expireDate, double strike, int contracts,
			                                     TimeInForce_t time, OrderType_t type, double limitPrice) {
				StringBuilder fixml = new StringBuilder(HEADER);
				// TODO: Support Buy To Cover
				fixml.AppendFormat(@"<Order TmInForce=""{0}"" Typ=""{1}"" Side=""{2}"" Px=""{3}"" PosEfct=""{4}"" Acct=""{5}"">",
				                   time.GetHashCode(), type.GetHashCode(), side.GetHashCode(), limitPrice, positionEffect.GetHashCode(), accountNumber);
				fixml.AppendFormat(@"<Instrmt CFI=""{0}"" SecTyp=""{1}"" MatDt=""{2:yyyy-MM-dd}T00:00:00.000-05:00"" StrkPx=""{3}"" Sym=""{4}""/>",
				                   cfi.ToString(), SecurityType_t.OPT.ToString(), expireDate, strike, underlying);
				fixml.AppendFormat(@"<OrdQty Qty=""{0}""/>", contracts);
				fixml.AppendFormat(@"</Order>");
				fixml.AppendFormat(FOOTER);
				
				return fixml.ToString();		
			}
			
			public static string CancelOrder(string accountNumber, string orderId) {
				StringBuilder fixml = new StringBuilder(HEADER);
				// TODO: Support Buy To Cover
				fixml.AppendFormat(@"<OrdCxlReq Side=""{0}"" Acct=""{1}"" OrigID=""{2}"">",
				                   "1", accountNumber, orderId);
				fixml.AppendFormat(@"<Instrmt SecTyp=""{0}"" Sym=""{1}""/>", "CS", "XYZ");
				fixml.AppendFormat(@"<OrdQty Qty=""{0}""/>", "0");
				fixml.AppendFormat(@"</OrdCxlReq>");
				fixml.AppendFormat(FOOTER);
				
				return fixml.ToString();
			}
		}
		
		public static class FixmlParse {
			// TODO: Optimise when supported [MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static string GetAttribute(string fixml, string attributeName) {
				if (String.IsNullOrEmpty(fixml)) {
					throw new ArgumentException("fixml is null or empty");
				}
				Regex reg = new Regex(attributeName + @"=""([^""]*)");
				return reg.Match(fixml).Groups[1].ToString();
			}
			
			public static OrderType_t GetOrderType(string fixml) {
				return (OrderType_t) Convert.ToChar(GetAttribute(fixml, " Typ"));
			}
			
			public static string GetAccount(string fixml) {
				return GetAttribute(fixml, " Acct");
			}
			
			public static string GetOrderId(string fixml) {
				return GetAttribute(fixml, " OrdID");
			}
			public static string GetId(string fixml) {
				return GetAttribute(fixml, " ID");
			}
			
			public static TimeInForce_t GetTimeInForce(string fixml) {
				return (TimeInForce_t) Convert.ToChar(GetAttribute(fixml, " TmInForce"));
			}
			
			public static SideOfOrder_t GetSideOfOrder(string fixml) {
				return (SideOfOrder_t) Convert.ToChar(GetAttribute(fixml, " Side"));
			}
			
			public static DateTime GetMaturityDate(string fixml) {
				return DateTime.Parse(GetAttribute(fixml, " MatDt"));
			}
			
			public static PositionEffect_t GetPositionEffect(string fixml) {
				return (PositionEffect_t) Convert.ToChar(GetAttribute(fixml, " PosEfct"));
			}
			public static AccountType_t GetAccountType(string fixml) {
				return (AccountType_t) Convert.ToChar(GetAttribute(fixml, " AcctTyp"));
			}
			
			public static double GetStrikePrice(string fixml) {
				return Convert.ToDouble(GetAttribute(fixml, " StrkPx"));
			}
			
			public static DateTime GetTradeDate(string fixml) {
				return DateTime.Parse(GetAttribute(fixml, " TrdDt"));
			}
			
			public static DateTime GetMaturityMonthYear(string fixml) {
				return DateTime.Parse(GetAttribute(fixml, " MMY"));
			}
			
			public static double GetPrice(string fixml) {
				return Convert.ToDouble(GetAttribute(fixml, " Px"));
			}
			
			public static SecurityType_t GetSecurityType(string fixml) {
				return (SecurityType_t) Enum.Parse(typeof(SecurityType_t), GetAttribute(fixml, " SecTyp"), true);
			}
			
			public static string GetSymbol(string fixml) {
				return GetAttribute(fixml, " Sym");
			}
			public static string GetUnderlyingSymbol(string fixml) {
				return GetAttribute(fixml, "Undly Sym");
			}
							
			public static string GetSecurityDescription(string fixml) {
				return GetAttribute(fixml, " Desc");
			}
			
			public static double GetQuantity(string fixml) {
				return Convert.ToDouble(GetAttribute(fixml, " Qty"));
			}
			
			public static double GetContractMultiplier(string fixml) {
				return Convert.ToDouble(GetAttribute(fixml, " Mult"));
			}
			
			public static string GetText(string fixml) {
				return GetAttribute(fixml, " Txt");
			}
			
			public static OrderStatus_t GetOrderStatus(string fixml) {
				return (OrderStatus_t) Convert.ToChar(GetAttribute(fixml, " Stat"));
			}
			
			public static DateTime GetTransactTime(string fixml) {
				return DateTime.Parse(GetAttribute(fixml, " TxnTm"));
			}
			
			public static double GetLeavesQuantity(string fixml) {
				return Convert.ToDouble(GetAttribute(fixml, " LeavesQty"));
			}
			
		}
	}
}


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
	public static class Fixml {
		private const string HEADER = @"<FIXML xmlns=""http://www.fixprotocol.org/FIXML-5-0-SP2"">";
		private const string FOOTER = @"</FIXML>";
		
		public enum OptionType {
			CALL,
			PUT
		}
		public enum TimeInForce {
			DayOrder = 0,
			GoodTillCancel = 1,
			MarketOnClose = 2
		}
		public enum SecurityType {
			CS,
			OPT
		}
		public enum Side {
			Buy = 1,
			Sell = 2,
			SellShort = 5,
		}
		public enum OrderType {
			Market = 1,
			Limit = 2,
			Stop = 3,
			StopLimit = 4,
			TrailingStop = 10 // TODO: See Doc P
		}
		public enum OrderOptionType {
			Open,
			Close
		}
		
		public static string StockOrder(string accountNumber, Side side, string symbol, int shares, TimeInForce time, OrderType type, double limitPrice) {
			StringBuilder fixml = new StringBuilder(HEADER);
			// TODO: Support Buy To Cover
			fixml.AppendFormat(@"<Order TmInForce=""{0}"" Typ=""{1}"" Side=""{2}"" Px=""{3}"" Acct=""{4}"">",
			                   (int)time, (int)type, (int)side, limitPrice.ToString(), accountNumber);
			fixml.AppendFormat(@"<Instrmt SecTyp=""{0}"" Sym=""{1}""/>", SecurityType.CS.ToString(), symbol);
			fixml.AppendFormat(@"<OrdQty Qty=""{0}""/>", shares.ToString());
			fixml.AppendFormat(@"</Order>");
			fixml.AppendFormat(FOOTER);
			
			return fixml.ToString();
		}
		public static string OptionOrder(string accountNumber, Side side, string underlying, OptionType oType, OrderOptionType ooType, DateTime date, double strike, int contracts, TimeInForce time, OrderType type, double limitPrice) {
			string sOptionType = "OC";
			if (oType == OptionType.PUT) {
				sOptionType = "OP";
			}
			
			string sOpenClose = "C";
			if (ooType == OrderOptionType.Open) {
				sOpenClose = "O";
			}
			
			StringBuilder fixml = new StringBuilder(HEADER);
			// TODO: Support Buy To Cover
			fixml.AppendFormat(@"<Order TmInForce=""{0}"" Typ=""{1}"" Side=""{2}"" Px=""{3}"" PosEfct=""{4}"" Acct=""{5}"">",
			                   (int)time, (int)type, (int)side, limitPrice.ToString(), sOpenClose, accountNumber);
			fixml.AppendFormat(@"<Instrmt CFI=""{0}"" SecTyp=""{1}"" MatDt=""{2:yyyy-MM-dd}T00:00:00.000-05:00"" StrkPx=""{3}"" Sym=""{4}""/>",
			                   sOptionType, SecurityType.OPT.ToString(), date, strike, underlying);
			fixml.AppendFormat(@"<OrdQty Qty=""{0}""/>", contracts.ToString());
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
}


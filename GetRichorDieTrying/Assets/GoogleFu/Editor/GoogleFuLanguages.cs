//----------------------------------------------
//    GoogleFu: Google Doc Unity integration
//         Copyright © 2013 Litteratus
//----------------------------------------------

using UnityEngine;
using UnityEditor;

/* Language Enumeration
 * 
 * The following Language Codes are all ISO 639-1
 * The Headers of the Google Spreadsheet that you
 * use to designate Languages must match these enumerations
 * 
 * If Google Translate supports other language codes, they
 * may be added to this enumeration. Be sure to add to the
 * GetCode function below as well.
 */

namespace GoogleFu
{
	public static class Language
	{
		public enum Code
		{
			/* Commonly supported languages
			 * When translating from English, these languages
			 * flow left to right, and contain mostly the same
			 * character set, requiring the least amount of
			 * effort for the biggest gain
			 */
			EN,		//	English
			FR,		//	French
			IT,		//	Italian
			DE,		//	German
			ES,		//	Spanish
			
			/* Uncommonly supported languages
			 * These languages are very popular, but require more
			 * effort to translate. They have unique character sets
			 * and flow, or the native speakers commonly also speak
			 * a more popular language.
			 */
			AR,		//	Arabic
			ZHCN,	//	Chinese (Simplified)
			JA,		//	Japanese
			KO,		//	Korean
			VI,		//	Vietnamese
			RU,		//	Russian
			NL,		//	Dutch
			PT,		//	Portuguese
			
			/* Rarely supported languages
			 * These languages, although some very popular are not
			 * generally localized. Most of the native speakers
			 * commonly will speak a more popular language, or there
			 * are simply not enough of them to warrant the cost of
			 * localizing.
			 */
			AF,		//	Afrikaans
			BE,		//	Belarusian
			BG,		//	Bulgarian
			CA,		//	Catalan
			CS,		//	Czech
			CY,		//	Welsh
			ET,		//	Estonian
			FA,		//	Persian
			FI,		//	Finnish
			GA,		//	Irish
			GL,		//	Galician
			HI,		//	Hindi
			HR,		//	Croatian
			HT,		//	Haitian
			ID,		//	Indonesian
			IS,		//	Icelandic
			IW,		//	Hebrew
			LT,		//	Lithuanian
			LV,		//	Latvian
			MK,		//	Macedonian
			MS,		//	Malay
			MT,		//	Maltese
			NO,		//	Norwegian
			PL,		//	Polish
			RO,		//	Romanian
			SK,		//	Slovak
			SL,		//	Slovenian
			SQ,		//	Albanian
			SR,		//	Serbian
			SV,		//	Swedish
			SW,		//	Swahili
			TH,		//	Thai
			TL,		//	Tagalog
			TR,		//	Turkish
			UK,		//	Ukrainian
			YI,		//	Yiddish
			ZHTW,	//	Chinese (Traditional)
			
			INVALID
		}
		
		public static string GetLanguageCodeAsString( Language.Code languageCode )
		{
			switch(languageCode)
			{
			case Language.Code.EN:		return "EN";
			case Language.Code.FR:		return "FR";
			case Language.Code.IT:		return "IT";
			case Language.Code.DE:		return "DE";
			case Language.Code.ES:		return "ES";
			case Language.Code.AR:		return "AR";
			case Language.Code.ZHCN:	return "ZH-CN";
			case Language.Code.JA:		return "JA";
			case Language.Code.KO:		return "KO";
			case Language.Code.VI:		return "VI";
			case Language.Code.RU:		return "RU";
			case Language.Code.NL:		return "NL";
			case Language.Code.AF:		return "AF";
			case Language.Code.BE:		return "BE";
			case Language.Code.BG:		return "BG";
			case Language.Code.CA:		return "CA";
			case Language.Code.CS:		return "CS";
			case Language.Code.CY:		return "CY";
			case Language.Code.ET:		return "ET";
			case Language.Code.FA:		return "FA";
			case Language.Code.FI:		return "FI";
			case Language.Code.GA:		return "GA";
			case Language.Code.GL:		return "GL";
			case Language.Code.HI:		return "HI";
			case Language.Code.HR:		return "HR";
			case Language.Code.HT:		return "HT";
			case  Language.Code.ID:		return "ID";
			case Language.Code.IS:		return "IS";
			case  Language.Code.IW:		return "IW";
			case Language.Code.LT:		return "LT";
			case Language.Code.LV:		return "LV";
			case Language.Code.MK:		return "MK";
			case Language.Code.MS:		return "MS";
			case Language.Code.MT:		return "MT";
			case Language.Code.NO:		return "NO";
			case Language.Code.PL:		return "PL";
			case Language.Code.PT:		return "PT";
			case Language.Code.RO:		return "RO";
			case Language.Code.SK:		return "SK";
			case Language.Code.SL:		return "SL";
			case Language.Code.SQ:		return "SQ";
			case Language.Code.SR:		return "SR";
			case Language.Code.SV:		return "SV";
			case Language.Code.SW:		return "SW";
			case Language.Code.TH:		return "TH";
			case Language.Code.TL:		return "TL";
			case Language.Code.TR:		return "TR";
			case Language.Code.UK:		return "UK";
			case Language.Code.YI:		return "YI";
			case Language.Code.ZHTW:	return "ZH-TW";
			}
			
			return "Invalid Language";
		}
		
		public static string GetLanguageString( Language.Code languageCode )
		{
			switch(languageCode)
			{
			case Language.Code.EN:		return "ENGLISH";
			case Language.Code.FR:		return "FRENCH";
			case Language.Code.IT:		return "ITALIAN";
			case Language.Code.DE:		return "GERMAN";
			case Language.Code.ES:		return "SPANISH";
			case Language.Code.AR:		return "ARABIC";
			case Language.Code.ZHCN:	return "CHINESE (SIMPLIFIED)";
			case Language.Code.JA:		return "JAPANESE";
			case Language.Code.KO:		return "KOREAN";
			case Language.Code.VI:		return "VIETNAMESE";
			case Language.Code.RU:		return "RUSSIAN";
			case Language.Code.NL:		return "DUTCH";
			case Language.Code.AF:		return "AFRIKAANS";
			case Language.Code.BE:		return "BELARUSIAN";
			case Language.Code.BG:		return "BULGARIAN";
			case Language.Code.CA:		return "CATALAN";
			case Language.Code.CS:		return "CZECH";
			case Language.Code.CY:		return "WELSH";
			case Language.Code.ET:		return "ESTONIAN";
			case Language.Code.FA:		return "PERSIAN";
			case Language.Code.FI:		return "FINNISH";
			case Language.Code.GA:		return "IRISH";
			case Language.Code.GL:		return "GALICIAN";
			case Language.Code.HI:		return "HINDI";
			case Language.Code.HR:		return "CROATIAN";
			case Language.Code.HT:		return "HAITIAN";
			case  Language.Code.ID:		return "INDONESIAN";
			case Language.Code.IS:		return "ICELANDIC";
			case  Language.Code.IW:		return "HEBREW";
			case Language.Code.LT:		return "LITHUANIAN";
			case Language.Code.LV:		return "LATVIAN";
			case Language.Code.MK:		return "MACEDONIAN";
			case Language.Code.MS:		return "MALAY";
			case Language.Code.MT:		return "MALTESE";
			case Language.Code.NO:		return "NORWEGIAN";
			case Language.Code.PL:		return "POLISH";
			case Language.Code.PT:		return "PORTUGUESE";
			case Language.Code.RO:		return "ROMANIAN";
			case Language.Code.SK:		return "SLOVAK";
			case Language.Code.SL:		return "SLOVENIAN";
			case Language.Code.SQ:		return "ALBANIAN";
			case Language.Code.SR:		return "SERBIAN";
			case Language.Code.SV:		return "SWEDISH";
			case Language.Code.SW:		return "SWAHILI";
			case Language.Code.TH:		return "THAI";
			case Language.Code.TL:		return "TAGALOG";
			case Language.Code.TR:		return "TURKISH";
			case Language.Code.UK:		return "UKRAINIAN";
			case Language.Code.YI:		return "YIDDISH";
			case Language.Code.ZHTW:	return "CHINESE (TRADITIONAL)";
			}
			
			return "Invalid Language";
		}
		
		public static Language.Code GetLanguageCode( string languageString )
		{
			switch(languageString.ToUpper())
			{
				case "EN":	case "ENGLISH":		return Language.Code.EN;
				case "FR":	case "FRENCH":		return Language.Code.FR;
				case "IT":	case "ITALIAN":		return Language.Code.IT;
				case "DE":	case "GERMAN":		return Language.Code.DE;
				case "ES":	case "SPANISH":		return Language.Code.ES;
				case "AR":	case "ARABIC":		return Language.Code.AR;
				case "ZHCN":	case "ZH-CN":	case "CHINESE":	case "CHINESE SIMPLIFIED":	case "CHINESE (SIMPLIFIED)":	return Language.Code.ZHCN;
				case "JA":	case "JAPANESE":	return Language.Code.JA;
				case "KO":	case "KOREAN":		return Language.Code.KO;
				case "VI":	case "VIETNAMESE":	return Language.Code.VI;
				case "RU":	case "RUSSIAN":		return Language.Code.DE;
				case "NL":	case "DUTCH":		return Language.Code.NL;
				case "AF":	case "AFRIKAANS":	return Language.Code.AF;
				case "BE":	case "BELARUSIAN":	return Language.Code.BE;
				case "BG":	case "BULGARIAN":	return Language.Code.BG;
				case "CA":	case "CATALAN":		return Language.Code.CA;
				case "CS":	case "CZECH":		return Language.Code.CS;
				case "CY":	case "WELSH":		return Language.Code.CY;
				case "ET":	case "ESTONIAN":	return Language.Code.ET;
				case "FA":	case "PERSIAN":		return Language.Code.FA;
				case "FI":	case "FINNISH":		return Language.Code.FI;
				case "GA":	case "IRISH":		return Language.Code.GA;
				case "GL":	case "GALICIAN":	return Language.Code.GL;
				case "HI":	case "HINDI":		return Language.Code.HI;
				case "HR":	case "CROATIAN":	return Language.Code.HR;
				case "HT":	case "HAITIAN":		return Language.Code.HT;
				case "ID":	case "INDONESIAN":	return Language.Code.ID;
				case "IS":	case "ICELANDIC":	return Language.Code.IS;
				case "IW":	case "HEBREW":		return Language.Code.IW;
				case "LT":	case "LITHUANIAN":	return Language.Code.LT;
				case "LV":	case "LATVIAN":		return Language.Code.LV;
				case "MK":	case "MACEDONIAN":	return Language.Code.MK;
				case "MS":	case "MALAY":		return Language.Code.MS;
				case "MT":	case "MALTESE":		return Language.Code.MT;
				case "NO":	case "NORWEGIAN":	return Language.Code.NO;
				case "PL":	case "POLISH":		return Language.Code.PL;
				case "PT":	case "PORTUGUESE":	return Language.Code.PT;
				case "RO":	case "ROMANIAN":	return Language.Code.RO;
				case "SK":	case "SLOVAK":		return Language.Code.SK;
				case "SL":	case "SLOVENIAN":	return Language.Code.SL;
				case "SQ":	case "ALBANIAN":	return Language.Code.SQ;
				case "SR":	case "SERBIAN":		return Language.Code.SR;
				case "SV":	case "SWEDISH":		return Language.Code.SV;
				case "SW":	case "SWAHILI":		return Language.Code.SW;
				case "TH":	case "THAI":		return Language.Code.TH;
				case "TL":	case "TAGALOG":		return Language.Code.TL;
				case "TR":	case "TURKISH":		return Language.Code.TR;
				case "UK":	case "UKRAINIAN":	return Language.Code.UK;
				case "YI":	case "YIDDISH":		return Language.Code.YI;
				case "ZHTW": case "ZH-TW":	case "CHINESE TRADITIONAL":	case "CHINESE (TRADITIONAL)":	return Language.Code.ZHTW;
			}
			
			return Language.Code.INVALID;
		}
		
		// Selection Box String - Language Code - Language Name
		public static string [,] languageStrings = {
			{"(AF) 	Afrikaans",		"AF",		"Afrikaans"},
			{"(AR)	Arabic", 		"AR",		"Arabic"},
			{"(BE) 	Belarusian",	"BE",		"Belarusian"},
			{"(BG) 	Bulgarian",		"BG",		"Bulgarian"},
			{"(CA) 	Catalan",		"CA",		"Catalan"},
			{"(CS) 	Czech",			"CS",		"Czech"},
			{"(CY) 	Welsh",			"CY",		"Welsh"},
			{"(DE) 	German",		"DE",		"German"},
			{"(EN) 	English",		"EN",		"English"},
			{"(ES) 	Spanish",		"ES",		"Spanish"},
			{"(ET) 	Estonian",		"ET",		"Estonian"},
			{"(FA) 	Persian",		"FA",		"Persian"},
			{"(FI) 	Finnish",		"FI",		"Finnish"},
			{"(FR) 	French",		"FR",		"French"},
			{"(GA) 	Irish",			"GA",		"Irish"},
			{"(GL) 	Galician",		"GL",		"Galician"},
			{"(HR) 	Croatian",		"HR",		"Croatian"},
			{"(HT) 	Haitian",		"HT",		"Haitian"},
			{"(ID) 	Indonesian",	"ID",		"Indonesian"},
			{"(IS) 	Icelandic",		"IS",		"Icelandic"},
			{"(IT) 	Italian",		"IT",		"Italian"},
			{"(IW) 	Hebrew",		"IW",		"Hebrew"},
			{"(JA) 	Japanese",		"JA",		"Japanese"},
			{"(KO) 	Korean",		"KO",		"Korean"},
			{"(LT) 	Lithuanian",	"LT",		"Lithuanian"},
			{"(LV) 	Latvian",		"LV",		"Latvian"},
			{"(MK) 	Macedonian",	"MK",		"Macedonian"},
			{"(MS) 	Malay",			"MS",		"Malay"},
			{"(MT) 	Maltese",		"MT",		"Maltese"},
			{"(NL) 	Dutch",			"NL",		"Dutch"},
			{"(NO) 	Norwegian",		"NO",		"Norwegian"},
			{"(PL) 	Polish",		"PL",		"Polish"},
			{"(PT) 	Portuguese",	"PT",		"Portuguese"},
			{"(RO) 	Romanian",		"RO",		"Romanian"},
			{"(RU) 	Russian",		"RU",		"Russian"},
			{"(SK) 	Slovak",		"SK",		"Slovak"},
			{"(SL) 	Slovenian",		"SL",		"Slovenian"},
			{"(SQ) 	Albanian",		"SQ",		"Albanian"},
			{"(SR) 	Serbian",		"SR",		"Serbian"},
			{"(SV) 	Swedish",		"SV",		"Swedish"},
			{"(SW) 	Swahili",		"SW",		"Swahili"},
			{"(TH) 	Thai",			"TH",		"Thai"},
			{"(TL) 	Tagalog",		"TL",		"Tagalog"},
			{"(TR) 	Turkish",		"TR",		"Turkish"},
			{"(UK) 	Ukrainian",		"UK",		"Ukrainian"},
			{"(VI) 	Vietnamese",	"VI",		"Vietnamese"},
			{"(YI) 	Yiddish",		"YI",		"Yiddish"},
			{"(ZHCN)	Chinese (Simplified)",	"ZH-CN",	"Chinese (Simplified)"},
			{"(ZHTW)	Chinese (Traditional)",	"ZH-TW",	"Chinese (Traditional)"}
		};
	}
}               
using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility.JSON.Internal
{
	public class Constants : MonoBehaviour 
	{
		public const string 	kNull				= "null";
		public const string 	kBoolTrue			= "true";
		public const string 	kBoolFalse			= "false";
		public const string		kWhiteSpaceLiterals	= " \n\t\r";
		public const string		kNumericLiterals	= "0123456789+-.eE";
	}
}
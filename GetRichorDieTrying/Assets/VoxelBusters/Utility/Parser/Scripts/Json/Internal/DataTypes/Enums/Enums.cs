using UnityEngine;
using System.Collections;

namespace VoxelBusters.Utility.JSON.Internal
{
	public enum Token
	{
		CURLY_OPEN	= 0,
		CURLY_CLOSE,
		SQUARED_OPEN,
		SQUARED_CLOSE,
		COLON,
		COMMA,
		STRING,
		NUMBER,
		WHITE_SPACE,
		TRUE,
		FALSE,
		NULL,
		NONE
	}
}
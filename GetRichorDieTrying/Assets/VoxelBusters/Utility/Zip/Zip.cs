using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;
using System.IO.Compression;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VoxelBusters.Utility
{
	/// <summary>
	/// Zip provides interface to compress and decompress files.
	/// </summary>
	/// <description>
	/// Credits: http://www.codeproject.com/Tips/319438/How-to-Compress-Decompress-directories
	/// </description>
	public class Zip : MonoBehaviour 
	{
		#region Delegates

		public delegate void ProgressDelegate (string _message);

		#endregion

#if UNITY_EDITOR
		#region MenuItems

		[MenuItem("Assets/Compress")]
		private static void Compress ()
		{
			string _compressFileAbsolutePath	= AssetsUtility.GUIDToAssetAbsolutePath(Selection.assetGUIDs[0]);

			// Generate output file path
			string _compressedFileName			= null;
			string _parentDirectoryPath			= null;

			if (Directory.Exists(_compressFileAbsolutePath))
			{
				DirectoryInfo _directoryInfo	= new DirectoryInfo(_compressFileAbsolutePath);

				// Set info
				_compressedFileName		= _directoryInfo.Name;
				_parentDirectoryPath	= _directoryInfo.Parent.FullName;

				// Output file name
				string _compressedFileAbsolutePath	= Path.Combine(_parentDirectoryPath, _compressedFileName) + ".gz";
				
				// Now compress the file
				CompressDirectory(_compressFileAbsolutePath, _compressedFileAbsolutePath, (string _outputMessage)=>{
					Console.WriteLine(_outputMessage);
				});
			}
		}

		[MenuItem("Assets/Compress", true)]
		private static bool CompressValidation ()
		{
			string[] _guids	= Selection.assetGUIDs;

			if (_guids.Length <= 0)
				return false;

			return Directory.Exists(AssetsUtility.GUIDToAssetAbsolutePath(_guids[0]));
		}

		#endregion
#endif

		#region Compress Methods

		private static void CompressFile (string _rootDirectory, string _relativePath, GZipStream _zipStream)
		{
			// Compress file name
			char[] _relativePathCharArray = _relativePath.ToCharArray();

			// Adding relative path length
			_zipStream.Write(BitConverter.GetBytes(_relativePathCharArray.Length), 0, sizeof(int));

			// Adding relative path
			foreach (char _char in _relativePathCharArray)
				_zipStream.Write(BitConverter.GetBytes(_char), 0, sizeof(char));
			
			// Compress file content
			byte[] _fileContentsByteArray = File.ReadAllBytes(Path.Combine(_rootDirectory, _relativePath));

			// Adding file contents length
			_zipStream.Write(BitConverter.GetBytes(_fileContentsByteArray.Length), 0, sizeof(int));

			// Adding file contents
			_zipStream.Write(_fileContentsByteArray, 0, _fileContentsByteArray.Length);
		}
	
		public static void CompressDirectory (string _inputDirectory, string _zippedFileName, ProgressDelegate _progress)
		{
			string[] _filesWithinDirectory 	= Directory.GetFiles(_inputDirectory, "*.*", SearchOption.AllDirectories);
			int _inputDirectoryStringLength = (_inputDirectory[_inputDirectory.Length - 1] == Path.DirectorySeparatorChar) ? _inputDirectory.Length : _inputDirectory.Length + 1;
			
			using (FileStream _zippedFileStream = new FileStream(_zippedFileName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				using (GZipStream _zipStream = new GZipStream(_zippedFileStream, CompressionMode.Compress))
				{
					// Iterate through each file to get relative path to the file and compress that file
					foreach (string _file in _filesWithinDirectory)
					{
						string _fileRelativePath = _file.Substring(_inputDirectoryStringLength);

						if (_progress != null)
							_progress("[Zip] Compressing file: " + _fileRelativePath);

						CompressFile(_inputDirectory, _fileRelativePath, _zipStream);
					}
				}
			}
		}

		#endregion

		#region Decompress Methods
		
		public static void DecompressToDirectory (string _compressedFilePath, string _decompressToDirectory, ProgressDelegate _progress = null)
		{
			using (FileStream _compressedFileStream = new FileStream(_compressedFilePath, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				using (GZipStream _zipStream = new GZipStream(_compressedFileStream, CompressionMode.Decompress, true))
				{
					while (DecompressFile(_decompressToDirectory, _zipStream, _progress));
				}
			}
		}
		
		private static bool DecompressFile (string _decompressToDirectory, GZipStream _zipStream, ProgressDelegate _progress)
		{
			// Decompressing file
			// Get compressed file's relative path string length
			byte[] _relativePathLengthByteArray = new byte[sizeof(int)];
			int _bytesReadLength 				= _zipStream.Read(_relativePathLengthByteArray, 0, sizeof(int));

			if (_bytesReadLength < sizeof(int))
				return false;
			
			int _relativePathStringLength 		= BitConverter.ToInt32(_relativePathLengthByteArray, 0);

			// Get compressed file's relative path, by reading all the characters
			byte[] _characterByteArray 					= new byte[sizeof(char)];
			StringBuilder _relativePathStringBuilder 	= new StringBuilder(_relativePathStringLength);

			for (int i = 0; i < _relativePathStringLength; i++)
			{
				_zipStream.Read(_characterByteArray, 0, sizeof(char));
				char _char = BitConverter.ToChar(_characterByteArray, 0);

				// Add character to string builder, to create relative file path
				_relativePathStringBuilder.Append(_char);
			}

			// Now we have the relative filename of compressed file
			string _compressedFileRelativePath = _relativePathStringBuilder.ToString();

			if (_progress != null)
				_progress("[Zip] Decompressing file: " + _compressedFileRelativePath);
			
			// Decompress file contents
			// Get compressed file's content length
			byte[] _compressedFileContentsLengthByteArray 	= new byte[sizeof(int)];
			_zipStream.Read(_compressedFileContentsLengthByteArray, 0, sizeof(int));

			int _compressedFileContentsLength 				= BitConverter.ToInt32(_compressedFileContentsLengthByteArray, 0);

			// Get compressed file's content
			byte[] _compressedFileContentsByteArray 		= new byte[_compressedFileContentsLength];
			_zipStream.Read(_compressedFileContentsByteArray, 0, _compressedFileContentsLength);
			
			string _compressedFileAbsolutePath 		= Path.Combine(_decompressToDirectory, _compressedFileRelativePath);
			string _compressedFileParentDirectory 	= Path.GetDirectoryName(_compressedFileAbsolutePath);

			if (!Directory.Exists(_compressedFileParentDirectory))
				Directory.CreateDirectory(_compressedFileParentDirectory);
			
			using (FileStream _outFileStream = new FileStream(_compressedFileAbsolutePath, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				_outFileStream.Write(_compressedFileContentsByteArray, 0, _compressedFileContentsLength);
			}
			
			return true;
		}

		#endregion
	}
}
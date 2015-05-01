﻿using UnityEngine;
using System.Collections;
using VoxelBusters.Utility;
using VoxelBusters.DebugPRO;

#if UNITY_EDITOR
using UnityEditor;

namespace VoxelBusters.NativePlugins
{
	using Internal;

	public partial class MediaLibraryEditor : MediaLibrary
	{
		#region Properties
	
		private EditorMediaGallery 		m_gallery;

		#endregion

		#region Unity Methods
		
		protected override void Start()
		{
			base.Start();

			// Load gallery
			LoadGallery();
		}
		
		#endregion

		#region Image

		public override bool IsCameraSupported ()
		{
			Console.LogError(Constants.kDebugTag, Constants.kErrorMessage);
			return base.IsCameraSupported();
		}
		
		public override void PickImage (eImageSource _source, float _scaleFactor, PickImageCompletion _onCompletion)
		{
			base.PickImage(_source, _scaleFactor, _onCompletion);

			if (_scaleFactor > 0f)
			{
				m_gallery.PickImage(_source);
			}
		}
		
		public override void SaveImageToGallery (byte[] _imageByteArray, SaveImageToGalleryCompletion _onCompletion)
		{
			base.SaveImageToGallery(_imageByteArray, _onCompletion);

			if (_imageByteArray != null)
			{
				// Feature isnt supported
				Console.LogError(Constants.kDebugTag, Constants.kErrorMessage);
				
				// Associated error event is raised
				SaveImageToGalleryFinished(false);
			}
		}

		#endregion

		#region Video
		
		public override void PlayYoutubeVideo (string _videoID, PlayVideoCompletion _onCompletion)
		{
			if(!string.IsNullOrEmpty(_videoID))
			{
				Application.OpenURL("http://www.youtube.com/watch?v=" + _videoID);
				PlayVideoFinished(ePlayVideoFinishReason.PLAYBACK_ENDED);
			}
			else
			{
				PlayVideoFinished(ePlayVideoFinishReason.PLAYBACK_ERROR);
			}
		}	

		public override void PlayEmbeddedVideo (string _embedHTMLString, PlayVideoCompletion _onCompletion)
		{
			base.PlayEmbeddedVideo(_embedHTMLString, _onCompletion);
			
			if (!string.IsNullOrEmpty(_embedHTMLString))
			{
				string _tempPath = FileUtil.GetUniqueTempPathInProject() + ".html";

				Console.LogError(Constants.kDebugTag, _tempPath);
				System.IO.StreamWriter _stream = System.IO.File.CreateText(_tempPath);
				_stream.Write(_embedHTMLString);
				_stream.Close();
				
				// Open URL
				Application.OpenURL("file://" + Application.dataPath + "/../" + _tempPath);

				// Associated error event is raised
				PlayVideoFinished(ePlayVideoFinishReason.PLAYBACK_ENDED);
			}
		}
		
		public override void PlayVideoFromURL (URL _URL, PlayVideoCompletion _onCompletion)
		{
			base.PlayVideoFromURL(_URL, _onCompletion);
			
			if (!string.IsNullOrEmpty(_URL.URLString))
			{
				Application.OpenURL(_URL.URLString);
				PlayVideoFinished(ePlayVideoFinishReason.PLAYBACK_ENDED);
			}
		}
		
		public override void PlayVideoFromGallery (PickVideoCompletion _onPickVideoCompletion, PlayVideoCompletion _onPlayVideoCompletion)
		{
			base.PlayVideoFromGallery(_onPickVideoCompletion, _onPlayVideoCompletion);

			m_gallery.PickVideoFromGallery();
			
			// Feature isnt supported
			Console.LogWarning(Constants.kDebugTag, "Video Playback from gallery not supported on Editor");

			// Associated event is raised.
			PlayVideoFinished(ePlayVideoFinishReason.PLAYBACK_ERROR);
		}
		
		#endregion

		#region Helpers

		private void LoadGallery ()
		{
			GameObject _gameObject 	= new GameObject("EditorGallery");
			_gameObject.hideFlags 	= HideFlags.HideInHierarchy;
			m_gallery = _gameObject.AddComponent<EditorMediaGallery>();
		}

		#endregion
	}
}
#endif	
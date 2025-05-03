using System.Collections.Generic;
using UnityEngine;

public class DownloadManager
{
	public interface IDownloadResult
	{
		string _pText { get; }

		Texture2D _pTexture { get; }
	}

	public interface IDownloadInProgress
	{
		float _pProgress { get; }

		void Cancel();
	}

	private class DownloadItem : IDownloadResult, IDownloadInProgress
	{
		public int _id;

		public string _url;

		public WWW _www;

		public DResultCallback _onResult;

		public DProgressCallback _onProgress;

		string IDownloadResult._pText
		{
			get
			{
				return (_www == null) ? null : _www.text;
			}
		}

		Texture2D IDownloadResult._pTexture
		{
			get
			{
				return (_www == null) ? null : _www.texture;
			}
		}

		float IDownloadInProgress._pProgress
		{
			get
			{
				return (_www == null) ? 0f : _www.progress;
			}
		}

		public bool _pIsQueued
		{
			get
			{
				return _id >= 1 && _www == null;
			}
		}

		public bool _pIsActive
		{
			get
			{
				return _id >= 1 && _www != null;
			}
		}

		public bool _pIsEnded
		{
			get
			{
				return _id == -1;
			}
		}

		public DownloadItem(int id, string url, DResultCallback onResult, DProgressCallback onProgress, bool isStart)
		{
			_id = id;
			_url = url;
			_www = null;
			_onResult = onResult;
			_onProgress = onProgress;
			if (isStart)
			{
				Start();
			}
		}

		void IDownloadInProgress.Cancel()
		{
			End();
		}

		public void Start()
		{
			_www = new WWW(_url);
		}

		public void End()
		{
			_id = -1;
		}
	}

	public delegate void DResultCallback(IDownloadResult download);

	public delegate void DProgressCallback(IDownloadInProgress download);

	private const int INVALID_DOWNLOAD_ID = -1;

	private const int VALID_DOWNLOAD_ID_START = 1;

	private int _nextDownloadId;

	private List<DownloadItem> _downloads;

	private bool _isAllowAsync;

	private int _newDownloadId
	{
		get
		{
			return _nextDownloadId++;
		}
	}

	public DownloadManager()
	{
		_nextDownloadId = 1;
		_downloads = new List<DownloadItem>();
		_isAllowAsync = true;
	}

	public DownloadManager(bool isAllowAsync)
	{
		_nextDownloadId = 1;
		_downloads = new List<DownloadItem>();
		_isAllowAsync = isAllowAsync;
	}

	public int? BeginDownload(string url, DResultCallback onResult, DProgressCallback onProgress)
	{
		return CreateDownload(url, onResult, onProgress, _isAllowAsync);
	}

	public int? QueueDownload(string url, DResultCallback onResult, DProgressCallback onProgress)
	{
		return CreateDownload(url, onResult, onProgress, false);
	}

	public void CancelDownload(int id)
	{
		DownloadItem downloadItem = FindDownload(id);
		if (downloadItem != null)
		{
			CancelDownload(downloadItem);
		}
	}

	private int CreateDownload(string url, DResultCallback onResult, DProgressCallback onProgress, bool isBegin)
	{
		int newDownloadId = _newDownloadId;
		_downloads.Add(new DownloadItem(newDownloadId, url, onResult, onProgress, isBegin));
		return newDownloadId;
	}

	private void UpdateDownload(DownloadItem dl)
	{
		if (dl._onProgress != null)
		{
			dl._onProgress(dl);
		}
		if (dl._pIsActive && dl._www.isDone)
		{
			if (dl._www.error != null && dl._www.error.Length > 0)
			{
				Debug.LogError("[DownloadManager] Error: " + dl._www.error);
			}
			else
			{
				Debug.Log("[DownloadManager] Download size: " + dl._www.bytesDownloaded + "B");
			}
			EndDownload(dl);
		}
	}

	private void CancelDownload(DownloadItem dl)
	{
		EndDownload(dl);
	}

	private void EndDownload(DownloadItem dl)
	{
		if (dl != null)
		{
			if (dl._onResult != null)
			{
				dl._onResult(dl);
			}
			dl.End();
		}
	}

	private DownloadItem FindDownload(int id)
	{
		return _downloads.Find((DownloadItem d) => d._id == id);
	}

	public void Update()
	{
		StartQueuedDownload();
		for (int i = 0; i < _downloads.Count; i++)
		{
			if (_downloads[i] != null)
			{
				UpdateDownload(_downloads[i]);
			}
		}
		CleanDownloads();
	}

	private void StartQueuedDownload()
	{
		DownloadItem downloadItem = null;
		int num = 0;
		foreach (DownloadItem download in _downloads)
		{
			if (download != null)
			{
				if (download._pIsActive)
				{
					num++;
				}
				else if (download._pIsQueued && downloadItem == null)
				{
					downloadItem = download;
				}
			}
		}
		if (num == 0 && downloadItem != null)
		{
			downloadItem.Start();
		}
	}

	private void CleanDownloads()
	{
		for (int i = 0; i < _downloads.Count; i++)
		{
			if (_downloads[i] == null || _downloads[i]._pIsEnded)
			{
				_downloads.RemoveAt(i--);
			}
		}
	}
}

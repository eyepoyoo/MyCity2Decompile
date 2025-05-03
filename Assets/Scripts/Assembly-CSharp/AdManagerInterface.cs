using System.Collections.Generic;

public interface AdManagerInterface
{
	bool _pIsInitialised { get; }

	bool _pIsFrozen { get; set; }

	bool _pDoesSupportInterstitial { get; }

	bool _pDoesSupportVideo { get; }

	bool _pDoesSupportBanner { get; }

	bool _pDoesSupportMoreApps { get; }

	bool _pDoesSupportTracking { get; }

	void init();

	void cacheInterstitial(string location);

	void showInterstitial(string location);

	void cacheVideo(string location);

	void showVideo(string location);

	void cacheBanner(string location);

	void showBanner(string location);

	void cacheMoreApps();

	void showMoreApps();

	void trackEvent(string eventIdentifier);

	void trackEventWithMetadata(string eventIdentifier, Dictionary<string, string> metadata);

	void trackEventWithValue(string eventIdentifier, float value);

	void trackEventWithValueAndMetadata(string eventIdentifier, float value, Dictionary<string, object> metadata);
}

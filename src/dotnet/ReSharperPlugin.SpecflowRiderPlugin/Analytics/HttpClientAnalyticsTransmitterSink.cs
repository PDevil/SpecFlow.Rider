using System;
using System.Threading.Tasks;
using JetBrains.Application;
using JetBrains.Util;
using Newtonsoft.Json;

namespace ReSharperPlugin.SpecflowRiderPlugin.Analytics
{
    [ShellComponent]
    public class HttpClientAnalyticsTransmitterSink : IAnalyticsTransmitterSink
    {
        private readonly Uri _appInsightsDataCollectionEndPoint = new Uri("https://dc.services.visualstudio.com/v2/track");
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IEnvironmentSpecFlowTelemetryChecker _environmentSpecFlowTelemetryChecker;
        private readonly AppInsightsConfiguration _appInsightsConfiguration;
        private readonly ILogger _logger;

        public HttpClientAnalyticsTransmitterSink(
            IHttpClientProvider httpClientProvider,          
            IEnvironmentSpecFlowTelemetryChecker environmentSpecFlowTelemetryChecker,
            AppInsightsConfiguration appInsightsConfiguration,
            ILogger logger
        )
        {
            _httpClientProvider = httpClientProvider;
            _environmentSpecFlowTelemetryChecker = environmentSpecFlowTelemetryChecker;
            _appInsightsConfiguration = appInsightsConfiguration;
            _logger = logger;
        }               

        public async Task TransmitEvent(IAnalyticsEvent analyticsEvent, string userId)
        {
            if (!_environmentSpecFlowTelemetryChecker.IsSpecFlowTelemetryEnabled())
                return;
            
            try
            {
                await TransmitEventAsync(analyticsEvent, userId);
            }
            catch (Exception e)
            {
                _logger.Verbose(e);
            }
        }

        private async Task TransmitEventAsync(IAnalyticsEvent analyticsEvent, string userId)
        {
            var eventTelemetry = new AppInsightsEventTelemetry(userId, _appInsightsConfiguration.InstrumentationKey, analyticsEvent);
            var content = JsonConvert.SerializeObject(eventTelemetry);
            await _httpClientProvider.PostStringAsync(_appInsightsDataCollectionEndPoint, content);            
        }
    }
}
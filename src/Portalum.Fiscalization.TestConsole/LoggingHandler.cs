using Microsoft.Extensions.Logging;

namespace Portalum.Fiscalization.TestConsole
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingHandler> _logger;

        public LoggingHandler(ILogger<LoggingHandler> logger)
        {
            this._logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this._logger.LogTrace($"{nameof(SendAsync)} - Request {request}");
            if (request.Content != null)
            {
                this._logger.LogTrace(await request.Content.ReadAsStringAsync());
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            this._logger.LogTrace($"{nameof(SendAsync)} - Response {response}");
            if (response.Content != null)
            {
                this._logger.LogTrace(await response.Content.ReadAsStringAsync());
            }

            return response;
        }
    }
}

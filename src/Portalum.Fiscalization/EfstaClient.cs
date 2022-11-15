using Microsoft.Extensions.Logging;
using Portalum.Fiscalization.Helpers;
using Portalum.Fiscalization.JsonConverters;
using Portalum.Fiscalization.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Portalum.Fiscalization
{
    /// <summary>
    /// Efsta EFR Client
    /// </summary>
    public class EfstaClient
    {
        private readonly ILogger<EfstaClient> _logger;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>
        /// Efsta EFR Client
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="httpClient"></param>
        public EfstaClient(
            ILogger<EfstaClient> logger,
            HttpClient httpClient)
        {
            this._logger = logger;
            this._httpClient = httpClient;

            this._jsonSerializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new DateTimeJsonConverter() }
                //WriteIndented = true
            };
        }

        /// <summary>
        /// Register Transaction
        /// </summary>
        /// <param name="request"></param>
        /// <param name="taxId"></param>
        /// <param name="client"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public async Task<RegisterResponse> RegisterAsync(
            RegisterRequest request,
            string taxId,
            string client = "def",
            CancellationToken cancellationToken = default,
            int timeoutInSeconds = 10)
        {
            using var timeoutCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutInSeconds));
            using var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellationTokenSource.Token, timeoutCancellationTokenSource.Token);

            try
            {
                using var response = await this._httpClient.PostAsJsonAsync($"/register?RN={client}&TaxId={taxId}",
                    request,
                    this._jsonSerializerOptions,
                    linkedCancellationTokenSource.Token);

                if (!response.IsSuccessStatusCode)
                {
                    this._logger.LogWarning($"{nameof(RegisterAsync)} - {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync(linkedCancellationTokenSource.Token);

                if (this._logger.IsEnabled(LogLevel.Trace))
                {
                    var prettyJson = JsonHelper.PrettyJson(json);
                    this._logger.LogTrace($"{nameof(RegisterAsync)} - {prettyJson}");
                }

                return JsonSerializer.Deserialize<RegisterResponse>(json);
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception, $"{nameof(RegisterAsync)}");
            }

            return null;
        }


        /// <summary>
        /// Register Transaction test test
        /// </summary>
        /// <param name="request"></param>
        /// <param name="taxId"></param>
        /// <param name="client"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public async Task<TransactionStartResponse> Register1Async(
            TransactionStartRequest request,
            string taxId,
            string client = "def",
            CancellationToken cancellationToken = default,
            int timeoutInSeconds = 10)
        {
            using var timeoutCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutInSeconds));
            using var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCancellationTokenSource.Token, timeoutCancellationTokenSource.Token);

            try
            {
                //using var response = await this._httpClient.PostAsJsonAsync($"/register?RN={client}&TaxId={taxId}",
                using var response = await this._httpClient.PostAsJsonAsync($"/register",
                    request,
                    this._jsonSerializerOptions,
                    linkedCancellationTokenSource.Token);

                if (!response.IsSuccessStatusCode)
                {
                    this._logger.LogWarning($"{nameof(Register1Async)} - {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync(linkedCancellationTokenSource.Token);

                if (this._logger.IsEnabled(LogLevel.Trace))
                {
                    var prettyJson = JsonHelper.PrettyJson(json);
                    this._logger.LogTrace($"{nameof(Register1Async)} - {prettyJson}");
                }

                return JsonSerializer.Deserialize<TransactionStartResponse>(json);
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception, $"{nameof(RegisterAsync)}");
            }

            return null;
        }

        /// <summary>
        /// Get State
        /// </summary>
        /// <returns></returns>
        public async Task<StateResponse> GetStateAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var response = await this._httpClient.GetAsync("/state", cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    this._logger.LogWarning($"{nameof(GetStateAsync)} - {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync(cancellationToken);

                if (this._logger.IsEnabled(LogLevel.Trace))
                {
                    var prettyJson = JsonHelper.PrettyJson(json);
                    this._logger.LogTrace($"{nameof(GetStateAsync)} - {prettyJson}");
                }

                return JsonSerializer.Deserialize<StateResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception, $"{nameof(GetStateAsync)}");
            }

            return null;
        }
    }
}

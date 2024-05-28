using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace GrupoLTM.WebSmart.Infrastructure.Helpers
{
    public class JsonRequest
    {
        private readonly RestClient _restClient;

        public JsonRequest(string url, params KeyValuePair<string, string>[] defaultHeaders)
        {
            _restClient = new RestClient(url);
            foreach (var header in defaultHeaders)
            {
                _restClient.AddDefaultHeader(header.Key, header.Value);
            }
            _restClient.AddDefaultHeader("Accept", "application/json");
        }

        public Tuple<T, TError> Post<T, TError>(string resource, object body, params KeyValuePair<string, string>[] specificHeaders)
            where T : new()
        {
            var response = ExecuteRequest<T>(resource, body, specificHeaders);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new Tuple<T, TError>(
                    JsonConvert.DeserializeObject<T>(response.Content),
                    default(TError));
            }
            else
            {
                return new Tuple<T, TError>(
                    default(T),
                    JsonConvert.DeserializeObject<TError>(response.Content));
            }
        }

        public T Post<T, TError>(string resource, object body, out TError error, params KeyValuePair<string, string>[] specificHeaders)
            where T : new()
        {
            var response = ExecuteRequest<T>(resource, body, specificHeaders);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                error = default(TError);
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            else
            {
                error = JsonConvert.DeserializeObject<TError>(response.Content);
                return default(T);
            }
        }

        public T Post<T>(string resource, object body, out HttpStatusCode statusCode, params KeyValuePair<string, string>[] specificHeaders)
            where T : new()
        {
            var response = ExecuteRequest<T>(resource, body, specificHeaders);
            statusCode = response.StatusCode;

            if (response.ErrorException != null)
            {
                throw new ApplicationException("Error retrieving response. Check inner details for more info.", response.ErrorException);
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public T Post<T>(string resource, object body, params KeyValuePair<string, string>[] specificHeaders)
            where T : new()
        {

            var response = ExecuteRequest<T>(resource, body, specificHeaders);

            if (response.ErrorException != null)
            {
                throw new ApplicationException("Error retrieving response. Check inner details for more info.", response.ErrorException);
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        private IRestResponse<T> ExecuteRequest<T>(string resource, object body, params KeyValuePair<string, string>[] specificHeaders)
            where T : new()
        {
            var request = new RestRequest(resource, Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddBody(body);

            foreach (var header in specificHeaders)
            {
                request.AddHeader(header.Key, header.Value);
            }

            var response = _restClient.Execute<T>(request);

            return response;
        }

        public T Get<T>(string resource, object parameters, params KeyValuePair<string, string>[] specificHeaders)
            where T : new()
        {
            var request = new RestRequest(resource, Method.GET);
            request.AddObject(parameters);

            foreach (var header in specificHeaders)
            {
                request.AddHeader(header.Key, header.Value);
            }

            var response = _restClient.Execute<T>(request);
            if (response.ErrorException != null)
            {
                throw new ApplicationException("Error retrieving response. Check inner details for more info.", response.ErrorException);
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public T Get<T>(string resource, params KeyValuePair<string, string>[] specificHeaders)
          where T : new()
        {
            var request = new RestRequest(resource, Method.GET);

            foreach (var header in specificHeaders)
            {
                request.AddHeader(header.Key, header.Value);
            }

            var response = _restClient.Execute<T>(request);

            if (response.ErrorException != null)
            {
                throw new ApplicationException("Error retrieving response. Check inner details for more info." + response.ErrorException.StackTrace, response.ErrorException);
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
}
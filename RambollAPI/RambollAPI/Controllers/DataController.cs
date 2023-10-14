using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RambollAPI.Models;
using System;
using System.Reflection;
using System.Security;
using System.Web;

namespace RambollAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private const string url = "https://search.worldbank.org/api/v2/wds?format=json";
        private List<string> AllowedQueryParameters = new List<string>() { "id", "owner", "count", "docty", "lang", "display_title", "projectid", "rows", "page", "os", "sort", "order" };
        public HttpClient HttpClient;
        public DataController(IHttpClientFactory httpClientFactory)
        {
            HttpClient = httpClientFactory.CreateClient("WorldBank");
        }

        [HttpGet()]
        public async Task<ActionResult<string>> Get(
            string? id,
            string? owner,
            string? count,
            string? docty,
            string? lang,
            string? display_title,
            string? repnb,
            string? sort,
            string? order,
            int rows = 10,
            int page = 1,
            int os = 0)
        {
            var queryParametersAreValid = ValidateQueryParameters();

            if(!queryParametersAreValid)
            {
                return StatusCode(422);
            }

            var query = BuildQuery(id, owner, count, docty, lang, display_title, repnb, sort, order, rows, page, os);

            try
            {
                var result = await GetData(query);

                if (result.Documents.Any())
                {
                    return StatusCode(200, result);
                }
                else
                {
                    return StatusCode(204, result);
                }
            } 
            catch (Exception)
            {
                return StatusCode(400);
            }
        }

        private async Task<DataResponse> GetData(string query)
        {
            var response = await HttpClient.GetAsync(query);

            var result = await response.Content.ReadAsStringAsync();

            var deserilizedResult = JsonConvert.DeserializeObject<DataResponse>(result)!;

            deserilizedResult.Documents.Remove("facets");

            return deserilizedResult;
        }

        private bool ValidateQueryParameters()
        {
            var queryString = HttpUtility.ParseQueryString(Request.QueryString.Value!);

            if (queryString is null)
            {
                return false;
            }

            var queryParameters = queryString.AllKeys.ToList();

            var queryParametersAreValid = queryParameters.All(key => AllowedQueryParameters.Contains(key));

            return queryParametersAreValid;
        }

     
        private string BuildQuery(
            string? id,
            string? owner,
            string? count,
            string? docty,
            string? lang,
            string? display_title,
            string? repnb,
            string? sort,
            string? order,
            int rows = 10,
            int page = 1,
            int os = 0)
        {
            var query = url;

            if (id is not null)
            {
                query += $"&id={id}";
            }

            if (owner is not null)
            {
                query += $"&owner={owner}";
            }

            if (count is not null)
            {
                query += $"&count={count}";
            }

            if (docty is not null)
            {
                query += $"&docty={docty}";
            }

            if (lang is not null)
            {
                query += $"&lang={lang}";
            }

            if (display_title is not null)
            {
                query += $"&display_title={display_title}";
            }

            if (repnb is not null)
            {
                query += $"&repnb={repnb}";
            }

            if (sort is not null)
            {
                query += $"&sort={sort}";
            }

            if (order is not null)
            {
                query += $"&order={order}";
            }

            query += $"&rows={rows}&page={page}&os={os}";

            return query;
        }
    }
}
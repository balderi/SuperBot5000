using System;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System.IO;
using Google.Apis.YouTube.v3.Data;
using System.Linq;

namespace SuperBot5000.Utils
{
    public class YoutubeUtil
    {
        private readonly YouTubeService _client;

        public YoutubeUtil()
        {
            var init = new BaseClientService.Initializer
            {
                ApiKey = File.ReadAllText("../apikey.txt"),
                ApplicationName = "SuperBot5000"
            };
            _client = new YouTubeService(init);
        }

        public SearchResult GetRelatedVideo(string id)
        {
            var search = _client.Search.List("id,snippet");
            search.RelatedToVideoId = id;
            search.Type = "video";
            search.Fields = "items(id/kind,id/videoId,snippet/title,snippet/thumbnails/default/url)";
            search.MaxResults = 1;
            var results = search.Execute();

            if (results.Items.Count < 1)
                throw new ArgumentException("No results found");
            return results.Items.First();
        }

        public string GetVideoId(string query)
        {
            var search = _client.Search.List("id,snippet");
            search.Type = "video";
            search.Fields = "items(id/videoId)";
            search.Q = query;
            var result = search.Execute();
            if (result.Items.Count < 0)
                return "";
            return result.Items.First().Id.VideoId;
        }

        public VideoListResponse GetVideosById(string id)
        {
            var result = _client.Videos.List("snippet,localizations,contentDetails");
            result.Id = id;
            return result.Execute();
        }

        public Video GetFirstVideoById(string id)
        {
            return GetVideosById(id).Items.First();
        }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Не выполнено требование на получение объектов в задаваемом пользователем интервале.

namespace ComicLbrry
{
    public class ApiHandlingCls
    {
        private readonly RequestHandlingCls _rqstHndlng;
        private string _url;
        private readonly string _identificationStr;

        private readonly List<PersonageCls> _personagesLst;
        private readonly List<ComicBookCls> _booksLst;
        private IList<JToken> _listResult;

        public List<PersonageCls> PersonagesLst { get; set; }
        public ApiHandlingCls()
        {
            _rqstHndlng = new RequestHandlingCls();
            _identificationStr = $"&ts={ApiConfigCls.TmStmp}" +
                                    $"&apikey={ApiConfigCls.PublicK}" +
                                    $"&hash={ApiConfigCls.Hash}";

            _personagesLst = new List<PersonageCls>();
            _booksLst = new List<ComicBookCls>();
            _listResult = new List<JToken>();
        }
        public List<PersonageCls> GetPersonages(int ofst = 0)
        {
            if (ofst >= _personagesLst.Count)
            {
                _url = $"https://gateway.marvel.com/v1/public/characters?offset={ofst}{_identificationStr}";
                _listResult = JObject.Parse(_rqstHndlng.RequestLaunch(_url))["data"]["results"]
                                            .Children()
                                            .ToList();
                foreach (JToken personageResult in _listResult)
                {
                    _personagesLst.Add(new PersonageCls
                    {
                        Id = Int32.Parse($"{personageResult["id"]}"),
                        Name = $"{personageResult["name"]}",
                        PicturePath = $"{personageResult["thumbnail"]["path"]}.{personageResult["thumbnail"]["extension"]}",
                        Description = $"{personageResult["description"]}"
                    });
                }
            }
            return _personagesLst;
        }
        public List<ComicBookCls> GetBooks(int id = 0)
        {
            if (id != 0)
            {
                _booksLst.Clear();
                _url = $"https://gateway.marvel.com/v1/public/characters/{id}/comics?{_identificationStr}";
                _listResult = JObject.Parse(_rqstHndlng.RequestLaunch(_url))["data"]["results"]
                                            .Children()
                                            .ToList();
                foreach (JToken bookResult in _listResult)
                {
                    _booksLst.Add(new ComicBookCls
                    {
                        Name = $"{bookResult["title"]}",
                        PicturePath = $"{bookResult["thumbnail"]["path"]}.{bookResult["thumbnail"]["extension"]}"
                    });
                }
            }
            return _booksLst;
        }
    }
}

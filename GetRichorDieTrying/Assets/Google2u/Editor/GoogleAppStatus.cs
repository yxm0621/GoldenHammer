using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

public class GoogleAppStatus
{
    readonly XmlNode _NodeRss;
    readonly XmlNode _NodeChannel;
    public Channel RowNews;

    public struct Channel
    {
        public string Title;
        public string Link;
        public string Description;
        public List<Item> Items;
    }

    public struct Item
    {
        public string Title;
        public string Link;
        public string Description;
        public string PubDate;
        public string Guid;
        public string DcDate;
    }

    public GoogleAppStatus()
    {
        RowNews = new Channel {Items = new List<Item>()};
        var rssReader = new XmlTextReader("http://www.google.com/appsstatus/rss/en");
        var rssDoc = new XmlDocument();
        rssDoc.Load(rssReader);
        // Loop for the <rss> tag
        for (var root = 0; root < rssDoc.ChildNodes.Count; root++)
        {
            if (rssDoc.ChildNodes[root].Name == "rss")
            {
                _NodeRss = rssDoc.ChildNodes[root];
                for (var channel = 0; channel < _NodeRss.ChildNodes.Count; channel++)
                {
                    if (_NodeRss.ChildNodes[channel].Name != "channel") 
                        continue;

                    _NodeChannel = _NodeRss.ChildNodes[channel];

                    var element = _NodeChannel["title"];
                    if (element != null)
                        RowNews.Title = element.InnerText;

                    element = _NodeChannel["link"];
                    if (element != null)
                        RowNews.Link = element.InnerText;

                    element = _NodeChannel["description"];
                    if (element != null)
                        RowNews.Description = element.InnerText;

                    for (var i = 0; i < _NodeChannel.ChildNodes.Count; i++)
                    {
                        if (_NodeChannel.ChildNodes[i].Name != "item")
                            continue;

                        var nodeItem = _NodeChannel.ChildNodes[i];

                        var itm = new Item();

                        element = nodeItem["title"];
                        if (element != null)
                            itm.Title = element.InnerText;

                        element = nodeItem["link"];
                        if (element != null)
                            itm.Link = element.InnerText;


                        element = nodeItem["description"];
                        if (element != null)
                        {
                            var htmlString = element.InnerText;
                            itm.Description = Regex.Replace(htmlString, "<.*?>", Environment.NewLine);
                            itm.Description = Regex.Replace(itm.Description, "&nbsp;", string.Empty);
                        }

                        element = nodeItem["pubDate"];
                        if (element != null)
                            itm.PubDate = element.InnerText;

                        element = nodeItem["guid"];
                        if (element != null)
                            itm.Guid = element.InnerText;

                        element = nodeItem["dc:date"];
                        if (element != null)
                            itm.DcDate = element.InnerText;

                        RowNews.Items.Add(itm);
                    }
                }
            }
        }
    }
}

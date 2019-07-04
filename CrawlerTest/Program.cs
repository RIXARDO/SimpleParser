using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Web;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Text;
using System.Net;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace CrawlerTest
{
    class Program
    {
        static void Main(string[] args)
        {

            StreamReader reader = new StreamReader(@"R:\132.html");//R:\TestPage.html"); //
            string html2;
            using (reader)
            {
                html2 = reader.ReadToEnd();
            }

            //IWebDriver driver = new ChromeDriver();
            //driver.Url=@"https://elmir.ua/processors/";
            bool NotEnd = true;
            int pageNumber = 0;

            List<CPU> cpus = new List<CPU>();
            List<GPU> gpus = new List<GPU>();
            CookieCollection cookie = new CookieCollection();
            do
            {


                string urlPage = @"http://v-comp.com.ua/protsessory/?paga=" + pageNumber + @"&col_page=50&sort=&minP=0&maxP=60941.396&bren=&param=bnVsbA==";

                var html = LoadPage(urlPage, ref cookie);

                //using(StreamWriter writer = new StreamWriter(@"R:\TestPage.html"))
                //{
                //    writer.Write(html);
                //}

                var document = new HtmlDocument();
                document.LoadHtml(html);

                cpus.AddRange(GetCPUs(document));

                //for(int i = 0+pageNumber*50; i<50+pageNumber*50; i++)
                //{
                //    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t", cpus[i].Manufacture, cpus[i].ProcessorNumber, cpus[i].NumberOfCores, cpus[i].PBF);
                //}
                //HtmlNodeCollection links2 = document.DocumentNode.SelectNodes("//table[@id='t_base']//div[@class='section_tovar_info']//div[@class='mini_msg']//table");

                //HtmlNodeCollection links = document.DocumentNode.SelectNodes("//table[@id='t_base']//div[@class='section_tovar_info']//h3//a");

                //Console.WriteLine(document.DocumentNode.InnerText);


                //Regex regex = new Regex("(AMD|)");

                //string itemText; 

                ////13 element
                //foreach (HtmlNode item in links2)
                //{
                //    itemText =  item.ChildNodes["tbody"].ChildNodes[1].ChildNodes[3].InnerHtml;
                //    while (itemText.IndexOf("&nbsp;") != -1)
                //    {
                //        itemText = itemText.Remove(itemText.IndexOf("&nbsp;"), 6);
                //        //Console.WriteLine(item.ChildNodes[0].ChildNodes[2].ChildNodes[1]);
                //    }
                //    Console.WriteLine(itemText.Trim("\n\t<p>/".ToCharArray()));
                //}


                //foreach (HtmlNode node in links)
                //{
                //    var item = node.InnerHtml;
                //    //Console.WriteLine("{0}", node.InnerText);


                //    if (item.ToUpper().Contains("AMD "))
                //    {
                //        CPU Entry = cpus.Find(x => x.ProcessorNumber == item.Remove(0, item.IndexOf("AMD ") + 4));
                //        if (Entry == null)
                //        {
                //            cpus.Add(new CPU { Manufacture = "AMD", ProcessorNumber = item.Remove(0, item.IndexOf("AMD ") + 4) });
                //        }
                //        //Console.WriteLine(AMD.Remove(0, AMD.IndexOf("AMD ") + 4));
                //    }
                //    if (item.ToUpper().Contains("INTEL"))
                //    {
                //        CPU Entry = cpus.Find(x=>x.ProcessorNumber==item.Remove(0, item.IndexOf("INTEL ".ToUpper()) + 6));
                //        if (Entry == null)
                //        {
                //            cpus.Add(new CPU { Manufacture = "Intel", ProcessorNumber = item.Remove(0, item.IndexOf("INTEL ".ToUpper()) + 6) });
                //        }
                //    }


                //}


                //foreach(var cpu in cpus)
                //{
                //    Console.WriteLine(cpu.ProcessorNumber);
                //}
                var f = document.DocumentNode.SelectSingleNode("//div[@id='c_base']//div[@class='section_tovar_info']");
                if (document.DocumentNode.SelectSingleNode("//div[@id='c_base']//div[@class='section_tovar_info']") == null || cpus.Count == 0)
                {
                    NotEnd = false;
                    break;
                }
                else
                {
                    pageNumber++;
                }
                //Console.ReadKey();
                Console.WriteLine("new page\n\n");
                Thread.Sleep(4000);

            } while (NotEnd);

            foreach (var cpu in cpus)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t", cpu.Manufacture, cpu.ProcessorNumber, cpu.NumberOfCores, cpu.PBF);
            }
            SaveCPUs(cpus);
        }

        

        //static string LoadPage(string url)
        //{
        //    var WebClient = new WebClient();
        //    return WebClient.DownloadString("http://v-comp.com.ua/protsessory/?paga=0&col_page=20&sort=&minP=0&maxP=60941.396&bren=&param=bnVsbA==");
        //}

        static string LoadPage(string url, ref CookieCollection cookieSession)
        {
            var result = "";
            System.Net.Cookie cookie = new System.Net.Cookie();
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.CookieContainer = new CookieContainer();
            if (cookieSession != null)
            {
                request.CookieContainer.Add(cookieSession);
            }
            var response = (HttpWebResponse)request.GetResponse();
            
            

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Cookies.Count > 0)
                {
                    cookieSession = response.Cookies;
                }
                var receiveStream = response.GetResponseStream();
                if (receiveStream != null)
                {
                    StreamReader readStream;
                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    result = readStream.ReadToEnd();
                    readStream.Close();
                }
                
                response.Close();
            }
            return result;
        }

        //static IEnumerable<WebProxy> ParseProxies()
        //{
        //    var result = new List<WebProxy>();
        //    var lines = LoadPage("http://www.gatherproxy.com/").Split(Environment.NewLine).Select(s => s.Trim()).Where(s => s.StartsWith("gp.insertPrx"));
        //    foreach (var line in lines)
        //    {
        //        var parts = line.Split(',').Select(s => s.Trim());
        //        var ipPart = parts.FirstOrDefault(s => s.Contains("PROXY_IP"));
        //        var portPart = parts.FirstOrDefault(s => s.Contains("PROXY_PORT"));
        //        var ip = ipPart.Replace("\"PROXY_IP\":", "").Trim('"');
        //        var port = Convert.ToInt32(portPart.Replace("\"PROXY_PORT\":", "").Trim('"'), 16);
        //        result.Add(new WebProxy(ip, port));
        //    }
        //    return result;
        //}

        private static ICollection<string> GetTextsFromHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return new List<string> { html };
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return GetTextsFromNode(htmlDoc.DocumentNode.ChildNodes);
        }

        private static ICollection<string> GetTextsFromNode(HtmlNodeCollection nodes)
        {
            var texts = new List<string>();
            foreach (var node in nodes)
            {
                var nodeName = node.Name.ToLowerInvariant();
                if (nodeName == "style" || nodeName == "script")
                    continue;
                if (node.HasChildNodes)
                {
                    texts.AddRange(GetTextsFromNode(node.ChildNodes));
                }
                else
                {
                    var innerText = node.InnerText;
                    if (!string.IsNullOrWhiteSpace(innerText))
                    {
                        texts.Add(innerText);
                    }
                }
            }

            return texts;
        }

        private static ICollection<string> GetTextsFromHtml2(string html)
        {
            if (string.IsNullOrEmpty(html))
                return new List<string> { html };
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var texts = new List<string>();
            var nodes = htmlDoc.DocumentNode.SelectNodes("//text()[(normalize-space(.) != '') and not(parent::script) and not(parent::style) and not(*)]");
            foreach (var htmlNode in nodes)
            {
                texts.Add(htmlNode.InnerText);
                //htmlNode.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(htmlNode.InnerText + "_translated"), htmlNode);
            }

            return texts;
        }

        public static string StripHtml(string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;
            //get rid of HTML tags
            var output = Regex.Replace(source, "<[^>]*>", string.Empty);
            //get rid of multiple blank lines
            output = Regex.Replace(output, @"^\s*$\n", string.Empty, RegexOptions.Multiline);
            return HttpUtility.HtmlDecode(output);
        }


        public static List<CPU> GetCPUs(HtmlDocument document)
        {
            HtmlNodeCollection htmlNodes = document.DocumentNode.SelectNodes("//div[@class='section_tovar_info']");
            if (htmlNodes != null)
            {
                List<CPU> CPUs = new List<CPU>();

                string name;
                string manufacture;
                string processorNumber;
                int numOfCores;
                float processorBaseFrequency;
                string pbfString;

                foreach (HtmlNode node in htmlNodes)
                {
                    //Take the Name of Processor
                    name = node.SelectSingleNode("./h3//a").InnerHtml;
                    if (name.IndexOf("Процессор") != -1)
                        name = name.Remove(0, 10).Trim("\n\t ".ToCharArray());
                    else
                        name = name.Trim("\n\t ".ToCharArray());
                    //Clear From '&nbsp;' for now
                    name = ClearFromUnnecessarySymbols(name);
                    //Take Manufacture from processor name
                    manufacture = name.Substring(0, name.IndexOf(' '));
                    //Take processor name without manufacture
                    processorNumber = name.Remove(0, name.IndexOf(' ') + 1);
                    //Initialization nubmberOfCores
                    numOfCores = 0;
                    //Parse numOfCores from html
                    int.TryParse(node.SelectSingleNode("//div[@class='mini_msg']//table/tbody/tr[2]/td[2]").InnerHtml, out numOfCores);
                    //Initialization pbf
                    processorBaseFrequency = 0;
                    //Take pbf like a string
                    pbfString = ClearFromUnnecessarySymbols(node.SelectSingleNode("//div[@class='mini_msg']//table/tbody/tr[3]/td[2]").InnerHtml.Trim("\n\t\r".ToCharArray()));
                    //Remove unnesessery part
                    pbfString = pbfString.Remove(pbfString.IndexOf("ГГц"), pbfString.Length - pbfString.IndexOf("ГГц")).Trim();
                    //Parse the pbf
                    float.TryParse(pbfString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out processorBaseFrequency);

                    //Add new CPU to list for return
                    CPUs.Add(new CPU() { Manufacture = manufacture, ProcessorNumber = processorNumber, PBF = processorBaseFrequency, NumberOfCores = numOfCores });
                    
                }

                return CPUs;
            }
            return new List<CPU>();
        }

        public static string ClearFromUnnecessarySymbols(string s)
        {
            bool unnecessarySymbols = true;
                while (unnecessarySymbols)
                {
                    if (s.Contains("&nbsp;"))
                    {
                        s = s.Remove(s.IndexOf("&nbsp;"), 6);
                    }
                    else
                    {
                        unnecessarySymbols = false;
                    }
                }
            return s;
        }

        public static void SaveCPUs(List<CPU> cpus)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<CPU>));

            using(FileStream fs = new FileStream(@"R:\CPUs.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, cpus);
            }

        }
    }
}
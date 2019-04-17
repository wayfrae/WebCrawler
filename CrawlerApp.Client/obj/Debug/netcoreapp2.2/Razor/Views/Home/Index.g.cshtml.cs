#pragma checksum "C:\Users\frazi\CS-4790\13-final-project-wayfrae\CrawlerApp.Client\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f649666bcabdae8f709f1a5d8656c65c65f310d7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\frazi\CS-4790\13-final-project-wayfrae\CrawlerApp.Client\Views\_ViewImports.cshtml"
using CrawlerApp.Client;

#line default
#line hidden
#line 2 "C:\Users\frazi\CS-4790\13-final-project-wayfrae\CrawlerApp.Client\Views\_ViewImports.cshtml"
using CrawlerApp.Client.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f649666bcabdae8f709f1a5d8656c65c65f310d7", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a16845a37b5ff97c0bbc27d9e876c169ef037114", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/signalr/dist/browser/signalr.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/links.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Users\frazi\CS-4790\13-final-project-wayfrae\CrawlerApp.Client\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
            BeginContext(45, 2929, true);
            WriteLiteral(@"
<div class=""text-center"">
    <h1 class=""display-4"">Web Crawler</h1>
    <h6 class=""display-6"">By Zach Frazier</h6>
    <canvas id=""myChart"" width=""400"" height=""400""></canvas>
    <script src=""https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js""></script>
    <script>
        var toCrawl;
        var haveCrawled;

        var ctx = document.getElementById('myChart').getContext('2d');

        $.getJSON(window.location + ""/links/tocrawl/count"").done(function (data)
        {
            console.log(data);
            toCrawl = data;
        });
        $.getJSON(window.location + ""/links/havecrawled/count"").done(function (data) {
            console.log(data);
            haveCrawled = data;
        });

        console.log(""To crawl: "" + toCrawl);
        console.log(""Have Crawled: "" + haveCrawled);

        function updateChart(chart, data) {
            removeData(chart);
            addData(data);
            chart.update();
        }

        function addData(");
            WriteLiteral(@"data) {
            chart.data.datasets.forEach((dataset) => {
                dataset.data.push(data);
            });
        }

        function removeData(chart) {
            chart.data.datasets.forEach((dataset) => {
                dataset.data.pop();
            });
        }
        
        var myChart = new Chart(ctx,
            {
                type: 'doughnut',
                data: {
                    labels: [""To Crawl"", ""Have Crawled""],
                    datasets: [
                        {
                            label: ""Population (millions)"",
                            backgroundColor: [""#3e95cd"", ""#8e5ea2""],
                            data: [1, 1]
                        }
                    ]
                },
                options: {
                    title: {
                        display: true,
                        text: 'Predicted world population (millions) in 2050'
                    }
                }
            });
       ");
            WriteLiteral(@" 

        
    </script>
    <div class=""container"">
        <input id=""toCrawl"" hidden />
        <input id=""haveCrawled"" hidden />
        <div class=""row"">&nbsp;</div>
        <div class=""row"">
            <div class=""col-6"">&nbsp;</div>
            <div class=""col-6"">
                User..........<input type=""text"" id=""userInput"" />
                <br />
                Message...<input type=""text"" id=""messageInput"" />
                <input type=""button"" id=""sendButton"" value=""Send Message"" />
            </div>
        </div>
        <div class=""row"">
            <div class=""col-12"">
                <hr />
            </div>
        </div>
        <div class=""row"">
            <div class=""col-6"">&nbsp;</div>
            <div class=""col-6"">
                <ul id=""messagesList""></ul>
            </div>
        </div>
    </div>
    ");
            EndContext();
            BeginContext(2974, 61, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f649666bcabdae8f709f1a5d8656c65c65f310d77209", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3035, 6, true);
            WriteLiteral("\r\n    ");
            EndContext();
            BeginContext(3041, 37, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f649666bcabdae8f709f1a5d8656c65c65f310d78392", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3078, 12, true);
            WriteLiteral("\r\n</div>\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591

"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/linkshub").build();
var toCrawl;
var haveCrawled;

var ctx = document.getElementById("myChart");
var myChart = new Chart(ctx,
    {
        type: 'doughnut',
        data: {
            labels: ["To Crawl", "Have Crawled"],
            datasets: [
                {
                    label: "links",
                    backgroundColor: ["#3e95cd", "#8e5ea2"],
                    data: [1, 1]
                }
            ]
        },
        options: {
            title: {
                display: true,
                text: 'All links retreived'
            }
        }
    });

setInterval(function() {
    $.getJSON(window.location + "links/tocrawl/count").done(function(data) {
        toCrawl = data;
    }).done(function() {
        $.getJSON(window.location + "links/havecrawled/count").done(function(data) {
            haveCrawled = data;
        }).done(function() {
            myChart.data.datasets[0].data[0] = toCrawl;
            myChart.data.datasets[0].data[1] = haveCrawled;
            myChart.options.title.text = toCrawl + " links to crawl | " + haveCrawled + " links have been crawled";
            myChart.update(0);
        });
    });
}, 1000);

connection.on("NotifyChange", function () {
    $.getJSON(window.location + "links/tocrawl/count").done(function (data) {
        console.log(data);
        toCrawl = data;
    }).done(function () {
        $.getJSON(window.location + "links/havecrawled/count").done(function (data) {
            console.log(data);
            haveCrawled = data;
        }).done(function () {
            myChart.data.datasets[0].data[0] = toCrawl;
            myChart.data.datasets[0].data[1] = haveCrawled;
            myChart.update(0);
        });
    });
});


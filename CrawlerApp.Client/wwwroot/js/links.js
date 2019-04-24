"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/linkshub").build();
var toCrawl;
var haveCrawled;

var ctx = document.getElementById("myChart");
var myChart = new Chart(ctx,
    {
        type: 'doughnut',
        responsive: true,
        data: {
            labels: ["To Crawl", "Have Crawled"],
            datasets: [
                {
                    label: "links",
                    backgroundColor: ["#3e95cd", "#8e5ea2"],
                    data: [0, 0]
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
var count = 0;
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
            if (count === 0) {
                myChart.update();
            } else {
                myChart.update(0);
            }
            });
        count++;
    });
}, 1000);

$(document).ready(function() {
    setTimeout(function() { populateLinks(); }, 10000)});

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

function populateLinks() {
    var linksToCrawl, linksBeenCrawled;

    $.getJSON(window.location + "links/tocrawl").done(function(data) {
        toCrawl = data;
        $("#linksToCrawl > #accordionToCrawl").empty();
        toCrawl.forEach(link =>
            $("#linksToCrawl > #accordionToCrawl").append('<div class="card"><div class= "card-header" id="cardheader' +
                link.id +
                '" >' +
                '<h5 class="mb-0">' +
                '<button class="btn btn-link" data-toggle="collapse" data-target="#header' +
                link.id +
                '" aria-expanded="true" aria-controls="header' +
                link.id +
                '">' +
                link.address +
                '</button ></h5 ></div >' +
                '<div id="header' +
                link.id +
                '"" class="collapse" aria-labelledby="header' +
                link.id +
                '"" data-parent="#accordionToCrawl">' +
                '<div class="card-body">' +
                '<p>Found On: ' +
                link.foundOn +
                '</p>' +
                '<p>Date: ' +
                link.date +
                '</p>' +
                '<p>Response: ' +
                link.response +
                '</p>' +
                '</div></div></div > ')
        );
    });
    $.getJSON(window.location + "links/havecrawled").done(function(data) {
        haveCrawled = data;
        $("#linksHaveCrawled > #accordionToCrawl").empty();
        haveCrawled.forEach(link =>
            $("#linksHaveCrawled > #accordionHaveCrawled").append(
                '<div class="card"><div class= "card-header" id="cardheader' +
                link.id +
                '" >' +
                '<h5 class="mb-0">' +
                '<button class="btn btn-link" data-toggle="collapse" data-target="#header' +
                link.id +
                '" aria-expanded="true" aria-controls="header' +
                link.id +
                '">' +
                link.address +
                '</button ></h5 ></div >' +
                '<div id="header' +
                link.id +
                '"" class="collapse" aria-labelledby="header' +
                link.id +
                '"" data-parent="#accordionHaveCrawled">' +
                '<div class="card-body">' +
                '<p>Found On: ' +
                link.foundOn +
                '</p>' +
                '<p>Date: ' +
                link.date +
                '</p>' +
                '<p>Response: <br>' +
                link.response +
                '</p>' +
                '</div></div></div > ')
        );
    });
    $.getJSON(window.location + "links").done(function (data) {
        haveCrawled = data;
        $("#allLinks > #accordionToCrawl").empty();

        haveCrawled.forEach(link =>
            $("#allLinks > #accordionAll").append(
                '<div class="card"><div class= "card-header" id="cardAllHeader' +
                link.id +
                '" >' +
                '<h5 class="mb-0">' +
                '<button class="btn btn-link" data-toggle="collapse" data-target="#allHeader' +
                link.id +
                '" aria-expanded="true" aria-controls="allHeader' +
                link.id +
                '">' +
                link.address +
                '</button ></h5 ></div >' +
                '<div id="allHeader' +
                link.id +
                '"" class="collapse" aria-labelledby="allHeader' +
                link.id +
                '"" data-parent="#accordionHaveCrawled">' +
                '<div class="card-body">' +
                '<p>Has been crawled: ' +
                (link.isCrawled ? "yes" : "no") +
                '</p></div></div></div > ')
        );
    });

}

function openLink(evt, linkName) {
    // Declare all variables
    var i, tabcontent, tablinks;

    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Get all elements with class="tablinks" and remove the class "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(linkName).style.display = "block";
    evt.currentTarget.className += " active";
}


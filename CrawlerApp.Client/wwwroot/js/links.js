"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/linkshub").build();
var toCrawl;
var haveCrawled;

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;   

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

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

connection.on("NotifyChange", function () {
    $.getJSON(window.location + "/links/tocrawl/count").done(function (data) {
        console.log(data);
        toCrawl = data;
    }).done(function () {
        $.getJSON(window.location + "/links/havecrawled/count").done(function (data) {
            console.log(data);
            haveCrawled = data;
        }).done(function () {
            myChart.data.datasets[0].data[0] = toCrawl;
            myChart.data.datasets[0].data[1] = haveCrawled;
            myChart.update();
        });
    });
    

});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();  
});
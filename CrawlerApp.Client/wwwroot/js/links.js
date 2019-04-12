"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/linkshub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;   

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
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
            updateChart(myChart, [toCrawl, haveCrawled]);
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
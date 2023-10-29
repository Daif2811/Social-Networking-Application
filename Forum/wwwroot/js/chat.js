"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message, senderId) {

    var messagesContainer = document.getElementById("messagesContainer");
    var messagesList = document.getElementById("messagesList");
    var li = document.createElement("li");

    // Check if the message is from the current user
    if (senderId === connection.connectionId) {
        li.className = "currentUserMessage";
    } else {
        li.className = "otherUserMessage";
    }

    li.textContent = `${message}`;
    messagesList.appendChild(li);

    // Scroll to the bottom of the messages container
    messagesContainer.scrollTop = messagesContainer.scrollHeight;
});


connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});



document.getElementById("sendButton").addEventListener("click", function (event) {
    var chatIdValue = document.getElementById("chatId");
    var chatId = parseInt(chatIdValue.value);
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessage", chatId, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

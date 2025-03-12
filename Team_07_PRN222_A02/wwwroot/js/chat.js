"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

if (connection.state === signalR.HubConnectionState.Disconnected) {
    connection.start().catch(err => console.error(err.toString()));
}


connection.on("ReceiveMessage", function (user, title, message) {
    var notificationList = document.getElementById("notificationList");
    var notificationBadge = document.getElementById("notificationBadge");

    if (notificationList.children.length === 1 && notificationList.children[0].tagName === "P") {
        notificationList.innerHTML = "";
    }

    var notificationItem = document.createElement("div");
    notificationItem.classList.add("dropdown-item");
    notificationItem.innerHTML = `<strong>${user}:${title}</strong><br>${message}`;

    notificationList.prepend(notificationItem);

    notificationBadge.style.display = "inline";


    if (connection.state === signalR.HubConnectionState.Disconnected) {
        connection.start().catch(err => console.error(err.toString()));
    }

    document.getElementById("notificationDropdown").addEventListener("click", function () {
        document.getElementById("notificationBadge").style.display = "none";
    });
});
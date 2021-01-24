function OK() {
    alert("Payment Success");
    window.location.href = "/Checkout/CleanAndUpdate" 
}

function search() {
    var criteria = document.getElementById("se").value;
    var url = "/Gallery/Search?criteria=" + criteria;
    window.location.href = url;
}

function updateItemsOnCart() {
    var canvas = document.getElementById("myCanvas");
    var ctx = canvas.getContext("2d");
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.beginPath();
    ctx.arc(canvas.width / 2, canvas.height / 2, 12, 0, 2 * Math.PI);
    ctx.font = "15px Arial";
    ctx.textAlign = "center";
    var items = document.getElementById("totalCartItems").value;
    ctx.fillText(items, canvas.width / 2, canvas.height / 2 + 3);
    ctx.stroke();
}

function checkout() {
    var cookies = document.cookie;
    var sessionId = cookies.split(';')[0].split('=')[1];
    let userId = null;
    if (cookies.split(';').length > 1) {
        userId = cookies.split(';')[1].split('=')[1];
    }
    if (userId == null) {
        alert("Please log in first to checkout");
        window.location.href = "/Login/Index?source=FromCart"
    }
    else {
        window.location.href = "/checkout/Index"
    }
}

function emptyCart() {
    var items = document.getElementById("totalCartItems").value;

    if (items == 0) {
        alert("Add an item to cart!");
    }

    return true;
}


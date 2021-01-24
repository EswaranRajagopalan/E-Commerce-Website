window.onload = function () {
    updateItemsOnCart();
    let ratingButtons = document.getElementsByClassName("RateBtn");

    for (let i = 0; i < ratingButtons.length; i++) {
        ratingButtons[i].addEventListener("click", onRate);
    }

}

function onRate(event) {
    var elemId = event.currentTarget.getAttribute("id").toString();
    var productId = elemId.substring(0, elemId.length - 3);
    var cookies = document.cookie;
    userId = cookies.split(';')[1].split('=')[1];
    var divId = productId + "did";
    var spanId = productId + "sid";
    document.getElementById(divId).style.display = "block";
    var span = document.getElementById(spanId);
    span.onclick = function () {
        document.getElementById(divId).style.display = "none";
        var rating = document.getElementById("ratingValue").value;
        updateRating(productId, userId, Number(rating));
    }
}

function handleChange(src) {
    document.getElementById("ratingValue").value = src.value;
}
function updateRating(productId, userId, rating) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/MyPurchases/RateProduct");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            // receives response from server
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                console.log("Successful operation: " + data.success);
            }
        }
    };

    // send product rating to server

    xhr.send(JSON.stringify({
        UserId: userId,
        ProductId: productId,
        Rate: rating
    }));
}
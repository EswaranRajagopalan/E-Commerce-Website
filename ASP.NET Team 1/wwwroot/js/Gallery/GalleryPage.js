window.onload = function () {
    //call function to display total items in the cart
    updateItemsOnCart();
    let cartButtons = document.getElementsByClassName("cartButton");

    //Event Listener for Add To Cart Buttons
    for (let i = 0; i < cartButtons.length; i++) {
        cartButtons[i].addEventListener("click", onAdd);
    }

}


/*function that is called when add to cart function is clicked
 *Calls a function that makes a AJAX request to the server
 */
function onAdd(event) {
    var elemId = event.currentTarget.getAttribute("id").toString();
    var productId = elemId.substring(0, elemId.length-3);
    document.getElementById("totalCartItems").value = Number(document.getElementById("totalCartItems").value) + 1;
    var cookies = document.cookie;
    var sessionId = cookies.split(';')[0].split('=')[1];
    let userId = null;
    if (cookies.split(';').length > 1) {
        userId = cookies.split(';')[1].split('=')[1];
    }
    if (userId == null) {
        modifyCart(sessionId, productId, null);
        updateItemsOnCart();
        return;
    }
    modifyCart(sessionId, productId, userId);
    updateItemsOnCart();
}

/*
 * Function that makes AJAX request to the server 
 * to modify the cart records in the database
 */
function modifyCart(sessionId, productId, userId) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Cart/GalleryIndex");
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

    // send cart item to server
    xhr.send(JSON.stringify({
        UserId: userId,
        ProductId: productId,
        SessionId: sessionId,
        Quantity:1
    }));
}


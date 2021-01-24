window.onload = function () {
    updateItemsOnCart();
    var removeCartItemButtons = document.getElementsByClassName('btn btn-danger');
    for (var i = 0; i < removeCartItemButtons.length; i++) {
        var button = removeCartItemButtons[i];
        button.addEventListener('click', removeCartItem);
    }
  
    var quantityInputs = document.getElementsByClassName('quantity-field');
    for (var i = 0; i < quantityInputs.length; i++) {
        var input = quantityInputs[i];
        input.addEventListener('click', quantityChanged);
    }

}



function removeCartItem(event) {
    var buttonClicked = event.target
    let prodID = buttonClicked.getAttribute("product_id")
    buttonClicked.parentElement.parentElement.remove()
    Delete_item(prodID)
    }

function quantityChanged(event) {
    var input = event.target
    let prodID = input.getAttribute("product_id")
    if (isNaN(input.value)) {
        input.value = 1
    }
    update(parseInt(input.value), prodID)
}




function update(input, prodID) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Cart/EditProductQuantity");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            // receive response from server
            if (this.status === 200 || this.status === 302) {
                let data = JSON.parse(this.responseText);

                if (this.status === 200) {
                    window.location.reload();
                    console.log("Successful operation: " + data.success);
                }
                else if (this.status === 302) {
                    window.location = data.redirect_url;
                }
            }
        }
    }

    // send like/unlike choice to server

    let data = { ProductID: prodID, quantity: input };

    xhr.send(JSON.stringify(data));
}

    function Delete_item(prodID) {
        let xhr = new XMLHttpRequest();

        xhr.open("Delete", "/Cart/RemoveFromCart");
        xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
        xhr.onreadystatechange = function () {
            if (this.readyState === XMLHttpRequest.DONE) {
                // receive response from server
                if (this.status === 200 || this.status === 302) {
                    let data = JSON.parse(this.responseText);

                    if (this.status === 200) {
                        window.location.reload();
                        console.log("Successful operation: " + data.success);
                    }
                    else if (this.status === 302) {
                        window.location = data.redirect_url;
                    }
                }
            }
        }

        // send like/unlike choice to server
        xhr.send(JSON.stringify({

            ProductID: prodID
        }));
    }

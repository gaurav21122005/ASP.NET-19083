// ===================== CART SYSTEM =========================

// Load existing cart
function getCart() {
    let cart = localStorage.getItem("cart");
    return cart ? JSON.parse(cart) : [];
}

// Save cart
function saveCart(cart) {
    localStorage.setItem("cart", JSON.stringify(cart));
}

// Add item to cart
function addToCart(id, name, price) {
    let cart = getCart();

    let existing = cart.find(i => i.id === id);

    if (existing) {
        existing.quantity++;
    } else {
        cart.push({
            id: id,
            name: name,
            price: price,
            quantity: 1
        });
    }

    saveCart(cart);
    alert(name + " added to cart!");
}

// Show on checkout page
function loadCheckoutPage() {
    let cart = getCart();

    let tbody = document.getElementById("cart-items");
    let totalSpan = document.getElementById("total-amount");

    if (!tbody) return;

    tbody.innerHTML = "";
    let total = 0;

    cart.forEach(item => {
        let subtotal = item.price * item.quantity;
        total += subtotal;

        tbody.innerHTML += `
            <tr>
                <td>${item.name}</td>
                <td>₹${item.price}</td>
                <td>${item.quantity}</td>
                <td>₹${subtotal}</td>
            </tr>
        `;
    });

    totalSpan.innerText = "₹" + total;

    // BACKEND JSON
    document.getElementById("ItemsJson").value = JSON.stringify(cart);
}

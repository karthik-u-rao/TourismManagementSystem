document.addEventListener('DOMContentLoaded', function() {
    const seatInput = document.getElementById("seatInput");
    const seatCount = document.getElementById("seatCount");
    const totalAmount = document.getElementById("totalAmount");
    const pricePerSeat = parseFloat(document.getElementById("pricePerSeat")?.innerText) || 0;
    const form = document.getElementById("bookingForm");
    const confirmBtn = document.getElementById("confirmBtn");

    function updateTotal() {
        const seats = parseInt(seatInput?.value) || 1;
        if (seatCount) seatCount.innerText = seats;
        if (totalAmount) totalAmount.innerText = (seats * pricePerSeat).toFixed(2);
    }

    function validateEmail(email) {
        return email.length > 0 && email.indexOf('@') > 0 && email.indexOf('.') > email.indexOf('@');
    }

    if (seatInput) {
        seatInput.addEventListener("input", updateTotal);
        seatInput.addEventListener("change", updateTotal);
    }
    
    if (form) {
        form.addEventListener('submit', function(e) {
            const customerName = document.querySelector('input[name="CustomerName"]')?.value.trim() || '';
            const email = document.querySelector('input[name="Email"]')?.value.trim() || '';
            const phone = document.querySelector('input[name="PhoneNumber"]')?.value.trim() || '';
            const seats = parseInt(document.querySelector('input[name="NumberOfSeats"]')?.value) || 0;

            console.log('Submitting booking:', { customerName, email, phone, seats });

            if (!customerName || customerName.length < 2) {
                alert('Please enter a valid name (at least 2 characters)');
                e.preventDefault();
                return false;
            }

            if (!validateEmail(email)) {
                alert('Please enter a valid email address');
                e.preventDefault();
                return false;
            }

            if (!phone || phone.length < 10) {
                alert('Please enter a valid phone number (at least 10 digits)');
                e.preventDefault();
                return false;
            }

            if (!seats || seats < 1 || seats > 10) {
                alert('Please select between 1 and 10 seats');
                e.preventDefault();
                return false;
            }

            if (confirmBtn) {
                confirmBtn.disabled = true;
                confirmBtn.innerHTML = '<i class="bi bi-hourglass-split"></i> Processing...';
            }
        });
    }
    
    updateTotal();
});